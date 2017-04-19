using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GPOrder.Models;
using Microsoft.AspNet.Identity;

namespace GPOrder.Controllers
{
    public class BillsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Bills
        public ActionResult Index()
        {
            var bills = db.Bills.Include(b => b.CreateUser).Include(b => b.GroupedOrder);
            return View(bills.ToList());
        }

        // GET: Bills/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // GET: Bills/Create
        public ActionResult Create(Guid groupedOrderId)
        {
            var currentUserId = User.Identity.GetUserId();
            var groupedorder = db.GroupedOrders.Single(go => go.Id == groupedOrderId);

            var newBill = new Bill();
            newBill.CreateUser_Id = currentUserId;
            newBill.Id = groupedOrderId;
            newBill.GroupedOrder = groupedorder;
            newBill.BillEvents = groupedorder.Orders.Select(o =>
                new BillEvent
                {
                    Amount = o.EstimatedPrice,
                    CreateUserId = currentUserId, // The delivery boy of the grouped order
                    DebitUser_Id = currentUserId, // The delivery boy of the grouped order
                    EventType = EventType.BillOrderEvent,
                    CreationDate = DateTime.UtcNow,
                    CreditUser_Id = o.CreateUser_Id,
                    CreditUser = o.CreateUser,
                    Order_Id = o.Id
                }
            ).ToList();

            return View(newBill);
        }

        // POST: Bills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CreateUser_Id,GroupedOrder,BillEvents")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var groupedOrder = db.GroupedOrders.Single(go => go.Id == bill.GroupedOrder.Id);
                    bill.Id = groupedOrder.Id;
                    bill.GroupedOrder = groupedOrder;

                    foreach (var billEvents in bill.BillEvents)
                    {
                        billEvents.CreationDate = DateTime.UtcNow;
                    }

                    db.Bills.Add(bill);
                    db.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    throw;
                }

                return RedirectToAction("Index");
            }

            return View(bill);
        }

        // GET: Bills/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }

            return View(bill);
        }

        // POST: Bills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CreateUser_Id,GroupedOrder,BillEvents")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bill).State = EntityState.Modified;
                foreach (var billEvents in bill.BillEvents)
                {
                    db.Entry(billEvents).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bill);
        }

        // GET: Bills/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // POST: Bills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Bill bill = db.Bills.Find(id);
            db.Bills.Remove(bill);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
