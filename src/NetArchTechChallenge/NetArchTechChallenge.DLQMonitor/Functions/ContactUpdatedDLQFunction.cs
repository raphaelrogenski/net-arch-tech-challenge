using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NetArchTechChallenge.Shared.Application.Messaging;
using NetArchTechChallenge.Shared.Domain.Events;

namespace NetArchTechChallenge.DLQMonitor.Functions
{
    public class ContactUpdatedDLQFunction
    {
        private readonly ILogger _logger;
        private readonly IMessageService _messageService;

        public ContactUpdatedDLQFunction(ILoggerFactory loggerFactory, IMessageService messageService)
        {
            _logger = loggerFactory.CreateLogger<ContactUpdatedDLQFunction>();
            _messageService = messageService;
        }

        [Function("ContactUpdatedDLQFunction")]
        public void Run([TimerTrigger("%DlqTimerSchedule%")] TimerInfo myTimer)
        {
            while (true)
            {
                var message = _messageService.ConsumeFromDLQ<ContactUpdatedEvent>();
                if (message == null)
                {
                    _logger.LogInformation("No messages in UpdatedDLQ");
                    break;
                }

                _logger.LogWarning("ContactUpdatedDLQ Received: {0}", JsonSerializer.Serialize(message));
            }
        }
    }
}
