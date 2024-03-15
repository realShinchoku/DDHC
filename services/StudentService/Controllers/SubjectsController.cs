using ApplicationBase.Security;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts.Attendances;
using Contracts.Lessons;
using Contracts.Notifications;
using Contracts.Subjects;
using MassTransit;
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
public class SubjectsController(
    DataContext context,
    IMapper mapper,
    IUserAccessor userAccessor,
    IPublishEndpoint publishEndpoint)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> Get()
    {
        var teacher = await context.Teachers.FirstOrDefaultAsync(x => x.Email == userAccessor.GetUserEmail());

        if (teacher == null)
            return NotFound("Teacher not found");

        var subjects = await context.Subjects
            .AsNoTracking()
            .AsSplitQuery()
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.UpdatedAt)
            .Where(x => x.TeacherId == teacher.Id)
            .ProjectTo<SubjectDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return Ok(subjects);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SubjectDetailDto>> GetById(Guid id)
    {
        var teacher = await context.Teachers.FirstOrDefaultAsync(x => x.Email == userAccessor.GetUserEmail());

        if (teacher == null)
            return NotFound("Teacher not found");

        var subject = await context.Subjects
            .AsNoTracking()
            .AsSplitQuery()
            .ProjectTo<SubjectDetailDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (subject == null)
            return NotFound("Subject not found");

        return Ok(subject);
    }

    [HttpGet("{subjectId:guid}/attendances")]
    public async Task<ActionResult<AttendanceStudentDto>> GetAttendances(Guid subjectId)
    {
        var subject = await context.Subjects
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Lessons)
            .ThenInclude(x => x.Attendances)
            .Include(x => x.Students)
            .FirstOrDefaultAsync(x => x.Id == subjectId);

        if (subject == null)
            return NotFound("Subject not found");

        var attendanceStudents = new List<AttendanceStudentDto>();

        foreach (var student in subject.Students)
        {
            var attendanceStudent = new AttendanceStudentDto
            {
                StudentId = student.Id,
                StudentCode = student.StudentCode,
                StudentName = student.Name,
                Lessons = new List<AttendanceLessonDto>()
            };

            foreach (var lesson in subject.Lessons)
            {
                var attendance = lesson.Attendances.FirstOrDefault(x => x.StudentId == student.Id);
                if (attendance == null)
                    attendanceStudent.Lessons.Add(new AttendanceLessonDto
                    {
                        LessonId = lesson.Id,
                        StartTime = lesson.StartTime,
                        Type = DateTime.UtcNow > lesson.EndTime ? AttendanceType.Absent : null
                    });
                else
                    attendanceStudent.Lessons.Add(new AttendanceLessonDto
                    {
                        LessonId = lesson.Id,
                        StartTime = lesson.StartTime,
                        AttendedTime = attendance.AttendedTime,
                        Type = attendance.Type
                    });
            }

            attendanceStudents.Add(attendanceStudent);
        }

        return Ok(attendanceStudents);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var teacher = await context.Teachers.FirstOrDefaultAsync(x => x.Email == userAccessor.GetUserEmail());

        if (teacher == null)
            return NotFound("Teacher not found");

        var subject = await context.Subjects
            .Include(x => x.Students)
            .Include(x => x.Lessons)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (subject == null)
            return NotFound("Subject not found");

        await publishEndpoint.Publish(new SubjectDeleted { Id = subject.Id });

        context.Remove(subject);

        var result = await context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Failed to delete subject");

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<SubjectDto>> Create(SubjectCreateDto subjectCreateDto)
    {
        var teacher = await context.Teachers.FirstOrDefaultAsync(x => x.Email == userAccessor.GetUserEmail());
        if (teacher == null)
            return NotFound("Teacher not found");

        var subject = mapper.Map<Subject>(subjectCreateDto);
        subject.Teacher = teacher;

        var studentEmails = subjectCreateDto.Students.Select(x => x.Email).ToList();

        var students = await context.Students
            .Where(x => studentEmails.Contains(x.Email))
            .ToListAsync();

        subject.Students = students;

        foreach (var studentCreateDto in subjectCreateDto.Students.Where(x =>
                     students.All(s => s.StudentCode != x.StudentCode)).ToList())
            subject.Students.Add(new Student
            {
                Name = studentCreateDto.Name,
                Email = studentCreateDto.Email,
                StudentCode = studentCreateDto.StudentCode,
                Class = studentCreateDto.Class,
                Faculty = studentCreateDto.Faculty
            });


        var lessons = new List<Lesson>();
        foreach (var lessonCreate in subjectCreateDto.Lessons)
            lessons.AddRange(GenerateDates(subject.DateStart, subject.DateEnd, lessonCreate));

        subject.Lessons = lessons;

        await context.AddAsync(subject);

        foreach (var lesson in subject.Lessons)
            await publishEndpoint.Publish(mapper.Map<LessonCreated>(lesson));


        foreach (var student in subject.Students)
            await publishEndpoint.Publish(new NotificationCreated
            {
                Title = "Môn học mới",
                Email = student.Email,
                Message = $"Bạn được thêm vào môn {subject.Name} của giảng viên {subject.Teacher.Name}",
                Type = NotificationType.Normal
            });

        var result = await context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Failed to create subject");

        return Created(subject.Id.ToString(), mapper.Map<SubjectDto>(subject));
    }

    private static IEnumerable<Lesson> GenerateDates(DateOnly startDate, DateOnly endDate,
        LessonCreateDto lessonCreateDto)
    {
        var lessons = new List<Lesson>();

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
            if (date.DayOfWeek == lessonCreateDto.DayOfWeek)
            {
                var startTime = new TimeOnly(lessonCreateDto.StartTime.Hour, lessonCreateDto.StartTime.Minute);
                var endTime = new TimeOnly(lessonCreateDto.EndTime.Hour, lessonCreateDto.EndTime.Minute);
                var end = new DateTime(date, endTime, DateTimeKind.Utc);
                var start = new DateTime(endTime < startTime ? date.AddDays(-1) : date, startTime, DateTimeKind.Utc);

                lessons.Add(new Lesson
                {
                    StartTime = start,
                    EndTime = end
                });
            }

        return lessons;
    }
}