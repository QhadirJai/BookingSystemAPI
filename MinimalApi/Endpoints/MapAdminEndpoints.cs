using Application.DTOs;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MinimalApi.Endpoints
{
    public static class AdminEndpoints
    {
        public static IEndpointRouteBuilder MapAdminApi(this IEndpointRouteBuilder app)
        {
            // Add a test endpoint without auth to verify basic endpoint registration
            app.MapGet("/api/test-no-auth", () =>
            {
                Console.WriteLine("No-auth test endpoint hit!");
                return Results.Ok("No auth test works!");
            });

            app.MapGet("/api/admin-endpoint", [Authorize] (HttpContext httpContext) =>
            {
                var claims = httpContext.User.Claims
                    .Select(c => new { c.Type, c.Value })
                    .ToList();

                return Results.Ok(new
                {
                    IsAuthenticated = httpContext.User.Identity?.IsAuthenticated,
                    Claims = claims
                });
            });

            // This endpoint still requires "AdminOnly" policy, you may want to update or remove it
            app.MapGet("/api/admin2", () => "This is test")
                .RequireAuthorization(); // Changed to require authentication only

            // Remove .RequireAuthorization("AdminOnly") from the group
            var group = app.MapGroup("/api/admin")
                .RequireAuthorization(); // Only requires authentication, not a specific role

            // Create Staff - with detailed logging
            group.MapPost("/staff", async (RegisterStaffDto dto, IUserService userService, ClaimsPrincipal user) =>
            {
                // Input validation
                if (string.IsNullOrWhiteSpace(dto.Email))
                {
                    return Results.BadRequest("Email is required");
                }

                if (string.IsNullOrWhiteSpace(dto.Password))
                {
                    return Results.BadRequest("Password is required");
                }

                if (string.IsNullOrWhiteSpace(dto.FullName))
                {
                    return Results.BadRequest("Full name is required");
                }

                try
                {
                    var result = await userService.RegisterStaffAsync(dto, user);

                    return result.Succeeded
                        ? Results.Ok(new { Message = "Staff account created successfully" })
                        : Results.BadRequest(result.Errors.Select(e => e.Description));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating staff: {ex.Message}");
                    return Results.Problem("An error occurred while creating staff account");
                }
            }).WithOpenApi();

            // Add a route listing endpoint inside the admin group for debugging
            group.MapGet("/routes", () =>
            {
                var endpoints = app.DataSources
                    .SelectMany(ds => ds.Endpoints)
                    .Where(e => e is RouteEndpoint)
                    .Select(e => new
                    {
                        Route = (e as RouteEndpoint)?.RoutePattern.RawText,
                        Method = string.Join(", ", e.Metadata.OfType<HttpMethodMetadata>()
                                .SelectMany(m => m.HttpMethods))
                    })
                    .ToList();

                return Results.Ok(endpoints);
            });

            Console.WriteLine("Admin endpoints mapped successfully");
            return app;
        }
    }
}
