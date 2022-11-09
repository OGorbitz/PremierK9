using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Web.Data;
using Web.Models;
using Web.Helpers;

namespace Web.Interfaces
{
    public class TokenService : ITokenService
    {
        private AppDbContext _dbContext;
        private TokenHelper _tokenHelper;

        public TokenService(AppDbContext dbContext, TokenHelper tokenHelper)
        {
            _dbContext = dbContext;
            _tokenHelper = tokenHelper;
        }
        
        public async Task<TokenResponse> GenerateTokensAsync(AppIdentityUser user)
        {
            var accessToken = await _tokenHelper.GenerateAccessToken(user.Id);
            var refreshToken = await _tokenHelper.GenerateRefreshToken();

            var userRecord = await _dbContext.Users.Include(o => o.RefreshTokens).FirstOrDefaultAsync(e => e.Id == user.Id);

            if (userRecord == null)
            {
                return null;
            }

            var salt = PasswordHelper.GetSecureSalt();

            var refreshTokenHashed = PasswordHelper.HashUsingPbkdf2(refreshToken, salt);

            if (userRecord.RefreshTokens.Any())
            {
                _dbContext.RemoveRange(userRecord.RefreshTokens.FindAll(item => item.ExpiryDate < DateTime.Now));
            }

            var rt = new RefreshToken
            {
                ExpiryDate = DateTime.Now.AddDays(30),
                Ts = DateTime.Now,
                User = userRecord,
                UserId = user.Id,
                TokenHash = refreshTokenHashed,
                TokenSalt = Convert.ToBase64String(salt)
            };
            
            userRecord.RefreshTokens.Add(rt);

            _dbContext.SaveChanges();

            return new TokenResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = user.Id,
                UserName = userRecord.UserName
            };
        }
    }
}
