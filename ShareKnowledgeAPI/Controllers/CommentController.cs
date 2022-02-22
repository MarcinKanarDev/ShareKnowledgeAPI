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
        public ActionResult GetAll([FromRoute]int postId)
        {
            var result = _commentService
                .GetAllCommentsFromPostAsync(postId);

            return Ok(result);
        }

        [HttpPost]
        public ActionResult CreateComment([FromRoute]int postId, [FromBody]CreateCommentDto commentDto)
        {
            var id = _commentService
                .CreateCommentToPostAsync(postId, commentDto)
                .Result;

            return Created($"api/post/{postId}/comment/{id}", null);
        }

        [HttpPut("{commentId}")]
        public ActionResult UpdateComment([FromRoute]int postId, [FromRoute]int commentId,
            [FromBody] UpdateCommentDto updateCommentDto)
        {
            if (_commentService
                .UpdateCommentFromPostAsync(updateCommentDto, postId, commentId)
                .Result == false) 
            {
                return BadRequest();
            }
                
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteComment([FromRoute] int postId, [FromRoute] int commentId )
        {
            if (_commentService
                .DeleteCommentFromPostAsync(postId, commentId)
                .Result == false) 
            {
                return NotFound();
            }
                
            return NoContent();
        }

    }
}
