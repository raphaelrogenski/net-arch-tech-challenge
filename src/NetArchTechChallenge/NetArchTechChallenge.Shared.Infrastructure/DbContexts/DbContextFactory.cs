using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NetArchTechChallenge.Shared.Infrastructure.DbContexts
{
    public static class DbContextFactory
    {
        public static AppDbContext CreateDbContext(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
