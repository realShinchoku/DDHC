using AttendanceService.Models;
using Contracts.Lessons;
using Contracts.Subjects;
using MassTransit;
using MongoDB.Entities;

namespace AttendanceService.Consumers;

public class SubjectDeletedConsumer(ILogger<SubjectDeletedConsumer> logger)
    : IConsumer<SubjectDeleted>
{
    public async Task Consume(ConsumeContext<SubjectDeleted> context)
    {
        logger.LogInformation("==> Consuming SubjectDeleted event: {MessageId}", context.Message.Id);

        var result = await DB.DeleteAsync<Lesson>(b => b.SubjectId == context.Message.Id);

        if (!result.IsAcknowledged)
            throw new MessageException(typeof(SubjectDeleted), "Problem deleting auction from mongoDB");
    }
}