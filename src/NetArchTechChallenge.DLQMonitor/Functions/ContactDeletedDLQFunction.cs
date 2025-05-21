using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NetArchTechChallenge.Shared.Application.Messaging;
using NetArchTechChallenge.Shared.Domain.Events;

namespace NetArchTechChallenge.DLQMonitor.Functions
{
    public class ContactDeletedDLQFunction
    {
        private readonly ILogger _logger;
        private readonly IMessageService _messageService;

        public ContactDeletedDLQFunction(ILoggerFactory loggerFactory, IMessageService messageService)
        {
            _logger = loggerFactory.CreateLogger<ContactDeletedDLQFunction>();
            _messageService = messageService;
        }

        [Function("ContactDeletedDLQFunction")]
        public void Run([TimerTrigger("*/30 * * * * *")] TimerInfo myTimer)
        {
            while (true)
            {
                var message = _messageService.ConsumeFromDLQ<ContactDeletedEvent>();
                if (message == null)
                {
                    _logger.LogInformation("No messages in DeletedDLQ");
                    break;
                }

                _logger.LogWarning("ContactDeletedDLQ Received: {0}", JsonSerializer.Serialize(message));
            }
        }
    }
}
