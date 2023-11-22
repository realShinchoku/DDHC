using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApplicationBase.Extensions;

public static class Application
{
    public static void AddApplicationService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Production) return;
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public static void UseApplicationBuilder(this IApplicationBuilder app)
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Production) return;
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}