using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Mapper.DTOs;
using ShareKnowledgeAPI.Seeder;
using ShareKnowledgeAPI.Services;
using System.Security.Claims;

namespace ShareKnowledgeAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin, User")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<PostDto>> GetAll([FromQuery] string searchPhrase) 
        {
            var result = _postService.GetAllPostsAsync(searchPhrase).Result;

            return Ok(result);
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "User, Admin")]
        public ActionResult GetPostById([FromRoute] int id)
        {
            var result = _postService.GetPostByIdAsync(id).Result;

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public ActionResult CreatePost([FromBody] CreatePostDto postDto)
        {   
            var id = _postService.CreatePostAsync(postDto).Result;

            return Created($"api/Post/{id}", null);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User, Admin")]
        public ActionResult UpdatePost([FromBody] Post post)
        {
            _postService.UpdatePostAsync(post);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<ActionResult> DeletePost([FromRoute] int id)
        {
            await _postService.DeletePostAsync(id);
            
            return NoContent();
        }

    }
}
