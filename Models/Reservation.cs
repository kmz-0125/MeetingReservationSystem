using System.ComponentModel.DataAnnotations;

namespace MeetingReservationSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public int RoomId { get; set; }

        [Required]
        public DateTimeOffset StartTime { get; set; }

        [Required]
        public DateTimeOffset EndTime { get; set; }

        public int ParticipantCount { get; set; }

        [MaxLength(1000)]
        public string? Announcement { get; set; }

        public bool IsCancelled { get; set; }

        public ApplicationUser? User { get; set; }

        public Room? Room { get; set; }
    }
}
