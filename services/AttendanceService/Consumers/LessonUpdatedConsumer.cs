using AttendanceService.Models;
using AutoMapper;
using Contracts.Lessons;
using MassTransit;
using MongoDB.Entities;

namespace AttendanceService.Consumers;

public class LessonUpdatedConsumer(IMapper mapper, ILogger<LessonUpdatedConsumer> logger)
    : IConsumer<LessonUpdated>
{
    public async Task Consume(ConsumeContext<LessonUpdated> context)
    {
        logger.LogInformation("==> Consuming LessonUpdated event: {MessageId}", context.Message.Id);

        var lesson = mapper.Map<Lesson>(context.Message);

        var result = await DB.Update<Lesson>()
            .Match(x => x.Id == context.Message.Id)
            .ModifyOnly(x => new
            {
                x.SubjectId,
                x.EndTime,
                x.StartTime,
                x.Code,
                x.TeacherName,
                x.Room,
                x.UpdatedAt,
                x.IsEnded,
                x.Wifi,
                x.Bluetooth,
                x.StudentEmails,
                x.Name,
                x.OpenAttendanceTime,
                x.CloseAttendanceTime
            }, lesson)
            .ExecuteAsync();

        if (!result.IsAcknowledged)
            throw new MessageException(typeof(LessonUpdated), "Problem updating mongoDB");
    }
}