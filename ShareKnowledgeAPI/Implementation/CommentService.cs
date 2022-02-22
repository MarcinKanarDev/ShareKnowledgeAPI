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

        public async Task<int> CreateCommentToPostAsync(int postId, CreateCommentDto commentDto)
        {
            var commentEntity = _mapper.Map<Comment>(commentDto);

            commentEntity.PostId = postId;

            await _context.Comments.AddAsync(commentEntity);
            await _context.SaveChangesAsync();

            return commentEntity.Id;
        }

        public async Task<bool> DeleteCommentFromPostAsync(int postId, int commentId)
        {
            var post = await _context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post is null)
                throw new NotFoundException("Post not found.");

            var commentToDelete = post.Comments
                .FirstOrDefault(c => c.Id == commentId);

            if (commentToDelete is null)
                throw new NotFoundException("Comment not found");

            _context.Remove(commentToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task <IEnumerable<CommentDto>> GetAllCommentsFromPostAsync(int postId)
        {
            var post = await _context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post is null)
                throw new NotFoundException("Post not found.");

            var commentDtos = _mapper.Map<List<CommentDto>>(post.Comments);

            return commentDtos;
        }

        public async Task<bool> UpdateCommentFromPostAsync(UpdateCommentDto updateCommentDto, int postId, int commentId)
        {
            var postFromDb = await _context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (postFromDb is null)
                throw new NotFoundException("Post not found.");

            var commentsFromPost = postFromDb.Comments
                .FirstOrDefault(c => c.Id == commentId);

            if (commentsFromPost is null)
                throw new NotFoundException("Comment not found.");

            commentsFromPost.CommentText = updateCommentDto.CommentText;
            
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
