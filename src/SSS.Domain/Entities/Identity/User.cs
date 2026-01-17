using Microsoft.AspNetCore.Identity;
using SSS.Domain.Enums;

namespace SSS.Domain.Entities.Identity;

public class User : IdentityUser
{
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Address { get; set; }
        public DateTime? Dob { get; set; }
        public Gender? Gender { get; set; }

    // Navigation properties
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
    // Add other navigation collections as needed (e.g., StudyPlans, UserNotifications, etc.)
}