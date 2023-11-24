using ApplicationBase.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Polly;
using StudentService.Consumers;
using StudentService.Data;
using StudentService.Models;
using StudentService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Logging.AddLoggingService(builder.Configuration);
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    if (builder.Environment.IsDevelopment()) options.EnableSensitiveDataLogging();
});
builder.Services.AddMassTransit(opts =>
{

    opts.AddEntityFrameworkOutbox<DataContext>(opt =>
    {
        opt.QueryDelay = TimeSpan.FromSeconds(10);
        opt.UseSqlServer();
        opt.UseBusOutbox();
    });

    opts.AddConsumersFromNamespaceContaining<StudentCreatedConsumer>();
    
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
        
        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddGrpc();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseApplicationBuilder();

app.MapControllers();

app.MapGrpcService<GrpcStudentService>();

var retryPolicy = Policy
    .Handle<Exception>()
    .WaitAndRetry(5, _ => TimeSpan.FromSeconds(10));

retryPolicy.ExecuteAndCapture(() => app.InitDb());

app.Run();