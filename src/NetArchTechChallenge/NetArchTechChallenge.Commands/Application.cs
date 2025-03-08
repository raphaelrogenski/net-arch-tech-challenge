using NetArchTechChallenge.Shared.Application.Messaging;
using NetArchTechChallenge.Shared.Application.Services;
using NetArchTechChallenge.Shared.Application.Validators;
using NetArchTechChallenge.Shared.Infrastructure.Messaging;
using NetArchTechChallenge.Shared.Infrastructure.Validators;
using Prometheus;

namespace NetArchTechChallenge.Commands
{
    public class Application
    {
        public void Run(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddScoped<IMessageService, RabbitMQService>();

            builder.Services.AddScoped<ContactCommandsService>();

            builder.Services.AddHttpClient<IContactUniquenessValidator, ContactUniquenessValidator>(client =>
            {
                client.BaseAddress = new Uri("http://app-query:80/api/");
            });

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Middleware Prometheus
            app.UseMetricServer(url: "/api/metrics");
            app.UseHttpMetrics();

            app.Run();
        }
    }
}
