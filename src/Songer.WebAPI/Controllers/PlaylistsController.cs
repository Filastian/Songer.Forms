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
    public class PlaylistsController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPlaylistRepository _playlistRepository;

        public PlaylistsController(
            IUserRepository userRepository,
            IPlaylistRepository playlistRepository)
        {
            _userRepository = userRepository;
            _playlistRepository = playlistRepository;
        }

        // GET: .../playlists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistDto>>> GetAllUserPlaylists()
        {
            int userId = GetUserId();
            var playlists = await _userRepository.GetAllUserPlaylistsAsync(userId);

            return Ok(playlists.ToPlaylistDtosList());
        }

        // GET: .../playlists/{id}
        [HttpGet("{playlistId}")]
        public async Task<ActionResult<PlaylistDto>> GetPlaylistDetail(int playlistId)
        {
            var playlist = await _playlistRepository.GetPlaylistAsync(playlistId);

            return Ok(new PlaylistDto(playlist));
        }

        // POST: .../playlists/add
        [HttpPost("add")]
        public async Task<ActionResult<PlaylistDto>> AddPlaylist([FromBody]CreatePlaylistModel model)
        {
            try
            {
                int userId = GetUserId();
                var playlist = await _playlistRepository.AddAsync(model, userId);

                return Ok(new PlaylistDto(playlist));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: .../playlists/edit/{id}
        [HttpPut("edit/{playlistId}")]
        public async Task<ActionResult<PlaylistDto>> UpdatePlaylist([FromBody]EditPlaylistModel model, int playlistId)
        {
            try
            {
                int userId = GetUserId();
                var playlist = await _playlistRepository.UpdateAsync(model, playlistId, userId);

                return Ok(new PlaylistDto(playlist));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: .../playlists/delete/{id}
        [HttpDelete("delete/{playlistId}")]
        public async Task<ActionResult> DeleteUserPlaylist(int playlistId)
        {
            try
            {
                int userId = GetUserId();
                await _playlistRepository.DeleteAsync(playlistId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Return user id from tokens
        private int GetUserId()
        {
            return Convert.ToInt32((User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name).Value);
        }
    }
}