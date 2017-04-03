using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GPOrder.Models;

namespace GPOrder.Entities
{
    public enum EventType
    {
        NoType = 1 << 0,
        BecomingDeliveryBoy = 1 << 1
    }

    public class Event
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }

        public string CreateUserId { get; set; }
        public virtual ApplicationUser CreateUser { get; set; }

        public EventType EventType { get; set; }

        public string Description { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

        public virtual GroupedOrderEvent GroupedOrderEvent { get; set; }
    }

    public class GroupedOrderEvent
    {
        [Key]
        public Guid Id { get; set; }

        public virtual Event Event {get; set; }

        public DateTime LimitDateTime { get; set; }

        public Guid GroupedOrder_Id { get; set; }
        public virtual GroupedOrder GroupedOrder { get; set; }
    }
}