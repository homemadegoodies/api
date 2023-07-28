using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Data.Models.Domain;

namespace Data.Models.DTO
{
    public class OrderCreateDTO
    {
        [JsonPropertyName("customerId")]
        [Required]
        public Guid CustomerId { get; set; }
        [JsonPropertyName("vendorId")]
        public Guid VendorId { get; set; }
        [JsonPropertyName("kitchenId")]
        [Required]
        public Guid KitchenId { get; set; }
        [JsonPropertyName("totalPrice")]
        [Required]
        public double TotalPrice { get; set; }
    }
}
