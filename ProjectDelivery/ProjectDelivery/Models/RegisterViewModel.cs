using System.ComponentModel.DataAnnotations;

namespace ProjectDelivery.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Ім'я користувача")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Пошта")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Мобільний телефон")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Підтвердіть пароль")]
        [Compare("Password", ErrorMessage = "Паролі не збігаються.")]
        public string ConfirmPassword { get; set; }
    }
}
