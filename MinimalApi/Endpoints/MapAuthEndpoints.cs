using Application.DTOs;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using System.Security.Claims; // ? Needed for ClaimsPrincipal

namespace MinimalApi.Endpoints
{
    public static class MapAuthEndpoints
    {
        public static void ConfigureAuthEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/auth").WithTags("Authentication");

            // Register Customer
            group.MapPost("/register", async (RegisterDto dto, IUserService userService) =>
            {
                var result = await userService.RegisterCustomerAsync(dto);
                return result.Contains("successful")
                    ? Results.Ok(result)
                    : Results.BadRequest(result);
            });

            // Login
            group.MapPost("/login", async (LoginDto dto, IUserService userService) =>
            {
                var result = await userService.LoginAsync(dto);
                return result != null
                    ? Results.Ok(result)
                    : Results.BadRequest("Login failed");
            });
        }
    }

}
