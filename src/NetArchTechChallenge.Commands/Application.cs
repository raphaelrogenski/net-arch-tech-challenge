using NetArchTechChallenge.Shared.Application.Messaging;
using NetArchTechChallenge.Shared.Application.Services;
using NetArchTechChallenge.Shared.Application.Validators;
using NetArchTechChallenge.Shared.Infrastructure.DbContexts;
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

            var dbInit = new DatabaseInitializer();
            var task = Task.Run(() => dbInit.InitializeAsync());
            task.Wait();

            builder.Services.AddScoped<IMessageService, RabbitMQService>();

            builder.Services.AddScoped<ContactCommandsService>();

            builder.Services.AddHttpClient<IContactUniquenessValidator, ContactUniquenessValidator>(client =>
            {
#if DEBUG
                client.BaseAddress = new Uri("http://localhost:8111/api/");
#else
                client.BaseAddress = new Uri("http://svc-query:80/api/");
#endif
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
