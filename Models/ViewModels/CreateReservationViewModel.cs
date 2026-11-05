using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MeetingReservationSystem.Models.ViewModels
{
    public class CreateReservationViewModel
    {
        [Required(ErrorMessage = "議題の登録は必要です。")]
        [MaxLength(100)]
        [Display(Name = "議題")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "会議室番号の選択は必要です。")]
        [Display(Name = "会議室番号")]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "開始時刻の登録は必要です。")]
        [Display(Name = "開始時刻")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "終了時刻の登録は必要です。")]
        [Display(Name = "終了時刻")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "人数の登録は必要です。")]
        [Display(Name = "予約人数")]
        [Range(1, 8)]
        public int ParticipantCount { get; set; }

        [MaxLength(1000)]
        [Display(Name = "連絡事項")]
        public string? Announcement { get; set; }

        public List<SelectListItem> Rooms { get; set; } = new List<SelectListItem>();
    }
}
