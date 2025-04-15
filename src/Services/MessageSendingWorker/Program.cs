using MessageSendingWorker;
using MessageSendingWorker.EmailSenderService;
using MessageSendingWorker.Models;
using MessageSendingWorker.RabbitMq;
using MessageSendingWorker.Repositories;
using Shared;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<DapperContext>(sp => new DapperContext(sp.GetRequiredService<IConfiguration>()));
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.Configure<Config>(builder.Configuration.GetSection("Config"));
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddSingleton<IRabbitMqConsumer, RabbitMqConsumer>();
builder.Services.AddSingleton<IPersonRepository, PersonRepository>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();