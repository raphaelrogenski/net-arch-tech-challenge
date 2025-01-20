using Contacts.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Contacts.Infrastructure.Contexts;

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
