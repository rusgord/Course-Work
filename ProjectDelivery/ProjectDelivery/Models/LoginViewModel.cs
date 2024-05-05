using System.ComponentModel.DataAnnotations;

namespace ProjectDelivery.Models
{
    public class LoginViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
