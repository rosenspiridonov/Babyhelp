namespace Babyhelp.Server.Features.Doctors
{
    using System.Linq;
    using System.Threading.Tasks;

    using Babyhelp.Server.Data;
    using Babyhelp.Server.Data.Models;

    public class DoctorsService : IDoctorsService
    {
        private readonly ApplicationDbContext dbContext;

        public DoctorsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> Create(string userId)
        {
            if (!this.dbContext.Users.Any(x => x.Id == userId))
            {
                return false;
            }

            var doctor = new Doctor()
            {
                UserId = userId
            };

            await this.dbContext.Doctors.AddAsync(doctor);
            await this.dbContext.SaveChangesAsync();

            return true;
        }
    }
}
