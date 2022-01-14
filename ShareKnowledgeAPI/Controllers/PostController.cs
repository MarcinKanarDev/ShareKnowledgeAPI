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
            var result = _postService.GetAll();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult GetPostById([FromRoute] int id)
        {
            var result = _postService.GetPostById(id).Result;

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public ActionResult CreatePost([FromBody] CreatePostDto postDto)
        {
            var id = _postService.CreatePost(postDto).Result;

            return Created($"api/Post/{id}", null);
        }

        [HttpPut("{id}")]
        public ActionResult UpdatePost([FromBody] Post post)
        {
            if (_postService.UpdatePost(post).Result == false)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePost([FromRoute] int id)
        {
            if (_postService.DeletePost(id).Result == false)
                return NotFound();

            return NoContent();
        }

    }
}
