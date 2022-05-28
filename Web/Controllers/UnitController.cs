using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using Web.Data;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UnitController : ControllerBase
    {
        private readonly ILogger<UnitController> _logger;
        private readonly AppDbContext _dbContext;

        public UnitController(ILogger<UnitController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public Unit GetUnit(string id)
        {
            var unitId = Guid.Parse(id);
            return new Unit()
            {
                ID = unitId,
                Name = "Unit K87",
                Temperature = 102.5f,
                UnitStatus = Shared.Status.CLOSED,
                FanStatus = true
            };
        }

        public IEnumerable<Unit> GetUnits()
        {
            var rand = new Random();
            var units = new List<Unit>();
            units.Add(new Unit()
            {
                Name = "Unit K87",
                Temperature = rand.Next(50, 120),
                UnitStatus = Shared.Status.CLOSED,
                FanStatus = true
            });
            units.Add(new Unit()
            {
                Name = "Unit K98",
                Temperature = rand.Next(50, 120),
                UnitStatus = Shared.Status.AUTO_OPENED,
                FanStatus = true
            });
            units.Add(new Unit()
            {
                Name = "Unit K5",
                Temperature = rand.Next(50, 120),
                UnitStatus = Shared.Status.MAN_OPENED,
                FanStatus = false
            });
            units.Add(new Unit()
            {
                Name = "Unit K28",
                UnitStatus = Shared.Status.OFFLINE
            });

            return units.ToArray();
        }

        public Unit UnitOpen(string Id)
        {
            Unit? unit = _dbContext.Units
                .Include(unit => unit.Authorizations)
                .ThenInclude(unit => unit.User)
                .First(unit => unit.ID == Guid.Parse(Id));

            //TODO: Add logic to authorize user and perform request

            return new Unit()
            {
                ID = Guid.Parse(Id),
                Name = "Unit K87",
                Temperature = 102.5f,
                UnitStatus = Shared.Status.MAN_OPENED,
                FanStatus = true
            };
        }
    }
}
