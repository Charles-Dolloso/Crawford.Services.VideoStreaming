using Crawford.Services.VideoStreaming.Domain.Entities;

namespace Crawford.Services.VideoStreaming.Domain.Interfaces
{
    public interface IVideoRepository
    {
        Task<IEnumerable<Video>> GetVideosAsync();
        Task AddAsync(Video entity);
        void Update(Video entity);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<Video?> FindByIdAsync(Guid id);
    }
}
