using Microsoft.AspNetCore.Mvc;
using ShareKnowledgeAPI.Services;

namespace ShareKnowledgeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService categoryService)
        {
            _service = categoryService;
        }

        [HttpGet]
        public ActionResult GetAll() 
        {
            var result = _service.GetAll().Result;

            return Ok(result);
        }
    }
}
