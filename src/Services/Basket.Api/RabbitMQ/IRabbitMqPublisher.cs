namespace Basket.Api.RabbitMQ;

public interface IRabbitMqPublisher
{
    Task Publish(string message);
}