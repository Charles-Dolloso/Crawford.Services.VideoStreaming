using Azure.Storage.Blobs;
using Crawford.Services.VideoStreaming.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Crawford.Services.VideoStreaming.Domain.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "videos";

        public FileStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(file.FileName);
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            return blobClient.Uri.ToString(); // Return the video URL
        }

        public async Task<string> UploadThumbnail(string path, string thumbnailName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(thumbnailName);
            using (var stream = File.OpenRead(path))
            {
                await blobClient.UploadAsync(stream, true);
            }

            return blobClient.Uri.ToString(); // Return the video URL
        }
    }
}
