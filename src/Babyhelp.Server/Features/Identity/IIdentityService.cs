namespace Babyhelp.Server.Features.Identity
{
    using System.Collections.Generic;

    public interface IIdentityService
    {
        string GenerateJwtToken(string userId, string usernamem, IList<string> roles, string appSecret);

        bool IsDoctor(int doctorId);

        bool IsPatient(int patientId);
    }
}
