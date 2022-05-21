using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private UserManager<IdentityUser> _userManager;
        private JwtHandler _jwtHandler;

        public AuthController(UserManager<IdentityUser> userManager, JwtHandler jwtHandler)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }

        public async Task<IActionResult> Login([FromBody] LoginRequest userAuthentication)
        {
            if (userAuthentication == null)
                return BadRequest();
            var user = await _userManager.FindByEmailAsync(userAuthentication.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, userAuthentication.Password))
                return Unauthorized(new LoginResult { ErrorMessage = "Invalid Authentication" });

            var token = await _jwtHandler.GenerateAccessToken(user.Id);

            return Ok(new LoginResult { Token = token });
        }
    }
}
