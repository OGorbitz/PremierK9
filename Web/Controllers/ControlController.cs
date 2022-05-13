using Microsoft.AspNetCore.Mvc;
using Shared;
using System;
using System.Collections.Generic;
using Web.ViewModels;

namespace Web.Controllers
{
    public class ControlController : Controller
    {
        public IActionResult Index()
        {
            ControlIndex viewModel = new ControlIndex()
            {
                UserFirstName = "Bob",
                Units = new List<UnitBrief>()
            };
            viewModel.Units.Add(new UnitBrief()
            {
                Name = "Unit K87",
                Temperature = 102.5f,
                UnitStatus = Shared.Status.CLOSED,
                FanStatus = true
            });
            viewModel.Units.Add(new UnitBrief()
            {
                Name = "Unit K98",
                Temperature = 82.4f,
                UnitStatus = Shared.Status.AUTO_OPENED,
                FanStatus = true
            });
            viewModel.Units.Add(new UnitBrief()
            {
                Name = "Unit K5",
                Temperature = 68.3f,
                UnitStatus = Shared.Status.MAN_OPENED,
                FanStatus = false
            });
            viewModel.Units.Add(new UnitBrief()
            {
                Name = "Unit K28",
                UnitStatus = Shared.Status.OFFLINE
            });

            return View(viewModel);
        }

        public IActionResult Unit(Guid guid)
        {
            if(guid == Guid.Empty)
                guid = Guid.NewGuid();

            Unit unit = new Unit()
            {
                ID = guid,
                Name = "Unit - Test K94",
                OrganizationID = Guid.NewGuid(),
                UnitStatus = Status.CLOSED,
                FanStatus = false,
                Temperature = 68.3f,
                UpdatedTime = DateTime.Now
            };
            // TODO: add DB fetching logic

            return View(new UnitViewModel(unit));
        }
    }
}
