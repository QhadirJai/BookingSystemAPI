using Microsoft.AspNetCore.Identity;
using Domain.Enums;

namespace Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Booking>? Bookings { get; set; }
    }
}
