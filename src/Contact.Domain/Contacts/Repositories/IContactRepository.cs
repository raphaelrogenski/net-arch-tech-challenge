using Contacts.Domain.Common.Repositories;
using Contacts.Domain.Contacts.Models;

namespace Contacts.Domain.Contacts.Repositories;

public interface IContactRepository : IRepositoryBase<Contact>
{
    bool ContactNameAlreadyExists(string contactName, Guid ignoreGuid = default);
    bool ContactPhoneAlreadyExists(string contactPhoneDDD, string contactPhoneNumber, Guid ignoreGuid = default);
    bool ContactEmailAlreadyExists(string contactEmailAddress, Guid ignoreGuid = default);
}
