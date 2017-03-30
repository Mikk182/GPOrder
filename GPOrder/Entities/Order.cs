using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPOrder.Models
{
    public class GroupedOrder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        public string CreateUser_Id { get; set; }
        public virtual ApplicationUser CreateUser { get; set; }

        public string DeliveryBoy_Id { get; set; }
        public virtual ApplicationUser DeliveryBoy { get; set; }

        [DataType(DataType.Date)]
        public DateTime? LimitDate { get; set; }
        public bool IsLocked { get; set; }

        public Guid? LinkedShop_Id { get; set; }
        public virtual Shop LinkedShop { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }

    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        public string CreateUser_Id { get; set; }
        [Required]
        public virtual ApplicationUser CreateUser { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        public bool IsLocked { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal EstimatedPrice { get; set; }
        [Range(0, double.MaxValue)]
        public decimal? RealPrice { get; set; }

        public virtual ICollection<OrderLine> OrderLines { get; set; }

        public Guid GroupedOrder_Id { get; set; }
        public virtual GroupedOrder GroupedOrder { get; set; }
    }

    public class OrderLine
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Description { get; set; }

        public Guid Order_Id { get; set; }
        public virtual Order Order { get; set; }
    }
}