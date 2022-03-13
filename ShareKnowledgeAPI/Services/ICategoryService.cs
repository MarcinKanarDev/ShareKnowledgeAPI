using ShareKnowledgeAPI.Entities;

namespace ShareKnowledgeAPI.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAll();
    }
}
