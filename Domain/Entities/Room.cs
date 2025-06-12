using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Status { get; set; } = "Available"; // 'Available', 'Booked', 'Maintenance'
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ? Fix: Add navigation property for bookings
        public ICollection<Booking>? Bookings { get; set; }
    }
}
