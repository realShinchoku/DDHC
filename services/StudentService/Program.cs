using System.Data.Common;
using ApplicationBase.Extensions;
using Contracts.Lessons;
using EFCoreSecondLevelCacheInterceptor;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Polly;
using StudentService.Consumers;
using StudentService.Data;
using StudentService.Extensions;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Logging.AddLoggingService(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddIdentityService(builder.Configuration);
builder.Services.AddCachingService(builder.Configuration);
builder.Services.AddMonitor();
builder.Services.AddDbContext<DataContext>((serviceProvider, options) => options
    .AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>())
    .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), opt => opt.EnableRetryOnFailure())
);

builder.Services.AddMassTransit(opts =>
{
    opts.AddEntityFrameworkOutbox<DataContext>(opt =>
    {
        opt.QueryDelay = TimeSpan.FromSeconds(10);
        opt.UsePostgres();
        opt.UseBusOutbox();
    });

    opts.AddConsumersFromNamespaceContaining<AttendanceCreatedConsumer>();

    opts.AddRequestClient<LessonCreated>();

    opts.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("student", false));

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

        cfg.ReceiveEndpoint("student-attendance-created", e =>
        {
            e.UseMessageRetry(r => r.Interval(5, 5));
            e.ConfigureConsumer<AttendanceCreatedConsumer>(context);
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

var retryPolicy = Policy
    .Handle<NpgsqlException>()
    .WaitAndRetry(5, _ => TimeSpan.FromSeconds(10));

retryPolicy.ExecuteAndCapture(() => app.InitDb());

app.Run();

public partial class Program {};