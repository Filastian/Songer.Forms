using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Songer.WebAPI.Models
{
    public class CreatePlaylistModel
    {
        [Required]
        public string Title { get; set; }

        public IEnumerable<int> SongIds { get; set; }
    }
}
