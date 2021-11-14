namespace Babyhelp.Server.Features.Doctors
{
    using Features.Doctors.Models;
    using Data.Models;
    using Infrastructure;

    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;

    using static WebConstants;

    [Authorize]
    public class DoctorsController : ApiController
    {
        private readonly IDoctorsService doctors;
        private readonly UserManager<User> userManager;

        public DoctorsController(
            IDoctorsService doctors,
            UserManager<User> userManager)
        {
            this.doctors = doctors;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create(DoctorRequestModel model)
        {
            if (this.User.IsDoctor())
            {
                return BadRequest("Current user is already a doctor");
            }

            if (this.User.IsPatient())
            {
                return BadRequest("Current user is a patient");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            bool created = await this.doctors.Create(this.User.GetId());

            if (!created)
            {
                return BadRequest();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            await this.userManager.AddToRoleAsync(user, DoctorRoleName);

            return Ok();
        }
    }
}
