namespace Babyhelp.Server.Features.Patients
{
    using System.Threading.Tasks;

    public interface IPatientsService
    {
        Task<bool> Create(string userId);
    }
}
