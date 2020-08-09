namespace Songer.WebAPI.Models
{
    public class UserDto
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }

        public UserDto(User user)
        {
            Login = user.Login;
            Name = user.Name;
            Role = user.Role;
            Token = user.Token;
        }
    }
}
