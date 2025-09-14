using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ProductCatalog.Api.Services
{
    public class AzureBlobFileService : IFileService
    {
        private readonly BlobContainerClient _containerClient;

        public AzureBlobFileService(IConfiguration config)
        {
            var connectionString = config["AzureStorage:ConnectionString"];
            var containerName = config["AzureStorage:ContainerName"];

            _containerClient = new BlobContainerClient(connectionString, containerName);
            _containerClient.CreateIfNotExists();
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            var blobName = $"{folder}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var blobClient = _containerClient.GetBlobClient(blobName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString();  
        }

        public async Task DeleteFileAsync(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl)) return;

            string blobName;

            if (Uri.TryCreate(fileUrl, UriKind.Absolute, out var uri))
            {
                // Extract the blob name from absolute URI
                blobName = string.Join("", uri.Segments.Skip(1));
            }
            else
            {
                // Handle relative paths (e.g., "images/abc.jpg" or "/images/abc.jpg")
                blobName = fileUrl.TrimStart('/');
            }

            await _containerClient.DeleteBlobIfExistsAsync(blobName);
        }

    }
}
