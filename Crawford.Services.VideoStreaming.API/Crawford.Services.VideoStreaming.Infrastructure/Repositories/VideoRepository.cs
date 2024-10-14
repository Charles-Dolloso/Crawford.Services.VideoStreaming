using Crawford.Services.VideoStreaming.Domain.Entities;
using Crawford.Services.VideoStreaming.Domain.Interfaces;
using Crawford.Services.VideoStreaming.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Crawford.Services.VideoStreaming.Infrastructure.Repositories
{
    public class VideoRepository : IVideoRepository
    {
        private readonly CrawfordDbContext _context;

        public VideoRepository(CrawfordDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Video>> GetVideosAsync()
        {
            return await _context.Videos.Include(i => i.Category).OrderBy(o => o.CreatedDateTime).ToListAsync();
        }

        public async Task AddAsync(Video entity)
        {
            await _context.Videos.AddAsync(entity);
        }

        public void Update(Video entity)
        {
            _context.Videos.Update(entity);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Video?> FindByIdAsync(Guid id)
        {
            return await _context.Videos.Include(i => i.Category).FirstOrDefaultAsync(c => c.VideoID.Equals(id));
        }
    }
}
