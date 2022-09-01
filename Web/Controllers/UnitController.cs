using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Web.Data;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class UnitController : ControllerBase
    {
        private readonly ILogger<UnitController> _logger;
        private readonly AppDbContext _dbContext;
        private readonly UserManager<AppIdentityUser> _userManager;

        public UnitController(ILogger<UnitController> logger, AppDbContext dbContext, UserManager<AppIdentityUser> userManager)
        {
            _logger = logger;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> GetUnit(string id)
        {
            if (Request.HttpContext.User.Identity == null || Request.HttpContext.User.Identity.Name == null)
                return Unauthorized();

            var unitId = Guid.Parse(id);
            var unit = _dbContext.Units
                .Include(u => u.Auths)
                .ThenInclude(a => a.User)
                .First(u => u.ID == unitId);

            var userId = Request.HttpContext.User.Identity.Name;


            var auth = AuthType.NONE;
            
            if (unit.Auths.Count() <= 0)
                return Unauthorized();

            
            foreach (var a in unit.Auths)
            {
                if (a.User.Id == userId)
                    auth = a.AuthType;
            }

            if (auth == AuthType.OWNER || auth == AuthType.VIEWER || auth == AuthType.USER)
            {
                return Ok(unit);
            }

            return Unauthorized();

            
        }

        public async Task<IActionResult> GetUnits()
        {
            if (Request.HttpContext.User.Identity == null || Request.HttpContext.User.Identity.Name == null)
                return Unauthorized();

            var rand = new Random();
            var units = new List<Unit>();

            var userId = Request.HttpContext.User.Identity.Name;
            var user = _userManager.Users
                .Include(u => u.UnitAuths)
                .ThenInclude(a => a.Unit)
                .First(u => u.Id == userId);

            if(user != null && user.UnitAuths.Count == 0)
            {
                var u = new Unit()
                {
                    Name = "Seeded Unit",
                    Temperature = rand.Next(50, 120),
                    UnitStatus = global::Data.Status.CLOSED,
                    FanStatus = true
                };
                var a = new UnitAuth()
                {
                    Unit = u,
                    User = user,
                    AuthType = AuthType.OWNER,
                    Id = Guid.NewGuid()
                };
                user.UnitAuths = new List<UnitAuth>();
                user.UnitAuths.Add(a);

                _dbContext.Users.Update(user);
                _dbContext.Units.Add(u);
                _dbContext.UnitAuths.Add(a);
                await _dbContext.SaveChangesAsync();
            }

            if(user != null && user.UnitAuths != null)
                user.UnitAuths.ForEach(a =>
                {
                    if (a.User == user && a.AuthType != AuthType.NONE)
                        units.Add(a.Unit);
                });

            return Ok(units.ToArray());

        }

        public async Task<IActionResult> OpenUnit(string id)
        {
            if (Request.HttpContext.User.Identity == null || Request.HttpContext.User.Identity.Name == null)
                return Unauthorized();

            var unitId = Guid.Parse(id);
            var unit = _dbContext.Units
                .Include(u => u.Auths)
                .ThenInclude(a => a.User)
                .First(u => u.ID == unitId);

            var userId = Request.HttpContext.User.Identity.Name;


            var auth = AuthType.NONE;

            if (unit.Auths.Count() <= 0)
                return Unauthorized();


            foreach (var a in unit.Auths)
            {
                if (a.User.Id == userId)
                    auth = a.AuthType;
            }

            if (auth == AuthType.OWNER || auth == AuthType.USER)
            {
                //Do Opening Logic Here!
                return Ok(unit);
            }

            return Unauthorized();
        }
    }
}
