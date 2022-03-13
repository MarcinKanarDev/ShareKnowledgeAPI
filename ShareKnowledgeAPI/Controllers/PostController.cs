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
        public async Task<ActionResult> GetAllPosts([FromQuery]Query query) 
        {
            var result = await _postService.GetAllPostsAsync(query);

            return Ok(result);
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetPostById([FromRoute] int id)
        {
            var result = await _postService.GetPostByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePost([FromBody] CreatePostDto postDto)
        {   
            var id = _postService.CreatePostAsync(postDto);

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
