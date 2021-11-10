namespace Babyhelp.Server.Infrastructure
{
    using System.Linq;
    using System.Security.Claims;

    public static class IdentityExtentions
    {
        public static string GetId(this ClaimsPrincipal user)
            => user
                .Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
                ?.Value;
    }
}
