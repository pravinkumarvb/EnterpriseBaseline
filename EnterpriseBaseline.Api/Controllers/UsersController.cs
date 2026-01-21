using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseBaseline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "Users.View")]
        public IActionResult Get()
        {
            return Ok("Users list");
        }

        [HttpPost]
        [Authorize(Policy = "Users.Create")]
        public IActionResult Create()
        {
            return Ok("User created");
        }
    }
}
