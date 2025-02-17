﻿using Contacts.Application.DI;
using Contacts.Infrastructure.Contexts;
using Contacts.Infrastructure.DI;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using System.Diagnostics.CodeAnalysis;

namespace Contacts.Api
{
    [ExcludeFromCodeCoverage]
    public static class Application
    {
        public static WebApplication GetWebApplication(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            builder.Services.AddSingleton<DbInitializer>();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddInfrastructureDI();
            builder.Services.AddApplicationDI();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var migrationRunner = scope.ServiceProvider.GetRequiredService<DbInitializer>();
                migrationRunner.Initialize();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            // Enable metrics for latency, requests by endpoint, etc
            app.UseHttpMetrics();

            // Enable route /metrics, default for prometheus
            app.UseMetricServer();

            return app;
        }
    }
}