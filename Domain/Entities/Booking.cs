namespace Domain.Entities
{
    public class Booking
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; } // Ensure this is a Guid, not string
        public int RoomId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public User? Customer { get; set; }  // Relationship to User (Customer)
        public Room? Room { get; set; }      // Relationship to Room
    }
}
