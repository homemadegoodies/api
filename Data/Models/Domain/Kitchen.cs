using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Data.Models.Domain
{
    [Table("kitchens")]
    public class Kitchen
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("vendor_id")]
        [Required]
        [ForeignKey(nameof(VendorId))]
        public Guid VendorId { get; set; }
        [JsonIgnore]
        public Vendor? Vendor { get; set; }
        [Column("name")]
        [Required]
        public string Name { get; set; } = string.Empty;
        [Column("description")]
        [Required]
        public string Description { get; set; } = string.Empty;
        [Column("image_url")]
        [Required]
        public string ImageURL { get; set; } = string.Empty;
        [Column("category")]
        [Required]
        public string Category { get; set; } = string.Empty;
        [Column("created_at")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column("updated_at")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }
}
