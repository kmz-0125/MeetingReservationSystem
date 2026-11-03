using System.ComponentModel.DataAnnotations;
namespace MeetingReservationSystem.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "社員番号は必須です")]
        [Display(Name = "社員番号")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "パスワードは必須です")]
        [DataType(DataType.Password)]
        [Display(Name = "パスワード")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "ログイン状態を保持する")]
        public bool RememberMe { get; set; }
    }
}