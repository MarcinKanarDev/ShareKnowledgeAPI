using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShareKnowledgeAPI.Database;
using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Exceptions;
using ShareKnowledgeAPI.Mapper.DTOs;
using ShareKnowledgeAPI.Services;

namespace ShareKnowledgeAPI.Implementation
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext dbContext, IMapper mapper) 
        {
            _mapper = mapper;
            _context = dbContext;
        }

        public async Task<int> CreateComment(int postId, CreateCommentDto commentDto)
        {
            var commentEntity = _mapper.Map<Comment>(commentDto);

            commentEntity.PostId = postId;

            await _context.Comments.AddAsync(commentEntity);
            await _context.SaveChangesAsync();

            return commentEntity.Id;
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

        public IEnumerable<CommentDto> GetAll()
        {
            var comments = _context.Comments
                .ToList();

            var commentDtos = _mapper.Map<List<CommentDto>>(comments);

            return commentDtos;
        }

        public async Task<CommentDto> GetCommentById(int commentId)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment is null)
                throw new NotFoundException("Comment not found");

            var commentDto = _mapper.Map<CommentDto>(comment);          
                    
            return commentDto;
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
