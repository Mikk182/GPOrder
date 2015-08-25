using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPOrder.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Unit Unit { get; set; }
        public ApplicationUser CreateUser { get; set; }
    }
}