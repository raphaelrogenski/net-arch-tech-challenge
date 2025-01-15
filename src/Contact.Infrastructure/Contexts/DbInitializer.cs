using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Contacts.Infrastructure.Contexts;

public class DbInitializer
{
    private readonly IConfiguration _configuration;

    public DbInitializer(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Initialize()
    {
        using var context = DbContextFactory.CreateDbContext(_configuration);

        if (!context.Database.CanConnect())
        {
            context.Database.Migrate();
        }
        else
        {
            var pendingMigrations = context.Database.GetPendingMigrations();

            if (pendingMigrations.Any())
            {
                context.Database.Migrate();
            }
        }
    }
}
