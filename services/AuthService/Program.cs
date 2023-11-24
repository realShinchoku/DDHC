using ApplicationBase.Extensions;
using AuthService.Consumers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddLoggingService(builder.Configuration);
builder.Services.AddIdentityService(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddMassTransit(opts =>
{
    opts.AddConsumersFromNamespaceContaining<StudentCreatedFaultConsumer>();

    opts.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auth", false));
    
    opts.UsingRabbitMq((context, cfg) =>
    {
        cfg.UseMessageRetry(r =>
        {
            r.Handle<RabbitMqConnectionException>();
            r.Interval(5, TimeSpan.FromSeconds(10));
        });

        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", host =>
        {
            host.Username(builder.Configuration.GetValue("RabbitMQ:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMQ:Password", "guest"));
        });

        cfg.ConfigureEndpoints(context);
    });
});

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseApplicationIdentity();
app.MapControllers();
app.Run();