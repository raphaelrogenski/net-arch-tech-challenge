using NetArchTechChallenge.Shared.Domain.Entities;
using ApplicationException = NetArchTechChallenge.Shared.Application.Exceptions.ApplicationException;

namespace NetArchTechChallenge.Shared.Application.DTOs
{
    public class ContactDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneDDD { get; set; }
        public string EmailAddress { get; set; }

        public static ContactDto Cast(Contact entity)
        {
            var dto = new ContactDto
            {
                Id = entity.Id,
                CreatedAt = entity.CreatedAt,
                Name = entity.Name,
                PhoneDDD = entity.PhoneDDD,
                PhoneNumber = entity.PhoneNumber,
                EmailAddress = entity.EmailAddress,
            };

            return dto;
        }

        public static Contact Cast(ContactDto dto, Contact entity = null)
        {
            if (dto.Id == Guid.Empty)
                entity = new Contact();

            if (entity == null)
                throw new ApplicationException("Entry not found!");

            if (dto.Id != Guid.Empty)
            {
                var hasChanged = false;
                hasChanged |= entity.Name != dto.Name;
                hasChanged |= entity.PhoneDDD != dto.PhoneDDD;
                hasChanged |= entity.PhoneNumber != dto.PhoneNumber;
                hasChanged |= entity.EmailAddress != dto.EmailAddress;

                if (!hasChanged)
                    throw new ApplicationException("Nothing to update!");
            }

            entity.Name = dto.Name;
            entity.PhoneDDD = dto.PhoneDDD;
            entity.PhoneNumber = dto.PhoneNumber;
            entity.EmailAddress = dto.EmailAddress;

            return entity;
        }
    }
}
