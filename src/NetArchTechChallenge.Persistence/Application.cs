using Microsoft.EntityFrameworkCore;
using NetArchTechChallenge.Persistence.Consumers;
using NetArchTechChallenge.Persistence.Workers;
using NetArchTechChallenge.Shared.Application.Messaging;
using NetArchTechChallenge.Shared.Application.Services;
using NetArchTechChallenge.Shared.Domain.Repositories;
using NetArchTechChallenge.Shared.Infrastructure.DbContexts;
using NetArchTechChallenge.Shared.Infrastructure.Messaging;
using NetArchTechChallenge.Shared.Infrastructure.Repositories;
using Prometheus;

namespace NetArchTechChallenge.Persistence
{
    public class Application
    {
        public void Run(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args);
            host.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                config.AddEnvironmentVariables();
            });

            host.ConfigureServices((context, services) =>
            {
#if DEBUG
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer("Server=localhost;Database=NetArch;User Id=sa;Password=SuaSenha123!;TrustServerCertificate=True;"));
#else
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer("Server=svc-sqlserver;Database=NetArch;User Id=sa;Password=SuaSenha123!;TrustServerCertificate=True;"));
#endif

                services.AddScoped<IMessageService, RabbitMQService>();

                services.AddScoped<IContactRepository, ContactRepository>();
                services.AddScoped<ContactPersistenceService>();

                services.AddScoped<ContactCreatedConsumer>();
                services.AddScoped<ContactUpdatedConsumer>();
                services.AddScoped<ContactDeletedConsumer>();

                services.AddHostedService<ConsumersWorker>();
            });

            var app = host.Build();

            var metricServer = new KestrelMetricServer(url: "/api/metrics", port: 9091);
            metricServer.Start();

            app.Run();
        }
    }
}
