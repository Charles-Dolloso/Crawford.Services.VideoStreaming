using Crawford.Services.VideoStreaming.Domain.Entities;
using Crawford.Services.VideoStreaming.Domain.Interfaces;
using Crawford.Services.VideoStreaming.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Crawford.Services.VideoStreaming.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CrawfordDbContext _context;

        public CategoryRepository(CrawfordDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Category>> GetVideosAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task AddAsync(Category entity)
        {
            await _context.Categories.AddAsync(entity);
        }

        public void Update(Category entity)
        {
            _context.Categories.Update(entity);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Category?> FindByIdAsync(Guid id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryID.Equals(id));
        }

        public async Task<Category?> FindByNameAsync(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Name.Equals(name));
        }
    }
}
