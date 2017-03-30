using System;
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
                    || go.CreateUser.Id == currentUserId);

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
            var groupedOrder = db.GroupedOrders.Include(o => o.CreateUser).Include(o => o.DeliveryBoy).Include(o => o.LinkedShop).Include(o => o.Orders).Single(o => o.Id == id);
            if (groupedOrder == null)
            {
                return HttpNotFound();
            }
            return View(groupedOrder);
        }

        //// GET: Orders/Create
        //public ActionResult Create(Guid shopId, Guid? groupedOrderId)
        //{
        //    var currentUser = UserManager.FindById(User.Identity.GetUserId());

        //    GroupedOrder groupedOrder;
        //    if (groupedOrderId.HasValue)
        //    {
        //        groupedOrder = new GroupedOrder
        //        {
        //            Id = groupedOrderId.Value
        //        };
        //    }
        //    else
        //    {
        //        groupedOrder = new GroupedOrder
        //        {
        //            LinkedShop = new Shop { Id = shopId }
        //        };
        //    }
        //    var order = new Order
        //    {
        //        CreationDate = DateTime.Now,
        //        CreateUser = currentUser,
        //        OrderDate = DateTime.Now,
        //        GroupedOrder = groupedOrder,
        //        OrderLines = new List<OrderLine> { new OrderLine() }
        //    };
        //    return View(order);
        //}

        //// POST: Orders/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,CreationDate,OrderDate,IsLocked,CreateUser,GroupedOrder,OrderLines")] Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var createUser = db.Users.Single(u => u.Id == order.CreateUser.Id);
        //        order.CreateUser = createUser;
        //        if (order.GroupedOrder.Id != Guid.Empty)
        //        {
        //            order.GroupedOrder = db.GroupedOrders.Single(go => go.Id == order.GroupedOrder.Id);
        //        }
        //        else
        //        {
        //            order.GroupedOrder = new GroupedOrder
        //            {
        //                CreateUser = createUser,
        //                CreationDate = order.CreationDate,
        //                LinkedShop = db.Shops.Single(s => s.Id == order.GroupedOrder.LinkedShop.Id)
        //            };
        //        }
        //        db.Orders.Add(order);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(order);
        //}

        //public PartialViewResult GetNewOrderLine()
        //{
        //    return PartialView("EditorTemplates/OrderLine", new OrderLine());
        //}

        //public ActionResult GetProductsNames(string term)
        //{
        //    var result = db.Products
        //        .Where(c => c.Name.StartsWith(term))
        //        .Take(10)
        //        .Select(c => new
        //        {
        //            id = c.Id,
        //            value = c.Name,
        //            label = c.Name
        //        })
        //        .ToList();
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        //// GET: Orders/Edit/5
        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var order = db.Orders.Include(o => o.OrderLines).Single(o => o.Id == id);
        //    if (order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(order);
        //}

        //// POST: Orders/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Date,OrderDate,IsLocked,OrderLines")] Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(order).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(order);
        //}

        //// GET: Orders/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Order order = db.Orders.Find(id);
        //    if (order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(order);
        //}

        //// POST: Orders/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    Order order = db.Orders.Find(id);
        //    db.Orders.Remove(order);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
