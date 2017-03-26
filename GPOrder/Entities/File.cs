using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using GPOrder.Models;

namespace GPOrder.Entities
{
    public enum FileType
    {
        UserAvatar = 1 << 0,
        ShopPicture = 1 << 1
    }

    public class File
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        public FileType FileType { get; set; }

        public ShopPicture ShopPicture { get; set; }
    }
}