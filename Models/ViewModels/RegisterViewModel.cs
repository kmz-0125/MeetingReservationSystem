using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace MeetingReservationSystem.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "社員番号は必須です")]
        [Display(Name = "社員番号")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "社員番号は6桁の数字で入力してください")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "苗字は必須です")]
        [MaxLength(100)]
        [Display(Name = "苗字")]
        public string FamilyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "名前は必須です")]
        [MaxLength(100)]
        [Display(Name = "名前")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "パスワードは必須です")]
        [MinLength(8, ErrorMessage = "パスワードは8文字以上で入力してください")]
        [DataType(DataType.Password)]
        [Display(Name = "パスワード")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "確認用パスワードは必須です")]
        [DataType(DataType.Password)]
        [Display(Name = "パスワード(確認)")]
        [Compare("Password", ErrorMessage = "パスワードが一致しません")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "部署は必須です")]
        [Display(Name = "部署")]
        public string DepartmentId { get; set; } = string.Empty;

        public List<SelectListItem> Departments { get; set; } = new List<SelectListItem>();
    }
}