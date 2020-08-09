using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Songer.WebAPI.Models
{
    public class User : Entity
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Role { get; set; }

        [NotMapped]
        public string Token { get; set; }

        public List<UserSong> UserSongs { get; set; }
        public List<Playlist> Playlists { get; set; }

        public User()
        {
            UserSongs = new List<UserSong>();
        }
    }
}
