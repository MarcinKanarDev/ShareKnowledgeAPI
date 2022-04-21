using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShareKnowledgeAPI.Database;
using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Mapper.DTOs;
using ShareKnowledgeAPI.Tests.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
namespace ShareKnowledgeAPI.Tests
{
    public class PostControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;
        

        public PostControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services.SingleOrDefault(service =>
                            service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                        services.Remove(dbContextOptions);
                        services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseInMemoryDatabase("TestDb"));


                        services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    }));

            _client = _factory.CreateClient();

        }

        [Theory]
        [InlineData("pageSize=5&pageNumber=1")]
        [InlineData("pageSize=15&pageNumber=2")]
        [InlineData("pageSize=10&pageNumber=3")]
        public async Task GetAllPosts_WithValidQueryParameters_ReturnsOkStatusCode(string queryParams)
        {
            //Act
            var response = await _client.GetAsync("/api/Post?" + queryParams);
            
            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("pageSize=2&pageNumber=1")]
        [InlineData("pageSize=15&pageNumber=-1")]
        [InlineData("pageSize=200&pageNumber=3")]
        public async Task GetAllPosts_WithInValidQueryParameters_ReturnsBadRequestStatusCode(string queryParams)
        {
            //Act
            var response = await _client.GetAsync("/api/Post?" + queryParams);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreatePost_ForValidPostModel_ReturnsCreatedStatusCode()
        {
            // Arrange
            var postModel = new CreatePostDto
            {
                Title = "Test",
                Description = "TestDesc",
                CategoryDtos = new List<CategoryDto>() { new CategoryDto { CategoryName = "Test" } }
            };

            var httpContent = postModel.ToJsonHttpContent();

            // Act
            var response = await _client.PostAsync("/api/Post", httpContent);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task CreatePost_ForInvalidPostModel_ReturnsBadRequestStatusCode()
        {
            // Arrange
            var postModel = new CreatePostDto
            {
                Description = "",
                CategoryDtos = new List<CategoryDto>() { new CategoryDto { CategoryName = "Test" } }
            };

            var httpContent = postModel.ToJsonHttpContent();

            // Act
            var response = await _client.PostAsync("/api/Post", httpContent);

            // Asserts
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        //Update
        [Fact]
        public async Task UpdatePost_ForValidUpdatePostModelAndPostOwner_ReturnsOkStatusCode()
        {
            // Arrange
            var postSeed = new Post
            {
                Title = "Test",
                Description = "TestDesc",
                CreatedById = 1
                
            };

            SeedPost(postSeed);
           
            var httpContent = postSeed.ToJsonHttpContent();

            //Act
            var response = await _client.PutAsync($"api/Post/{postSeed.Id}", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdatePost_ForValidUpdatePostModelAndInvalidPostOwner_ReturnsForbiddStatusCode()
        {
            // Arrange
            var postSeed = new Post
            {
                Title = "Test",
                Description = "TestDesc",
                CreatedById = 9

            };

            SeedPost(postSeed);

            var httpContent = postSeed.ToJsonHttpContent();

            //Act
            var response = await _client.PutAsync($"api/Post/{postSeed.Id}", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeletePost_ForPostOwner_ReturnsNoContent()
        {
            // Arrange
            var postSeed = new Post
            {
                Title = "Test",
                Description = "TestDesc",
                CreatedById = 1

            };

            SeedPost(postSeed);

            //Act
            var response = await _client.DeleteAsync($"api/Post/{postSeed.Id}");

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeletePost_ForNotPostOwner_ReturnsForbiddenStatusCode()
        {
            // Arrange
            var postSeed = new Post
            {
                Title = "Test",
                Description = "TestDesc",
                CreatedById = 999

            };

            SeedPost(postSeed);

            //Act
            var response = await _client.DeleteAsync($"api/Post/{postSeed.Id}");

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeletePost_ForNoExistPost_ReturnsNotFound()
        {
            //Act
            var response = await _client.DeleteAsync($"api/Post/-1");

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        private void SeedPost(Post post)
        {
            //Create DbContext Scope
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope(); 
            var _dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

            _dbContext.Posts.Add(post);
            _dbContext.SaveChanges();
        }
    }
}