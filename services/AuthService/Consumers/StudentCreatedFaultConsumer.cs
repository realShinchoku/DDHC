using Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Consumers;

public class StudentCreatedFaultConsumer : IConsumer<Fault<StudentCreated>>
{
    private readonly ILogger<StudentCreatedFaultConsumer> _logger;

    public StudentCreatedFaultConsumer(ILogger<StudentCreatedFaultConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<Fault<StudentCreated>> context)
    {
        _logger.LogError("==> Consuming faulty  StudentCreated");

        var exception = context.Message.Exceptions.First();

        if (exception.ExceptionType == typeof(DbUpdateException).FullName)
            await context.Publish(context.Message.Message);
        else
            _logger.LogError(exception.Message);
    }
}