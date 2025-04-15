namespace MessageSendingWorker.RabbitMq;

public interface IRabbitMqConsumer
{
    Task ConsumeAsync();
}