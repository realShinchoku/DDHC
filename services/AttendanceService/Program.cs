using ApplicationBase.Extensions;
using AttendanceService.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Logging.AddLoggingService(builder.Configuration);
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddControllers();


builder.Services.AddMassTransit(opts =>
{
    opts.AddMongoDbOutbox(o =>
    {
        o.Connection = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException();
        o.DatabaseName = "AttendanceOutboxDb";
    });

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

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddScoped<GrpcStudentClient>();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseApplicationBuilder();

app.UseAuthorization();

app.MapControllers();

app.Run();