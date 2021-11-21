namespace Babyhelp.Server.Infrastructure
{
    using System.Linq;
    using System.Security.Claims;

    using Babyhelp.Server.Data;
    using Babyhelp.Server.Data.Models;

    using Microsoft.AspNetCore.Identity;

    using static WebConstants;

    public static class IdentityExtentions
    {
        public static string GetId(this ClaimsPrincipal user)
            => user
                .Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
                ?.Value;

        public static bool IsDoctor(this ClaimsPrincipal user)
            => user
                .IsInRole(DoctorRoleName);

        public static bool IsPatient(this ClaimsPrincipal user)
            => user
                .IsInRole(PatientRoleName);

        public static bool IsAdmin(this ClaimsPrincipal user)
           => user
               .IsInRole(AdminRoleName);
    }
}
