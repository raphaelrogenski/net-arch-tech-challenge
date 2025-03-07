namespace NetArchTechChallenge.Shared.Application.Messaging
{
    public interface IMessageService
    {
        void Publish<T>(T message);
        void Consume<T>(Func<T, Task> handler);
    }
}
