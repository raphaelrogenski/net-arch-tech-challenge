namespace NetArchTechChallenge.Shared.Domain.Events
{
    public record ContactCreatedEvent(string name, string phoneDDD, string phoneNumber, string emailAddress);
}
