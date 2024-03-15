using AutoMapper;
using Contracts.Notifications;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Entities;
using NotificationService.Hubs;
using NotificationService.Models;

namespace NotificationService.Consumers;

public class NotificationCreatedConsumer(
    ILogger<NotificationCreatedConsumer> logger,
    IMapper mapper,
    IHubContext<NotificationHub, INotificationHub> hubContext)
    : IConsumer<NotificationCreated>
{
    public async Task Consume(ConsumeContext<NotificationCreated> context)
    {
        logger.LogInformation("==> Consuming NotificationCreated event");

        var notification = mapper.Map<Notification>(context.Message);

        await notification.SaveAsync();

        await hubContext.Clients.Groups(notification.Email).ReceiveNotification(notification);
    }
}