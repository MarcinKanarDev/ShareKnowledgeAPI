using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShareKnowledgeAPI.Database;
using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Exceptions;
using ShareKnowledgeAPI.Mapper.DTOs;
using ShareKnowledgeAPI.Services;

namespace ShareKnowledgeAPI.Implementation
{
    public class PostService : IPostService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public PostService(ApplicationDbContext dbContext, IMapper mapper) 
        {
            _mapper = mapper;
            _context = dbContext;
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync()
        {
            var posts = await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Categories)
                .ToListAsync();

            var postDtos = _mapper.Map<IEnumerable<PostDto>>(posts);

            return postDtos;
        }

        public async Task<PostDto> GetPostByIdAsync(int postId)
        {
            var post = await _context.Posts.
                FirstOrDefaultAsync(p => p.Id == postId);

            if (post is null)
                throw new NotFoundException($"Post not found");

            var postDto = _mapper.Map<PostDto>(post);

            return postDto;
        }

        public async Task<int> CreatePostAsync(CreatePostDto postDto)
        {
            var categories = _mapper.Map<List<Category>>(postDto.CategoryDtos);
                       
            var post = _mapper.Map<Post>(postDto);
            post.Categories = categories;

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return post.Id;
        }

        public async Task DeletePostAsync(int postId)
        {
            var post = await _context.Posts.
                FirstOrDefaultAsync(p => p.Id == postId);

            if (post is null)
                throw new NotFoundException($"Post not found");

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
      
        public async Task UpdatePostAsync(Post post)
        {
            var postFromDb = await _context.Posts
                .FirstOrDefaultAsync(p => p.Id == post.Id);

            if (postFromDb is null)
                throw new NotFoundException($"Post not found");

            postFromDb.Title = post.Title;
            postFromDb.Description = post.Description;
            postFromDb.Brains = post.Brains;
            postFromDb.Comments = post.Comments;
            postFromDb.Categories = post.Categories;
            
            await _context.SaveChangesAsync();
        }
    }
}
