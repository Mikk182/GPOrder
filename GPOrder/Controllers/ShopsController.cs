using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using GPOrder.Entities;
using GPOrder.Models;
using GPOrder.ValidationHelpers;
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
            var shop = db.Shops
                .Include(path => path.OwnerUser)
                .Include(path => path.CreateUser)
                .Include(s => s.ShopPictures.Select(sp => sp.LinkedFile))
                .SingleOrDefault(g => g.Id == id);
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
        public ActionResult Edit([Bind(Include = "Id,OwnerUserId,IsLocked,Name,Adress,PhoneNumber,Mail,Description")] Shop shop)
        {

            if (ModelState.IsValid)
            {
                var dbShop = db.Shops.Single(g => g.Id == shop.Id);
                dbShop.OwnerUserId = shop.OwnerUserId;
                dbShop.IsLocked = shop.IsLocked;
                dbShop.Name = shop.Name;
                dbShop.Adress = shop.Adress;
                dbShop.PhoneNumber = shop.PhoneNumber;
                dbShop.Mail = shop.Mail;
                dbShop.Description = shop.Description;

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

        // GET: Shops/AddPicture/5
        [Authorize]
        public ActionResult AddPictures(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var shop = db.Shops.Find(id);
            if (shop == null)
            {
                return HttpNotFound();
            }

            var shopPicture = new ShopPicture
            {
                ShopId = id.Value,
                Shop = shop
            };

            return View(shopPicture);
        }

        // POST: Shops/AddPictures/5
        [HttpPost, ActionName("AddPictures")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult AddPictures(ShopPicture shopPicture, HttpPostedFileBase upload)
        {
            try
            {
                var dbShop = db.Shops.Single(s => s.Id == shopPicture.ShopId);
                
                var userId = User.Identity.GetUserId();

                var shopPic = new ShopPicture
                {
                    CreationDate = DateTime.UtcNow,
                    CreateUser = db.Users.Single(u => u.Id == userId),
                    Name = System.IO.Path.GetFileName(upload.FileName),

                    LinkedFile = new File
                    {
                        FileType = FileType.ShopPicture,
                        ContentType = upload.ContentType,
                    },

                    ShopId = dbShop.Id
                };

                var spv = new ShopPictureValidation();
                spv.Validate(shopPicture, ModelState);
                spv.Validate(upload, ModelState);

                if (!ModelState.IsValid)
                    return View(shopPicture);

                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    shopPic.LinkedFile.Content = reader.ReadBytes(upload.ContentLength);
                }

                db.ShopPictures.Add(shopPic);
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
            return RedirectToAction("Details", new { Id = shopPicture.ShopId });
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
