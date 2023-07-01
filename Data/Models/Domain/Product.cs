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
    [Table("products")]
    public class Product
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("kitchen_id")]
        [Required]
        [ForeignKey(nameof(KitchenId))]
        public Guid KitchenId { get; set; }
        [JsonIgnore]
        public Kitchen? Kitchen { get; set; }
        [Column("name")]
        [Required, MinLength(2), MaxLength(100), Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;
        [Column("description")]
        [Required, MinLength(2), MaxLength(100), Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;
        [Column("price")]
        [Required]
        public double Price { get; set; }
        [Column("image_url")]
        [Required, MinLength(2), MaxLength(100), Display(Name = "VideoURL")]
        public string ImageURL { get; set; } = string.Empty;
        [Column("recipe", TypeName = "jsonb")]
        public List<Step> Recipe { get; set; } = new List<Step>();
        [Column("ingredients", TypeName = "jsonb")]
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        [Column("created_at")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column("updated_at")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }
    public class Step
    {
        [JsonPropertyName("stepNumber")]
        [Required]
        public int StepNumber { get; set; }
        [JsonPropertyName("step")]
        [Required]
        public string? StepDescription { get; set; }
    }

    public class Ingredient
    {
        [JsonPropertyName("ingredientName")]
        [Required]
        public string? IngredientName { get; set; }
        [JsonPropertyName("ingredientQuantity")]
        [Required]
        public string? IngredientQuantity { get; set; }
    }
}
