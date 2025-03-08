using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NetArchTechChallenge.Shared.Application.Messaging;
using NetArchTechChallenge.Shared.Domain.Events;

namespace NetArchTechChallenge.DLQMonitor.Functions
{
    public class ContactCreatedDLQFunction
    {
        private readonly ILogger _logger;
        private readonly IMessageService _messageService;

        public ContactCreatedDLQFunction(ILoggerFactory loggerFactory, IMessageService messageService)
        {
            _logger = loggerFactory.CreateLogger<ContactCreatedDLQFunction>();
            _messageService = messageService;
        }

        [Function("ContactCreatedDLQFunction")]
        public void Run([TimerTrigger("%DlqTimerSchedule%")] TimerInfo myTimer)
        {
            while (true)
            {
                var message = _messageService.ConsumeFromDLQ<ContactCreatedEvent>();
                if (message == null)
                {
                    _logger.LogInformation("No messages in CreatedDLQ");
                    break;
                }

                _logger.LogWarning("ContactCreatedDLQ Received: {0}", JsonSerializer.Serialize(message));
            }
        }
    }
}
