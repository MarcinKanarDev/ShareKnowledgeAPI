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

        public async Task<IEnumerable<Category>> GetAll()
        {
            var categories = await _context.Categories.ToListAsync();

            return categories;
        }
    }
}
