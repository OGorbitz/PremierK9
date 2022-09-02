using Web.Data;
using Web.Models;

namespace Web.Interfaces
{
    public interface ITokenService
    {
        Task<Tuple<string, string, string, string>> GenerateTokensAsync(string userId);
        Task<ValidateRefreshTokenResponse> ValidateRefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    }
}
