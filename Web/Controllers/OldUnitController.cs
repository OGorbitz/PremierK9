using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OldUnitController : ControllerBase
    {
        private readonly ILogger<OldUnitController> _logger;

        public OldUnitController(ILogger<OldUnitController> logger)
        {
            _logger = logger;
        }

        public Unit GetUnit(Guid id)
        {
            if (id == Guid.Empty)
                return new Unit()
                {
                    ID = Guid.NewGuid()
                };

            var unit = new Unit()
            {
                ID = id,
                Name = "Test Unit",
                OrganizationID = Guid.NewGuid(),
                Temperature = 25.0f,
                UpdatedTime = DateTime.Now,
                UnitStatus = Status.CLOSED
            };
            _logger.LogDebug("New unit created:\nGuid: " + unit.ID + "\nName: " + unit.Name + "\nTemp: " + unit.Temperature);
            return unit;
        }

        public void PostUnit(Unit unit)
        {
            if (unit == null || unit.ID == Guid.Empty)
                return;

            var serverUnit = new Unit()
            {
                ID = unit.ID,
                Name = unit.Name,
                OrganizationID = unit.OrganizationID,
                Temperature = 20.0f,
                UpdatedTime = DateTime.Now,
                UnitStatus = Status.CLOSED
            };
        }

        [HttpGet]
        [ActionName("OpenUnit")]
        public void OpenUnit(Guid unitID)
        {
            //CRITICAL CODE - DECIDES TO OPEN UNIT
            var unit = new Unit()
            {
                ID = unitID,
                Name = "Test Unit",
                OrganizationID = Guid.NewGuid(),
                Temperature = 25.0f,
                UpdatedTime = DateTime.Now,
                UnitStatus = Status.CLOSED
            };
        }
    }
}
