using Microsoft.EntityFrameworkCore;
using ShareKnowledgeAPI.Database;
using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Seeder;
using ShareKnowledgeAPI.Services;

namespace ShareKnowledgeAPI.Implementation
{
    public class PostService : IPostService
    {
        private readonly DataSeeder _seeder;
        private readonly ApplicationDbContext _context;

        public PostService(ApplicationDbContext dbContext, DataSeeder seeder) 
        {
            _seeder = seeder;
            _context = dbContext;
        }

        public IEnumerable<Post> GetAll()
        {
            var posts = _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Categories)
                .ToList();

            return posts;
        }

        public async Task<Post> GetPostById(int postId)
        {
            var post = await _context.Posts.
                FirstOrDefaultAsync(p => p.Id == postId);

            if (post is null)
                return null;

            return post;
        }

        public async Task<int> CreatePost(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return post.Id;
        }

        public async Task<bool> DeletePost(int postId)
        {
            var post = _context.Posts.
                FirstOrDefault(p => p.Id == postId);

            if (post is null)
                return false;

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return true;
        }
      
        public async Task<bool> UpdatePost(Post post)
        {
            var postFromDb = await _context.Posts
                .FirstOrDefaultAsync(p => p.Id == post.Id);

            if (postFromDb is null)
                return false;

            postFromDb.Title = post.Title;
            postFromDb.Description = post.Description;
            postFromDb.Brains = post.Brains;
            postFromDb.Comments = post.Comments;
            postFromDb.Categories = post.Categories;
            
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
