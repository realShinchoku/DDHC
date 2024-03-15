using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ApplicationBase.Security;

public class UserAccessor(IHttpContextAccessor httpContextAccessor) : IUserAccessor
{
    public string GetUserEmail()
    {
        return httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
    }

    public string GetUserName()
    {
        return httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "name")?.Value ??
               string.Empty;
    }
}