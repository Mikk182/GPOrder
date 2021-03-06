﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPOrder.Helpers;
using GPOrder.Models;
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

            var newDate = DateTime.UtcNow.AddMinutes(15);

            if (groupedOrder.DeliveryBoy == null)
            {
                groupedOrder.LimitDate = newDate;
                return View("BecomingDeliveryBoy", groupedOrder);
            }

            var groupedOrderEvent = new GroupedOrderEventAskDeliveryBoy
            {
                LimitDateTime = newDate,
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
        public ActionResult BecomingDeliveryBoy([Bind(Include = "Id,CreateUser,CreationDate,LimitDate,LinkedShop")] GroupedOrder groupedOrder)
        {
            var dbGroupedOrder = db.GroupedOrders.Single(go => go.Id == groupedOrder.Id);
            var currentUserId = User.Identity.GetUserId();

            if (dbGroupedOrder.Orders.All(o => o.CreateUser_Id != currentUserId))
            {
                ModelState.AddModelError("DeliveryBoy", "You have to join this order before becoming delivery boy.");
            }

            if (ModelState.IsValid)
            {
                dbGroupedOrder.DeliveryBoy_Id = currentUserId;
                dbGroupedOrder.LimitDate = groupedOrder.LimitDate;

                db.Entry(dbGroupedOrder).State = EntityState.Modified;

                var newGroupedOrderEvent = new GroupedOrderEvent
                {
                    CreateUserId = User.Identity.GetUserId(),
                    CreationDate = DateTime.UtcNow,
                    Description = "{0} became delivery boy by its own.",
                    EventStatus = GroupedOrderEventStatus.Accepted,
                    EventType = EventType.BecomingDeliveryBoy,
                    GroupedOrderId = groupedOrder.Id,
                    Users = dbGroupedOrder.Orders.Select(o => o.CreateUser).ToList()
                };
                db.Entry(newGroupedOrderEvent).State = EntityState.Added;

                db.SaveChanges();

                return RedirectToAction("Index", "GroupedOrders");
            }

            return View(groupedOrder);
        }

        /// <summary>
        /// POST: DeliveryBoy/AskForDeliveryBoy
        /// </summary>
        /// <param name="groupedOrderEventAskDeliveryBoy"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AskForDeliveryBoy([Bind(Include = "Id,GroupedOrderId,GroupedOrder,LimitDateTime")] GroupedOrderEventAskDeliveryBoy groupedOrderEventAskDeliveryBoy)
        {
            var dbGroupedOrder = db.GroupedOrders.Single(go => go.Id == groupedOrderEventAskDeliveryBoy.GroupedOrderId);
            var currentUserId = User.Identity.GetUserId();

            if (dbGroupedOrder.Orders.All(o => o.CreateUser_Id != currentUserId))
            {
                ModelState.AddModelError("DeliveryBoy", "You have to join this order before becoming delivery boy.");
            }
            if (dbGroupedOrder.GroupedOrderEvents.Any(goe =>
                        goe.CreateUserId == currentUserId &&
                        goe.EventStatus == GroupedOrderEventStatus.Submitted))
                ModelState.AddModelError("DeliveryBoy", "You have already submit a request to become delivery boy.");

            if (ModelState.IsValid)
            {
                var currentGroupedOrderDeliveryBoy = db.Users.Single(u => u.Id == dbGroupedOrder.DeliveryBoy_Id);

                groupedOrderEventAskDeliveryBoy.GroupedOrder = null;
                groupedOrderEventAskDeliveryBoy.CreationDate = DateTime.UtcNow;
                groupedOrderEventAskDeliveryBoy.CreateUserId = currentUserId;
                groupedOrderEventAskDeliveryBoy.EventType = EventType.BecomingDeliveryBoy;
                groupedOrderEventAskDeliveryBoy.Users = new List<ApplicationUser> { currentGroupedOrderDeliveryBoy };
                groupedOrderEventAskDeliveryBoy.EventStatus = GroupedOrderEventStatus.Submitted;

                db.Entry(groupedOrderEventAskDeliveryBoy).State = EntityState.Added;
                db.SaveChanges();

                var description = "{0} asked {1} to become delivery boy on {2}. {3}";

                groupedOrderEventAskDeliveryBoy.Description = description;
                db.Entry(groupedOrderEventAskDeliveryBoy).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "GroupedOrders");
            }

            return View(groupedOrderEventAskDeliveryBoy);
        }

        /// <summary>
        /// GET: DeliveryBoy/AcceptDeliveryBoyRequest
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AcceptDeliveryBoyRequest(Guid id)
        {
            var groupedOrderEvent = db.GroupedOrderEventsAskDeliveryBoy.Single(go => go.Id == id);

            return View("AcceptDeliveryBoyRequest", groupedOrderEvent);
        }

        /// <summary>
        /// POST: DeliveryBoy/AcceptDeliveryBoyRequest
        /// </summary>
        /// <param name="groupedOrderEventAskDeliveryBoy"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptDeliveryBoyRequest(
            [Bind(Include = "Id,CreateUserId,CreateUser,GroupedOrderId,GroupedOrder,LimitDateTime")] GroupedOrderEventAskDeliveryBoy groupedOrderEventAskDeliveryBoy)
        {
            if (ModelState.IsValid)
            {
                // Mise à jour de la commande groupée
                var dbGroupedOrder = db.GroupedOrders.Single(go => go.Id == groupedOrderEventAskDeliveryBoy.GroupedOrderId);
                dbGroupedOrder.DeliveryBoy_Id = groupedOrderEventAskDeliveryBoy.CreateUserId;
                dbGroupedOrder.LimitDate = groupedOrderEventAskDeliveryBoy.LimitDateTime;
                db.Entry(dbGroupedOrder).State = EntityState.Modified;

                //Mise à jour de l'event de demande de DeliveryBoy
                var dbGroupedOrderEvent = db.GroupedOrderEvents.Single(goe => goe.Id == groupedOrderEventAskDeliveryBoy.Id);
                dbGroupedOrderEvent.EventStatus = GroupedOrderEventStatus.Accepted;
                db.Entry(dbGroupedOrderEvent).State = EntityState.Modified;

                // Création d'un evenement d'acceptation de la demande
                var newDeliveryBoy = db.Users.Single(u => u.Id == groupedOrderEventAskDeliveryBoy.CreateUserId);
                var newGroupedOrderEvent = new GroupedOrderEvent
                {
                    CreateUserId = User.Identity.GetUserId(),
                    CreationDate = DateTime.UtcNow,
                    Description = "Delivery Boy Request accepted by {0}.",
                    EventStatus = GroupedOrderEventStatus.Accepted,
                    EventType = EventType.AcceptDeliveryBoyRequest,
                    GroupedOrderId = groupedOrderEventAskDeliveryBoy.GroupedOrderId,
                    Users = new List<ApplicationUser> { newDeliveryBoy }
                };
                db.Entry(newGroupedOrderEvent).State = EntityState.Added;

                db.SaveChanges();

                return RedirectToAction("Details", "GroupedOrders",
                    new {Id = groupedOrderEventAskDeliveryBoy.GroupedOrderId});
            }

            return View(groupedOrderEventAskDeliveryBoy);
        }

        /// <summary>
        /// GET: DeliveryBoy/LeaveDeliveryBoy
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult LeaveDeliveryBoy(Guid id)
        {
            var groupedOrder = db.GroupedOrders.Single(go => go.Id == id);

            if (groupedOrder.DeliveryBoy == null)
                throw new Exception("Cannot Leave the delivery boy function, No delivery boy defined.");
            if (groupedOrder.DeliveryBoy_Id != User.Identity.GetUserId())
                throw new Exception("Cannot leave delivery boy function, you are not the current delivery boy");

            return View(groupedOrder);
        }

        /// <summary>
        /// POST: DeliveryBoy/BecomingDeliveryBoy
        /// </summary>
        /// <param name="groupedOrder"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LeaveDeliveryBoy([Bind(Include = "Id")] GroupedOrder groupedOrder)
        {
            if (ModelState.IsValid)
            {
                var dbGroupedOrder = db.GroupedOrders.Single(go => go.Id == groupedOrder.Id);
                dbGroupedOrder.DeliveryBoy_Id = null;
                dbGroupedOrder.DeliveryBoy = null;
                dbGroupedOrder.LimitDate = null;

                db.Entry(dbGroupedOrder).State = EntityState.Modified;

                var leaveDeliveryBoyEvent = new GroupedOrderEvent
                {
                    CreateUserId = User.Identity.GetUserId(),
                    CreationDate = DateTime.UtcNow,
                    Description = "{0} has leave the delivery boy function",
                    EventType = EventType.LeaveDeliveryBoyFunction,
                    Users = dbGroupedOrder.Orders.Select(o => o.CreateUser).ToList(),
                    EventStatus = GroupedOrderEventStatus.Accepted,
                    GroupedOrderId = groupedOrder.Id
                };

                db.Entry(leaveDeliveryBoyEvent).State = EntityState.Added;

                db.SaveChanges();

                return RedirectToAction("Index", "GroupedOrders");
            }

            return View(groupedOrder);
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
