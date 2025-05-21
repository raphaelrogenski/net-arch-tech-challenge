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
#if DEBUG
        var connString = "amqp://guest:guest@localhost:5672/"; //// configuration.GetConnectionString("RabbitMQ");
#else
        var connString = "amqp://guest:guest@svc-rabbitmq:5672/"; //// configuration.GetConnectionString("RabbitMQ");
#endif
        ////var connString = "amqp://guest:guest@svc-rabbitmq:5672/"; //// configuration.GetConnectionString("RabbitMQ");

        _logger.Log(LogLevel.Information, "Connecting to RabbitMQ on ...");
        _logger.Log(LogLevel.Information, connString);

        var factory = new ConnectionFactory
        {
            Uri = new Uri(connString)
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void Publish<T>(T message)
    {
        var queueName = typeof(T).Name;
        var dlqName = $"{queueName}-dlq";

        var args = new Dictionary<string, object>();
        args["x-dead-letter-exchange"] = "";
        args["x-dead-letter-routing-key"] = dlqName;

        _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: args);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        _channel.BasicPublish("", queueName, null, body);

        _logger.LogInformation($"Mensagem publicada na fila {queueName}: {typeof(T).Name}");
    }

    public void Consume<T>(Func<T, Task> handler, bool useDeadLetter = true)
    {
        var queueName = typeof(T).Name;
        var dlqName = $"{queueName}-dlq";

        var args = new Dictionary<string, object>();

        if (useDeadLetter)
        {
            args["x-dead-letter-exchange"] = "";
            args["x-dead-letter-routing-key"] = dlqName;
        }

        _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: args);
        _channel.QueueDeclare(queue: dlqName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));

                if (message != null)
                {
                    await handler(message);
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao processar mensagem da fila {queueName}");
                _channel.BasicNack(ea.DeliveryTag, false, false);
            }
        };

        _channel.BasicConsume(queueName, autoAck: false, consumer);
        _logger.LogInformation($"Consumindo fila: {queueName}");
    }

    public T? ConsumeFromDLQ<T>()
    {
        var queueName = $"{typeof(T).Name}-dlq";

        var result = _channel.BasicGet(queueName, autoAck: true);
        if (result == null)
            return default;

        var body = result.Body.ToArray();
        var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));

        return message;
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
