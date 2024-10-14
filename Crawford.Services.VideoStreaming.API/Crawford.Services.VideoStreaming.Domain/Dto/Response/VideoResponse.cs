using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawford.Services.VideoStreaming.Domain.Dto.Response
{
    public class VideoResponse
    {
        public Guid ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Categories { get; set; } = string.Empty;
        public string ThumbnailPath { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
    }
}
