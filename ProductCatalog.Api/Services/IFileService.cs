//using Microsoft.AspNetCore.Http;

//namespace ProductCatalog.Api.Services
//{
//    public interface IFileService
//    {
//        Task<string> SaveFileAsync(IFormFile file, string subFolder = "images");
//        Task DeleteFileAsync(string relativePath);
//    }
//}

using Microsoft.AspNetCore.Http;

namespace ProductCatalog.Api.Services
{ 
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folder);
        Task DeleteFileAsync(string filePath);
    }
}
