using System.ComponentModel.DataAnnotations;

namespace ProjectDelivery.Models
{
    public class LoginViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Мобільний телефон")]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запам'ятати мене")]
        public bool RememberMe { get; set; }
    }
}
