using AttendanceService.Models;
using Contracts.Lessons;
using MassTransit;
using MongoDB.Entities;

namespace AttendanceService.Consumers;

public class LessonDeletedConsumer(ILogger<LessonDeletedConsumer> logger)
    : IConsumer<LessonDeleted>
{
    public async Task Consume(ConsumeContext<LessonDeleted> context)
    {
        logger.LogInformation("==> Consuming LessonDeleted event: {MessageId}", context.Message.Id);

        var result = await DB.DeleteAsync<Lesson>(context.Message.Id);

        if (!result.IsAcknowledged)
            throw new MessageException(typeof(LessonDeleted), "Problem deleting auction from mongoDB");
    }
}