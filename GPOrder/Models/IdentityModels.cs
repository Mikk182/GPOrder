using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GPOrder.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Group> CreatedGroups { get; set; }
        public virtual ICollection<Group> OwnedGroups { get; set; }
        public virtual ICollection<Group> LinkedGroups { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            //Database.SetInitializer<ApplicationDbContext>(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());

            Database.SetInitializer<ApplicationDbContext>(new CreateDatabaseIfNotExists<ApplicationDbContext>());

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasKey(l => l.Id);

            modelBuilder.Entity<OrderLine>()
                .HasKey(l => l.Id);

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

            modelBuilder.Entity<ApplicationUser>().
                HasMany(au => au.LinkedGroups)
                .WithMany(au => au.ApplicationUsers);

            modelBuilder.Entity<IdentityUserLogin>().HasKey(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

        }

        public System.Data.Entity.DbSet<GPOrder.Models.Group> Groups { get; set; }

        public System.Data.Entity.DbSet<GPOrder.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<GPOrder.Models.Order> Orders { get; set; }

        public System.Data.Entity.DbSet<GPOrder.Models.OrderLine> OrderLines { get; set; }
    }
}