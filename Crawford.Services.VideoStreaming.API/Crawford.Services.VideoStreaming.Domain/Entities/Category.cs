using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crawford.Services.VideoStreaming.Domain.Entities
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public Guid CategoryID { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
