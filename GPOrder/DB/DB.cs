namespace GPOrder.DB
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DB : DbContext
    {
        // Your context has been configured to use a 'DB' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'GPOrder.DB.DB' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'DB' 
        // connection string in the application configuration file.
        public DB()
            : base("name=DB")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Unit Unit { get; set; }
    }

    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}