using Microsoft.AspNetCore.Identity;
using ProjectDelivery.Enums;

namespace ProjectDelivery.Models
{
    public class AccModel : IdentityUser
    {
        public string Name { get; set; }
        public EnumRoles Roles { get; set; } = 0;
        public EnumCities City { get; set; } = 0;
    }
}
