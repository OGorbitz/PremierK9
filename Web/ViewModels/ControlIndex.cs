using Shared;
using System.Collections.Generic;

namespace Web.ViewModels
{
    public class ControlIndex
    {
        public string UserFirstName { get; set; }
        public List<UnitBrief> Units { get; set; }
    }

    public class UnitBrief
    {
        public string Name { get; set; }
        public float Temperature { get; set; }
        public Status UnitStatus { get; set; }
        public bool FanStatus { get; set; }

        public UnitBrief()
        {

        }
        public UnitBrief(Unit unit)
        {
            Name = unit.Name;
            Temperature = unit.Temperature;
            UnitStatus = unit.UnitStatus;
            FanStatus = unit.FanStatus;
        }
    }
}
