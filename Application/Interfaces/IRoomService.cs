using Application.DTOs;


namespace Application.Interfaces
{
    public interface IRoomService
    {
        Task<List<RoomDto>> GetAllRoomsAsync();
        Task<RoomDto?> GetRoomByIdAsync(int id);
        Task<int> AddRoomAsync(RoomDto dto); // was void
        Task<bool> UpdateRoomAsync(RoomDto dto); // added return type
        Task DeleteRoomAsync(int id);
    }
}
