using System.Net;
using ApplicationBase.Extensions;
using AttendanceService.Consumers;
using AttendanceService.Data;
using AttendanceService.Services;
using MassTransit;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Logging.AddLoggingService(builder.Configuration);
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddIdentityService(builder.Configuration);
builder.Services.AddHttpClient<StudentServiceHttpClient>().AddPolicyHandler(GetPolicy());
builder.Services.AddMonitor();
builder.Services.AddMassTransit(opts =>
{
    opts.AddConsumersFromNamespaceContaining<LessonCreatedConsumer>();

    opts.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("attendance", false));

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

        cfg.ReceiveEndpoint("attendance-lesson-created", e =>
        {
            e.UseMessageRetry(r => r.Interval(5, 5));
            e.ConfigureConsumer<LessonCreatedConsumer>(context);
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();
app.UseApplicationIdentity();
app.MapControllers();
app.UseMonitor();

app.Lifetime.ApplicationStarted.Register(() =>
    Policy.Handle<TimeoutException>()
        .WaitAndRetryAsync(5, _ => TimeSpan.FromSeconds(10))
        .ExecuteAndCaptureAsync(async () => await app.InitDb())
);

app.Run();

return;

static IAsyncPolicy<HttpResponseMessage> GetPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));
}