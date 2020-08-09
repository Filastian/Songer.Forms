using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Songer.WebAPI.Models
{
    public class CreateSongModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Performer { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}