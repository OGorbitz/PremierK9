using System;

namespace Shared
{
    public enum Status { CLOSED, AUTO_OPENED, MAN_OPENED, OFFLINE }

    public class Unit
    {
        /// <summary>
        /// A unit-specific identifier
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// A user-created name for the device, e.g. "Unit K123"
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Represents the device's owning organization, if any
        /// </summary>
        public Guid OrganizationID { get; set; }
        /// <summary>
        /// Unit's measured temperature
        /// </summary>
        public float Temperature { get; set; }
        /// <summary>
        /// Represents unit's door status
        /// </summary>
        public Status UnitStatus { get; set; }
        /// <summary>
        /// True if fan is running, false if not
        /// </summary>
        public bool FanStatus { get; set; }
        /// <summary>
        /// The time the status was fetched from the server
        /// </summary>
        public DateTime UpdatedTime { get; set; }
    }
}
