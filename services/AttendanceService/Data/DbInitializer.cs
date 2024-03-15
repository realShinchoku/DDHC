using AttendanceService.Models;
using AttendanceService.Services;
using MongoDB.Driver;
using MongoDB.Entities;

namespace AttendanceService.Data;

public static class DbInitializer
{
    public static async Task InitDb(this WebApplication app)
    {
        await DB.InitAsync("AttendanceDb",
            MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("DefaultConnection")));

        await DB.Index<Lesson>()
            .Key(x => x.SubjectId, KeyType.Text)
            .Key(x => x.Room, KeyType.Text)
            .Key(x => x.Code, KeyType.Text)
            .Key(x => x.Name, KeyType.Text)
            .Key(x => x.IsEnded, KeyType.Text)
            .Key(x => x.StartTime, KeyType.Descending)
            .Key(x => x.EndTime, KeyType.Descending)
            .Key(x => x.CreatedAt, KeyType.Descending)
            .Key(x => x.UpdatedAt, KeyType.Descending)
            .CreateAsync();

        using var scope = app.Services.CreateScope();
        var httpClient = scope.ServiceProvider.GetRequiredService<StudentServiceHttpClient>();

        var items = await httpClient.GetLessons();

        if (items.Count > 0)
        {
            Console.WriteLine($"==> Update {items.Count} lessons to database");
            await DB.SaveAsync(items);
        }
    }
}