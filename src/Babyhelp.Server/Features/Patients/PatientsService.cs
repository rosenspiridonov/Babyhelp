namespace Babyhelp.Server.Features.Patients
{
    using System.Linq;
    using System.Threading.Tasks;

    using Data;
    using Data.Models;

    public class PatientsService : IPatientsService
    {
        private readonly ApplicationDbContext dbContext;

        public PatientsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> Create(string userId)
        {
            //if (!this.dbContext.Users.Any(x => x.Id == userId))
            //{
            //    return false;
            //}

            var patient = new Patient()
            {
                UserId = userId
            };

            await this.dbContext.Patients.AddAsync(patient);
            await this.dbContext.SaveChangesAsync();

            return true;
        }
    }
}
