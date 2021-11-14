namespace Babyhelp.Server.Features.Events
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Babyhelp.Server.Data;
    using Babyhelp.Server.Data.Models;

    public class EventsService : IEventsService
    {
        private readonly ApplicationDbContext dbContext;

        public EventsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> Create(
            string title, 
            string description, 
            DateTime start, 
            DateTime end, 
            int doctorId, 
            int patientId)
        {
            var isValid = !this.
                dbContext.
                Events.
                Any(x => (x.Start > start && x.Start < end
                    || x.End > start && x.End < end)
                    && (x.DoctorId == doctorId || x.PatientId == patientId));

            if (!isValid)
            {
                return false;
            }

            var @event = new Event()
            {
                Title = title,
                Description = description,
                Start = start,
                End = end,
                Duration = start.Subtract(end),
                DoctorId = doctorId,
                PatientId = patientId
            };

            await this.dbContext.Events.AddAsync(@event);
            await this.dbContext.SaveChangesAsync();

            return true;
        }
    }
}
