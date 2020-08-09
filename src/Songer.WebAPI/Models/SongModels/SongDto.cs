namespace Songer.WebAPI.Models
{
    public class SongDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Performer { get; set; }

        public SongDto(Song song)
        {
            Id = song.Id;
            Title = song.Title;
            Performer = song.Performer;
        }
    }
}
