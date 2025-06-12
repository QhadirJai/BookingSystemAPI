namespace Application.DTOs
{
    public class UpdateBookingDto
    {
        public int Id { get; set; } // Required for lookup
        public int RoomId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
