using System.Security.Claims;
using System.Security.Principal;

namespace ArticoleCalarie.Web.Utils
{
    public static class IdentityExtensions
    {
        public static string GetUserFullName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("FullName");

            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}