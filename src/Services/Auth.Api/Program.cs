var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("UsersDb")!;

// Configure database
builder.Services.AddScoped<DapperContext>(_ => new DapperContext(connectionString));
builder.Services.AddScoped<IRegisterUserRepository, RegisterUserRepository>();
builder.Services.AddScoped<IGetUserRepository, GetUserRepository>();
builder.Services.AddScoped<JwtTokenGenerator>();

// Configure Auth
builder.Services.AddScoped<PasswordHasher<object>>();

// Add services to the container.
var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

var app = builder.Build();

app.AddRegisterUserRoute();
app.AddUserLoginRoute();

app.Run();