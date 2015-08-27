using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPOrder.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsLocked { get; set; }
        public IEnumerable<OrderLine> OrderLines { get; set; }

    }

    public class OrderLine
    {
        public Guid Id { get; set; }
        public int OrderedQty { get; set; }
        public int BuyQty { get; set; }
        public decimal? Weight { get; set; }
        public Product Product { get; set; }

    }
}