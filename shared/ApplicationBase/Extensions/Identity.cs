using ApplicationBase.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;

namespace ApplicationBase.Extensions;

public static class Identity
{
    public static void AddIdentityService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserAccessor, UserAccessor>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"))
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddMicrosoftGraph(configuration.GetSection("DownstreamApi"))
            .AddInMemoryTokenCaches();
        services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            var existingOnMessageReceivedHandler = options.Events.OnMessageReceived;
            options.Events.OnMessageReceived = async context =>
            {
                await existingOnMessageReceivedHandler(context);
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notify"))
                    context.Token = accessToken;
            };
        });
    }

    public static void UseApplicationIdentity(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}