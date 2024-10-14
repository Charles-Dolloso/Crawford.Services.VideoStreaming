using Crawford.Services.VideoStreaming.Domain.Entities;

namespace Crawford.Services.VideoStreaming.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetVideosAsync();
        Task AddAsync(Category entity);
        void Update(Category entity);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<Category?> FindByIdAsync(Guid id);
        Task<Category?> FindByNameAsync(string name);
    }
}
