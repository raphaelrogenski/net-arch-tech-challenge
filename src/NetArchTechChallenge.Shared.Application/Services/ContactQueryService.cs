using NetArchTechChallenge.Shared.Application.DTOs;
using ApplicationException = NetArchTechChallenge.Shared.Application.Exceptions.ApplicationException;
using NetArchTechChallenge.Shared.Domain.Repositories;

namespace NetArchTechChallenge.Shared.Application.Services
{
    public class ContactQueryService
    {
        private readonly IContactRepository contactRepository;

        public ContactQueryService(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public IList<ContactDto> List(string ddd)
        {
            return contactRepository.Query(tracking: false)
                .Where(r => string.IsNullOrEmpty(ddd) || r.PhoneDDD == ddd)
                .Select(ContactDto.Cast).ToList();
        }

        public bool ContactNameAlreadyExists(string contactName, Guid ignoreGuid = default)
        {
            return contactRepository.ContactNameAlreadyExists(contactName, ignoreGuid); 
        }

        public bool ContactPhoneAlreadyExists(string contactPhoneDDD, string contactPhoneNumber, Guid ignoreGuid = default)
        {
            return contactRepository.ContactPhoneAlreadyExists(contactPhoneDDD, contactPhoneNumber, ignoreGuid);
        }

        public bool ContactEmailAlreadyExists(string contactEmailAddress, Guid ignoreGuid = default)
        {
            return contactRepository.ContactEmailAlreadyExists(contactEmailAddress, ignoreGuid);
        }
    }
}
