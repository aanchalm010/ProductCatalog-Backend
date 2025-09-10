using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Api.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        // Relative path (e.g. /images/abc.jpg)
        public string? ImageUrl { get; set; }
    }
}
