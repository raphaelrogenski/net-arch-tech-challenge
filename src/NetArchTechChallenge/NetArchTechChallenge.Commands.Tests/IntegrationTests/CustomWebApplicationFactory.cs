//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.DependencyInjection.Extensions;
//using Moq;
//using NetArchTechChallenge.Commands;
//using NetArchTechChallenge.Shared.Application.Validators;

//namespace NetArchTechChallenge.Commands.Tests.IntegrationTests
//{
//    public class CustomWebApplicationFactory : WebApplicationFactory<Application>
//    {
//        protected override void ConfigureWebHost(IWebHostBuilder builder)
//        {
//            builder.ConfigureServices(services =>
//            {
//                // Remove o IContactUniquenessValidator real
//                var descriptor = services.SingleOrDefault(d =>
//                    d.ServiceType == typeof(IContactUniquenessValidator));
//                if (descriptor != null)
//                    services.Remove(descriptor);

//                // Adiciona um mock que sempre retorna false
//                services.AddSingleton(provider =>
//                {
//                    var mock = new Mock<IContactUniquenessValidator>();
//                    mock.Setup(x => x.ContactEmailAlreadyInUse(It.IsAny<string>(), It.IsAny<Guid>()))
//                        .ReturnsAsync(false);
//                    return mock.Object;
//                });
//            });
//        }
//    }
//}
