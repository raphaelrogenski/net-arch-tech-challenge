using NetArchTechChallenge.Shared.Application.DTOs;
using NetArchTechChallenge.Shared.Application.Services;
using NetArchTechChallenge.Shared.Domain.Events;

namespace NetArchTechChallenge.Persistence.Consumers
{
    public class ContactCreatedConsumer
    {
        private readonly ContactPersistenceService service;

        public ContactCreatedConsumer(ContactPersistenceService service)
        {
            this.service = service;
        }

        public async Task Handle(ContactCreatedEvent @event)
        {
            Console.WriteLine("Handling ContactCreatedEvent");

            var dto = new ContactDto()
            {
                Name = @event.name,
                PhoneDDD = @event.phoneDDD,
                PhoneNumber = @event.phoneNumber,
                EmailAddress = @event.emailAddress
            };

            service.Create(dto);
        }
    }
}
