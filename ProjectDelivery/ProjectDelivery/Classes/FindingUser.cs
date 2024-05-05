using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectDelivery.Models;

namespace ProjectDelivery.Classes
{
    public class FindingUser : User
    {
        override public async Task<AccModel> FindByPhoneNumberAsync(UserManager<AccModel> _userManager, string phoneNumber)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }
    }
}
