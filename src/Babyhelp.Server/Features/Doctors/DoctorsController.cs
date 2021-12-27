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
    using Babyhelp.Server.Features.Identity;

    [Authorize]
    public class DoctorsController : ApiController
    {
        private readonly IDoctorsService doctors;
        private readonly UserManager<User> userManager;
        private readonly IIdentityService identity;

        public DoctorsController(
            IDoctorsService doctors,
            UserManager<User> userManager,
            IIdentityService identity)
        {
            this.doctors = doctors;
            this.userManager = userManager;
            this.identity = identity;
        }

        [HttpPost]
        [Route(nameof(Create) + "/" + "{userId}")]
        [Authorize(Roles = AdminRoleName)]
        public async Task<IActionResult> Create(/*DoctorRequestModel model*/string userId)
        {
            var inputUser = await this.userManager.FindByIdAsync(userId);

            if (inputUser == null)
            {
                return BadRequest("User not found");
            }

            var isDoctor = await this.userManager.IsInRoleAsync(inputUser, DoctorRoleName);
           
            if (isDoctor)
            {
                return BadRequest("This user is alredy a doctor");
            }

            bool created = await this.doctors.Create(userId);

            if (!created)
            {
                return BadRequest();
            }

            var user = await this.userManager.FindByIdAsync(userId);
            await this.userManager.AddToRoleAsync(user, DoctorRoleName);

            return Ok();
        }
    }
}
