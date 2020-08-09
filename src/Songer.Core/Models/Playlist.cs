using System.Collections.Generic;

namespace Songer.Core.Models
{
    public class Playlist : Model
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<Song> Songs { get; set; }
    }
}
