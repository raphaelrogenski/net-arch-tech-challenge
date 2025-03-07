using NetArchTechChallenge.Shared.Application.DTOs;
using ApplicationException = NetArchTechChallenge.Shared.Application.Exceptions.ApplicationException;
using NetArchTechChallenge.Shared.Domain.Repositories;

namespace NetArchTechChallenge.Shared.Application.Services
{
    public class ContactPersistenceService
    {
        private readonly IContactRepository contactRepository;

        public ContactPersistenceService(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public void Create(ContactDto dto)
        {
            EnsureValidation(dto);

            var entity = ContactDto.Cast(dto);
            contactRepository.Create(entity);
        }

        public void Update(ContactDto dto)
        {
            EnsureValidation(dto);

            var entity = contactRepository.GetById(dto.Id);
            var updatedEntity = ContactDto.Cast(dto, entity);

            contactRepository.Update(updatedEntity);
        }

        public void Delete(Guid id)
        {
            contactRepository.Delete(id);
        }

        private void EnsureValidation(ContactDto dto)
        {
            var errorList = new List<string>();

            var nameAlreadyInUse = contactRepository.ContactNameAlreadyExists(dto.Name, dto.Id);
            if (nameAlreadyInUse)
                errorList.Add("Name already in use!");

            var phoneAlreadyInUse = contactRepository.ContactPhoneAlreadyExists(dto.PhoneDDD, dto.PhoneNumber, dto.Id);
            if (phoneAlreadyInUse)
                errorList.Add("Phone already in use!");

            var emailAlreadyInUse = contactRepository.ContactEmailAlreadyExists(dto.EmailAddress, dto.Id);
            if (emailAlreadyInUse)
                errorList.Add("Email already in use!");

            if (errorList.Count > 0)
                throw new ApplicationException(string.Join("\n", errorList));
        }
    }
}
