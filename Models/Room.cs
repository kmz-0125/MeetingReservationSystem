using System.ComponentModel.DataAnnotations;

namespace MeetingReservationSystem.Models
{
    public class Room
    {
        public int RoomId { get; set; }

        [Required]
        [MaxLength(100)]
        public string RoomName { get; set; } = string.Empty;

        public int Capacity { get; set; }
    }
}
