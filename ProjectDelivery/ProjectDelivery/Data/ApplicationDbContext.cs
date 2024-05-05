using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectDelivery.Models;

namespace ProjectDelivery.Data
{
    public class ApplicationDbContext : IdentityDbContext<AccModel>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Package> Packages { get; set; }
        public DbSet<ArticleModel> Articles { get; set; }
    }
}
