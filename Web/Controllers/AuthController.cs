using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Web.Models;
using Web.Helpers;
using Web.Interfaces;
using Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private UserManager<AppIdentityUser> _userManager;
        private TokenService _tokenService;
        private AppDbContext _dbContext;

        public AuthController(UserManager<AppIdentityUser> userManager, TokenService tokenService, AppDbContext dbContext)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (loginRequest == null)
                return BadRequest(new TokenResponse
                {
                    Success = false,
                    Error = "Please enter email and password!",
                    ErrorCode = "L02"
                });
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
                return Unauthorized(new TokenResponse
                {
                    Success = false,
                    Error = "Invalid Email or Password!",
                    ErrorCode = "L02"
                });

            var token = await System.Threading.Tasks.Task.Run(() => _tokenService.GenerateTokensAsync(user.Id));

            return Ok(new TokenResponse
            {
                Success = true,
                AccessToken = token.Item1,
                RefreshToken = token.Item2,
                UserName = token.Item3,
                UserId = token.Item4
            });

        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            if (refreshTokenRequest == null || string.IsNullOrEmpty(refreshTokenRequest.RefreshToken) || refreshTokenRequest.UserId == null || refreshTokenRequest.UserId == "")
            {
                return BadRequest(new TokenResponse
                {
                    Error = "Missing refresh token details",
                    ErrorCode = "R01"
                });
            }

            var validateRefreshTokenResponse = await _tokenService.ValidateRefreshTokenAsync(refreshTokenRequest);

            if (!validateRefreshTokenResponse.Success)
            {
                return Unauthorized(validateRefreshTokenResponse);
            }

            var token = await _tokenService.GenerateTokensAsync(validateRefreshTokenResponse.UserId);

            return Ok(new TokenResponse
            {
                Success = true,
                AccessToken = token.Item1,
                RefreshToken = token.Item2,
                UserName = token.Item3,
                UserId = token.Item4
            });
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string userId)
        {
            var refreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(o => o.UserId == userId);
            if (refreshToken == null)
            {
                return Ok(new LogoutResponse { Success = true });
            }

            _dbContext.RefreshTokens.Remove(refreshToken);

            var saveResponse = await _dbContext.SaveChangesAsync();

            if (saveResponse >= 0)
            {
                return Ok(new LogoutResponse { Success = true });
            }

            return StatusCode(500, new LogoutResponse { Success = false, Error = "Unable to logout user", ErrorCode = "L04" });

        }
    }
}
