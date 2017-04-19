using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using GPOrder.Entities;
using GPOrder.Models;
using GPOrder.ValidationHelpers;
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
        public ActionResult Edit([Bind(Include = "Id,CreateUser_Id,GroupedOrder,BillEvents,BillPictures")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.Identity.GetUserId();
                var dbBill = db.Bills.Single(b => b.Id == bill.Id);
                if (dbBill.GroupedOrder.DeliveryBoy_Id != currentUserId)
                    throw new Exception("Cannot create a bill if youre not the delivery boy.");

                foreach (var billEvents in bill.BillEvents)
                {
                    db.Entry(billEvents).State = EntityState.Modified;
                }

                if (dbBill.BillPictures == null)
                    dbBill.BillPictures = new List<BillPicture>();
                if (bill.BillPictures == null)
                    bill.BillPictures = new List<BillPicture>();
                // Removing BillPictures
                var bpsToDelete = dbBill.BillPictures.Where(dbbp => !bill.BillPictures.Select(sl => sl.Id).Contains(dbbp.Id)).ToArray();
                db.Files.RemoveRange(bpsToDelete.Select(sp => sp.LinkedFile));
                db.BillPictures.RemoveRange(bpsToDelete);

                db.SaveChanges();
                return RedirectToAction("Details", "GroupedOrders", new { bill.GroupedOrder.Id });
            }

            return View(bill);
        }

        // GET: Bills/AddPicture/5
        [Authorize]
        public ActionResult AddPictures(Guid? id)
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

            var billPicture = new BillPicture
            {
                Bill_Id = id.Value,
                Bill = bill
            };

            return View(billPicture);
        }

        // POST: Bills/AddPictures/5
        [HttpPost, ActionName("AddPictures")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult AddPictures(BillPicture billPicture, HttpPostedFileBase upload)
        {
            try
            {
                var dbBill = db.Bills.Single(s => s.Id == billPicture.Bill_Id);

                var userId = User.Identity.GetUserId();

                var billPic = new BillPicture
                {
                    CreationDate = DateTime.UtcNow,
                    CreateUser = db.Users.Single(u => u.Id == userId),
                    Name = System.IO.Path.GetFileName(upload.FileName),

                    LinkedFile = new File
                    {
                        FileType = FileType.ShopPicture,
                        ContentType = upload.ContentType,
                    },

                    Bill_Id = dbBill.Id
                };

                // Parametre 'LeaveOpen = true' car la validation a peut etre besoin de relire la stream (sinon le Dispose() du BinaryReader efface le contenu)
                using (var reader = new System.IO.BinaryReader(upload.InputStream, Encoding.Default, true))
                {
                    billPic.LinkedFile.Content = reader.ReadBytes(upload.ContentLength);
                }

                var spv = new PictureValidation<BillPicture>();
                spv.Validate(billPicture, ModelState);
                spv.Validate(upload, ModelState);

                if (!ModelState.IsValid)
                    return View(billPicture);

                db.BillPictures.Add(billPic);
                db.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("", e.Message);
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return RedirectToAction("Edit", new { Id = billPicture.Bill_Id });
        }

        // GET: Bills/ShowPictures/5
        [Authorize]
        public ActionResult ShowPicture(Guid id)
        {
            var shopPicture = db.BillPictures
                .SingleOrDefault(sp => sp.Id == id);

            if (shopPicture == null)
                return HttpNotFound();

            return View(shopPicture);
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
