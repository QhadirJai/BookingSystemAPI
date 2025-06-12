using Domain.Entities;
using Application.DTOs;
using Infrastructure.Persistence.Data;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly AppDbContext _context;

        public BookingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateBookingAsync(Guid userId, CreateBookingDto dto)
        {
            var booking = new Booking
            {
                CustomerId = userId,
                RoomId = dto.RoomId,
                BookingDate = dto.BookingDate,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = "Pending"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookingAsync(UpdateBookingDto dto)
        {
            var booking = await _context.Bookings.FindAsync(dto.Id);
            if (booking == null) return;

            booking.RoomId = dto.RoomId;
            booking.BookingDate = dto.BookingDate;
            booking.StartTime = dto.StartTime;
            booking.EndTime = dto.EndTime;
            booking.Status = dto.Status;

            await _context.SaveChangesAsync();
        }

        public async Task CancelBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null) return;

            booking.Status = "Cancelled";
            await _context.SaveChangesAsync();
        }

        public async Task<List<BookingDto>> GetBookingsByUserAsync(Guid userId)
        {
            return await _context.Bookings
                .Where(b => b.CustomerId == userId)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    CustomerId = b.CustomerId,
                    RoomId = b.RoomId,
                    BookingDate = b.BookingDate,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    Status = b.Status
                })
                .ToListAsync();
        }

        public async Task<List<BookingDto>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    CustomerId = b.CustomerId,
                    RoomId = b.RoomId,
                    BookingDate = b.BookingDate,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    Status = b.Status
                })
                .ToListAsync();
        }


        public async Task<BookingDto?> GetBookingByIdAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            return booking == null ? null : MapToBookingDto(booking);
        }

        private static BookingDto MapToBookingDto(Booking b)
        {
            return new BookingDto
            {
                Id = b.Id,
                CustomerId = b.CustomerId,
                RoomId = b.RoomId,
                BookingDate = b.BookingDate,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                Status = b.Status
            };
        }
    }
}
