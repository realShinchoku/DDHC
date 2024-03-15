using MongoDB.Driver;
using MongoDB.Entities;
using NotificationService.Models;

namespace NotificationService.Data;

public static class DbInitializer
{
    public static async Task InitDb(this WebApplication app)
    {
        await DB.InitAsync("NotificationDb",
            MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("DefaultConnection")));

        await DB.Index<Notification>()
            .Key(x => x.Email, KeyType.Text)
            .Key(x => x.Type, KeyType.Text)
            .Key(x => x.CreatedAt, KeyType.Descending)
            .CreateAsync();
    }
}