using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetArchTechChallenge.Persistence;
using NetArchTechChallenge.Persistence.Consumers;
using NetArchTechChallenge.Persistence.Workers;
using NetArchTechChallenge.Shared.Application.Messaging;
using NetArchTechChallenge.Shared.Application.Services;
using NetArchTechChallenge.Shared.Domain.Events;
using NetArchTechChallenge.Shared.Domain.Repositories;
using NetArchTechChallenge.Shared.Infrastructure.DbContexts;
using NetArchTechChallenge.Shared.Infrastructure.Messaging;
using NetArchTechChallenge.Shared.Infrastructure.Repositories;

namespace NetArchTechChallenge.Persistence.Tests.IntegrationTests
{
    public class ContactPersistenceIntegrationTests : IAsyncLifetime
    {
        private Application _application;
        private IServiceProvider _serviceProvider;

        public async Task InitializeAsync()
        {
            _application = new Application();
            _application.Run(Array.Empty<string>());

            var host = Host.CreateDefaultBuilder();
            host.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                config.AddEnvironmentVariables();
            });

            host.ConfigureServices((context, services) =>
            {
                services.AddDbContext<AppDbContext>(options =>
                     options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));

                services.AddScoped<IMessageService, RabbitMQService>();

                services.AddScoped<IContactRepository, ContactRepository>();
            });

            var app = host.Build();

            _serviceProvider = app.Services;

            app.Start();
        }

        public async Task DisposeAsync()
        {
        }

        [Fact]
        public async Task ContactCreatedEvent_ShouldPersistContactInDatabase()
        {
            // Arrange
            var messageService = _serviceProvider.GetRequiredService<IMessageService>();
            var contactRepository = _serviceProvider.GetRequiredService<IContactRepository>();

            var name = $"Raphael{Guid.NewGuid}";
            var phoneDDD = "42";
            var phoneNumber = "999999999";
            var emailAddress = "test@email.com";

            var @event = new ContactCreatedEvent(name, phoneDDD, phoneNumber, emailAddress);

            // Act
            messageService.Publish(@event);

            // Assert
            await Task.Delay(2000);

            var contact = await contactRepository.Query(tracking: false).FirstOrDefaultAsync(r => r.Name == name);
            Assert.NotNull(contact);
            Assert.Equal(name, contact.Name);
            Assert.Equal(phoneDDD, contact.PhoneDDD);
            Assert.Equal(phoneNumber, contact.PhoneNumber);
            Assert.Equal(emailAddress, contact.EmailAddress);
        }
    }
}