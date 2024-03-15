using ApplicationBase.Security;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using StudentService.Data;
using StudentService.DTOs;
using StudentService.Models;

namespace StudentService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
[AuthorizeForScopes(ScopeKeySection = "AzureAd:Scopes")]
public class StudentsController(
    DataContext context,
    IMapper mapper,
    IUserAccessor userAccessor)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<StudentDto>> Auth()
    {
        var userEmail = userAccessor.GetUserEmail();

        if (userEmail == null) return NotFound("User not found");

        var student = await context.Students.FirstOrDefaultAsync(x => x.Email == userEmail);

        if (student != null) return Ok(mapper.Map<StudentDto>(student));

        student = new Student
        {
            Name = userAccessor.GetUserName(),
            Email = userEmail,
            StudentCode = userEmail.Split('@')[0]
        };

        await context.Students.AddAsync(student);
        await context.SaveChangesAsync();

        return Ok(mapper.Map<StudentDto>(student));
    }

    [HttpPut]
    public async Task<ActionResult<StudentDto>> Edit(StudentUpdateDto studentUpdateDto)
    {
        var student = await context.Students.FirstOrDefaultAsync(x => x.Email == userAccessor.GetUserEmail());
        if (student == null) return NotFound();

        mapper.Map(studentUpdateDto, student);

        var result = await context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Failed to update student");

        return mapper.Map<StudentDto>(student);
    }
}