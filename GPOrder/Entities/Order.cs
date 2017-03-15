using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPOrder.Models
{
    public partial class Order
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

    public partial class Order : IValidatableObject
    {
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            List<ValidationResult> res = new List<ValidationResult>();
            if (Date < OrderDate.Date)
            {
                ValidationResult mss = new ValidationResult("Vous ne pouvez commander pour un jour précédent.");
                res.Add(mss);

            }
            return res;
        }
    }

    public class OrderLine
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Range(1, int.MaxValue)]
        public int OrderedQty { get; set; }
        [Range(0, int.MaxValue)]
        public int BuyQty { get; set; }
        public Product Product { get; set; }

    }
}