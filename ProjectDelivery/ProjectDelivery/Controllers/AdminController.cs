using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectDelivery.Classes;
using ProjectDelivery.Data;
using ProjectDelivery.Enums;
using ProjectDelivery.Interfaces;
using ProjectDelivery.Models;

namespace ProjectDelivery.Controllers
{
    public class AdminController : Controller, IAdmin
    {
        private readonly UserManager<AccModel> _userManager;
        private readonly ApplicationDbContext _context;
        public AdminController(UserManager<AccModel> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> AdminPanel()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && user.Roles == Enums.EnumRoles.Admin)
            {
                var allUsers = _userManager.Users.ToList();
                var usersExceptCurrent = allUsers.Where(u => u.Id != user.Id).ToList();
                var articles = _context.Articles.ToList();
                AdminPanelViewModel model = new() {
                    accModels = usersExceptCurrent,
                    articleModels = articles
                };
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> AddUser()
        {
            AddUserModel newUser = new AddUserModel();
            return View(newUser);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddUser(AddUserModel newUser)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await new FindingUser().FindByPhoneNumberAsync(_userManager, newUser.PhoneNumber); ;
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Цей телефон вже використовується!");
                    return View(newUser);
                }
                var user = new AccModel { Name = newUser.Name, UserName = newUser.Email, Email = newUser.Email, PhoneNumber = newUser.PhoneNumber, Roles = newUser.Roles, City = newUser.City };
                var result = await _userManager.CreateAsync(user, newUser.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("AdminPanel");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(newUser);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateUser(string userId, string Name, string Email, string Phone, EnumRoles Role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.Name = Name;
                user.Email = Email;
                user.PhoneNumber = Phone;
                user.Roles = Role;

                await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("AdminPanel");
            }

            return RedirectToAction("AdminPanel");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("AdminPanel");
            }

            return RedirectToAction("AdminPanel");
        }
        [Authorize]
        public async Task<IActionResult> AddArticle()
        {
            ArticleModel newArticle = new ArticleModel();
            return View(newArticle);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddArticle(ArticleModel newArticle)
        {
            if (ModelState.IsValid)
            {
                var existingArticle = await _context.Articles.FirstOrDefaultAsync(a => a.Title == newArticle.Title);
                if (existingArticle != null)
                {
                    ModelState.AddModelError(string.Empty, "Сттатя з таким заголовком вже існує!");
                    return View(newArticle);
                }

                var article = new ArticleModel { Title = newArticle.Title, Description = newArticle.Description };
                await _context.Articles.AddAsync(article);
                await _context.SaveChangesAsync();

                return RedirectToAction("AdminPanel");
            }

            return View(newArticle);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateArticle(int articleId, string Title, string Description)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == articleId);
            article.Description = Description;
            if (article != null)
            {
                article.Title = Title;
                article.Description = Description;
                _context.Update(article);
                await _context.SaveChangesAsync();
                return RedirectToAction("AdminPanel");
            }

            return RedirectToAction("AdminPanel");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteArticle(int articleId)
        {
            var article = await _context.Articles.FindAsync(articleId);
            if (article != null)
            {
                _context.Remove(article);
                await _context.SaveChangesAsync();

                return RedirectToAction("AdminPanel");
            }

            return RedirectToAction("AdminPanel");
        }
    }
}
