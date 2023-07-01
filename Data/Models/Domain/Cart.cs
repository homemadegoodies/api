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
    [Table("carts")]
    public class Cart
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
        [Column("cart_products", TypeName = "jsonb")]
        public List<CartProduct>? CartProducts { get; set; } = new List<CartProduct>();
        [Column("total_price")]
        [Required]
        public double TotalPrice { get; set; }
        [Column("created_at")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column("updated_at")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }
    [Table("cart_products")]
    public class CartProduct
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
