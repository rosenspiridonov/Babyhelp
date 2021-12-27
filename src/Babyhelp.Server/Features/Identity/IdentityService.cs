namespace Babyhelp.Server.Features.Identity
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using Babyhelp.Server.Data;
    using Babyhelp.Server.Data.Models;

    using Microsoft.IdentityModel.Tokens;

    public class IdentityService : IIdentityService
    {
        private readonly ApplicationDbContext dbContext;

        public IdentityService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public string GenerateJwtToken(string userId, string username, IList<string> roles, string appSecret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            foreach (var role in roles)
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool IsDoctor(int doctorId)
        {
            var doctor = this
                .dbContext
                .Doctors
                .FirstOrDefault(x => x.Id == doctorId);

            if (doctor == null)
            {
                return false;
            }

            return true;
        }

        public bool IsPatient(int patientId)
        {
            var patient = this
                .dbContext
                .Patients
                .FirstOrDefault(x => x.Id == patientId);

            if (patient == null)
            {
                return false;
            }

            return true;
        }
    }
}
