using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Mapper.DTOs;

namespace ShareKnowledgeAPI.Services
{
    public interface ICommentService
    {
        Task <IEnumerable<CommentDto>> GetAllCommentsFromPostAsync(int postId);
        Task<int> CreateCommentToPostAsync(int postId, CreateCommentDto commentDto);
        Task UpdateCommentFromPostAsync(UpdateCommentDto updateCommentDto, int postId, int commentId);
        Task DeleteCommentFromPostAsync(int postId, int commentId);
    }
}
