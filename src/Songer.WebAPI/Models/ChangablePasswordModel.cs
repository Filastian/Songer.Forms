using System.ComponentModel.DataAnnotations;

namespace Songer.WebAPI.Models
{
    public class ChangablePasswordModel
    {
        [Required]
        public string Password { get; set; }
    }
}
