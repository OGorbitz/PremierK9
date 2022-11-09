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
            try
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

                var tokenResponse = await _tokenService.GenerateTokensAsync(user);
                return Ok(tokenResponse);
                
            } catch(Exception e) {
                return Problem(e.Message, statusCode: 500, title: e.Source);
            }


        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                if (refreshTokenRequest == null || string.IsNullOrEmpty(refreshTokenRequest.RefreshToken) || refreshTokenRequest.UserId == null || refreshTokenRequest.UserId == "")
                {
                    return BadRequest(new TokenResponse
                    {
                        Error = "Missing refresh token details",
                        ErrorCode = "R01"
                    });
                }

                var refreshToken = await _dbContext.RefreshTokens.Include(o => o.User).FirstOrDefaultAsync(e => e.UserId == refreshTokenRequest.UserId);

                var response = new TokenResponse()
                {
                    Success = false
                };

                if (refreshToken == null)
                {
                    response.Success = false;
                    response.Error = "Invalid session or user is already logged out";
                    response.ErrorCode = "R02";
                    return Unauthorized(response);
                }

                if (refreshToken.ExpiryDate < DateTime.Now)
                {
                    response.Success = false;
                    response.Error = "Refresh token has expired";
                    response.ErrorCode = "R03";
                    return Unauthorized(response);
                }

                var refreshTokenToValidateHash = PasswordHelper.HashUsingPbkdf2(refreshTokenRequest.RefreshToken, Convert.FromBase64String(refreshToken.TokenSalt));

                if (refreshToken.TokenHash != refreshTokenToValidateHash)
                {
                    response.Success = false;
                    response.Error = "Invalid refresh token";
                    response.ErrorCode = "R04";
                    return Unauthorized(response);
                }

                _dbContext.RefreshTokens.Remove(refreshToken);

                response.Success = true;
                response.UserId = refreshToken.UserId;


                var user = refreshToken.User;

                var tokenResponse = await _tokenService.GenerateTokensAsync(user);
                return Ok(tokenResponse);
            }
            catch (Exception e)
            {
                return Problem(detail: e.Message + "\n" + e.InnerException, statusCode: 500, title: e.Source);
            }
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
