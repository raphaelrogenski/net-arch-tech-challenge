using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace NetArchTechChallenge.Gateway
{
    public class Application
    {
        public void Run(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

            builder.Services.AddOcelot();

            var app = builder.Build();

            app.UseOcelot().Wait();
            app.Run();
        }
    }
}
