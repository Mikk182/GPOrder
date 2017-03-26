using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GPOrder.Entities;

namespace GPOrder.Models
{
    public class Shop
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }

        public string CreateUserId { get; set; }
        public string OwnerUserId { get; set; }
        public ApplicationUser CreateUser { get; set; }
        public ApplicationUser OwnerUser { get; set; }

        public bool IsLocked { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Adress { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        [EmailAddress]
        public string Mail { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Description { get; set; }

        public ICollection<ShopPicture> ShopPictures { get; set; }
    }

    public class ShopPicture
    {
        [Key]
        public Guid Id { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }

        public string CreateUserId { get; set; }
        public ApplicationUser CreateUser { get; set; }

        public bool IsLocked { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        /// <summary>
        /// JPG, GIF, BMP, PNG ...
        /// </summary>
        public File LinkedFile { get; set; }

        public Guid ShopId { get; set; }
        public Shop Shop { get; set; }
    }
}