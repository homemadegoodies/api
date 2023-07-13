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
    [Table("orders")]
    public class Order
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("customer_id")]
        [Required]
        [ForeignKey(nameof(CustomerId))]
        public Guid CustomerId { get; set; }
        [Column("kitchen_id")]
        [Required]
        [ForeignKey(nameof(KitchenId))]
        public Guid KitchenId { get; set; }
        [JsonIgnore]
        public Customer? Customer { get; set; }
        [Column("order_products", TypeName = "jsonb")]
        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
        [Column("payment_details", TypeName = "jsonb")]
        public PaymentDetail? PaymentDetails { get; set; }
        [Column("total_price")]
        [Required]
        public double TotalPrice { get; set; }
        [Column("status")]
        [Required]
        public string Status { get; set; } = "Pending";
        [Column("created_at")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column("updated_at")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }
    public class OrderProduct
    {
        [JsonPropertyName("productId")]
        [Required]
        [ForeignKey(nameof(ProductId))]
        public Guid ProductId { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }
        [JsonPropertyName("quantity")]
        [Required]
        public int Quantity { get; set; }
    }
    public class PaymentDetail
    {
        [JsonPropertyName("paymentMethod")]
        [Required]
        public string PaymentMethod { get; set; }
        [JsonPropertyName("paymentStatus")]
        [Required]
        public string PaymentStatus { get; set; }
        [JsonPropertyName("paymentAmount")]
        [Required]
        public double PaymentAmount { get; set; }
        [JsonPropertyName("paymentDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PaymentDate { get; set; } = DateTime.Now;
    }
}
