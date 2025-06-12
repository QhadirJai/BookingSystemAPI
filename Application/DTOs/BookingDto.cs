namespace Application.DTOs
{
    public class BookingDto
    {
        public Guid Id { get; set; }              // ?? Changed from int to Guid
        public Guid CustomerId { get; set; }
        public int RoomId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
