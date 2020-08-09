using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Songer.WebAPI.Helpers;
using Songer.WebAPI.Models;
using Songer.WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TestAuth.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: .../users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();

            return Ok(users.ToUserDtosList());
        }

        // GET: .../users/{id}
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDto>> GetUser(int userId)
        {
            try
            {
                var user = await _userRepository.GetAsync(userId);

                return Ok(new UserDto(user));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: .../users/edit
        [HttpPut("edit")]
        public async Task<ActionResult> PutUser([FromBody]UpdateUserModel model)
        {
            try
            {
                var userId = GetUserId();
                await _userRepository.UpdateAsync(model, userId);

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: .../users/delete/{id}
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("delete/{userId}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            try
            {
                await _userRepository.DeleteAsync(userId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: .../users/password
        [HttpPost("password")]
        public async Task<ActionResult<bool>> IsCorrectPassword([FromBody]ChangablePasswordModel model)
        {
            var userId = GetUserId();
            var user = await _userRepository.GetAsync(userId);

            return Ok(user.Password == model.Password);
        }

        // Return user id from tokens
        private int GetUserId()
        {
            return Convert.ToInt32((User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name).Value);
        }
    }
}
