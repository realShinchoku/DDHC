using ApplicationBase.Security;
using Contracts.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Web;
using MongoDB.Entities;
using NotificationService.Models;

namespace NotificationService.Hubs;

[Authorize]
[AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
[AuthorizeForScopes(ScopeKeySection = "AzureAd:Scopes")]
public class NotificationHub(IUserAccessor userAccessor) : Hub<INotificationHub>
{
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userAccessor.GetUserEmail());
        await GetNotifications();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, userAccessor.GetUserEmail());
    }

    public async Task GetNotifications()
    {
        var notifications = await DB.Find<Notification>()
            .Match(x => x.Email == userAccessor.GetUserEmail())
            .Sort(x => x.CreatedAt, Order.Descending)
            .ExecuteAsync();
        await Clients.Caller.ReceiveNotifications(notifications);
    }

    public async Task MarkAsRead(Guid id)
    {
        var notification = await DB.Find<Notification>().Match(x => x.Id == id).ExecuteFirstAsync();
        notification.IsRead = true;
        await notification.SaveAsync();
        await Clients.Caller.ReceiveNotification(notification);
    }

    public async Task Test(string message)
    {
        var notify = new Notification
        {
            Title = message,
            Email = userAccessor.GetUserEmail(),
            Message = message,
            Type = NotificationType.Normal
        };
        await notify.SaveAsync();
        await Clients.Group(userAccessor.GetUserEmail()).ReceiveNotification(notify);
    }
}