using DomainEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppServices.OnBoardContext
{
    public class OnBoardDbContext : IdentityDbContext<User>
    {
        public OnBoardDbContext(DbContextOptions<OnBoardDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}