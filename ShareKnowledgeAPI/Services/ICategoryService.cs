using ShareKnowledgeAPI.Entities;

namespace ShareKnowledgeAPI.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAll();
        Task<int> CreateCategory(Category category);
        Task<bool> UpdateCategory(Category category);
        Task<bool> DeleteCategory(int categoryId);
        Task<Category> GetCategoryById(int categoryId);
    }
}
