namespace Babyhelp.Server.Features.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Data;
    using Data.Models;

    using Features.Events.Models;

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
            int patientId,
            bool approved = false)
        {
            bool isValid = IsEventValid(start, end, doctorId, patientId);

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
                Duration = end.Subtract(start),
                DoctorId = doctorId,
                PatientId = patientId,
                Approved = approved
            };

            await this.dbContext.Events.AddAsync(@event);
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public EventServiceModel ById(int id)
            => this
                .dbContext
                .Events
                .Where(x => x.Id == id)
                .Select(x => new EventServiceModel()
                {
                    Title = x.Title,
                    Description = x.Description,
                    Start = x.Start,
                    End = x.End,
                    DoctorId = x.DoctorId,
                    PatientId = x.PatientId,
                    Approved = x.Approved
                })
                .FirstOrDefault();

        public async Task<bool> Edit(
            int id,
            string title,
            string description,
            DateTime start,
            DateTime end,
            int doctorId,
            int patientId,
            bool approved = false)
        {
            var @event = await this
                .dbContext
                .Events
                .FindAsync(id);

            if (@event == null)
            {
                return false;
            }

            bool isValid = IsEventValid(start, end, doctorId, patientId);

            if (!isValid)
            {
                return false;
            }

            @event.Title = title;
            @event.Description = description;
            @event.Start = start;
            @event.End = end;
            @event.Duration = end.Subtract(start);
            @event.DoctorId = doctorId;
            @event.PatientId = patientId;
            @event.Approved = approved;

            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var @event = await this
                .dbContext
                .Events
                .FindAsync(id);

            if (@event == null)
            {
                return false;
            }

            this.dbContext.Events.Remove(@event);
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public List<EventServiceModel> ByDoctor(int doctorId)
            => this
                .dbContext
                .Events
                .Where(x => x.DoctorId == doctorId)
                .Select(x => new EventServiceModel()
                {
                    Title = x.Title,
                    Description = x.Description,
                    Start = x.Start,
                    End = x.End,
                    DoctorId = x.DoctorId,
                    PatientId = x.PatientId,
                    Approved = x.Approved
                })
                .ToList();

        public EventServiceModel ByDoctor(int doctorId, DateTime startTime)
                => this
                    .dbContext
                    .Events
                    .Where(x => x.DoctorId == doctorId && x.Start == startTime)
                    .Select(x => new EventServiceModel()
                    {
                        Title = x.Title,
                        Description = x.Description,
                        Start = x.Start,
                        End = x.End,
                        DoctorId = x.DoctorId,
                        PatientId = x.PatientId,
                        Approved = x.Approved
                    })
                    .FirstOrDefault();

        public List<EventServiceModel> ByDoctorAndPatient(int doctorId, int patientId)
                => this
                    .dbContext
                    .Events
                    .Where(x => x.DoctorId == doctorId && x.PatientId == patientId)
                    .Select(x => new EventServiceModel()
                    {
                        Title = x.Title,
                        Description = x.Description,
                        Start = x.Start,
                        End = x.End,
                        DoctorId = x.DoctorId,
                        PatientId = x.PatientId,
                        Approved = x.Approved
                    })
                    .ToList();

        public List<EventServiceModel> ByPatient(int patientId)
                => this
                    .dbContext
                    .Events
                    .Where(x => x.PatientId == patientId)
                    .Select(x => new EventServiceModel()
                    {
                        Title = x.Title,
                        Description = x.Description,
                        Start = x.Start,
                        End = x.End,
                        DoctorId = x.DoctorId,
                        PatientId = x.PatientId,
                        Approved = x.Approved
                    })
                    .ToList();

        public EventServiceModel ByPatient(int patientId, DateTime startTime)
                 => this
                    .dbContext
                    .Events
                    .Where(x => x.PatientId == patientId && x.Start == startTime)
                    .Select(x => new EventServiceModel()
                    {
                        Title = x.Title,
                        Description = x.Description,
                        Start = x.Start,
                        End = x.End,
                        DoctorId = x.DoctorId,
                        PatientId = x.PatientId,
                        Approved = x.Approved
                    })
                    .FirstOrDefault();

        // TODO: Test if this works correctly
        private bool IsEventValid(DateTime start, DateTime end, int doctorId, int patientId)
        {
            var doctorsEvents = this
                .dbContext
                .Events
                .Where(e => e.DoctorId == doctorId)
                .ToList();

            if (doctorsEvents.All(e => (start <= e.Start && end <= e.Start) || (start >= e.End && end >= e.End)))
            {
                return true;
            }

            return false;
        }
    }
}
