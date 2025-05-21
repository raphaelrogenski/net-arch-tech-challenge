using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NetArchTechChallenge.Shared.Infrastructure.DbContexts
{
    public static class DbContextFactory
    {
        public static AppDbContext CreateDbContext(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

#if DEBUG
            var connectionString = "Server=localhost;Database=NetArch;User Id=sa;Password=SuaSenha123!;TrustServerCertificate=True;";
#else
            var connectionString = "Server=svc-sqlserver;Database=NetArch;User Id=sa;Password=SuaSenha123!;TrustServerCertificate=True;";
#endif

            ////var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
