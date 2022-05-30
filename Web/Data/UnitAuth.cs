using Data;

namespace Web.Data
{
    public enum AuthType { OWNER, USER, VIEWER, NONE }

    public class UnitAuth
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public AuthType AuthType { get; set; } = AuthType.NONE;
        public AppIdentityUser User { get; set; }
        public Unit Unit { get; set; }
    }
}
