using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Web.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //Fetch user roles from identity core
        public async Task Invoke(HttpContext context, UserManager<IdentityUser> userManager)
        {
            var user = await userManager.FindByIdAsync(context.User.Identity.Name);

            if (user != null)
            {

                var roles = await userManager.GetRolesAsync(user);
                var claims = new List<Claim>();

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                var appIdentity = new ClaimsIdentity(claims);
                context.User.AddIdentity(appIdentity);
            }


            await _next(context);
        }
    }

    public static class JwtMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomJwtMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtMiddleware>();
        }
    }
}
