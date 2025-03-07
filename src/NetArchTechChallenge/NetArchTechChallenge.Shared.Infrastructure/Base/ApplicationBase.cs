using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetArchTechChallenge.Shared.Application.Messaging;
using NetArchTechChallenge.Shared.Infrastructure.DbContexts;
using NetArchTechChallenge.Shared.Infrastructure.Messaging;

namespace NetArchTechChallenge.Shared.Infrastructure.Base
{
    public static class ApplicationBase
    {
        public static IHostApplicationBuilder AddAppSettingsFile(this IHostApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            return builder;
        }

        public static IHostApplicationBuilder AddMessaging(this IHostApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IMessageService, RabbitMQService>();
            return builder;
        }

        public static IHostApplicationBuilder AddPersistence(this IHostApplicationBuilder builder)
        {
            builder.Services.AddSingleton<DbInitializer>();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            return builder;
        }
    }
}
