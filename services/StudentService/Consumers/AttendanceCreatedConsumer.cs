using AutoMapper;
using Contracts.Attendances;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using StudentService.Data;
using StudentService.Models;

namespace StudentService.Consumers;

public class AttendanceCreatedConsumer(DataContext dbContext, ILogger<AttendanceCreatedConsumer> logger)
    : IConsumer<AttendanceCreated>
{
    public async Task Consume(ConsumeContext<AttendanceCreated> context)
    {
        logger.LogInformation("Consuming AttendanceCreated event");
        var lesson = await dbContext.Lessons.FirstOrDefaultAsync(x => x.Id == context.Message.LessonId);
        if (lesson == null)
            throw new ArgumentOutOfRangeException(nameof(lesson), "Lesson not found");

        var student = await dbContext.Students.FirstOrDefaultAsync(x => x.Email == context.Message.StudentEmail);

        if (student == null)
            throw new ArgumentOutOfRangeException(nameof(student), "Student not found");

        var attendance =
            dbContext.Attendances.FirstOrDefault(x => x.LessonId == lesson.Id && x.StudentId == student.Id);

        if (attendance != null)
        {
            attendance.AttendedTime = context.Message.AttendedTime;
            attendance.Type = context.Message.Type;
        }
        else
        {
            attendance = new Attendance
            {
                LessonId = lesson.Id,
                StudentId = student.Id,
                AttendedTime = context.Message.AttendedTime,
                Type = context.Message.Type
            };
            await dbContext.AddAsync(attendance);
        }

        await dbContext.SaveChangesAsync();
    }
}