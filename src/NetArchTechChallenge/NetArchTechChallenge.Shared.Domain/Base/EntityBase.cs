namespace NetArchTechChallenge.Shared.Domain.Base
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
