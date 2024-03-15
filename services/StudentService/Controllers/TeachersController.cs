using ApplicationBase.Security;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using StudentService.Data;
using StudentService.DTOs;
using StudentService.Models;

namespace StudentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController(
    GraphServiceClient graphClient,
    DataContext context,
    IMapper mapper,
    IUserAccessor userAccessor) : ControllerBase
{
    [HttpGet]
    [Authorize]
    [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    [AuthorizeForScopes(ScopeKeySection = "AzureAd:Scopes")]
    public async Task<ActionResult<StudentDto>> Auth()
    {
        var teacher = await context.Teachers.FirstOrDefaultAsync(x => x.Email == userAccessor.GetUserEmail());

        if (teacher != null) return Ok(mapper.Map<TeacherDto>(teacher));

        // var user = await graphClient.Me.GetAsync(rq => rq.QueryParameters.Select =
        //     ["id", "displayName", "mail", "mobilePhone", "department", "jobTitle", "mailNickname", "City"]);
        
        var userEmail = userAccessor.GetUserEmail();

        if (string.IsNullOrWhiteSpace(userEmail)) return NotFound("User not found");

        // if (user.JobTitle != "Giang Vien")
        //     return Forbid();

        teacher = new Teacher
        {
            Name = userAccessor.GetUserName(),
            Email = userEmail,
        };

        await context.AddAsync(teacher);
        await context.SaveChangesAsync();

        return Ok(mapper.Map<TeacherDto>(teacher));
    }

    [HttpPut]
    public async Task<ActionResult<TeacherDto>> Edit(TeacherUpdateDto teacherUpdateDto)
    {
        var teacher = await context.Teachers.FirstOrDefaultAsync(x => x.Email == userAccessor.GetUserEmail());
        if (teacher == null) return NotFound();

        mapper.Map(teacherUpdateDto, teacher);

        teacher.Status = UserStatus.Active;

        var result = await context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Failed to update teacher");

        return mapper.Map<TeacherDto>(teacher);
    }
}