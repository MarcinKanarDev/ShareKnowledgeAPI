using Microsoft.AspNetCore.Mvc;
using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Implementation;
using ShareKnowledgeAPI.Mapper.DTOs;
using ShareKnowledgeAPI.Services;

namespace ShareKnowledgeAPI.Controllers
{
    [Route("api/post/{postId}/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            var result = _commentService.GetAll();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult GetCommentById([FromRoute] int id)
        {
            var result = _commentService.GetCommentById(id).Result;

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public ActionResult CreateComment([FromRoute]int postId,  [FromBody]CreateCommentDto commentDto)
        {
            var id = _commentService.CreateComment(postId, commentDto).Result;

            return Created($"api/post/{postId}/comment/{id}", null);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateComment([FromBody] Comment comment)
        {
            if (_commentService.UpdateComment(comment).Result == false)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePost([FromRoute] int id)
        {
            if (_commentService.DeleteComment(id).Result == false)
                return NotFound();

            return NoContent();
        }

    }
}
