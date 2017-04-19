using System;
using System.Data.Entity;
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

        // GET: Bills/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bill = db.Bills.Find(id);
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

            if (groupedorder.DeliveryBoy_Id != currentUserId)
                throw new Exception("Cannot create a bill if youre not the delivery boy.");

            var newBill = new Bill
            {
                CreateUser_Id = currentUserId,
                Id = groupedOrderId,
                GroupedOrder = groupedorder,
                BillEvents = groupedorder.Orders.Select(o =>
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
                ).ToList()
            };

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
                var groupedOrder = db.GroupedOrders.Single(go => go.Id == bill.GroupedOrder.Id);

                var currentUserId = User.Identity.GetUserId();
                if (groupedOrder.DeliveryBoy_Id != currentUserId)
                    throw new Exception("Cannot create a bill if youre not the delivery boy.");

                bill.Id = groupedOrder.Id;
                bill.GroupedOrder = groupedOrder;

                foreach (var billEvents in bill.BillEvents)
                {
                    billEvents.CreationDate = DateTime.UtcNow;
                }

                db.Bills.Add(bill);
                db.SaveChanges();

                return RedirectToAction("Details", "GroupedOrders", new { bill.GroupedOrder.Id });
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
            var bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }

            var currentUserId = User.Identity.GetUserId();
            if (bill.GroupedOrder.DeliveryBoy_Id != currentUserId)
                throw new Exception("Cannot create a bill if youre not the delivery boy.");

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
                var currentUserId = User.Identity.GetUserId();
                var groupedOrder = db.GroupedOrders.Single(go => go.Id == bill.GroupedOrder.Id);
                if (groupedOrder.DeliveryBoy_Id != currentUserId)
                    throw new Exception("Cannot create a bill if youre not the delivery boy.");

                foreach (var billEvents in bill.BillEvents)
                {
                    db.Entry(billEvents).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("Details", "GroupedOrders", new { bill.GroupedOrder.Id });
            }

            return View(bill);
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
