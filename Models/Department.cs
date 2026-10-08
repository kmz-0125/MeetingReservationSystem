using System.ComponentModel.DataAnnotations;

namespace MeetingReservationSystem.Models
{
    public class Department
    {
        [MaxLength(5)]
        public string DepartmentId { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string DepartmentName { get; set; } = string.Empty;
    }
}
