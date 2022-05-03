using System;

namespace Shared
{
    public enum Status { CLOSED, AUTO_OPENED, MAN_OPENED }

    public class Unit
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public Guid OrganizationID { get; set; }
        public float Temperature { get; set; }
        public Status UnitStatus { get; set; } = Status.CLOSED;
        public DateTime UpdatedTime { get; set; }
    }
}
