using NetArchTechChallenge.Shared.Application.Validators;

namespace NetArchTechChallenge.Shared.Infrastructure.Validators
{
    public class ContactUniquenessValidator : IContactUniquenessValidator
    {
        private readonly HttpClient _client;

        public ContactUniquenessValidator(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> ContactNameAlreadyInUse(string name, Guid id)
        {
            var url = $"Contacts/NameAlreadyExists?name={Uri.EscapeDataString(name)}&id={id}";
            return await GetBooleanResultAsync(url);
        }

        public async Task<bool> ContactPhoneAlreadyInUse(string ddd, string phone, Guid id)
        {
            var url = $"Contacts/PhoneAlreadyExists?ddd={Uri.EscapeDataString(ddd)}&phone={Uri.EscapeDataString(phone)}&id={id}";
            return await GetBooleanResultAsync(url);
        }

        public async Task<bool> ContactEmailAlreadyInUse(string email, Guid id)
        {
            var url = $"Contacts/EmailAlreadyExists?email={Uri.EscapeDataString(email)}&id={id}";
            return await GetBooleanResultAsync(url);
        }

        private async Task<bool> GetBooleanResultAsync(string url)
        {
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return bool.TryParse(result, out var value) && value;
        }
    }
}
