using System.ComponentModel.DataAnnotations;

namespace ProjectDelivery.Models
{
    public class ArticleModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Title { get; set; }
        [StringLength(3000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Description { get; set; }
    }
}
