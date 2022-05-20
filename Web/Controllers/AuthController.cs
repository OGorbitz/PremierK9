using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class AuthController : Controller
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

            var signingCredentials = _jwtHandler.GetSigningCredentials();
            var claims = _jwtHandler.GetClaims(user);
            var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new LoginResult { Token = token });
        }
    }
}
