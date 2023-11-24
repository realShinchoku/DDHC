using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly GraphServiceClient _graphClient;

    public AuthController(IPublishEndpoint publishEndpoint, GraphServiceClient graphClient)
    {
        _publishEndpoint = publishEndpoint;
        _graphClient = graphClient;
    }

    [Authorize]
    [HttpGet("me")]
    [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    [AuthorizeForScopes(ScopeKeySection = "AzureAd:Scopes")]
    public async Task<ActionResult> Get()
    {
        var user = await _graphClient.Me.GetAsync(rq => rq.QueryParameters.Select = new[]
            { "id", "displayName", "mail", "mobilePhone", "department", "jobTitle", "mailNickname" });

        if (user == null) return NotFound("User not found");


        var student = new StudentCreated
        {
            Id = Guid.Parse(user.Id),
            Name = user.DisplayName,
            Email = user.Mail,
            Phone = user.MobilePhone,
            DepartmentName = user.Department,
            GradeName = user.JobTitle[..2],
            StudentCode = user.MailNickname,
            ClassName = user.JobTitle
        };

        await _publishEndpoint.Publish(student);

        return Ok(student);
    }
}