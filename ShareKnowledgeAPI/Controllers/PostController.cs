using Microsoft.AspNetCore.Mvc;

namespace ShareKnowledgeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetAll() 
        {
            return Ok();
        }
    }
}
