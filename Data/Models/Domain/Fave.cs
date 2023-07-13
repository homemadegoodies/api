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
    [Table("faves")]
    public class Fave
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("customer_id")]
        [Required]
        [ForeignKey(nameof(CustomerId))]
        public Guid CustomerId { get; set; }
        [JsonIgnore]
        public Customer? Customer { get; set; }
        [Column("product_id")]
        [Required]
        [ForeignKey(nameof(ProductId))]
        public Guid ProductId { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }
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
