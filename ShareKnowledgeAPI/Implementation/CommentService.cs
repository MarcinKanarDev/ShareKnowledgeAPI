using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ShareKnowledgeAPI.Authorization;
using ShareKnowledgeAPI.Database;
using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Exceptions;
using ShareKnowledgeAPI.Mapper.DTOs;
using ShareKnowledgeAPI.Services;

namespace ShareKnowledgeAPI.Implementation
{
    public class CommentService : ICommentService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext dbContext, IMapper mapper, 
            IAuthorizationService authorizationService, IUserContextService userContextService) 
        {
            _authorizationService = authorizationService;
            _userContextService = userContextService;
            _mapper = mapper;
            _context = dbContext;
        }

        public async Task<int> CreateCommentToPostAsync(int postId, CreateCommentDto commentDto)
        {
            var postFromDb = _context.Posts.FirstOrDefault(p => p.Id == postId);

            if (postFromDb is null) 
            {
                throw new NotFoundException("Post not found");
            }

            var commentEntity = _mapper.Map<Comment>(commentDto);

            commentEntity.PostId = postId;
            commentEntity.CreatedById = _userContextService.GetUserId;

            await _context.Comments.AddAsync(commentEntity);
            await _context.SaveChangesAsync();

            return commentEntity.Id;
        }

        public async Task DeleteCommentFromPostAsync(int postId, int commentId)
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

            var authorizeResult = _authorizationService.AuthorizeAsync(_userContextService.User, commentToDelete,
                new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizeResult.Succeeded) 
            {
                throw new ForbidException("You don't have an access to this resorce.");
            }

            _context.Remove(commentToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CommentDto>> GetAllCommentsFromPostAsync(int postId)
        {
            var posts = _context.Posts.ToList();

            var post = await _context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post is null)
                throw new NotFoundException("Post not found.");

            var commentDtos = _mapper.Map<List<CommentDto>>(post.Comments);

            return commentDtos;
        }

        public async Task UpdateCommentFromPostAsync(UpdateCommentDto updateCommentDto, int postId, int commentId)
        {
            var postFromDb = await _context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (postFromDb is null)
                throw new NotFoundException("Post not found.");

            var commentFromPost = postFromDb.Comments
                .FirstOrDefault(c => c.Id == commentId);

            if (commentFromPost is null)
                throw new NotFoundException("Comment not found.");

            var authorizeResult = _authorizationService.AuthorizeAsync(_userContextService.User, commentFromPost,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizeResult.Succeeded) 
            {
                throw new ForbidException("You don't have an access to this resorce.");
            }

            commentFromPost.CommentText = updateCommentDto.CommentText;
            
            await _context.SaveChangesAsync();
        }
    }
}
