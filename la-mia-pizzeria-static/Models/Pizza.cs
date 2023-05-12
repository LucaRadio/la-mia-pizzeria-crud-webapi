
using la_mia_pizzeria_static.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_static.Models
{
    [Table("pizzas")]
    public class Pizza
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The length must be between 5 and 50", MinimumLength = 5)]
        public string Name { get; set; }
        [Required]
        [DescriptionValidate]
        public string Description { get; set; }
        [Required]
        [Url]
        public string Img { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public double Price { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<Ingredient>? Ingredients { get; set; } = new List<Ingredient>();

        public Pizza(string name, string description, string img, double price)
        {
            Name = name;
            Description = description;
            Img = img;
            Price = price;
        }
        public Pizza()
        {
        }


    }
}
