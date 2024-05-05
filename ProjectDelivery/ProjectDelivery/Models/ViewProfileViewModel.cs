using ProjectDelivery.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectDelivery.Models
{
    public class ViewProfileViewModel
    {
        [Required]
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
        [Required]
        [Phone]
        [Display(Name = "Номер телефону")]
        public string PhoneNumber { get; set; }
        public EnumCities City { get; set; } = 0;
        public bool Changed { get; set; } = false;
    }
}
