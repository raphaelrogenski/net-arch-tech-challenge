using NetArchTechChallenge.Shared.Application.Services;
using NetArchTechChallenge.Shared.Domain.Repositories;
using NetArchTechChallenge.Shared.Infrastructure.Base;
using NetArchTechChallenge.Shared.Infrastructure.Repositories;

namespace NetArchTechChallenge.Persistence
{
    public static class Application
    {
        public static IHost GetApplication(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.AddAppSettingsFile();

            builder.AddMessaging();
            builder.AddPersistence();

            builder.AddRepositories();
            builder.AddServices();
            builder.AddHostedServices();

            var app = builder.Build();

            ////using (var scope = app.Services.CreateScope())
            ////{
            ////    var migrationRunner = scope.ServiceProvider.GetRequiredService<DbInitializer>();
            ////    migrationRunner.Initialize();
            ////}

            return app;
        }

        private static IHostApplicationBuilder AddRepositories(this IHostApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IContactRepository, ContactRepository>();
            return builder;
        }

        private static IHostApplicationBuilder AddServices(this IHostApplicationBuilder builder)
        {
            builder.Services.AddSingleton<ContactPersistenceService>();
            return builder;
        }

        private static IHostApplicationBuilder AddHostedServices(this IHostApplicationBuilder builder)
        {
            builder.Services.AddHostedService<Worker>();
            return builder;
        }
    }
}
