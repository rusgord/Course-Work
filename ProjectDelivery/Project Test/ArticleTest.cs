using ProjectDelivery.Models;
using Microsoft.AspNetCore.Identity;
using ProjectDelivery.Enums;

namespace Project_Test
{
    public class ArticleTest
    {
        [Fact]
        public void ArticleModel()
        {
            var article = new ArticleModel
            {
                Id = 1,
                Title = "Test title",
                Description = "Some text"
            };
            Assert.Equal(1, article.Id);
            Assert.Equal("Test title", article.Title);
            Assert.Equal("Some text", article.Description);
        }
    }
}
