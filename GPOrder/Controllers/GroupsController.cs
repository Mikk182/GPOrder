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
    public class GroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index()
        {
            var list = db.Groups.Include(path => path.CreateUser).Include(path => path.OwnerUser).ToList();
            return View(list);
        }

        // GET: Products/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Include(path => path.OwnerUser).Include(path => path.CreateUser).SingleOrDefault(g => g.Id == id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // GET: Products/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Group group)
        {
            if (ModelState.IsValid)
            {
                group.CreationDate = DateTime.UtcNow;
                var userId = User.Identity.GetUserId();
                group.OwnerUser = group.CreateUser = db.Users.Single(u => u.Id == userId);

                db.Entry(group).State = EntityState.Added;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(group);
        }

        // GET: Products/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Group group = db.Groups.Include(g => g.OwnerUser).SingleOrDefault(g => g.Id == id);
            if (group == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.GetUserId() == group.OwnerUserId)
            {
                return View(group);
            }

            return View("Details", group);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CreateUserId,CreationDate,IsLocked,Name,OwnerUser,OwnerUserId,ApplicationUsers")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        // GET: Products/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Join(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Group group = db.Groups.Include(g => g.OwnerUser).Include(g => g.ApplicationUsers).SingleOrDefault(g => g.Id == id);
            if (group == null)
            {
                return HttpNotFound();
            }

            return View("Join", group);
        }

        // POST: Groups/Join/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Join([Bind(Include = "Id")] Group group)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUserId = User.Identity.GetUserId();
                    var currentUser = db.Users.Single(u => u.Id == currentUserId);

                    var currentGroup = db.Groups.Include(g => g.ApplicationUsers).Single(g => g.Id == group.Id);
                    if (currentGroup.ApplicationUsers == null)
                        group.ApplicationUsers = new List<ApplicationUser>();

                    // ReSharper disable once PossibleNullReferenceException
                    currentGroup.ApplicationUsers.Add(currentUser);

                    db.Entry(currentGroup).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(group);
            }
            catch (DbEntityValidationException e)
            {
                return View(group);
            }
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
