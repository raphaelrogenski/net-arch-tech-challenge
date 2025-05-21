using NetArchTechChallenge.Persistence.Consumers;
using NetArchTechChallenge.Shared.Application.Messaging;
using NetArchTechChallenge.Shared.Domain.Events;

namespace NetArchTechChallenge.Persistence.Workers;

public class ConsumersWorker : BackgroundService
{
    private readonly IServiceScopeFactory scopeFactory;

    public ConsumersWorker(IServiceScopeFactory scopeFactory)
    {
        this.scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
            var createdConsumer = scope.ServiceProvider.GetRequiredService<ContactCreatedConsumer>();
            var updatedConsumer = scope.ServiceProvider.GetRequiredService<ContactUpdatedConsumer>();
            var deletedConsumer = scope.ServiceProvider.GetRequiredService<ContactDeletedConsumer>();

            messageService.Consume<ContactCreatedEvent>(createdConsumer.Handle);
            messageService.Consume<ContactUpdatedEvent>(updatedConsumer.Handle);
            messageService.Consume<ContactDeletedEvent>(deletedConsumer.Handle);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
