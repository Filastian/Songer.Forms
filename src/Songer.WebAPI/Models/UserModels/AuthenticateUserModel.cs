using System.ComponentModel.DataAnnotations;

namespace Songer.WebAPI.Models
{
    public class AuthenticateUserModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
