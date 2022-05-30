using Microsoft.AspNetCore.Identity;
using Data;
using Web.Models;

namespace Web.Data
{
    public class AppIdentityUser : IdentityUser
    {
        public List<RefreshToken> RefreshTokens;
        public List<UnitAuth> UnitAuths;
    }
}
