using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPOrder.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Unit Unit { get; set; }
    }
}