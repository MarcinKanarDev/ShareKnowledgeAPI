using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Mapper.DTOs;
using System.Security.Claims;

namespace ShareKnowledgeAPI.Services
{
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetAllPostsAsync(string searchPhrase);
        Task<int> CreatePostAsync(CreatePostDto createPostDto);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(int postId);
        Task<PostDto> GetPostByIdAsync(int postId);
    }
}
