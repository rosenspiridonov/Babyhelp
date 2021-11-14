namespace Babyhelp.Server.Features.Events
{
    using System.Threading.Tasks;

    using Babyhelp.Server.Features.Events.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class EventsController : ApiController
    {
        private readonly IEventsService events;

        public EventsController(IEventsService events)
        {
            this.events = events;
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create(EventRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Event is not valid");
            }

            if (model.Start > model.End)
            {
                return BadRequest("Start time cannot be after end time");
            }

            var created = await this.events.Create(
                model.Title,
                model.Description,
                model.Start,
                model.End,
                model.DoctorId,
                model.PatientId);

            if (!created)
            {
                return BadRequest("Cannot create event");
            }

            return Ok();
        }
    }
}
