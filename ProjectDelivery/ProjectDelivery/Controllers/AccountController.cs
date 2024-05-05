using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectDelivery.Classes;
using ProjectDelivery.Data;
using ProjectDelivery.Interfaces;
using ProjectDelivery.Models;

namespace ProjectDelivery.Controllers
{
    public class AccountController : Controller, IAccount
    {
        private readonly UserManager<AccModel> _userManager;
        private readonly SignInManager<AccModel> _signInManager;
        private readonly ApplicationDbContext _context;
        public delegate Task UserEventHandler(string userId, string message);
        public event UserEventHandler UserEventOccurred;
        public AccountController(UserManager<AccModel> userManager, SignInManager<AccModel> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            UserEventOccurred += async (userId, message) =>
            {
                TempData["UserNotification"] = message;
            };
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await new FindingUser().FindByPhoneNumberAsync(_userManager, model.PhoneNumber);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Цей телефон вже використовується!");
                    return View(model);
                }
                var user = new AccModel { Name = model.Name, UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    await UserEventOccurred?.Invoke(user.Id, "Користувач успішно зареєстрований!");
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await new FindingUser().FindByPhoneNumberAsync(_userManager, model.Phone);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    await _signInManager.SignInAsync(user, model.RememberMe);
                    await UserEventOccurred?.Invoke(user.Id, "Успішна авторизація!");
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Помилка авторизації!");
                return View(model);
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction(nameof(Login));
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                await _signInManager.RefreshSignInAsync(user);
                model.Changed = true;
                return View(model);
            }
            return View(model);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var model = new ViewProfileViewModel
            {
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                City = user.City
            };

            return View(model);
        }
        [HttpPost]  
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewProfile(ViewProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                user.Name = model.Name;
                user.PhoneNumber = model.PhoneNumber;
                user.City = model.City;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    model.Changed = true;
                    return View(model);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> HistoryPackages()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(Login));
            }
            List<Package> packages = _context.Packages.ToList();
            return View(packages);
        }
    }
}
