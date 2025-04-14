using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Basket.Api.RabbitMQ;

public class RabbitMqPublisher(IOptions<RabbitMqSettings> options) : IRabbitMqPublisher
{
    private readonly RabbitMqSettings _settings = options.Value;

    public async Task Publish(string message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _settings.Host,
            UserName = _settings.Username,
            Password = _settings.Password
        };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: _settings.QueueName, durable: false, exclusive: false,
            autoDelete: false);

        var body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(exchange: "", routingKey: _settings.QueueName, body: body);
    }
}