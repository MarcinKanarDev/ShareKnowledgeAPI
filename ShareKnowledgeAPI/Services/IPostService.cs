using ShareKnowledgeAPI.Entities;

namespace ShareKnowledgeAPI.Services
{
    public interface IPostService
    {
        IEnumerable<Post> GetAll();
        Task<int> CreatePost(Post post);
        Task<bool> UpdatePost(Post post);
        Task<bool> DeletePost(int postId);
        Task<Post> GetPostById(int postId);
    }
}
