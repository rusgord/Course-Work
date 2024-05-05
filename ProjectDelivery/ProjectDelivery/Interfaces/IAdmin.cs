using Microsoft.AspNetCore.Mvc;
using ProjectDelivery.Enums;

namespace ProjectDelivery.Interfaces
{
    public interface IAdmin
    {
        public Task<IActionResult> AdminPanel();
        public Task<IActionResult> AddUser();
        public Task<IActionResult> UpdateUser(string userId, string Name, string Email, string Phone, EnumRoles Role);
        public Task<IActionResult> DeleteUser(string userId);
        public Task<IActionResult> AddArticle();
        public Task<IActionResult> UpdateArticle(int articleId, string Title, string Description);
        public Task<IActionResult> DeleteArticle(int articleId);
    }
}
