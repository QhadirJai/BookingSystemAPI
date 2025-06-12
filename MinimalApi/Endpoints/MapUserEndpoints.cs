using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

public static class MapUserEndpoints
{
    public static void MapUserApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users");

        // Register customer
        group.MapPost("/register", async (RegisterDto dto, IUserService userService) =>
        {
            var result = await userService.RegisterCustomerAsync(dto);
            return Results.Ok(result); // Returns a token or message
        });

        // Register staff - requires authorization
        group.MapPost("/register-staff", async (RegisterStaffDto dto, ClaimsPrincipal user, IUserService userService) =>
        {
            var result = await userService.RegisterStaffAsync(dto, user);
            return Results.Ok(result);
        }).RequireAuthorization("AdminOnly");

        // Login
        group.MapPost("/login", async (LoginDto dto, IUserService userService) =>
        {
            var userDto = await userService.LoginAsync(dto);
            if (userDto == null) return Results.Unauthorized();

            return Results.Ok(userDto); // userDto has Id, Email, Role
        });
    }
}
