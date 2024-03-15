using ApplicationBase.Security;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts.Lessons;
using Contracts.Notifications;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using StudentService.Data;
using StudentService.DTOs;

namespace StudentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LessonsController(
    DataContext context,
    IMapper mapper,
    IUserAccessor userAccessor,
    IPublishEndpoint publishEndpoint) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<LessonDto>>> GetAllLessons(string updatedAt)
    {
        var query = context.Lessons
            .AsNoTracking()
            .AsSplitQuery()
            .OrderByDescending(x => x.UpdatedAt)
            .ThenByDescending(x => x.CreatedAt)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(updatedAt))
            query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(updatedAt).ToUniversalTime()) > 0);

        return await query.ProjectTo<LessonDto>(mapper.ConfigurationProvider).ToListAsync();
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    [AuthorizeForScopes(ScopeKeySection = "AzureAd:Scopes")]
    public async Task<ActionResult<LessonDto>> Update(Guid id, LessonUpdateDto lessonUpdate)
    {
        var teacher = await context.Teachers.FirstOrDefaultAsync(x => x.Email == userAccessor.GetUserEmail());

        if (teacher == null)
            return NotFound("Teacher not found");

        var lesson = await context.Lessons
            .Include(x => x.Subject)
            .ThenInclude(x => x.Teacher)
            .Include(x => x.Subject)
            .ThenInclude(x => x.Students)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (lesson == null)
            return NotFound("Lesson not found");

        if (lesson.Subject.Teacher.Id != teacher.Id)
            return Forbid();

        mapper.Map(lessonUpdate, lesson);

        await publishEndpoint.Publish(mapper.Map<LessonUpdated>(lesson));

        foreach (var student in lesson.Subject.Students)
            await publishEndpoint.Publish(new NotificationCreated
            {
                Title = "Thông báo điểm danh",
                Email = student.Email,
                Message =
                    $"Tiết học ngày {lesson.StartTime.AddHours(7):dd/MM/yyyy} của môn {lesson.Subject.Name} - {lesson.Subject.Code} được phép điểm danh từ {lessonUpdate.OpenAttendanceTime.AddHours(7):HH:mm} đến {lessonUpdate.CloseAttendanceTime.AddHours(7):HH:mm}",
                Type = NotificationType.Normal
            });

        var result = await context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Failed to update lesson");

        return Ok(mapper.Map<LessonDto>(lesson));
    }
}