using MessageSendingWorker.RabbitMq;

namespace MessageSendingWorker;

public class Worker : BackgroundService
{
    private readonly IRabbitMqConsumer _rabbitMqConsumer;

    public Worker(IRabbitMqConsumer rabbitMqConsumer)
    {
        _rabbitMqConsumer = rabbitMqConsumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _rabbitMqConsumer.ConsumeAsync();

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}