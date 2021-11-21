namespace Babyhelp.Server.Features.Events
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Babyhelp.Server.Features.Events.Models;

    public interface IEventsService
    {
        Task<bool> Create(
            string title, 
            string description, 
            DateTime start, 
            DateTime end, 
            int doctorId, 
            int patientId, 
            bool approved = false);

        EventServiceModel ById(int id);

        Task<bool> Edit(int id, 
            string title, 
            string description, 
            DateTime start, 
            DateTime end, 
            int doctorId, 
            int patientId, 
            bool approved = false);

        Task<bool> Delete(int id);

        List<EventServiceModel> ByDoctor(int doctorId);

        EventServiceModel ByDoctor(int doctorId, DateTime startTime);

        List<EventServiceModel> ByPatient(int patientId);

        EventServiceModel ByPatient(int patientId, DateTime startTime);

        List<EventServiceModel> ByDoctorAndPatient(int doctorId, int patientId);
    }
}
