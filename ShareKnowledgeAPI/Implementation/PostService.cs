using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShareKnowledgeAPI.Database;
using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Exceptions;
using ShareKnowledgeAPI.Mapper.DTOs;
using ShareKnowledgeAPI.Seeder;
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

        public IEnumerable<PostDto> GetAll()
        {
            var posts = _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Categories)
                .ToList();

            var postDtos = _mapper.Map<IEnumerable<PostDto>>(posts);

            return postDtos;
        }

        public async Task<PostDto> GetPostById(int postId)
        {
            var post = await _context.Posts.
                FirstOrDefaultAsync(p => p.Id == postId);

            if (post is null)
                throw new NotFoundException($"Resource wih Id = {postId} not found");

            var postDto = _mapper.Map<PostDto>(post);

            return postDto;
        }

        public async Task<int> CreatePost(CreatePostDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return post.Id;
        }

        public async Task DeletePost(int postId)
        {
            var post = _context.Posts.
                FirstOrDefault(p => p.Id == postId);

            if (post is null)
                throw new NotFoundException($"Resource wih Id = {postId} not found");

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
      
        public async Task UpdatePost(Post post)
        {
            var postFromDb = await _context.Posts
                .FirstOrDefaultAsync(p => p.Id == post.Id);

            if (postFromDb is null)
                throw new NotFoundException($"Resource wih Id = {post.Id} not found");

            postFromDb.Title = post.Title;
            postFromDb.Description = post.Description;
            postFromDb.Brains = post.Brains;
            postFromDb.Comments = post.Comments;
            postFromDb.Categories = post.Categories;
            
            await _context.SaveChangesAsync();
        }
    }
}
