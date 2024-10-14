using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawford.Services.VideoStreaming.Domain.Dto.Request
{
    public class VideoUploadDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Categories { get; set; }
        public IFormFile File { get; set; } // The uploaded video file
    }
}
