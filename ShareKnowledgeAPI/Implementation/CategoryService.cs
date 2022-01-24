using Microsoft.EntityFrameworkCore;
using ShareKnowledgeAPI.Database;
using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Services;

namespace ShareKnowledgeAPI.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext applicatioDbContext)
        {
            _context = applicatioDbContext;
        }

        public Task<int> CreateCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            var categories = await _context.Categories
                .Include(p => p.Posts)
                .ToListAsync();

            return categories;
        }

        public Task<Category> GetCategoryById(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateCategory(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
