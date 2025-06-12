using Application.DTOs;

namespace Application.Interfaces
{
    public interface IBookingService
    {
        Task CreateBookingAsync(Guid userId, CreateBookingDto dto);
        Task<List<BookingDto>> GetBookingsByUserAsync(Guid userId);
        Task<List<BookingDto>> GetAllBookingsAsync();
        Task<BookingDto?> GetBookingByIdAsync(int bookingId);
        Task UpdateBookingAsync(UpdateBookingDto dto);
        Task CancelBookingAsync(int bookingId);
    }
}
