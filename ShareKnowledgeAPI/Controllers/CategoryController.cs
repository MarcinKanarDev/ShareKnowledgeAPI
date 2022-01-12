using Microsoft.AspNetCore.Mvc;

namespace ShareKnowledgeAPI.Controllers
{
    public class CategoryController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
