using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPOrder.Models
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsLocked { get; set; }
        public ICollection<OrderLine> OrderLines { get; set; }

    }

    public class OrderLine
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public int OrderedQty { get; set; }
        public int BuyQty { get; set; }
        public Product Product { get; set; }

    }
}