using ApplicationBase.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.AddLoggingService(builder.Configuration);
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
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
app.MapReverseProxy();
app.Run();