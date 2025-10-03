using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using Microsoft.Extensions.Logging; // Adicionado para logs

namespace TaskManager.Api.Subscribers;

public class UserCreatedSubscriber : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<UserCreatedSubscriber> _logger;
    private const string QueueName = "usercreated-queue";

    public UserCreatedSubscriber(IServiceProvider serviceProvider, ILogger<UserCreatedSubscriber> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("UserCreatedSubscriber está iniciando.");
        
        var factory = new ConnectionFactory { HostName = "rabbitmq"};

        await using var connection = await factory.CreateConnectionAsync(stoppingToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(queue: QueueName,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null,
                                        cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("Mensagem recebida: {Message}", message);

            try
            {
                var userEvent = JsonSerializer.Deserialize<UserCreatedEvent>(message);

                if (userEvent != null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var userReplicaRepository = scope.ServiceProvider.GetRequiredService<IUserReplicaRepository>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var user = UserReplica.Create(userEvent.UserId, userEvent.Email);

                    var exists = await userReplicaRepository.GetByIdAsync(userEvent.UserId) != null;

                    if (!exists)
                    {
                        await userReplicaRepository.AddAsync(user);
                        await unitOfWork.SaveChangesAsync(stoppingToken);
                    }
                    
                    _logger.LogInformation("Usuário com e-mail {Email} processado com sucesso.", userEvent.Email);
                }
                
                await channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao processar a mensagem: {Message}", message);
                await channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: true);
            }
        };

        await channel.BasicConsumeAsync(queue: QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
        
        _logger.LogInformation("Consumidor iniciado na fila '{QueueName}'. Aguardando mensagens.", QueueName);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }

        _logger.LogInformation("UserCreatedSubscriber está parando.");
    }
}

// Defina este record para a deserialização da mensagem
public record UserCreatedEvent(Guid UserId, string Email);