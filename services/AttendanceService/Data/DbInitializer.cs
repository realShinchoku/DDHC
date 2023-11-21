using MongoDB.Driver;
using MongoDB.Entities;

namespace AttendanceService.Data;

public static class DbInitializer
{
    public static async Task InitDb(this WebApplication app)
    {
        await DB.InitAsync("AttendanceDb",
            MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("DefaultConnection")));
    }
}