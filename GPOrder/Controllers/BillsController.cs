using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
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
            newBill.GroupedOrder_Id = groupedOrderId;
            newBill.BillEvents = groupedorder.Orders.Select(o=> 
                new BillEvent
                {
                    Amount = o.EstimatedPrice,
                    CreateUserId = currentUserId, // The delivery boy of the grouped order
                    DebitUser_Id = currentUserId, // The delivery boy of the grouped order
                    EventType = EventType.BillOrderEvent,
                    CreationDate= DateTime.UtcNow,
                    CreditUser_Id = o.CreateUser_Id
                }
            ).ToList();

            return View();
        }

        // POST: Bills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CreateUser_Id,GroupedOrder_Id")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                bill.Id = Guid.NewGuid();
                db.Bills.Add(bill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreateUser_Id = new SelectList(db.ApplicationUsers, "Id", "UiCulture", bill.CreateUser_Id);
            ViewBag.Id = new SelectList(db.GroupedOrders, "Id", "CreateUser_Id", bill.Id);
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
            ViewBag.CreateUser_Id = new SelectList(db.ApplicationUsers, "Id", "UiCulture", bill.CreateUser_Id);
            ViewBag.Id = new SelectList(db.GroupedOrders, "Id", "CreateUser_Id", bill.Id);
            return View(bill);
        }

        // POST: Bills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CreateUser_Id,GroupedOrder_Id")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreateUser_Id = new SelectList(db.ApplicationUsers, "Id", "UiCulture", bill.CreateUser_Id);
            ViewBag.Id = new SelectList(db.GroupedOrders, "Id", "CreateUser_Id", bill.Id);
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
