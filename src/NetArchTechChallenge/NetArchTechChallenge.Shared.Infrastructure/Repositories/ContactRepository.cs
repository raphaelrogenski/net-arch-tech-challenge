using NetArchTechChallenge.Shared.Domain.Entities;
using NetArchTechChallenge.Shared.Domain.Repositories;
using NetArchTechChallenge.Shared.Infrastructure.Base;
using NetArchTechChallenge.Shared.Infrastructure.DbContexts;

namespace NetArchTechChallenge.Shared.Infrastructure.Repositories
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(AppDbContext context)
        : base(context)
        {
        }

        public bool ContactNameAlreadyExists(string contactName, Guid ignoreGuid = default)
        {
            var query = Query().Where(r => r.Name == contactName);

            if (ignoreGuid != Guid.Empty)
                query = query.Where(r => r.Id != ignoreGuid);

            return query.Any();
        }

        public bool ContactPhoneAlreadyExists(string contactPhoneDDD, string contactPhoneNumber, Guid ignoreGuid = default)
        {
            var query = Query().Where(r => r.PhoneDDD == contactPhoneDDD && r.PhoneNumber == contactPhoneNumber);

            if (ignoreGuid != Guid.Empty)
                query = query.Where(r => r.Id != ignoreGuid);

            return query.Any();
        }

        public bool ContactEmailAlreadyExists(string contactEmailAddress, Guid ignoreGuid = default)
        {
            var query = Query().Where(r => r.EmailAddress == contactEmailAddress);

            if (ignoreGuid != Guid.Empty)
                query = query.Where(r => r.Id != ignoreGuid);

            return query.Any();
        }
    }
}
