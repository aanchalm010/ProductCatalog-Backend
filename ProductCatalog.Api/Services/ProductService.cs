using Microsoft.AspNetCore.Http;
using ProductCatalog.Api.Dtos;
using ProductCatalog.Api.Models;
using ProductCatalog.Api.Repositories;

namespace ProductCatalog.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _hca;

        public ProductService(IProductRepository repo, IFileService fileService, IHttpContextAccessor hca)
        {
            _repo = repo;
            _fileService = fileService;
            _hca = hca;
        }

        private ProductDto Map(Product p) =>
            new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = ToAbsoluteUrl(p.ImageUrl)
            };

        private string? ToAbsoluteUrl(string? relative)
        {
            if (string.IsNullOrWhiteSpace(relative)) return null;
            var req = _hca.HttpContext?.Request;
            if (req == null) return relative;
            var baseUrl = $"{req.Scheme}://{req.Host}";
            if (relative.StartsWith("http")) return relative;
            return $"{baseUrl}{relative}";
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _repo.GetAllAsync();
            return products.Select(Map);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            return p == null ? null : Map(p);
        }

        //public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        //{
        //    var p = new Product { Name = dto.Name, Price = dto.Price };
        //    await _repo.AddAsync(p);
        //    await _repo.SaveChangesAsync();
        //    return Map(p);
        //}

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            var p = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl // accept URL directly if provided
            };

            await _repo.AddAsync(p);
            await _repo.SaveChangesAsync();
            return Map(p);
        }

        //public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
        //{
        //    var p = await _repo.GetByIdAsync(id);
        //    if (p == null) return false;
        //    p.Name = dto.Name;
        //    p.Price = dto.Price;
        //    _repo.Update(p);
        //    return await _repo.SaveChangesAsync();
        //}

        public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return null;

            p.Name = dto.Name;
            p.Price = dto.Price;

            if (dto.ImageUrl != null)
            {
                p.ImageUrl = dto.ImageUrl;
            }

            _repo.Update(p);
            await _repo.SaveChangesAsync();
            return Map(p);
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return false;
            if (!string.IsNullOrWhiteSpace(p.ImageUrl)) await _fileService.DeleteFileAsync(p.ImageUrl);
            _repo.Remove(p);
            return await _repo.SaveChangesAsync();
        }

        public async Task<ProductDto?> UploadImageAsync(int id, IFormFile file)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return null;
            if (!string.IsNullOrWhiteSpace(p.ImageUrl)) await _fileService.DeleteFileAsync(p.ImageUrl);
            var relativePath = await _fileService.SaveFileAsync(file, "images");
            p.ImageUrl = relativePath;
            _repo.Update(p);
            await _repo.SaveChangesAsync();
            return Map(p);
        }
    }
}
