using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProjectDelivery.Data;
using ProjectDelivery.Enums;
using ProjectDelivery.Models;
using ProjectDelivery.Classes;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ProjectDeliveryTest
{
    public class testAccount
    {
        private readonly Mock<UserManager<AccModel>> _mockUserManager;
        private readonly ApplicationDbContext _dbContext;

        public testAccount()
        {
            // Mock UserManager
            var store = new Mock<IUserStore<AccModel>>();
            _mockUserManager = new Mock<UserManager<AccModel>>(store.Object, null, null, null, null, null, null, null, null);

            // In-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dbContext = new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetAdminPanelViewModelAsync_ReturnsViewModel_WhenUserIsAdmin()
        {
            // Arrange
            var adminUser = new AccModel { Id = "1", Roles = EnumRoles.Admin };
            var users = new List<AccModel>
            {
                adminUser,
                new AccModel { Id = "2", Roles = EnumRoles.User }
            };
            _mockUserManager.Setup(m => m.Users).Returns(users.AsQueryable());
            var articles = new List<ArticleModel>();
            _dbContext.Articles.AddRange(articles);
            await _dbContext.SaveChangesAsync();

            var adminService = new AdminService(_mockUserManager.Object, _dbContext);

            // Act
            var result = await adminService.GetAdminPanelViewModelAsync(adminUser);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Errors);
            Assert.NotNull(result.accModels);
            Assert.Equal(1, result.accModels.Count());
            Assert.NotNull(result.articleModels);
            Assert.Equal(0, result.articleModels.Count());
        }

        [Fact]
        public async Task AddUserAsync_ReturnsSuccess_WhenUserDoesNotExist()
        {
            // Arrange
            var newUser = new AddUserModel { Email = "test@example.com" };
            _mockUserManager.Setup(m => m.FindByEmailAsync(newUser.Email)).ReturnsAsync((AccModel)null);
            _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<AccModel>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var adminService = new AdminService(_mockUserManager.Object, _dbContext);

            // Act
            var result = await adminService.AddUserAsync(newUser);

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public async Task AddArticleAsync_ReturnsSuccess_WhenArticleDoesNotExist()
        {
            // Arrange
            var newArticle = new ArticleModel { Title = "Test Article", Description = "Test Description" };
            _dbContext.Articles.Add(newArticle);
            await _dbContext.SaveChangesAsync();

            var adminService = new AdminService(_mockUserManager.Object, _dbContext);

            // Act
            var result = await adminService.AddArticleAsync(newArticle);

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.Errors);
        }

    }
}
