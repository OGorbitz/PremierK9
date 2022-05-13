using Shared;
using System;

namespace Web.ViewModels
{
    public class UnitViewModel
    {
        public Guid ID;
        public string Name { get; set; }
        public float Temperature { get; set; }
        public Status UnitStatus { get; set; }
        public bool FanStatus { get; set; }
        public DateTime UpdatedTime { get; set; }

        public UnitViewModel()
        {

        }
        public UnitViewModel(Unit unit)
        {
            ID = unit.ID;
            Name = unit.Name;
            Temperature = unit.Temperature;
            UnitStatus = unit.UnitStatus;
            FanStatus = unit.FanStatus;
            UpdatedTime = unit.UpdatedTime;
        }
    }
}
