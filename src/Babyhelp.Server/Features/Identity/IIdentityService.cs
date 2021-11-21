namespace Babyhelp.Server.Features.Identity
{
    public interface IIdentityService
    {
        string GenerateJwtToken(string userId, string username, string appSecret);

        bool IsDoctor(int doctorId);

        bool IsPatient(int patientId);
    }
}
