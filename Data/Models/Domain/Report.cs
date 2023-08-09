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
    [Table("reports")]
    public class Report
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("customer_id")]
        [ForeignKey(nameof(CustomerId))]
        public Guid CustomerId { get; set; }
        [JsonIgnore]
        public Customer? Customer { get; set; }
        [Column("vendor_id")]
        [ForeignKey(nameof(VendorId))]
        public Guid VendorId { get; set; }
        [JsonIgnore]
        public Vendor? Vendor { get; set; }
        [Column("reason")]
        [Required, MinLength(2), MaxLength(500), Display(Name = "Reason")]
        public string Reason { get; set; } = string.Empty;
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