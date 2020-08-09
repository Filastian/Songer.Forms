using Microsoft.AspNetCore.Mvc;
using Songer.WebAPI.Models;
using Songer.WebAPI.Services;
using System;
using System.Threading.Tasks;

namespace TestAuth.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegisterController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        // POST : .../register
        [HttpPost]
        public async Task<ActionResult> Register([FromBody]RegisterUserModel model)
        {
            try
            {
                await _registrationService.RegisterAsync(model);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}