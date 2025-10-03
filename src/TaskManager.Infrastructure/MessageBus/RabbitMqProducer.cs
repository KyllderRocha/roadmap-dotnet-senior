using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using TaskManager.Application.Common.Interfaces;

namespace TaskManager.Infrastructure.MessageBus;

public class RabbitMqProducer : IMessageBusProducer
{
    public async Task PublishAsync<T>(T message)
    {
        var factory = new ConnectionFactory { HostName = "rabbitmq"};
        // var factory = new ConnectionFactory { HostName = "localhost"};

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
        
        var queueName = message.GetType().Name.ToLower().Replace("event", "").Replace("command", "") + "-queue";
        await channel.QueueDeclareAsync(queue: queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        await channel.BasicPublishAsync(exchange: string.Empty,
                                        routingKey: queueName,
                                        body: body);
    }
}