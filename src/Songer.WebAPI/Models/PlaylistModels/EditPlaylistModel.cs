using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Songer.WebAPI.Models
{
    public class EditPlaylistModel
    {
        [Required]
        public string Title { get; set; }

        public List<int> SongIds { get; set; }
    }
}
