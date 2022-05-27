using Microsoft.AspNetCore.Identity;
using Shared;
using Web.Models;

namespace Web.Data
{
    public class AppIdentityUser : IdentityUser
    {
        public List<RefreshToken> RefreshTokens;
        public List<UnitAuthorization> UnitAuthorizations;
    }
}
