using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Web.Data
{
    public class DatabaseInitializer
    {
        public static void CreateOrMigrate(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    //var context = services.GetRequiredService<AppDbContext>();
                    var identityContext = services.GetRequiredService<AppDbContext>();
                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<AppIdentityUser>>();
                    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                    //context.Database.Migrate();
                    identityContext.Database.Migrate();

                    var adminRole = new IdentityRole("Admin");
                    if (!identityContext.Roles.Any())
                    {
                        roleMgr.CreateAsync(adminRole).GetAwaiter().GetResult();
                    }

                    if (!identityContext.Users.Any())
                    {
                        var adminUser = new AppIdentityUser
                        {
                            UserName = "admin@test.com",
                            Email = "admin@test.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false
                        };
                        var result = userMgr.CreateAsync(adminUser, "password").GetAwaiter().GetResult();
                        var result2 = userMgr.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }

            }
        }
    }
}
