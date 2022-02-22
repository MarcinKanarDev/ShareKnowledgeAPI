using Microsoft.AspNetCore.Mvc;
using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Mapper.DTOs;
using ShareKnowledgeAPI.Seeder;
using ShareKnowledgeAPI.Services;

namespace ShareKnowledgeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public ActionResult GetAll() 
        {
            var result = _postService.GetAllPostsAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
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
        public ActionResult UpdatePost([FromBody] Post post)
        {
            _postService.UpdatePostAsync(post);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePost([FromRoute] int id)
        {
            _postService.DeletePostAsync(id);
            
            return NoContent();
        }

    }
}
