using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetArchTechChallenge.Shared.Application.Services;
using NetArchTechChallenge.Shared.Domain.Repositories;
using NetArchTechChallenge.Shared.Infrastructure.DbContexts;
using NetArchTechChallenge.Shared.Infrastructure.Repositories;

namespace NetArchTechChallenge.Query
{
    public class Application
    {
        public void Run(string[] args)
        {
            var builder = FunctionsApplication.CreateBuilder(args);

            builder.ConfigureFunctionsWebApplication();

            builder.Configuration.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddFunctionsWorkerDefaults();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IContactRepository, ContactRepository>();
            builder.Services.AddScoped<ContactQueryService>();

            var app = builder.Build();

            app.Run();
        }
    }
}
