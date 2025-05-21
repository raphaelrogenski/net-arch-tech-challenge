namespace NetArchTechChallenge.Shared.Application.Validators
{
    public interface IContactUniquenessValidator
    {
        Task<bool> ContactNameAlreadyInUse(string name, Guid id);
        Task<bool> ContactPhoneAlreadyInUse(string ddd, string phone, Guid id);
        Task<bool> ContactEmailAlreadyInUse(string email, Guid id);
    }
}
