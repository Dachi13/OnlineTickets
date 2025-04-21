using MessageSendingWorker;
using MessageSendingWorker.EmailSenderService;
using MessageSendingWorker.Models;
using MessageSendingWorker.RabbitMq;
using MessageSendingWorker.Repositories;
using Shared;

var builder = Host.CreateApplicationBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Database")!;
builder.Services.AddSingleton<DapperContext>(_ => new DapperContext(connectionString));
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.Configure<Config>(builder.Configuration.GetSection("Config"));
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddSingleton<IRabbitMqConsumer, RabbitMqConsumer>();
builder.Services.AddSingleton<IPersonRepository, PersonRepository>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();