using AttendanceService.Models;
using AutoMapper;
using Contracts.Lessons;
using MassTransit;
using MongoDB.Entities;

namespace AttendanceService.Consumers;

public class LessonCreatedConsumer(IMapper mapper, ILogger<LessonCreatedConsumer> logger)
    : IConsumer<LessonCreated>
{
    public async Task Consume(ConsumeContext<LessonCreated> context)
    {
        logger.LogInformation("==> Consuming LessonCreated event");

        var lesson = mapper.Map<Lesson>(context.Message);

        await lesson.SaveAsync();
    }
}