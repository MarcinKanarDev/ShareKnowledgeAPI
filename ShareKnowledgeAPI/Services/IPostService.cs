using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Mapper.DTOs;

namespace ShareKnowledgeAPI.Services
{
    public interface IPostService
    {
        IEnumerable<PostDto> GetAll();
        Task<int> CreatePost(CreatePostDto createPostDto);
        Task UpdatePost(Post post);
        Task DeletePost(int postId);
        Task<PostDto> GetPostById(int postId);
    }
}
