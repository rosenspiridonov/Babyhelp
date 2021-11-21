namespace Babyhelp.Server.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Event;

    public class Event
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [MaxLength(DecriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        public int PatientId { get; set; }

        public Patient Patient { get; set; }

        public int DoctorId { get; set; }

        public Doctor Doctor { get; set; }

        public bool Approved { get; set; }
    }
}
