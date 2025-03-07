using NetArchTechChallenge.Shared.Domain.Base;
using NetArchTechChallenge.Shared.Domain.Entities;

namespace NetArchTechChallenge.Shared.Domain.Repositories
{
    public interface IContactRepository : IRepositoryBase<Contact>
    {
        bool ContactNameAlreadyExists(string contactName, Guid ignoreGuid = default);
        bool ContactPhoneAlreadyExists(string contactPhoneDDD, string contactPhoneNumber, Guid ignoreGuid = default);
        bool ContactEmailAlreadyExists(string contactEmailAddress, Guid ignoreGuid = default);
    }
}
