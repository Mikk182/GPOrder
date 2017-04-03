using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using GPOrder.Entities;
using GPOrder.Models;
using GPOrder.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GPOrder.Controllers
{
    [Authorize]
    public class DeliveryBoyController : Controller
    {
        private ApplicationUserManager _userManager;

        private ApplicationDbContext db = new ApplicationDbContext();

        public DeliveryBoyController()
        {
        }

        public DeliveryBoyController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        /// <summary>
        /// GET: DeliveryBoy/AskForDeliveryBoy
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AskForDeliveryBoy(Guid id)
        {
            var groupedOrder = db.GroupedOrders.Single(go => go.Id == id);

            if (groupedOrder.DeliveryBoy == null)
                return View("BecomingDeliveryBoy", groupedOrder);

            var groupedOrderEvent = new GroupedOrderEvent
            {
                LimitDateTime = DateTime.UtcNow,
                GroupedOrderId = groupedOrder.Id,
                GroupedOrder = groupedOrder
            };
            return View("AskForDeliveryBoy", groupedOrderEvent);
        }

        /// <summary>
        /// POST: DeliveryBoy/BecomingDeliveryBoy
        /// </summary>
        /// <param name="groupedOrder"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BecomingDeliveryBoy([Bind(Include = "Id,CreateUser,CreationDate,LinkedShop")] GroupedOrder groupedOrder)
        {
            if (ModelState.IsValid)
            {
                var dbGroupedOrder = db.GroupedOrders.Single(go => go.Id == groupedOrder.Id);
                var currentUserId = User.Identity.GetUserId();
                var currentUser = db.Users.Single(u => u.Id == currentUserId);
                dbGroupedOrder.DeliveryBoy = currentUser;

                db.Entry(dbGroupedOrder).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "GroupedOrders");
            }

            return View(groupedOrder);
        }

        /// <summary>
        /// POST: DeliveryBoy/AskForDeliveryBoy
        /// </summary>
        /// <param name="groupedOrderEvent"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AskForDeliveryBoy([Bind(Include = "Id,GroupedOrderId,GroupedOrder,LimitDateTime")] GroupedOrderEvent groupedOrderEvent)
        {
            if (ModelState.IsValid)
            {
                var dbGroupedOrder = db.GroupedOrders.Single(go => go.Id == groupedOrderEvent.GroupedOrderId);
                var currentGroupedOrderDeliveryBoy = db.Users.Single(u => u.Id == dbGroupedOrder.DeliveryBoy_Id);

                groupedOrderEvent.CreationDate = DateTime.UtcNow;
                groupedOrderEvent.CreateUserId = User.Identity.GetUserId();
                groupedOrderEvent.EventType = EventType.BecomingDeliveryBoy;
                groupedOrderEvent.Users = new List<ApplicationUser> { currentGroupedOrderDeliveryBoy };
                groupedOrderEvent.EventStatus = GroupedOrderEventStatus.Submitted;
                groupedOrderEvent.GroupedOrderId = groupedOrderEvent.GroupedOrderId;

                db.Entry(groupedOrderEvent).State = EntityState.Added;
                db.SaveChanges();

                var groupedOrderLink = string.Format("<a href='{0}'>Order</a>", Url.Action("Details", "GroupedOrders", new { dbGroupedOrder.Id }));
                var acceptLink = string.Format("<a href='{0}'>Accept</a>", Url.Action("AcceptDeliveryBoyRequest", "DeliveryBoy", new { groupedOrderEvent.Id }));
                var description = string.Format("Hello {0}, I want to become delivery boy on this {1}. {2}",
                    dbGroupedOrder.DeliveryBoy.UserName, groupedOrderLink, acceptLink);

                groupedOrderEvent.Description = description;
                db.Entry(groupedOrderEvent).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "GroupedOrders");
            }

            return View(groupedOrderEvent);
        }

        /// <summary>
        /// GET: DeliveryBoy/AcceptDeliveryBoyRequest
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AcceptDeliveryBoyRequest(Guid id)
        {
            var groupedOrderEvent = db.GroupedOrderEvents.Single(go => go.Id == id);

            return View("AcceptDeliveryBoyRequest", groupedOrderEvent);
        }


        /// <summary>
        /// POST: DeliveryBoy/AskForDeliveryBoy
        /// </summary>
        /// <param name="groupedOrderEvent"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptDeliveryBoyRequest(
            [Bind(Include = "Id,GroupedOrder,LimitDateTime")] GroupedOrderEvent groupedOrderEvent)
        {
            if (ModelState.IsValid)
                return View(groupedOrderEvent);
            return View(groupedOrderEvent);
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
