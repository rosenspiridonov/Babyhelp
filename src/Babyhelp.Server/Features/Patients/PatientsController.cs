namespace Babyhelp.Server.Features.Patients
{
    using Infrastructure;
    using Data.Models;

    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;

    using static WebConstants;

    [Authorize]
    public class PatientsController : ApiController
    {
        private readonly IPatientsService patients;
        private readonly UserManager<User> userManager;

        public PatientsController(
            IPatientsService patients,
            UserManager<User> userManager)
        {
            this.patients = patients;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create()
        {
            if (this.User.IsDoctor())
            {
                return BadRequest("Current user is a doctor");
            }

            if (this.User.IsPatient())
            {
                return BadRequest("Current user is already a patient");
            }

            if (!ModelState.IsValid)    
            {
                return BadRequest();
            }

            bool created = await this.patients.Create(this.User.GetId());

            if (!created)
            {
                return BadRequest();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            await this.userManager.AddToRoleAsync(user, PatientRoleName);

            return Ok();
        }
    }
}
