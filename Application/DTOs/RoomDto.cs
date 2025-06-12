namespace Application.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = default!;
        public int Capacity { get; set; }
        public string Status { get; set; } = default!;
        public string? Description { get; set; }
    }
}
