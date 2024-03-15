using AutoMapper;
using Contracts.Lessons;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using StudentService.Data;

namespace StudentService.Consumers;

public class LessonWifiBluetoothUpdatedConsumer(
    DataContext dbContext,
    IMapper mapper,
    ILogger<LessonWifiBluetoothUpdatedConsumer> logger) : IConsumer<LessonWifiBluetoothUpdated>
{
    public async Task Consume(ConsumeContext<LessonWifiBluetoothUpdated> context)
    {
        logger.LogInformation("==> Consuming LessonWifiBluetoothUpdated event: {MessageId}", context.Message.Id);

        var lesson = await dbContext.Lessons
            .Include(x => x.Bluetooth)
            .Include(x => x.Wifi)
            .FirstOrDefaultAsync(x => x.Id == context.Message.Id);

        if (lesson == null)
            throw new MessageException(typeof(LessonWifiBluetoothUpdated), "Lesson not found");

        mapper.Map(context.Message, lesson);
        var result = await dbContext.SaveChangesAsync() > 0;

        if (!result)
            throw new MessageException(typeof(LessonWifiBluetoothUpdated), "Problem updating SqlServer");
    }
}