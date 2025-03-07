using Microsoft.EntityFrameworkCore;
using NetArchTechChallenge.Shared.Infrastructure.Mappings;
using System.Diagnostics.CodeAnalysis;

namespace NetArchTechChallenge.Shared.Infrastructure.DbContexts
{
    [ExcludeFromCodeCoverage]
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContactMapping());
            base.OnModelCreating(modelBuilder);
        }
    }
}
