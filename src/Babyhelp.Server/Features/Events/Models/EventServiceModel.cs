namespace Babyhelp.Server.Features.Events.Models
{
    using System;

    public class EventServiceModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public bool Approved { get; set; }
    }
}
