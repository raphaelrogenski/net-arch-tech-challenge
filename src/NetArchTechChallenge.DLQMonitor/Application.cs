using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetArchTechChallenge.Shared.Application.Messaging;
using NetArchTechChallenge.Shared.Infrastructure.Messaging;

namespace NetArchTechChallenge.DLQMonitor
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

            builder.Services.AddScoped<IMessageService, RabbitMQService>();

            var app = builder.Build();
            app.Run();
        }
    }
}
