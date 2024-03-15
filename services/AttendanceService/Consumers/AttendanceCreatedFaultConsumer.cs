using Contracts.Attendances;
using MassTransit;

namespace AttendanceService.Consumers;

public class AttendanceCreatedFaultConsumer(ILogger<AttendanceCreatedFaultConsumer> logger)
    : IConsumer<Fault<AttendanceCreated>>
{
    public Task Consume(ConsumeContext<Fault<AttendanceCreated>> context)
    {
        logger.LogError("==> Consuming AttendanceCreated fault event");

        var exception = context.Message.Exceptions.FirstOrDefault();

        logger.LogError(exception?.Message);

        return Task.CompletedTask;
    }
}