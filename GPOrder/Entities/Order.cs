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
        //public Guid CreateUserId { get; set; }
        public ApplicationUser CreateUser { get; set; }
        //public Guid? DeliveryBoyId { get; set; }
        public ApplicationUser DeliveryBoy { get; set; }
        [DataType(DataType.Date)]
        public DateTime? LimitDate { get; set; }
        public bool IsLocked { get; set; }
        //public Guid? LinkedShopyId { get; set; }
        public Shop LinkedShop { get; set; }

        public ICollection<Order> Orders { get; set; }
    }

    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        [Required]
        public ApplicationUser CreateUser { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        public bool IsLocked { get; set; }
        [Required]
        public decimal EstimatedPrice { get; set; }
        public decimal? RealPrice { get; set; }

        public ICollection<OrderLine> OrderLines { get; set; }

        public GroupedOrder GroupedOrder { get; set; }
    }
    
    public class OrderLine
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [StringLength(100, MinimumLength = 3)]
        public string Description { get; set; }
    }
}