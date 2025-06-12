using Domain.Enums;

namespace Application.DTOs
{
    public class RegisterStaffDto
    {     
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;
            public UserRole? Role { get; set; }
    }
}
