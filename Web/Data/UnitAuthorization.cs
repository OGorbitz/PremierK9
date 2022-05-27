using Shared;
using System;
using Web.Data;

namespace Shared
{
    public enum AuthType { OWNER, USER, VIEWER, NONE }

    public class UnitAuthorization
    {
        public Guid Id { get; set; }
        public Unit Unit { get; set; }
        public AppIdentityUser User { get; set; }
        public AuthType AuthType { get; set; } = AuthType.NONE;
    }
}
