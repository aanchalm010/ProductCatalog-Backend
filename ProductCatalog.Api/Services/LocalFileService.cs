using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ProductCatalog.Api.Services
{
    public class LocalFileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        public LocalFileService(IWebHostEnvironment env) => _env = env;

        public async Task<string> SaveFileAsync(IFormFile file, string subFolder = "images")
        {
            if (file == null || file.Length == 0) throw new ArgumentException("File is empty", nameof(file));

            var uploadsRoot = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), subFolder);
            if (!Directory.Exists(uploadsRoot)) Directory.CreateDirectory(uploadsRoot);

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsRoot, fileName);

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

            return $"/{subFolder}/{fileName}";
        }

        public Task DeleteFileAsync(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return Task.CompletedTask;

            var trimmed = relativePath.TrimStart('/');
            var fileOnDisk = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), trimmed);
            if (File.Exists(fileOnDisk)) File.Delete(fileOnDisk);
            return Task.CompletedTask;
        }
    }
}
