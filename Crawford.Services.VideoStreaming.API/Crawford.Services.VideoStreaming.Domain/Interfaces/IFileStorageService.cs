using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawford.Services.VideoStreaming.Domain.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task<string> UploadThumbnail(string path, string thumbnailName);
    }
}
