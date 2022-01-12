using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShareKnowledgeAPI.Database;
using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Services;

namespace ShareKnowledgeAPI.Implementation
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext dbContext) 
        {
            _context = dbContext;
        }

        public async Task<int> CreateComment(Comment comment)
{
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return comment.Id;
        }

        public async Task<bool> DeleteComment(int commentId)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment is null)
                return false;

            _context.Remove(comment);
            await _context.SaveChangesAsync();

            return true;
        }

        public IEnumerable<Comment> GetAll()
        {
            var comments = _context.Comments
                .ToList();

            return comments;
        }

        public async Task<Comment> GetCommentById(int commentId)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment is null)
                return null;
                    
            return comment;
        }

        public async Task<bool> UpdateComment(Comment comment)
        {
            var commentFromDb = await _context.Comments
                .FirstOrDefaultAsync(p => p.Id == comment.Id);

            if (commentFromDb is null)
                return false;

            commentFromDb.CommentText = comment.CommentText;
            commentFromDb.Brains = comment.Brains;
            
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
