using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MinimalApi.Endpoints
{
    public static class BookingEndpoints
    {
        public static IEndpointRouteBuilder MapBookingEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/api/bookings").WithTags("Bookings");

            // Create booking
            group.MapPost("/", [Authorize] async (HttpContext context, CreateBookingDto dto, IBookingService service) =>
            {
                var userIdString = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(userIdString, out var userId))
                    return Results.Unauthorized();

                await service.CreateBookingAsync(userId, dto);
                return Results.Ok("Booking created successfully");
            });

            // Get bookings by user
            group.MapGet("/", [Authorize] async (HttpContext context, IBookingService service) =>
            {
                var userIdString = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(userIdString, out var userId))
                    return Results.Unauthorized();

                var bookings = await service.GetBookingsByUserAsync(userId);
                return Results.Ok(bookings);
            });

            // Cancel booking
            group.MapDelete("/{id:int}", [Authorize] async (int id, IBookingService service) =>
            {
                await service.CancelBookingAsync(id);
                return Results.Ok("Booking cancelled successfully");
            });

            return endpoints;
        }
    }
}
