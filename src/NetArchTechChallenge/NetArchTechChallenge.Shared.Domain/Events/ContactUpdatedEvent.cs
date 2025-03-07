namespace NetArchTechChallenge.Shared.Domain.Events
{
    public record ContactUpdatedEvent(Guid id, string name, string phoneDDD, string phoneNumber, string emailAddress);
}
