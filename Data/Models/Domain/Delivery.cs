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
    [Table("deliveries")]
    public class Delivery
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
        [Column("kitchen_id")]
        [Required]
        [ForeignKey(nameof(KitchenId))]
        public Guid KitchenId { get; set; }
        [JsonIgnore]
        public Kitchen? Kitchen { get; set; }
        [Column("order_id")]
        [Required]
        [ForeignKey(nameof(OrderId))]
        public Guid OrderId { get; set; }
        [JsonIgnore]
        public Order? Order { get; set; }
        [Column("delivery_products", TypeName = "jsonb")]
        public List<DeliveryProduct>? DeliveryProducts { get; set; } = new List<DeliveryProduct>();
        [Column("total_price")]
        [Required]
        public double TotalPrice { get; set; }
        [Column("status")]
        [Required]
        public string Status { get; set; } = "Pending";
        [Column("delivery_date")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; } = DateTime.Now.AddHours(2);
        [Column("created_at")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column("updated_at")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }
    public class DeliveryProduct
    {
        [Column("product_id")]
        [Required]
        [ForeignKey(nameof(ProductId))]
        public Guid ProductId { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }
        [Column("quantity")]
        [Required]
        public int Quantity { get; set; }
    }
}