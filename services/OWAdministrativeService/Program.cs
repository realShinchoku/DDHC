using ApplicationBase.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using OWAdministrativeService.Data;
using OWAdministrativeService.FileHelper;
using OWAdministrativeService.Validators;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Logging.AddLoggingService(builder.Configuration);
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddIdentityService(builder.Configuration);
builder.Services.AddMonitor();
builder.Services.AddScoped<IFileHelper, FileHelper>();
builder.Services.AddValidatorsFromAssemblyContaining<StudentCardFormDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddMassTransit(opts =>
{
    opts.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("oneway", false));

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

builder.Services.AddCors(options =>
    options.AddPolicy("CorsPolicy", b =>
        b.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true)
    )
);


var app = builder.Build();
app.UseCors("CorsPolicy");
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