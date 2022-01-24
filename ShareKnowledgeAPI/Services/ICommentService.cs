using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Mapper.DTOs;

namespace ShareKnowledgeAPI.Services
{
    public interface ICommentService
    {
        IEnumerable<CommentDto> GetAll();
        Task<int> CreateComment(int postId, CreateCommentDto commentDto);
        Task<bool> UpdateComment(Comment comment);
        Task<bool> DeleteComment(int commentId);
        Task<CommentDto> GetCommentById(int commentId);
    }
}
