using Web.Data;
using Web.Models;

namespace Web.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResponse> GenerateTokensAsync(AppIdentityUser user);
    }
}
