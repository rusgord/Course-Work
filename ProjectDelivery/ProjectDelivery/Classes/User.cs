using Microsoft.AspNetCore.Identity;
using ProjectDelivery.Models;

namespace ProjectDelivery.Classes
{
    public abstract class User
    {
        public abstract Task<AccModel> FindByPhoneNumberAsync(UserManager<AccModel> _userManager, string phoneNumber);
    }
}
