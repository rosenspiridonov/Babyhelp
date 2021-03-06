namespace Babyhelp.Server.Infrastructure
{
    using Features.Events;
    using Features.Doctors;
    using Features.Patients;
    using Features.Identity;

    using System.Text;

    using Data;
    using Data.Models;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;

    public static class ServiceCollectionExtentions
    {
        public static AppSettings GetAppSettings(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("ApplicationSettings");
            services.Configure<AppSettings>(appSettingsSection);
            return appSettingsSection.Get<AppSettings>();
        }

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
               .AddIdentity<User, IdentityRole>(options =>
               {
                   options.Password.RequiredLength = 6;
                   options.Password.RequireDigit = false;
                   options.Password.RequireNonAlphanumeric = false;
                   options.Password.RequireUppercase = false;
                   options.Password.RequireLowercase = false;
               })
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = false;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services
                .AddTransient<IIdentityService, IdentityService>()
                .AddTransient<IEventsService, EventsService>()
                .AddTransient<IDoctorsService, DoctorsService>()
                .AddTransient<IPatientsService, PatientsService>();

            return services;
        }

    }
}
