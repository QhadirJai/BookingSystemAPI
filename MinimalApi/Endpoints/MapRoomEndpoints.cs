using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Application.DTOs;
using Application.Interfaces;

namespace MinimalApi.Endpoints
{
	public static class MapRoomEndpoints
	{
		public static void MapRoomApi(this IEndpointRouteBuilder app)
		{
			var group = app.MapGroup("/api/rooms");

			group.MapPost("/create", [Authorize(Roles = "Admin")] async (RoomDto dto, IRoomService roomService) =>
			{
				var id = await roomService.AddRoomAsync(dto); // Changed method name to AddRoomAsync
				return id > 0 ? Results.Ok("Room created successfully.") : Results.BadRequest("Failed to create room.");
			});

			group.MapPut("/update/{id}", [Authorize(Roles = "Admin")] async (int id, RoomDto dto, IRoomService roomService) =>
			{
				dto.Id = id;
				var result = await roomService.UpdateRoomAsync(dto);
				return result ? Results.Ok("Room updated successfully.") : Results.NotFound("Room not found.");
			});

		}
	}
}
