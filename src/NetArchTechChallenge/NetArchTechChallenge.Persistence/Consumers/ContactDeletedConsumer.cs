using NetArchTechChallenge.Shared.Application.Services;
using NetArchTechChallenge.Shared.Domain.Events;

namespace NetArchTechChallenge.Persistence.Consumers
{
    public class ContactDeletedConsumer
    {
        private readonly ContactPersistenceService service;

        public ContactDeletedConsumer(ContactPersistenceService service)
        {
            this.service = service;
        }

        public async Task Handle(ContactDeletedEvent @event)
        {
            var id = @event.id;
            service.Delete(id);
        }
    }
}
