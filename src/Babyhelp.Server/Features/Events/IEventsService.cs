namespace Babyhelp.Server.Features.Events
{
    using System;
    using System.Threading.Tasks;

    public interface IEventsService
    {
        Task<bool> Create(string title, string description, DateTime start, DateTime end, int doctorId, int patientId);
    }
}
