using System.ComponentModel.DataAnnotations;

namespace Songer.WebAPI.Models
{
    public class RegisterUserModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public string Name { get; set; }
    }
}
