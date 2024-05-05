using System.ComponentModel.DataAnnotations;

namespace ProjectDelivery.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Ваш пароль")]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новий пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Підтвердіть новий пароль")]
        [Compare("NewPassword", ErrorMessage = "Паролі не співпадають!")]
        public string ConfirmPassword { get; set; }
        public bool Changed { get; set; } = false;
    }
}
