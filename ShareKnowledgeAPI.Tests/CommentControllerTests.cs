using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShareKnowledgeAPI.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization.Policy;
using System.Net.Http;
using ShareKnowledgeAPI.Entities;
using System.Threading.Tasks;
using FluentAssertions;
using ShareKnowledgeAPI.Mapper.DTOs;
using ShareKnowledgeAPI.Tests.Helpers;
using System.Collections.Generic;

namespace ShareKnowledgeAPI.Tests
{
    public class CommentControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public CommentControllerTests(WebApplicationFactory<Program> webApplicationFactory)
        {
            _factory = webApplicationFactory.WithWebHostBuilder(builder =>
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(service =>
                        service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    services.Remove(dbContextOptions);
                    services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("TestDb"));

                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                }));

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetAllCommentsFromPost_ForGivenValidPostId_ShouldReturnOkStatusCode()
        {
            // Arrange
            var post = GetPost();
            SeedPostData(post);

            // Act
            var response = await _client.GetAsync($"/api/post/{post.Id}/comment");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetAllCommentsFromPost_ForGivenInvalidPostId_ShouldReturnNotFoundStatusCode()
        {
            // Arrange
            var post = GetPost();
            SeedPostData(post);

            // Act 
            var response = await _client.GetAsync($"/api/post/{-99}/comment");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateComment_ForValidpostIdAndCommentDto_ShouldReturnCreatedStatusCode()
        {
            // Arrange

            var post = GetPost();
            SeedPostData(post);

            var commentDto = new CommentDto
            {
                CommentText = "Test comment text"
            };

            var httpContent = commentDto.ToJsonHttpContent();

            // Act
            var response = await _client.PostAsync($"/api/post/{post.Id}/comment", httpContent);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task CreateComment_ForInValidpostIdAndValidCommentDto_ShouldReturnNotFoundStatusCode()
        {
            // Arrange
            var post = GetPost();
            SeedPostData(post);

            var commentDto = new CommentDto
            {
                CommentText = "Tets comment text"
            };

            var httpContent = commentDto.ToJsonHttpContent();

            // Act
            var response = await _client.PostAsync($"/api/post/{-999}/comment", httpContent);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateComment_ForValidPostIdAndCommnetIdAndUpdateCommentDto_ShouldReturnOkStatusCode()
        {
            // Arrange
            var post = GetPost();
            SeedPostData(post);

            var commentId = post.Comments.First().Id;

            var updateCommentDto = new UpdateCommentDto
            {
                CommentText = "Test update comment"
            };

            // Act
            var httpContent = updateCommentDto.ToJsonHttpContent();

            var response = await _client.PutAsync($"/api/post/{post.Id}/comment/{commentId}", httpContent);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateComment_ForValidPostAndCommentDtoDataAndInvalidcommentId_ShouldReturnNotFoundStatusCode()
        {
            // Arrange
            var post = GetPost();
            SeedPostData(post);

            var commentId = post.Comments.First().Id;

            var updateCommentDto = new UpdateCommentDto
            {
                CommentText = "Test update comment"
            };

            // Act
            var httpContent = updateCommentDto.ToJsonHttpContent();

            var response = await _client.PutAsync($"/api/post/{post.Id}/comment/{-99}", httpContent);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteComment_ForValidPostIdAndCommentId_ShouldReturnNoContentStatusCode() 
        {
            // Arrange
            var post = GetPost();
            SeedPostData(post);
            
            var commentId = post.Comments.First().Id;

            // Act
            var response = await _client.DeleteAsync($"/api/post/{post.Id}/comment/{commentId}");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteComment_ForInvalidCommentId_ShoulReturnNotFoundStatusCode()
        {
            // Arrange
            var post = GetPost();
            SeedPostData(post);

            // Act
            var response = await _client.DeleteAsync($"/api/post/{post.Id}/comment/{-99}");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteComment_ForInvalidPostId_ShoulReturnNotFoundStatusCode()
        {
            // Arrange
            var post = GetPost();
            SeedPostData(post);

            var commentId = post.Comments.First().Id;

            // Act
            var response = await _client.DeleteAsync($"/api/post/{-99}/comment/{commentId}");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        private Post GetPost() => new Post
        {
            Title = "Test",
            Description = "Test Desc",
            CreatedById = 1,
            Comments = new List<Comment>
            {
                new Comment { CommentText = "Test Comment", CreatedById = 1 },
            }
        };

        private void SeedPostData(Post post)
            {
                var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
                using var scope = scopeFactory.CreateScope();
                var _dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

                _dbContext.Posts.AddAsync(post);
                _dbContext.SaveChangesAsync();
            }
        }
}
