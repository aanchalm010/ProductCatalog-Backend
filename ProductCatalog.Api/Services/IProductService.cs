using Microsoft.AspNetCore.Http;
using ProductCatalog.Api.Dtos;

namespace ProductCatalog.Api.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(CreateProductDto dto);
        Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto);
        Task<bool> DeleteAsync(int id);
        Task<ProductDto?> UploadImageAsync(int id, IFormFile file);
        //--------------------
        Task<ProductDto?> RemoveImageAsync(int id);
    }
}
