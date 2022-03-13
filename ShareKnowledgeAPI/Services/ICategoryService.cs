using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Mapper.DTOs;
using ShareKnowledgeAPI.Models;

namespace ShareKnowledgeAPI.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync(Query query);
        Task DeleteCategoryAsync(int categoryId);
    }
}
