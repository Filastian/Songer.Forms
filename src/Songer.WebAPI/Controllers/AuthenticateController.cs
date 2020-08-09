using Microsoft.AspNetCore.Mvc;
using Songer.WebAPI.Models;
using Songer.WebAPI.Services;
using System.Threading.Tasks;

namespace Songer.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticateController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        // POST : .../authenticate
        [HttpPost]
        public async Task<ActionResult<UserDto>> Authenticate([FromBody]AuthenticateUserModel model)
        {
            var user = await _authenticationService.AuthenticateAsync(model);

            if (user == null)
                return BadRequest(new { message = "Login or password is incorrect" });

            return Ok(user);
        }
    }
}