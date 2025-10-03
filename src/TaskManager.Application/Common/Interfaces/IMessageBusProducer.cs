namespace TaskManager.Application.Common.Interfaces;

public interface IMessageBusProducer
{
    Task PublishAsync<T>(T message);
}