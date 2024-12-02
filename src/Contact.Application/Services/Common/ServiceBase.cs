using Contacts.Domain.Common;
using Contacts.Domain.Common.Repositories;

namespace Contacts.Application.Services.Common;

public class ServiceBase<TEntity, TRepository>
    where TEntity : EntityBase
    where TRepository : IRepositoryBase<TEntity>
{
    protected TRepository Repository { get; }

    public ServiceBase(TRepository repository)
    {
        Repository = repository;
    }
}
