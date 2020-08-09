namespace Songer.Core.Models
{
    public class User : Model
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
