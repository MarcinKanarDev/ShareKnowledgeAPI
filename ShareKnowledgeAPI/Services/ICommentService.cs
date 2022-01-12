using ShareKnowledgeAPI.Entities;

namespace ShareKnowledgeAPI.Services
{
    public interface ICommentService
    {
        IEnumerable<Comment> GetAll();
        Task<int> CreateComment(Comment comment);
        Task<bool> UpdateComment(Comment comment);
        Task<bool> DeleteComment(int commentId);
        Task<Comment> GetCommentById(int commentId);
    }
}
