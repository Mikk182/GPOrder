using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPOrder.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GPOrder.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationUserManager _userManager;

        private ApplicationDbContext db = new ApplicationDbContext();

        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager)
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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public PartialViewResult MyEvents()
        {
            var currentUserId = User.Identity.GetUserId();
            
            return PartialView(GetUserEvents(currentUserId));
        }

        private IEnumerable<IEvent> GetUserEvents(string userId)
        {
            IEnumerable<IEvent> eventList = new List<IEvent>();

            var groupedOrderEvents = db.GroupedOrderEvents.Where(e => e.Users.Any(u => u.Id == userId));
            eventList = eventList.Concat(groupedOrderEvents);

            return eventList;
        }
    }
}