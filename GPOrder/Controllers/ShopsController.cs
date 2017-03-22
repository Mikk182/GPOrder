using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GPOrder.Models;
using Microsoft.AspNet.Identity;
using EntityState = System.Data.Entity.EntityState;

namespace GPOrder.Views
{
    [Authorize]
    public class ShopsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Shops
        public ActionResult Index()
        {
            var list = db.Shops.Include(path => path.CreateUser).Include(path => path.OwnerUser).ToList();
            return View(list);
        }

        // GET: Shops/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shop shop = db.Shops.Include(path => path.OwnerUser).Include(path => path.CreateUser).SingleOrDefault(g => g.Id == id);
            if (shop == null)
            {
                return HttpNotFound();
            }
            return View(shop);
        }

        // GET: Shops/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Shops/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Adress,PhoneNumber,Mail,Description")] Shop shop)
        {
            if (ModelState.IsValid)
            {
                shop.CreationDate = DateTime.UtcNow;
                var userId = User.Identity.GetUserId();
                shop.OwnerUser = shop.CreateUser = db.Users.Single(u => u.Id == userId);

                db.Entry(shop).State = EntityState.Added;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(shop);
        }

        // GET: Shops/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Shop shop = db.Shops.Include(g => g.OwnerUser).SingleOrDefault(g => g.Id == id);
            if (shop == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.GetUserId() == shop.OwnerUserId)
            {
                return View(shop);
            }

            return View("Details", shop);
        }

        // POST: Shops/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CreateUserId,CreationDate,IsLocked,Name,OwnerUser,OwnerUserId,ApplicationUsers")] Shop shop)
        {
            if (ModelState.IsValid)
            {
                var dbShop = db.Shops.Single(g => g.Id == shop.Id);
                dbShop.IsLocked = shop.IsLocked;
                dbShop.Name = shop.Name;
                dbShop.OwnerUserId = shop.OwnerUserId;

                db.Entry(dbShop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shop);
        }

        // GET: Shops/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shop shop = db.Shops.Find(id);
            if (shop == null)
            {
                return HttpNotFound();
            }
            return View(shop);
        }

        // POST: Shops/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Shop shop = db.Shops.Find(id);
            db.Shops.Remove(shop);
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
