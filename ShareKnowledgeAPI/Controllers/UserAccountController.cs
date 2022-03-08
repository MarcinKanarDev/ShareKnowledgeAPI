using Microsoft.AspNetCore.Mvc;
using ShareKnowledgeAPI.Mapper.DTOs;
using ShareKnowledgeAPI.Services;

namespace ShareKnowledgeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;

        public UserAccountController(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] UserRegisterDto registerDto)
        {
            _userAccountService.RegisterUser(registerDto);

            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto loginDto)
        {
            string token = _userAccountService.GenerateJwt(loginDto);

            return Ok(token);
        }

        [HttpGet]
        public ActionResult GetAll() 
        {
            var result = _userAccountService.GetAll();

            return Ok(result);
        }
    }
}
