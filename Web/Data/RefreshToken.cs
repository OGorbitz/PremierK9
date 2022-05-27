using Microsoft.AspNetCore.Identity;
using Web.Data;

namespace Web.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string TokenHash { get; set; }
        public string TokenSalt { get; set; }
        public DateTime Ts { get; set; }
        public DateTime ExpiryDate { get; set; }
        public virtual AppIdentityUser User { get; set; }
    }
}
