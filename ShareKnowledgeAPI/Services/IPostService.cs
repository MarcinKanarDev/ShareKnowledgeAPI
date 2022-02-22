using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Mapper.DTOs;

namespace ShareKnowledgeAPI.Services
{
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetAllPostsAsync();
        Task<int> CreatePostAsync(CreatePostDto createPostDto);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(int postId);
        Task<PostDto> GetPostByIdAsync(int postId);
    }
}
