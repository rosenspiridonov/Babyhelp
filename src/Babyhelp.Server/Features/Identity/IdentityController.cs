namespace Babyhelp.Server.Features.Identity
{
    using Data.Models;
    using Models;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;

    using static WebConstants;
    using Babyhelp.Server.Features.Patients;
    using Babyhelp.Server.Infrastructure;
    using System.Linq;

    public class IdentityController : ApiController
    {
        private readonly UserManager<User> userManager;
        private readonly IIdentityService identity;
        private readonly IPatientsService patients;
        private readonly AppSettings appSettings;

        public IdentityController(
            UserManager<User> userManager,
            IIdentityService identity,
            IOptions<AppSettings> appSettings,
            IPatientsService patients)
        {
            this.userManager = userManager;
            this.identity = identity;
            this.patients = patients;
            this.appSettings = appSettings.Value;
        }

        [HttpPost]
        [Route(nameof(Register))]
        public async Task<IActionResult> Register(RegisterRequestModel model)
        {
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await this.userManager.CreateAsync(user, model.Password);

            if (result.Errors.Any())
            {
                return BadRequest(result.Errors);
            }

            bool created = await this.patients.Create(user.Id);

            if (!created)
            {
                return BadRequest("Error in creating patient");
            }

            await this.userManager.AddToRoleAsync(user, PatientRoleName);
            return Ok();
        }

        [HttpPost]
        [Route(nameof(Login))]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel model)
        {
            var user = await this.userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                return Unauthorized();
            }

            var passwordValid = await this.userManager.CheckPasswordAsync(user, model.Password);

            if (!passwordValid)
            {
                return Unauthorized();
            }

            var roles = await userManager.GetRolesAsync(user);

            var encryptedToken = this.identity.GenerateJwtToken(
                user.Id, 
                user.UserName,
                roles,
                this.appSettings.Secret);

            return new LoginResponseModel
            {
                Token = encryptedToken
            };
        }
    }
}
