using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectDelivery.Data;
using ProjectDelivery.Enums;
using ProjectDelivery.Models;
using System.Security.Claims;

namespace ProjectDelivery.Classes
{
    public class ControlsAdmin
    {
        private readonly UserManager<AccModel> _userManager;
        private readonly ApplicationDbContext _context;

        public ControlsAdmin(UserManager<AccModel> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<AdminPanelViewModel> GetAdminPanelViewModelAsync(ClaimsPrincipal user)
        {
            var currentUser = await _userManager.GetUserAsync(user);
            if (currentUser != null && currentUser.Roles == Enums.EnumRoles.Admin)
            {
                var allUsers = _userManager.Users.ToList();
                var usersExceptCurrent = allUsers.Where(u => u.Id != currentUser.Id).ToList();
                var articles = await _context.Articles.ToListAsync();
                return new AdminPanelViewModel
                {
                    accModels = usersExceptCurrent,
                    articleModels = articles
                };
            }
            return null;
        }

        public async Task<OperationResult> AddUserAsync(AddUserModel newUser)
        {
            var existingUser = await _userManager.FindByEmailAsync(newUser.Email);
            if (existingUser != null)
            {
                return new OperationResult(false, "This email is already in use.");
            }

            var user = new AccModel { Name = newUser.Name, UserName = newUser.Email, Email = newUser.Email, PhoneNumber = newUser.PhoneNumber, Roles = newUser.Roles, City = newUser.City };
            var result = await _userManager.CreateAsync(user, newUser.Password);

            if (result.Succeeded)
            {
                return new OperationResult(true);
            }
            else
            {
                var errors = result.Errors.Select(e => new IdentityError { Description = e.Description });
                return new OperationResult(false, "Failed to add user.") { Errors = errors };
            }
        }

        public async Task<OperationResult> UpdateUserAsync(string userId, string Name, string Email, string Phone, EnumRoles Role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new OperationResult(false, "User not found.");
            }

            user.Name = Name;
            user.Email = Email;
            user.PhoneNumber = Phone;
            user.Roles = Role;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new OperationResult(true);
            }
            return new OperationResult(false, "Failed to update user.");
        }

        public async Task<OperationResult> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new OperationResult(false, "User not found.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return new OperationResult(true);
            }
            return new OperationResult(false, "Failed to delete user.");
        }

        public async Task<OperationResult> AddArticleAsync(ArticleModel newArticle)
        {
            var existingArticle = await _context.Articles.FirstOrDefaultAsync(a => a.Title == newArticle.Title);
            if (existingArticle != null)
            {
                return new OperationResult(false, "Article with this title already exists.");
            }

            _context.Articles.Add(newArticle);
            await _context.SaveChangesAsync();
            return new OperationResult(true);
        }

        public async Task<OperationResult> UpdateArticleAsync(int articleId, string Title, string Description)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == articleId);
            if (article == null)
            {
                return new OperationResult(false, "Article not found.");
            }

            article.Title = Title;
            article.Description = Description;

            await _context.SaveChangesAsync();
            return new OperationResult(true);
        }

        public async Task<OperationResult> DeleteArticleAsync(int articleId)
        {
            var article = await _context.Articles.FindAsync(articleId);
            if (article == null)
            {
                return new OperationResult(false, "Article not found.");
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return new OperationResult(true);
        }
    }
}
