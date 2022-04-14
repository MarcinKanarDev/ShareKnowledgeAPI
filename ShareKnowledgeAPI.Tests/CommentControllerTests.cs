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
            // arrange
            var response = await _client.GetAsync($"/api/post/{2}/comment");

            // act


            // assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        // private voidd

        private void SeedComment(Comment comment)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

            _dbContext.AddAsync(comment);
            _dbContext.SaveChangesAsync();
        }

    }
}
