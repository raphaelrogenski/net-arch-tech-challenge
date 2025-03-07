using Microsoft.EntityFrameworkCore;
using NetArchTechChallenge.Shared.Application.Messaging;
using NetArchTechChallenge.Shared.Application.Services;
using NetArchTechChallenge.Shared.Domain.Repositories;
using NetArchTechChallenge.Shared.Infrastructure.Base;
using NetArchTechChallenge.Shared.Infrastructure.DbContexts;
using NetArchTechChallenge.Shared.Infrastructure.Messaging;
using NetArchTechChallenge.Shared.Infrastructure.Repositories;
using Prometheus;
using System;

namespace NetArchTechChallenge.API
{
    public static class Application
    {
        public static IHost GetApplication(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
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

        protected override IHostApplicationBuilder GetBuilder(string[] args)
        {
            return WebApplication.CreateBuilder(args);
        }

        protected override void AddServices(IHostApplicationBuilder builder)
        {
            // Messaging
            base.AddMessaging(builder);

            // Services
            builder.Services.AddScoped<ContactAPIService>();

            // Controllers
            builder.Services.AddControllers();

            //Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }

        protected override void ConfigureApplication(IHost app)
        {
            var webapp = app as WebApplication;

            // Enable Swagger
            if (webapp.Environment.IsDevelopment())
            {
                webapp.UseSwagger();
                webapp.UseSwaggerUI();
            }

            // Enable Https
            webapp.UseHttpsRedirection();

            // Emable Authorization
            webapp.UseAuthorization();

            // Map Controllers
            webapp.MapControllers();

            //// Enable metrics for latency, requests by endpoint, etc
            //webapp.UseHttpMetrics();

            //// Enable route /metrics, default for prometheus
            //webapp.UseMetricServer();
        }
    }
}
