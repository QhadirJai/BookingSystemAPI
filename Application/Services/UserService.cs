using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public UserService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<string> RegisterCustomerAsync(RegisterDto dto)
        {
            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                Role = UserRole.Customer
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return $"Registration failed: {string.Join(", ", result.Errors.Select(e => e.Description))}";
            }

            // Use the string role name instead of enum
            await _userManager.AddToRoleAsync(user, "Customer");
            return "Registration successful";
        }

        public async Task<IdentityResult> RegisterStaffAsync(RegisterStaffDto dto, ClaimsPrincipal currentUser)
        {
            // No admin validation required; anyone can register staff (adjust as needed for your business logic)

            var staff = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                Role = dto.Role ?? UserRole.Staff // Default to Staff if not specified
            };

            var result = await _userManager.CreateAsync(staff, dto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(staff, staff.Role.ToString());
            }

            return result;
        }




        public async Task<UserDto?> LoginAsync(LoginDto dto)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(dto.Email))
                    throw new ArgumentException("Email is required");
                if (string.IsNullOrWhiteSpace(dto.Password))
                    throw new ArgumentException("Password is required");

                // Find user
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                {
                    Console.WriteLine($"Login failed: User not found for email {dto.Email}");
                    return null;
                }

                // Verify password
                if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                {
                    Console.WriteLine($"Login failed: Invalid password for user {user.Email}");
                    return null;
                }

                // Prepare claims
                // In UserService.cs, update your claims generation to handle nulls:
                var claims = new List<Claim>
{
    new Claim("userId", user.Id.ToString()),
    new Claim("userName", user.UserName ?? string.Empty),
    new Claim("email", user.Email ?? string.Empty),
    new Claim("roles", user.Role.ToString().ToUpper()) // Force uppercase for consistency
};

                // Get additional claims from the database
                var userClaims = await _userManager.GetClaimsAsync(user);
                claims.AddRange(userClaims);

                // Get configuration from appsettings
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var jwtSecret = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");
                var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer not configured");
                var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience not configured");
                var expiryMinutes = Convert.ToInt32(jwtSettings["ExpiryInMinutes"] ?? "120"); // Default 2 hours

                // Configure token
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                // Return user DTO with token
                return new UserDto
                {
                    Id = user.Id.ToString(),
                    FullName = user.FullName ?? "Unknown",
                    Email = user.Email ?? "unknown@example.com",
                    Role = user.Role.ToString(),
                    Token = tokenString
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                throw; // Re-throw for API to handle
            }
        }
    }
}
