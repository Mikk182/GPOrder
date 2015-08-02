using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using GPOrder.Entities;

public class GPOrderContext : DbContext
{
    //protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //{
    //    modelBuilder.Conventions.Remove<System.Data.Entity.Infrastructure.IncludeMetadataConvention>();
    //}

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    public virtual DbSet<Product> Product { get; set; }
    public virtual DbSet<Unit> Unit { get; set; }
}