using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectDelivery.Data;
using ProjectDelivery.Models;
using System.Security.Claims;

namespace ProjectDelivery.Classes
{
    public class ControlsAccount
    {
        private readonly UserManager<AccModel> _userManager;
        private readonly SignInManager<AccModel> _signInManager;
        private readonly ApplicationDbContext _context;

        public ControlsAccount(UserManager<AccModel> userManager, SignInManager<AccModel> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<OperationResult> RegisterAsync(RegisterViewModel model)
        {
            var user = new AccModel { Name = model.Name, UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return new OperationResult(true);
            }
            return new OperationResult(false, "Failed to register user.");
        }

        public async Task<OperationResult> LoginAsync(LoginViewModel model)
        {
            var user = await new FindingUser().FindByPhoneNumberAsync(_userManager, model.Phone);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                await _signInManager.SignInAsync(user, model.RememberMe);
                return new OperationResult(true);
            }
            return new OperationResult(false, "Invalid login attempt.");
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<OperationResult> ChangePasswordAsync(ChangePasswordViewModel model, ClaimsPrincipal User)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return new OperationResult(false, "User not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return new OperationResult(true);
            }
            return new OperationResult(false, "Failed to change password.");
        }

        public async Task<ViewProfileViewModel> GetProfileViewModelAsync(ClaimsPrincipal User)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return null;
            }
            return new ViewProfileViewModel
            {
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                City = user.City
            };
        }

        public async Task<OperationResult> UpdateProfileAsync(ViewProfileViewModel model, ClaimsPrincipal User)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return new OperationResult(false, "User not found.");
            }

            user.Name = model.Name;
            user.PhoneNumber = model.PhoneNumber;
            user.City = model.City;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new OperationResult(true);
            }

            var errors = result.Errors.Select(e => new IdentityError { Description = e.Description });
            return new OperationResult(false, "Failed to update profile.") { Errors = errors };
        }

        public async Task<List<Package>> GetPackageHistoryAsync(ClaimsPrincipal User)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return null;
            }
            return _context.Packages.ToList();
        }
    }
}
