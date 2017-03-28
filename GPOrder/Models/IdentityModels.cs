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

        public virtual ICollection<Shop> CreatedShop { get; set; }
        public virtual ICollection<Shop> OwnedShop { get; set; }
        public virtual ICollection<ShopPicture> PostedPictures { get; set; }

        public virtual ICollection<File> Files { get; set; }

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

            modelBuilder.Entity<Shop>()
                .HasKey(l => l.Id)
                .HasRequired(c => c.CreateUser)
                .WithMany(u => u.CreatedShop)
                .HasForeignKey(u => u.CreateUserId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Shop>()
                .HasRequired(c => c.OwnerUser)
                .WithMany(u => u.OwnedShop)
                .HasForeignKey(u => u.OwnerUserId).WillCascadeOnDelete(false);

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

            modelBuilder.Entity<ApplicationUser>().
                HasMany(au => au.LinkedGroups)
                .WithMany(au => au.ApplicationUsers);
            modelBuilder.Entity<ApplicationUser>().
                HasMany(au => au.PostedPictures)
                .WithRequired(au => au.CreateUser);

            modelBuilder.Entity<IdentityUserLogin>().HasKey(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

        }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderLine> OrderLines { get; set; }

        public DbSet<Shop> Shops { get; set; }

        public DbSet<ShopPicture> ShopPictures { get; set; }

        public DbSet<ShopLink> ShopLinks { get; set; }

        public DbSet<File> Files { get; set; }
    }
}