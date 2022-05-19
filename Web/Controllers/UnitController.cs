using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnitController : ControllerBase
    {
        private readonly ILogger<UnitController> _logger;

        public UnitController(ILogger<UnitController> logger)
        {
            _logger = logger;
        }

        public IEnumerable<Unit> GetUnits()
        {
            var units = new List<Unit>();
            units.Add(new Unit()
            {
                Name = "Unit K87",
                Temperature = 102.5f,
                UnitStatus = Shared.Status.CLOSED,
                FanStatus = true
            });
            units.Add(new Unit()
            {
                Name = "Unit K98",
                Temperature = 82.4f,
                UnitStatus = Shared.Status.AUTO_OPENED,
                FanStatus = true
            });
            units.Add(new Unit()
            {
                Name = "Unit K5",
                Temperature = 68.3f,
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
    }
}
