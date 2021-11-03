namespace Homeohelp.Server.Infrastructure
{

    using Microsoft.Extensions.Configuration;

    public static class ConfigurationExtentions
    {
        public static string GetDefaultConnectionString(this IConfiguration configuration)
            => configuration.GetConnectionString("DefaultConnection");
    }
}
