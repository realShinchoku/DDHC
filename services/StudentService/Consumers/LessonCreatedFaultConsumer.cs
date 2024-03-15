using Contracts.Lessons;
using MassTransit;

namespace StudentService.Consumers;

public class LessonCreatedFaultConsumer(ILogger<LessonCreatedFaultConsumer> logger) : IConsumer<Fault<LessonCreated>>
{
    public Task Consume(ConsumeContext<Fault<LessonCreated>> context)
    {
        logger.LogError("==> Consuming faulty LessonCreated event");

        var exception = context.Message.Exceptions.First();

        logger.LogError(exception.Message);

        return Task.CompletedTask;
    }
}