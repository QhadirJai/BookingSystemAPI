using Application.DTOs;
using Infrastructure.Persistence.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;



namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<string> RegisterCustomerAsync(RegisterDto dto);
        Task<IdentityResult> RegisterStaffAsync(RegisterStaffDto dto, ClaimsPrincipal currentUser);
        Task<UserDto?> LoginAsync(LoginDto dto);
    }
}
