using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPOrder.Models
{
    public partial class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Unit Unit { get; set; }
        public ApplicationUser CreateUser { get; set; }
    }

    public partial class Product
    {
        public override string ToString()
        {
            return string.Format("{0} : {1} €", Name, Price);
        }
    }
}