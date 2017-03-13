using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPOrder.Models
{
    public partial class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal Price { get; set; }

        public Unit Unit { get; set; }

        public ApplicationUser CreateUser { get; set; }
    }

    public partial class Product
    {
        public override string ToString()
        {
            return string.Format("{0} : {1} €", Name, Price);
        }
    }
}