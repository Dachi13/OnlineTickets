namespace MessageSendingWorker.RabbitMq;

public class RabbitMqConsumer : IRabbitMqConsumer, IAsyncDisposable
{
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<RabbitMqConsumer> _logger;
    private readonly IEmailSender _service;
    private readonly IPersonRepository _repository;
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqConsumer(
        IOptions<RabbitMqSettings> options,
        ILogger<RabbitMqConsumer> logger,
        IEmailSender service, IPersonRepository repository)
    {
        _logger = logger;
        _service = service;
        _repository = repository;
        _settings = options.Value;
    }

    public async Task ConsumeAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.Host,
            UserName = _settings.Username,
            Password = _settings.Password
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(
            queue: _settings.QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var basketJson = Encoding.UTF8.GetString(body);

            EventsBasket? eventsBasket = null;
            try
            {
                eventsBasket = JsonSerializer.Deserialize<EventsBasket>(basketJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[DeserializationError] Invalid JSON: {basketJson}");
            }

            if (eventsBasket == null)
            {
                _logger.LogError($"[Error] basket is null: \n {basketJson} \n Time: {DateTimeOffset.Now}");
                return;
            }

            _logger.LogInformation($"[Consumed] basket: {basketJson} Time: {DateTimeOffset.Now}");

            var person = await _repository.GetPersonAsync(eventsBasket.BuyerId);

            if (person == null)
            {
                _logger.LogError($"[Error] person was not found {eventsBasket.BuyerId} \n Time: {DateTimeOffset.Now}");
                return;
            }

            var subject = "🎟️Your Ticket Purchase Was Successful!";
            var message = $"You have successfully purchased the ticket\n" +
                          $"Amount: {eventsBasket.Events.Count}\n" +
                          $"Total Price: {eventsBasket.TotalPrice}";

            await _service.SendEmailAsync(subject, person.Email, person.Name, message);
        };

        await _channel.BasicConsumeAsync(
            queue: _settings.QueueName,
            autoAck: true,
            consumer: consumer);
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null) await _connection.DisposeAsync();
        if (_channel != null) await _channel.DisposeAsync();
    }
}