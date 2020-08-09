using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Songer.WebAPI.Helpers;
using Songer.WebAPI.Models;
using Songer.WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TestAuth.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly ISongRepository _songRepository;
        private readonly IUserRepository _userRepository;

        public SongsController(
            ISongRepository songRepository,
            IUserRepository userRepository)
        {
            _songRepository = songRepository;
            _userRepository = userRepository;
        }

        // GET: .../songs
        [HttpGet]   
        public async Task<ActionResult<IEnumerable<SongDto>>> GetAllSongs()
        {
            var songs = await _songRepository.GetAllSongsAsync();

            return Ok(songs.ToSongDtosList());
        }

        // GET: .../songs/{id}
        [HttpGet("{songId}")]
        public async Task<FileCallbackResult> GetSong(int songId)
        {
            var song = await _songRepository.GetSongAsync(songId);

            return new FileCallbackResult(new MediaTypeHeaderValue("audio/mpeg"), async (outputStream, _) =>
            {
                var audio = new HttpFileStream(song.Path);

                audio.WriteToStream(outputStream, _);
            })
            {
                FileDownloadName = "temp.mp3"
            };
        }

        // GET: .../songs/my
        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<SongDto>>> GetUserSongs()
        {
            int userId = GetUserId();
            var songs = await _userRepository.GetAllUserSongsAsync(userId);

            return Ok(songs.ToSongDtosList());
        }

        // POST: .../songs/add
        [HttpPost("add")]
        public async Task<ActionResult<SongDto>> AddNewSong([FromForm]CreateSongModel model)
        {
            try
            {
                int userId = GetUserId();
                var song = await _songRepository.AddAsync(model);

                await _userRepository.AddSongToUserAsync(userId, song.Id);

                return Ok(new SongDto(song));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: .../songs/my/add/{id}
        [HttpPost("my/add/{songId}")]
        public async Task<ActionResult> AddSongToUser(int songId)
        {
            try
            {
                int userId = GetUserId();
                await _userRepository.AddSongToUserAsync(userId, songId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message + songId });
            }
        }

        // PUT: .../songs/edit/{id}
        [Authorize(Roles = Role.Admin)]
        [HttpPut("edit/{songId}")]
        public async Task<ActionResult<SongDto>> UpdateSong([FromBody]UpdateSongModel model, int songId)
        {
            try
            {
                var song = await _songRepository.UpdateAsync(model, songId);

                return Ok(new SongDto(song));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: .../songs/my/delete/{id}
        [HttpDelete("my/delete/{songId}")]
        public async Task<ActionResult> DeleteUserSong(int songId)
        {
            try
            {
                int userId = GetUserId();
                await _userRepository.DeleteUserSongAsync(userId, songId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: .../songs/delete/{id}
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("delete/{songId}")]
        public async Task<ActionResult> DeleteSong(int songId)
        {
            try
            {
                await _songRepository.DeleteSongAsync(songId);

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