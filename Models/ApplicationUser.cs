using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MeetingReservationSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FamilyName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(5)]
        public string DepartmentId { get; set; } = string.Empty;

        public Department? Department { get; set; }
    }
}
