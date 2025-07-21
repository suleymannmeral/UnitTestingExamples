using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnderstandingDependencies.Api.Services;

namespace UnderstandingDependencies.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
           
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
          
        }
    }
}
