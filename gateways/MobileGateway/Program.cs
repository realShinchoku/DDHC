using ApplicationBase.Extensions;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Yarp.ReverseProxy.Swagger;
using Yarp.ReverseProxy.Swagger.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Logging.AddLoggingService(builder.Configuration);
builder.Services.AddIdentityService(builder.Configuration);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddSwagger(builder.Configuration.GetSection("ReverseProxy"));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    var filterDescriptors = new List<FilterDescriptor>();
    foreach (var cluster in builder.Configuration.GetSection("ReverseProxy:Clusters").GetChildren())
        opts.SwaggerDoc(cluster.Key, new OpenApiInfo { Title = cluster.Key, Version = cluster.Key });
    filterDescriptors.Add(new FilterDescriptor
    {
        Type = typeof(ReverseProxyDocumentFilter),
        Arguments = Array.Empty<object>()
    });
    opts.DocumentFilterDescriptors = filterDescriptors;
});


var app = builder.Build();

// Configure the HTTP request pipeline.

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var cluster in builder.Configuration.GetSection("ReverseProxy:Clusters").GetChildren())
            options.SwaggerEndpoint($"/swagger/{cluster.Key}/swagger.json", cluster.Key);
    });
}

app.MapReverseProxy();

app.UseApplicationIdentity();

app.Run();