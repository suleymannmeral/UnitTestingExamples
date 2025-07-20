using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Users.Api.Dtos;
using Users.Api.Services;

namespace Users.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersController(IUserService userService):ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult>GetAll(CancellationToken cancellationToken)
        {
            var users= await userService.GetAllAsync(cancellationToken);
            return Ok(users);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id,CancellationToken cancellationToken)
        {
            var user = await userService.GetByIdAsync(id, cancellationToken);
            return Ok(user);

        }

        [HttpPost]
        public async Task<IActionResult>Create(CreateUserDto request,CancellationToken cancellationToken)
        {
            var result= await userService.CreateUserAsync(request,cancellationToken);
            return Ok(new {Result=result});
        }

        [HttpDelete("{id}")]    
        public async Task<IActionResult>DeleteById(Guid id,CancellationToken cancellationToken)
        {
            var result= await userService.DeleteByIdAsync(id,cancellationToken);
            return Ok(new {Result=result});
        }

    }
}
