using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetArchTechChallenge.Shared.Application.Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NetArchTechChallenge.Shared.Infrastructure.Messaging;

public class RabbitMQService : IMessageService, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<RabbitMQService> _logger;

    public RabbitMQService(IConfiguration configuration, ILogger<RabbitMQService> logger)
    {
        _logger = logger;
        var factory = new ConnectionFactory
        {
            Uri = new Uri("amqp://guest:guest@localhost:5672/")
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void Publish<T>(T message)
    {
        var queueName = typeof(T).Name;
        _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        _channel.BasicPublish("", queueName, null, body);

        _logger.LogInformation($"Mensagem publicada na fila {queueName}: {typeof(T).Name}");
    }

    public void Consume<T>(Func<T, Task> handler)
    {
        var queueName = typeof(T).Name;
        _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));

            if (message != null)
            {
                await handler(message);
                _channel.BasicAck(ea.DeliveryTag, false);
            }
        };

        _channel.BasicConsume(queueName, false, consumer);
        _logger.LogInformation($"Consumindo fila: {queueName}");
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
