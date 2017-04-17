using GPOrder.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPOrder.Models
{
    public class Bill
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        
        public string CreateUser_Id { get; set; }
        public virtual ApplicationUser CreateUser { get; set; }

        public Guid GroupedOrder_Id { get; set; }
        public virtual GroupedOrder GroupedOrder { get; set; }

        public ICollection<BillPicture> BillPictures { get; set; }

        public ICollection<BillEvent> BillEvents { get; set; }
    }

    public class BillPicture
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        public string CreateUser_Id { get; set; }
        public virtual ApplicationUser CreateUser { get; set; }
        
        public bool IsLocked { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
        
        /// <summary>
        /// JPG, GIF, BMP, PNG ...
        /// </summary>
        public File LinkedFile { get; set; }

        public Guid? Bill_Id { get; set; }
        public virtual Bill Bill { get; set; }
    }

    public class BillEvent : Event, IEvent
    {
        public decimal Amount { get; set; }

        public string DebitUser_Id { get; set; }
        public ApplicationUser DebitUser { get; set; }

        public string CreditUser_Id { get; set; }
        public ApplicationUser CreditUser { get; set; }

        public Guid? Order_Id { get; set; }
        public Order Order { get; set; }
        
        public Guid? Bill_Id { get; set; }
        public virtual Bill Bill { get; set; }
    }
}