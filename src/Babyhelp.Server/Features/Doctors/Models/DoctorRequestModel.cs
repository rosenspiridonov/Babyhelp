namespace Babyhelp.Server.Features.Doctors.Models
{
    using System.ComponentModel.DataAnnotations;

    public class DoctorRequestModel
    {
        [Required]
        public string UserId { get; set; }
    }
}
