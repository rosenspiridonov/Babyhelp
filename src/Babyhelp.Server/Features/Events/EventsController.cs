namespace Babyhelp.Server.Features.Events
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Babyhelp.Server.Data.Models;
    using Babyhelp.Server.Features.Events.Models;
    using Babyhelp.Server.Features.Identity;
    using Babyhelp.Server.Infrastructure;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class EventsController : ApiController
    {
        private readonly IEventsService events;
        private readonly UserManager<User> userManager;
        private readonly IIdentityService identity;

        public EventsController(
            IEventsService events,
            UserManager<User> userManager,
            IIdentityService identity)
        {
            this.events = events;
            this.userManager = userManager;
            this.identity = identity;
        }

        [HttpPost]
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
            
            // Check if doctorId is valid

            if (!this.identity.IsDoctor(model.DoctorId))
            {
                return BadRequest("Invalid doctor id");
            }

            // Check if patientId is valid
            if (!this.identity.IsPatient(model.PatientId))
            {
                return BadRequest("Invalid patient id");
            }

            bool approved = this.User.IsDoctor() || this.User.IsAdmin();

            var created = await this.events.Create(
                model.Title,
                model.Description,
                model.Start,
                model.End,
                model.DoctorId,
                model.PatientId,
                approved);

            if (!created)
            {
                return BadRequest("Cannot create event");
            }

            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<EventServiceModel> Get(int id)
        {
            var model = this.events.ById(id);

            return model;
        }

        [HttpPut]
        public async Task<IActionResult> Edit(EventRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Event is not valid");
            }

            if (model.Start > model.End)
            {
                return BadRequest("Start time cannot be after end time");
            }

            // Check if doctorId is valid

            if (!this.identity.IsDoctor(model.DoctorId))
            {
                return BadRequest("Invalid doctor id");
            }

            // Check if patientId is valid
            if (!this.identity.IsPatient(model.PatientId))
            {
                return BadRequest("Invalid patient id");
            }

            bool approved = this.User.IsDoctor() || this.User.IsAdmin();

            if (model.Id == null)
            {
                return BadRequest("You must provide an id");
            }

            var modified = await this.events.Edit(
                model.Id.Value,
                model.Title,
                model.Description,
                model.Start,
                model.End,
                model.DoctorId,
                model.PatientId,
                approved);

            if (!modified)
            {
                return BadRequest("Cannot modify event");
            }

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await this.events.Delete(id);

            if (!deleted)
            {
                return BadRequest("Cannot delete event");
            }

            return Ok();
        }

        [HttpGet]
        [Route(nameof(ByDoctor) + "/{id}")]
        public ActionResult<List<EventServiceModel>> ByDoctor(int id)
        {
            var model = this.events.ByDoctor(id);

            if (model == null)
            {
                return BadRequest("Invalid id");
            }

            return model;
        }

        [HttpGet]
        [Route(nameof(ByDoctor))]
        public ActionResult<EventServiceModel> ByDoctor([FromQuery]int id, [FromQuery]DateTime startTime)
        {
            var model = this.events.ByDoctor(id, startTime);

            if (model == null)
            {
                return BadRequest("Invalid id");
            }

            return model;
        }

        [HttpGet]
        [Route(nameof(ByPatient) + "/{id}")]
        public ActionResult<List<EventServiceModel>> ByPatient(int id)
        {
            var model = this.events.ByPatient(id);

            if (model == null)
            {
                return BadRequest("Invalid id");
            }

            return model;
        }

        [HttpGet]
        [Route(nameof(ByPatient))]
        public ActionResult<EventServiceModel> ByPatient([FromQuery] int id, [FromQuery] DateTime startTime)
        {
            var model = this.events.ByPatient(id, startTime);

            if (model == null)
            {
                return BadRequest("Invalid id");
            }

            return model;
        }

        [HttpGet]
        [Route(nameof(ByDoctorAndPatient))]
        public ActionResult<List<EventServiceModel>> ByDoctorAndPatient([FromQuery] int doctorId, [FromQuery] int patientId)
        {
            var model = this.events.ByDoctorAndPatient(doctorId, patientId);

            if (model == null)
            {
                return BadRequest("Invalid id");
            }

            return model;
        }
    }
}
