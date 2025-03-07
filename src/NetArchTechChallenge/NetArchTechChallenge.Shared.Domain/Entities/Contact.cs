using NetArchTechChallenge.Shared.Domain.Base;

namespace NetArchTechChallenge.Shared.Domain.Entities
{
    public class Contact : EntityBase
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneDDD { get; set; }
        public string EmailAddress { get; set; }
    }
}
