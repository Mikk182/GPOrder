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
using Microsoft.AspNet.Identity.Owin;
using EntityState = System.Data.Entity.EntityState;

namespace GPOrder.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private ApplicationUserManager _userManager;

        private ApplicationDbContext db = new ApplicationDbContext();

        public OrdersController()
        {
        }

        public OrdersController(ApplicationUserManager userManager)
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

        // GET: Orders
        public ActionResult Index()
        {
            return View(db.Orders.Include(o => o.OrderLines).Include(o => o.OrderLines).ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Orders.Include(o => o.OrderLines).Single(o => o.Id == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        public ActionResult ChooseShop()
        {
            var shops = db.Shops.Include(s => s.CreateUser).Include(s => s.OwnerUser);
            return View(shops);
        }

        // GET: Orders/Create
        public ActionResult Create(Guid? shopId, Guid? groupedOrderId)
        {
            if (!shopId.HasValue)
                return RedirectToAction("ChooseShop");

            var currentUser = UserManager.FindById(User.Identity.GetUserId());

            GroupedOrder groupedOrder;
            if (groupedOrderId.HasValue)
            {
                groupedOrder = db.GroupedOrders.Include(go => go.LinkedShop).Single(go => go.Id == groupedOrderId.Value);
            }
            else
            {
                groupedOrder = new GroupedOrder
                {
                    LinkedShop = db.Shops.Single(s => s.Id == shopId)
                };
            }

            var order = new Order
            {
                CreationDate = DateTime.Now,
                CreateUser = currentUser,
                OrderDate = DateTime.Now,
                GroupedOrder = groupedOrder,
                OrderLines = new List<OrderLine> { new OrderLine() }
            };

            return View(order);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CreationDate,OrderDate,EstimatedPrice,IsLocked,CreateUser,GroupedOrder,OrderLines")] Order order)
        {
            if (ModelState.IsValid)
            {
                var createUser = db.Users.Single(u => u.Id == order.CreateUser.Id);
                order.CreateUser = createUser;
                if (order.GroupedOrder.Id != Guid.Empty)
                {
                    order.GroupedOrder = db.GroupedOrders.Include(go => go.LinkedShop).Single(go => go.Id == order.GroupedOrder.Id);
                }
                else
                {
                    order.GroupedOrder = new GroupedOrder
                    {
                        CreateUser = createUser,
                        CreationDate = order.CreationDate,
                        LinkedShop = db.Shops.Single(s => s.Id == order.GroupedOrder.LinkedShop.Id)
                    };
                }
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index", "GroupedOrders", new { shopId = order.GroupedOrder.LinkedShop.Id });
            }

            return View(order);
        }

        public PartialViewResult GetNewOrderLine()
        {
            return PartialView("EditorTemplates/OrderLine", new OrderLine());
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Orders.Include(o => o.CreateUser).Include(o => o.GroupedOrder.LinkedShop).Include(o => o.OrderLines).Single(o => o.Id == id);
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
        public ActionResult Edit([Bind(Include = "Id,CreationDate,OrderDate,EstimatedPrice,IsLocked,CreateUser,GroupedOrder,OrderLines")] Order order)
        {
            if (ModelState.IsValid)
            {
                if (order.OrderLines == null)
                    order.OrderLines = new List<OrderLine>();

                var dbOrder = db.Orders.Include(o => o.CreateUser).Include(o => o.GroupedOrder).Include(o => o.OrderLines).Single(o => o.Id == order.Id);
                if (dbOrder.OrderLines == null)
                    dbOrder.OrderLines = new List<OrderLine>();

                dbOrder.OrderDate = order.OrderDate;
                dbOrder.EstimatedPrice = order.EstimatedPrice;

                var orderLinesToDelete = dbOrder.OrderLines != null ?
                    dbOrder.OrderLines.Where(dbol => !order.OrderLines.Select(ol => ol.Id).Contains(dbol.Id)) : new List<OrderLine>();
                db.OrderLines.RemoveRange(orderLinesToDelete);

                foreach (var orderLine in order.OrderLines.Where(ol => ol.Id != Guid.Empty))
                {
                    var dbOrderLine = dbOrder.OrderLines.Single(ol => ol.Id == orderLine.Id);
                    dbOrderLine.Description = orderLine.Description;
                    db.Entry(dbOrderLine).State = EntityState.Modified;
                }

                foreach (var orderLine in order.OrderLines.Where(ol => ol.Id == Guid.Empty))
                {
                    //db.Entry(orderLine).State = EntityState.Added;
                    dbOrder.OrderLines.Add(orderLine);
                }
                try
                {
                    db.Entry(dbOrder).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    throw e;
                }

                return RedirectToAction("Details", "GroupedOrders", new { order.GroupedOrder.Id });
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
            Order order = db.Orders.Include(o => o.GroupedOrder.Orders).Single(o => o.Id == id);
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
            var groupedOrderIsRemoved = false;
            var order = db.Orders.Include(o => o.GroupedOrder.Orders).Single(o => o.Id == id);
            if (order.GroupedOrder?.Orders != null && order.GroupedOrder.Orders.Count == 1)
            {
                db.GroupedOrders.Remove(order.GroupedOrder);
                groupedOrderIsRemoved = true;
            }
            db.Orders.Remove(order);
            db.SaveChanges();

            return !groupedOrderIsRemoved ? RedirectToAction("Details", "GroupedOrders", new {order.GroupedOrder_Id}) : RedirectToAction("Index","Home");
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
