using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPOrder.Models
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
    }

    public enum GroupedOrderEventStatus
    {
        Submitted = 1 << 0,
        Accepted = 1 << 1,
        Refused = 1 << 2
    }

    public class GroupedOrderEvent : Event
    {
        public GroupedOrderEventStatus EventStatus { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime LimitDateTime { get; set; }

        public Guid GroupedOrderId { get; set; }
        public virtual GroupedOrder GroupedOrder { get; set; }
    }
}