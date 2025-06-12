using Domain.Entities;
using Application.DTOs;
using Infrastructure.Persistence.Data;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly AppDbContext _context;

        public RoomService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoomDto>> GetAllRoomsAsync()
        {
            return await _context.Rooms
                .Select(r => new RoomDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Capacity = r.Capacity,
                    Status = r.Status,
                    Description = r.Description
                })
                .ToListAsync();
        }

        public async Task<RoomDto?> GetRoomByIdAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return null;

            return new RoomDto
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                Capacity = room.Capacity,
                Status = room.Status,
                Description = room.Description
            };
        }

        public async Task<int> AddRoomAsync(RoomDto dto)
        {
            var room = new Room
            {
                RoomNumber = dto.RoomNumber,
                Capacity = dto.Capacity,
                Status = dto.Status,
                Description = dto.Description
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room.Id;
        }


        public async Task<bool> UpdateRoomAsync(RoomDto dto)
        {
            var room = await _context.Rooms.FindAsync(dto.Id); // Use dto.Id instead of `id`
            if (room == null) return false;

            room.RoomNumber = dto.RoomNumber;
            room.Capacity = dto.Capacity;
            room.Status = dto.Status;
            room.Description = dto.Description;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteRoomAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
        }


    }
}
