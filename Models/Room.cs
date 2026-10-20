using System.ComponentModel.DataAnnotations;

namespace MeetingReservationSystem.Models
{
    public class Room
    {
        public int RoomId { get; set; }

        [Required]
        [MaxLength(100)]
        public string RoomName { get; set; } = string.Empty;

        [Required]
        [Range(1, 8)]
        public int Capacity { get; set; }
    }
}
