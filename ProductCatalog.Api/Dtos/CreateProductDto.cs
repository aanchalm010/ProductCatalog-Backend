using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Api.Dtos
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; } = null!;

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        // Optional: allow providing an external URL at creation
        public string? ImageUrl { get; set; }
    }
}
