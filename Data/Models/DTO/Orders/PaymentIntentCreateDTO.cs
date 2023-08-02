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
    public class PaymentIntentCreateDTO
    {
        [JsonPropertyName("amount")]
        [Required]
        public double Amount { get; set; }
    }
}
