using Crawford.Services.VideoStreaming.Domain.Dto.Request;
using Crawford.Services.VideoStreaming.Domain.Dto.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawford.Services.VideoStreaming.Domain.Interfaces
{
    public interface IVideoService
    {
        Task<IEnumerable<VideoResponse>> GetVideoList();
        Task<VideoResponse> GetVideoByID(Guid id);
        Task<(bool Success, string Message)> UploadVideoAsync(VideoUploadDto dto);
        bool ValidateFile(IFormFile file);
    }
}
