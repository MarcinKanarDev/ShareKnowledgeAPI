using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Mapper.DTOs;
using ShareKnowledgeAPI.Models;
using System.Security.Claims;

namespace ShareKnowledgeAPI.Services
{
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetAllPostsAsync(PostQuery query);
        Task<int> CreatePostAsync(CreatePostDto createPostDto);
        Task UpdatePostAsync(UpdatePostDto updatePostDto, int id);
        Task DeletePostAsync(int postId);
        Task<PostDto> GetPostByIdAsync(int postId);
    }
}
