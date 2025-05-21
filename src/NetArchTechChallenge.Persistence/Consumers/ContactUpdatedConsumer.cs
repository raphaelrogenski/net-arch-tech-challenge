using NetArchTechChallenge.Shared.Application.DTOs;
using NetArchTechChallenge.Shared.Application.Services;
using NetArchTechChallenge.Shared.Domain.Events;

namespace NetArchTechChallenge.Persistence.Consumers
{
    public class ContactUpdatedConsumer
    {
        private readonly ContactPersistenceService service;

        public ContactUpdatedConsumer(ContactPersistenceService service)
        {
            this.service = service;
        }

        public async Task Handle(ContactUpdatedEvent @event)
        {
            Console.WriteLine("Handling ContactUpdatedEvent");

            var dto = new ContactDto()
            {
                Id = @event.id,
                Name = @event.name,
                PhoneDDD = @event.phoneDDD,
                PhoneNumber = @event.phoneNumber,
                EmailAddress = @event.emailAddress
            };

            service.Update(dto);
        }
    }
}
