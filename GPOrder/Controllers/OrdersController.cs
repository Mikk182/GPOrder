using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GPOrder.Models;
using EntityState = System.Data.Entity.EntityState;

namespace GPOrder.Views
{
    [Authorize]
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        public ActionResult Index()
        {
            return View(db.Orders.Include(o => o.OrderLines).Include(o => o.OrderLines.Select(ol => ol.Product)).ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Orders.Include(o => o.OrderLines).Include(o => o.OrderLines.Select(ol => ol.Product)).Single(o => o.Id == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            var order = new Order
            {
                OrderDate = DateTime.Now,
                Date = DateTime.Now,
                OrderLines = new List<OrderLine> { new OrderLine() }
            };
            return View(order);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,OrderDate,IsLocked,OrderLines")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.User = db.Users.Single(u => u.UserName == User.Identity.Name);
                foreach (var ol in order.OrderLines)
                {
                    var product = db.Products.Single(p => p.Id == ol.Product.Id);
                    ol.Product = product;
                }
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        public PartialViewResult GetNewOrderLine()
        {
            return PartialView("EditorTemplates/OrderLine", new OrderLine());
        }

        public ActionResult GetProductsNames(string term)
        {
            var result = db.Products
                .Where(c => c.Name.StartsWith(term))
                .Take(10)
                .Select(c => new
                {
                    id = c.Id,
                    value = c.Name,
                    label = c.Name
                })
                .ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Orders.Include(o => o.OrderLines).Include(o => o.OrderLines.Select(ol => ol.Product)).Single(o => o.Id == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,OrderDate,IsLocked,OrderLines")] Order order)
        {
            if (ModelState.IsValid)
            {
                foreach (var ol in order.OrderLines)
                {
                    var product = db.Products.Single(p => p.Id == ol.Product.Id);
                    ol.Product = product;
                    db.Entry(ol).State = ol.Id == Guid.Empty ? EntityState.Added : EntityState.Modified;
                }
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
