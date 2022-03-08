using Microsoft.EntityFrameworkCore;
using ShareKnowledgeAPI.Database;
using ShareKnowledgeAPI.Entities;

namespace ShareKnowledgeAPI.Seeder
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;

        public DataSeeder(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public void SeedData()
        {
            if (!_context.Posts.Any()) 
            {
                var posts = GetPosts();
                _context.Posts.AddRange(posts);
                _context.SaveChanges();
            }

            if (!_context.Permissions.Any())
            {
                var permissions = GetPermissions();
                _context.Permissions.AddRange(permissions);
                _context.SaveChanges();
            }
        }

        private IEnumerable<Permission> GetPermissions() 
        {
            var permissions = new List<Permission>()
            {

                new Permission
                {
                    PermissionName = "Admin"
                },
                new Permission 
                {
                    PermissionName = "User"
                }
            };

            return permissions;
        }

        private IEnumerable<Post> GetPosts() 
        {
            var posts = new List<Post>() 
            {
                new Post()
                {
                        Title = "Test post 1",
                        Description = "Some description",
                        Brains = 2,
                        Comments = new List<Comment>()
                        {
                            new Comment()
                            {
                                CommentText = "Some comment text",
                                Brains = 1,
                            }
                        },
                        Categories = new List<Category>()
                        {
                            new Category()
                            {
                                CategoryName = "Physics"
                            }
                        }
                },
                new Post()
                {
                    Title = "Test post 2",
                       Description = "Some description 2",
                       Brains = 2,
                       Comments = new List<Comment>()
                       {
                           new Comment()
                           {
                               CommentText = "Commentary",
                               Brains = 4,
                           },
                           new Comment()
                           {
                               CommentText = "Wooo",
                               Brains = 1,
                           }
                       },
                       Categories = new List<Category>()
                       {
                           new Category()
                           {
                               CategoryName = "Biology"
                           }
                       }
                }   
                  
            };

            return posts;
        }
    }
}
