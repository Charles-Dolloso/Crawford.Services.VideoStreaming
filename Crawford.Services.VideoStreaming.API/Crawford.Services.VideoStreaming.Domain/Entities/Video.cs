using Crawford.Services.VideoStreaming.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawford.Services.VideoStreaming.Domain.Entities
{
    [Table("Video")]
    public class Video
    {
        [Key]
        public Guid VideoID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ThumbnailPath { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime CreatedDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }

        // Foreign key to Category
        [ForeignKey("Category")]
        public Guid CategoryID { get; set; }
        public virtual Category Category { get; set; }
    }
}
