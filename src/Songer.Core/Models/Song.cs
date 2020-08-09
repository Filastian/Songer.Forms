namespace Songer.Core.Models
{
    public class Song : Model
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Performer { get; set; }
    }
}
