namespace Babyhelp.Server.Infrastructure
{
    using System;
    using System.Threading.Tasks;

    using Babyhelp.Server.Data.Models;

    using Data;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using static WebConstants;

    public static class ApplicationBuilderExtentions
    {
        public static IApplicationBuilder UseSwaggerWithUI(this IApplicationBuilder app)
            => app
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Babyhelp v1");
                    c.RoutePrefix = string.Empty;
                });

        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;
            var dbContext = services.GetRequiredService<ApplicationDbContext>();

            ApplyMigrations(services);

            SeedRoles(services);
            SeedAdministrator(services);

            return app;
        }

        private static void ApplyMigrations(IServiceProvider services)
        {
            var dbContext = services.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }

        private static void SeedRoles(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    await CreateRoleAsync(roleManager, AdminRoleName);
                    await CreateRoleAsync(roleManager, DoctorRoleName);
                    await CreateRoleAsync(roleManager, PatientRoleName);
                })
                .GetAwaiter()
                .GetResult();
        }

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();

            Task
                .Run(async () =>
                {
                    var user = new User()
                    {
                        Email = "admin1@babyhelp.com",
                        UserName = "admin1",
                    };

                    await userManager.CreateAsync(user, "admin1admin1");
                    await userManager.AddToRoleAsync(user, AdminRoleName);
                })
                .GetAwaiter()
                .GetResult();
        }

        private static async Task CreateRoleAsync(
            RoleManager<IdentityRole> roleManager,
            string role)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
