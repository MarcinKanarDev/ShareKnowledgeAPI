using Microsoft.EntityFrameworkCore;
using ShareKnowledgeAPI.Database;
using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Services;
using System.ComponentModel.Design;

namespace ShareKnowledgeAPI.Implementation
{
    public class CategoryService : ICategoryService
    {
        private ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext dbContext) 
        {
            _context = dbContext;
        }

        public async Task<int> CreateCategory(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return category.Id;
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category is null)
                return false;

            _context.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }

        public IEnumerable<Category> GetAll()
        {
            var categories = _context.Categories
                .ToList();

            return categories;
        }

        public async Task<Category> GetCategoryById(int categoryId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category is null)
                return null;

            return category;
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            var categoryFromDb = await _context.Categories
                .FirstOrDefaultAsync(p => p.Id == category.Id);

            if (categoryFromDb is null)
                return false;

            categoryFromDb.CategoryName = category.CategoryName;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
