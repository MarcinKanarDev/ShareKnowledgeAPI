using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Mapper.DTOs;
using ShareKnowledgeAPI.Models;
using ShareKnowledgeAPI.Services;

namespace ShareKnowledgeAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "User, Admin")]
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
        public ActionResult<IEnumerable<PostDto>> GetAllPosts([FromQuery]PostQuery query) 
        {
            var result = _postService.GetAllPostsAsync(query).Result;

            return Ok(result);
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult GetPostById([FromRoute] int id)
        {
            var result = _postService.GetPostByIdAsync(id).Result;

            return Ok(result);
        }

        [HttpPost]
        public ActionResult CreatePost([FromBody] CreatePostDto postDto)
        {   
            var id = _postService.CreatePostAsync(postDto).Result;

            return Created($"api/Post/{id}", null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePost([FromBody] UpdatePostDto updatePostDto, [FromRoute] int id)
        {
            await _postService.UpdatePostAsync(updatePostDto, id);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost([FromRoute] int id)
        {
            await _postService.DeletePostAsync(id);
            
            return NoContent();
        }
    }
}
