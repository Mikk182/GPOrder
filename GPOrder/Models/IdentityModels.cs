using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using GPOrder.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GPOrder.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string UiCulture { get; set; }

        public string TimeZone { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            userIdentity.AddClaim(new Claim("UiCulture", UiCulture));
            userIdentity.AddClaim(new Claim("TimeZone", TimeZone));

            return userIdentity;
        }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Group> CreatedGroups { get; set; }
        public virtual ICollection<Group> OwnedGroups { get; set; }
        public virtual ICollection<Group> LinkedGroups { get; set; }

        public virtual ICollection<Shop> CreatedShop { get; set; }
        public virtual ICollection<Shop> OwnedShop { get; set; }
        public virtual ICollection<ShopPicture> PostedPictures { get; set; }

        public virtual ICollection<Bill> CreatedBills { get; set; }
        public virtual ICollection<BillPicture> BillPictures { get; set; }
        public virtual ICollection<BillEvent> DebitBillEvents { get; set; }
        public virtual ICollection<BillEvent> CreditBillEvents { get; set; }

        public virtual ICollection<File> Files { get; set; }

        public virtual ICollection<GroupedOrder> CreateUserGroupedOrders { get; set; }
        public virtual ICollection<GroupedOrder> DeliveryBoyGroupedOrders { get; set; }

        public virtual ICollection<Order> CreateUserOrders { get; set; }

        public virtual ICollection<Event> CreateUserEvent { get; set; }
        public virtual ICollection<Event> ConcernedUserByEvent { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            //Database.SetInitializer<ApplicationDbContext>(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());

            Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupedOrder>()
                .HasKey(l => l.Id)
                .HasRequired(go => go.CreateUser)
                .WithMany(o => o.CreateUserGroupedOrders).HasForeignKey(o => o.CreateUser_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<GroupedOrder>()
                .HasOptional(go => go.DeliveryBoy)
                .WithMany(o => o.DeliveryBoyGroupedOrders).HasForeignKey(go => go.DeliveryBoy_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<GroupedOrder>()
                .HasMany(go => go.Orders)
                .WithRequired(o => o.GroupedOrder).WillCascadeOnDelete(true);
            modelBuilder.Entity<GroupedOrder>()
                .HasRequired(go => go.LinkedShop)
                .WithMany(o => o.GroupedOrders).HasForeignKey(go => go.LinkedShop_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<GroupedOrder>()
                .HasMany(go => go.GroupedOrderEvents)
                .WithRequired(goe => goe.GroupedOrder).HasForeignKey(goe => goe.GroupedOrderId).WillCascadeOnDelete(true);
            modelBuilder.Entity<GroupedOrder>()
                .HasOptional(go => go.LinkedBill)
                .WithRequired(b => b.GroupedOrder).WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasKey(l => l.Id)
                .HasRequired(o => o.CreateUser)
                .WithMany(u => u.CreateUserOrders).HasForeignKey(o => o.CreateUser_Id);
            modelBuilder.Entity<Order>()
                .HasRequired(o => o.GroupedOrder)
                .WithMany(u => u.Orders).HasForeignKey(o => o.GroupedOrder_Id);
            modelBuilder.Entity<Order>()
                .HasMany(go => go.BillEvents)
                .WithRequired(goe => goe.Order).HasForeignKey(goe => goe.Order_Id).WillCascadeOnDelete(true);

            modelBuilder.Entity<OrderLine>()
                .HasKey(l => l.Id)
                .HasRequired(ol => ol.Order)
                .WithMany(ol => ol.OrderLines)
                .HasForeignKey(ol => ol.Order_Id);

            modelBuilder.Entity<Product>()
                .HasKey(l => l.Id)
                .HasRequired(c => c.CreateUser).WithMany(u => u.Products);

            modelBuilder.Entity<Group>()
                .HasKey(l => l.Id)
                .HasRequired(c => c.CreateUser)
                .WithMany(u => u.CreatedGroups)
                .HasForeignKey(u => u.CreateUserId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Group>()
                .HasRequired(c => c.OwnerUser)
                .WithMany(u => u.OwnedGroups)
                .HasForeignKey(u => u.OwnerUserId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Group>()
                .HasMany(u => u.ApplicationUsers)
                .WithMany(u => u.LinkedGroups);

            modelBuilder.Entity<Shop>()
                .HasKey(l => l.Id)
                .HasRequired(c => c.CreateUser)
                .WithMany(u => u.CreatedShop)
                .HasForeignKey(u => u.CreateUserId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Shop>()
                .HasRequired(c => c.OwnerUser)
                .WithMany(u => u.OwnedShop)
                .HasForeignKey(u => u.OwnerUserId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Shop>()
                .HasMany(s => s.GroupedOrders)
                .WithRequired(go => go.LinkedShop);

            modelBuilder.Entity<ShopPicture>()
                .HasKey(sp => sp.Id)
                .HasRequired(c => c.CreateUser)
                .WithMany(u => u.PostedPictures)
                .HasForeignKey(u => u.CreateUserId).WillCascadeOnDelete(false);
            modelBuilder.Entity<ShopPicture>()
                .HasKey(sp => sp.Id)
                .HasRequired(c => c.Shop)
                .WithMany(u => u.ShopPictures)
                .HasForeignKey(u => u.ShopId).WillCascadeOnDelete(true);
            modelBuilder.Entity<ShopPicture>()
                .HasRequired(sp => sp.LinkedFile)
                .WithOptional(lf => lf.ShopPicture).WillCascadeOnDelete(true);

            modelBuilder.Entity<ShopLink>()
                .HasKey(sl => sl.Id)
                .HasRequired(c => c.Shop)
                .WithMany(u => u.ShopLinks)
                .HasForeignKey(u => u.ShopId).WillCascadeOnDelete(true);

            modelBuilder.Entity<File>()
                .HasKey(f => f.Id);
            modelBuilder.Entity<File>()
                .HasOptional(f => f.ShopPicture)
                .WithRequired(sp => sp.LinkedFile);
            modelBuilder.Entity<File>()
                .HasOptional(f => f.BillPicture)
                .WithRequired(sp => sp.LinkedFile);

            modelBuilder.Entity<Event>()
                .HasKey(e => e.Id)
                .ToTable("Events");
            modelBuilder.Entity<Event>()
                .HasRequired(e => e.CreateUser)
                .WithMany(u => u.CreateUserEvent).HasForeignKey(e => e.CreateUserId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Users)
                .WithMany(u => u.ConcernedUserByEvent);

            modelBuilder.Entity<GroupedOrderEvent>()
                .HasKey(goe => goe.Id)
                .ToTable("GroupedOrderEvents");
            modelBuilder.Entity<GroupedOrderEvent>()
                .HasRequired(goe => goe.GroupedOrder)
                .WithMany(go => go.GroupedOrderEvents).HasForeignKey(goe => goe.GroupedOrderId).WillCascadeOnDelete(true);
            modelBuilder.Entity<GroupedOrderEventAskDeliveryBoy>()
                .HasKey(goe => goe.Id)
                .ToTable("GroupedOrderEventsAskDeliveryBoy");

            #region Bill

            modelBuilder.Entity<Bill>()
                .HasKey(l => l.Id)
                .HasRequired(c => c.CreateUser)
                .WithMany(u => u.CreatedBills)
                .HasForeignKey(u => u.CreateUser_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<Bill>()
                .HasRequired(c => c.GroupedOrder)
                .WithOptional(go => go.LinkedBill).WillCascadeOnDelete(false);
            modelBuilder.Entity<Bill>()
                .HasMany(b => b.BillPictures)
                .WithRequired(be => be.Bill).HasForeignKey(be => be.Bill_Id).WillCascadeOnDelete(true);
            modelBuilder.Entity<Bill>()
                .HasMany(b => b.BillEvents)
                .WithRequired(be => be.Bill).HasForeignKey(be => be.Bill_Id).WillCascadeOnDelete(true);

            modelBuilder.Entity<BillPicture>()
                .HasKey(sp => sp.Id)
                .HasRequired(c => c.CreateUser)
                .WithMany(u => u.BillPictures)
                .HasForeignKey(u => u.CreateUser_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<BillPicture>()
                .HasKey(sp => sp.Id)
                .HasRequired(c => c.Bill)
                .WithMany(u => u.BillPictures)
                .HasForeignKey(u => u.Bill_Id).WillCascadeOnDelete(true);
            modelBuilder.Entity<BillPicture>()
                .HasRequired(sp => sp.LinkedFile)
                .WithOptional(lf => lf.BillPicture).WillCascadeOnDelete(true);

            modelBuilder.Entity<BillEvent>()
                .HasKey(goe => goe.Id)
                .ToTable("BillEvents");
            modelBuilder.Entity<BillEvent>()
                .HasRequired(be => be.DebitUser)
                .WithMany(du => du.DebitBillEvents).HasForeignKey(be => be.DebitUser_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<BillEvent>()
                .HasRequired(be => be.CreditUser)
                .WithMany(du => du.CreditBillEvents).HasForeignKey(be => be.CreditUser_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<BillEvent>()
                .HasRequired(goe => goe.Order)
                .WithMany(go => go.BillEvents).HasForeignKey(goe => goe.Order_Id).WillCascadeOnDelete(true);
            modelBuilder.Entity<BillEvent>()
                .HasRequired(goe => goe.Bill)
                .WithMany(go => go.BillEvents).HasForeignKey(goe => goe.Bill_Id).WillCascadeOnDelete(true);

            #endregion

            modelBuilder.Entity<ApplicationUser>().
                HasMany(au => au.LinkedGroups)
                .WithMany(au => au.ApplicationUsers);
            modelBuilder.Entity<ApplicationUser>().
                HasMany(au => au.PostedPictures)
                .WithRequired(au => au.CreateUser);
            modelBuilder.Entity<ApplicationUser>().
                HasMany(au => au.CreateUserGroupedOrders)
                .WithRequired(au => au.CreateUser);
            modelBuilder.Entity<ApplicationUser>().
                HasMany(au => au.DeliveryBoyGroupedOrders)
                .WithOptional(au => au.DeliveryBoy);
            modelBuilder.Entity<ApplicationUser>().
                HasMany(au => au.CreateUserOrders)
                .WithRequired(au => au.CreateUser);
            modelBuilder.Entity<ApplicationUser>().
                HasMany(au => au.CreateUserEvent)
                .WithRequired(au => au.CreateUser);
            modelBuilder.Entity<ApplicationUser>().
                HasMany(au => au.ConcernedUserByEvent)
                .WithMany(e => e.Users);

            modelBuilder.Entity<ApplicationUser>().
                HasMany(u => u.CreatedBills)
                .WithRequired(b => b.CreateUser)
                .HasForeignKey(u => u.CreateUser_Id);
            modelBuilder.Entity<ApplicationUser>().
                HasMany(u => u.BillPictures)
                .WithRequired(b => b.CreateUser).HasForeignKey(bp => bp.CreateUser_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<ApplicationUser>().
                HasMany(au => au.DebitBillEvents)
                .WithRequired(au => au.DebitUser)
                .HasForeignKey(due => due.DebitUser_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<ApplicationUser>().
                HasMany(au => au.CreditBillEvents)
                .WithRequired(au => au.CreditUser)
                .HasForeignKey(due => due.CreditUser_Id).WillCascadeOnDelete(false);

            modelBuilder.Entity<IdentityUserLogin>().HasKey(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

        }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<GroupedOrder> GroupedOrders { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderLine> OrderLines { get; set; }

        public DbSet<Shop> Shops { get; set; }

        public DbSet<ShopPicture> ShopPictures { get; set; }

        public DbSet<ShopLink> ShopLinks { get; set; }

        public DbSet<File> Files { get; set; }

        public DbSet<Event> Events { get; set; }
        public DbSet<GroupedOrderEvent> GroupedOrderEvents { get; set; }
        public DbSet<GroupedOrderEventAskDeliveryBoy> GroupedOrderEventsAskDeliveryBoy { get; set; }

        public System.Data.Entity.DbSet<GPOrder.Models.Bill> Bills { get; set; }

        public System.Data.Entity.DbSet<GPOrder.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}