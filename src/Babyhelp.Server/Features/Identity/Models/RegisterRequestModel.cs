namespace Babyhelp.Server.Features.Identity.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterRequestModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
