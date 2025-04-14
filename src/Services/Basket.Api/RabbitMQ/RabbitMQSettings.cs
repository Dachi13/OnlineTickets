namespace Basket.Api.RabbitMQ;

public class RabbitMqSettings
{
    public string Host { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
    public string QueueName { get; init; }
}