using ApplicationBase.Extensions;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Consumers;
using NotificationService.Data;
using NotificationService.Hubs;
using NotificationService.RequestHelpers;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Logging.AddLoggingService(builder.Configuration);
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddIdentityService(builder.Configuration);
builder.Services.AddMonitor();
builder.Services.AddMassTransit(opts =>
{
    opts.AddConsumersFromNamespaceContaining<NotificationCreatedConsumer>();

    opts.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("notify", false));

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

        cfg.ReceiveEndpoint("notify-notification-created", e =>
        {
            e.UseMessageRetry(r => r.Interval(5, 5));
            e.ConfigureConsumer<NotificationCreatedConsumer>(context);
        });

        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, EmailBasedUserIdProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();
app.UseApplicationIdentity();
app.MapControllers();
app.UseMonitor();
app.MapHub<NotificationHub>("/notify");

app.Lifetime.ApplicationStarted.Register(() =>
    Policy.Handle<TimeoutException>()
        .WaitAndRetryAsync(5, _ => TimeSpan.FromSeconds(10))
        .ExecuteAndCaptureAsync(async () => await app.InitDb()));

app.Run();