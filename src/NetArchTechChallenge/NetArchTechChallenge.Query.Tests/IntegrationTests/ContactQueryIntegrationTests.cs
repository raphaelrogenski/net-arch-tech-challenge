//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using NetArchTechChallenge.Query.Functions;
//using NetArchTechChallenge.Shared.Application.Services;
//using NetArchTechChallenge.Shared.Domain.Repositories;
//using NetArchTechChallenge.Shared.Infrastructure.DbContexts;
//using NetArchTechChallenge.Shared.Infrastructure.Repositories;

//namespace NetArchTechChallenge.Query.Tests.IntegrationTests
//{
//    public class ContactQueryIntegrationTests : IAsyncLifetime
//    {
//        private IServiceProvider _serviceProvider;

//        public async Task InitializeAsync()
//        {
//            var host = Host.CreateDefaultBuilder();
//            host.ConfigureAppConfiguration((hostingContext, config) =>
//            {
//                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
//                config.AddEnvironmentVariables();
//            });

//            host.ConfigureServices((context, services) =>
//            {
//                services.AddDbContext<AppDbContext>(options =>
//                     options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));

//                services.AddScoped<IContactRepository, ContactRepository>();

//                services.AddScoped<ContactQueryService>();

//                services.AddScoped<ContactQueryFunction>();
//                services.AddScoped<ContactNameAlreadyExistsFunction>();
//                services.AddScoped<ContactPhoneAlreadyExistsFunction>();
//                services.AddScoped<ContactEmailAlreadyExistsFunction>();
//            });

//            var app = host.Build();

//            _serviceProvider = app.Services;

//            app.Start();
//        }

//        public async Task DisposeAsync()
//        {
//        }

//        [Fact]
//        public async Task Query_ShouldReturnContacts_FromMockedRepository()
//        {
//            // Arrange
//            var queryFunction = _serviceProvider.GetRequiredService<ContactQueryFunction>();

//            var context = new DefaultHttpContext();
//            var request = context.Request;

//            // Act
//            var response = await queryFunction.Run(request);

//            // Assert
//            Assert.IsType<OkObjectResult>(response);
//        }
//    }
//}