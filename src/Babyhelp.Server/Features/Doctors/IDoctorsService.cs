namespace Babyhelp.Server.Features.Doctors
{
    using System.Threading.Tasks;

    public interface IDoctorsService
    {
        Task<bool> Create(string userId);
    }
}
