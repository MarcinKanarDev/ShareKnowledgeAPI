using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> GetAll([FromRoute]int postId)
        {
            var result = await _commentService
                .GetAllCommentsFromPostAsync(postId);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> CreateComment([FromRoute]int postId, [FromBody]CreateCommentDto commentDto)
        {
            var id = await _commentService
                .CreateCommentToPostAsync(postId, commentDto);

            return Created($"api/post/{postId}/comment/{id}", null);
        }

        [HttpPut("{commentId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> UpdateComment([FromRoute]int postId, [FromRoute]int commentId,
            [FromBody] UpdateCommentDto updateCommentDto)
        {
            await _commentService
                .UpdateCommentFromPostAsync(updateCommentDto, postId, commentId);
                
            return Ok();
        }

        
        [HttpDelete("{commentId}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> DeleteComment([FromRoute] int postId, [FromRoute] int commentId )
        {
            await _commentService
                .DeleteCommentFromPostAsync(postId, commentId);
      
            return NoContent();
        }
    }
}
