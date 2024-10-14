using Crawford.Services.VideoStreaming.Domain.Entities;
using Crawford.Services.VideoStreaming.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Crawford.Services.VideoStreaming.Infrastructure.Context
{
    public class CrawfordDbContext : DbContext
    {
        public CrawfordDbContext(DbContextOptions<CrawfordDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Data seed category
            modelBuilder.Entity<Category>().HasData(new Category { CategoryID = new Guid("183DF134-28ED-4928-A816-402B6019AB90"), Name = "Sample" });

            #region Data Seed Video
            modelBuilder.Entity<Video>().HasData(new Video
            {
                VideoID = Guid.NewGuid(),
                CategoryID = new Guid("183DF134-28ED-4928-A816-402B6019AB90"),
                Title = "Sample Video 1",
                Description = "Sample Video Only",
                ThumbnailPath = "https://crawfordblobcontainer.blob.core.windows.net/videos/df2a3c37-ff4c-452c-89fc-f4cde425f1c8.jpg",
                FilePath = "https://crawfordblobcontainer.blob.core.windows.net/videos/videoplayback.mp4",
                CreatedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now
            });

            modelBuilder.Entity<Video>().HasData(new Video
            {
                VideoID = Guid.NewGuid(),
                CategoryID = new Guid("183DF134-28ED-4928-A816-402B6019AB90"),
                Title = "Sample Video 2",
                Description = "Sample Video Only",
                ThumbnailPath = "https://crawfordblobcontainer.blob.core.windows.net/videos/41a57cba-0bc8-477c-8ecd-ed9693804048.jpg",
                FilePath = "https://crawfordblobcontainer.blob.core.windows.net/videos/videoplayback %281%29.mp4",
                CreatedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now
            });

            modelBuilder.Entity<Video>().HasData(new Video
            {
                VideoID = Guid.NewGuid(),
                CategoryID = new Guid("183DF134-28ED-4928-A816-402B6019AB90"),
                Title = "Sample Video 3",
                Description = "Sample Video Only",
                ThumbnailPath = "https://crawfordblobcontainer.blob.core.windows.net/videos/0d319bc7-bdbd-4655-901b-a38f58ad833e.jpg",
                FilePath = "https://crawfordblobcontainer.blob.core.windows.net/videos/videoplayback %282%29.mp4",
                CreatedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now
            });
            #endregion


            base.OnModelCreating(modelBuilder);
        }

        // ef core will ensure non-null value here, thus can use null-forgiving operator here
        public DbSet<Video> Videos { get; set; } = null;
        public DbSet<Category> Categories { get; set; } = null;
    }
}
