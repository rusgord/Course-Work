using ProjectDelivery.Enums;
using System.ComponentModel.DataAnnotations;
namespace ProjectDelivery.Models
{
    public class AddUserModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public EnumRoles Roles { get; set; } = 0;
        public EnumCities City { get; set; } = 0;

    }
}
