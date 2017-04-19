using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GPOrder.Models;
using GPOrder.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GPOrder.Controllers
{
    [Authorize]
    public class GroupedOrdersController : Controller
    {
        private ApplicationUserManager _userManager;

        private ApplicationDbContext db = new ApplicationDbContext();

        public GroupedOrdersController()
        {
        }

        public GroupedOrdersController(ApplicationUserManager userManager)
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

        // GET: GroupedOrders
        public ActionResult Index(Guid? shopId)
        {
            var currentUserId = User.Identity.GetUserId();

            var currentUser = db.Users.Single(u => u.Id == currentUserId);

            var usersInMyGroups = currentUser.LinkedGroups
                .SelectMany(g => g.ApplicationUsers.Select(u => u.Id))
                .Distinct();

            var groupedOrders = db.GroupedOrders
                .Where(go => usersInMyGroups.Contains(go.CreateUser.Id)
                    || go.CreateUser.Id == currentUserId
                    || go.Orders.Any(o => o.CreateUser_Id == currentUserId));

            if (shopId.HasValue)
                groupedOrders = groupedOrders.Where(go => go.LinkedShop.Id == shopId);

            var shopGroupedOrdersViewModel = new ShopGroupedOrders
            {
                ShopId = shopId,
                GroupOrders = groupedOrders
            };

            return View(shopGroupedOrdersViewModel);
        }

        // GET: GroupedOrders/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var groupedOrder = db.GroupedOrders.Single(o => o.Id == id);
            if (groupedOrder == null)
            {
                return HttpNotFound();
            }
            return View(groupedOrder);
        }

        public PartialViewResult GetGroupedOrderEvents(Guid groupedOrderId)
        {
            var groupedOrderEvents = db.GroupedOrderEvents.Where(e =>
                e.GroupedOrderId == groupedOrderId);
            return PartialView(groupedOrderEvents);
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
