using MongoDB.Driver;
using MongoDB.Entities;
using OWAdministrativeService.Models;

namespace OWAdministrativeService.Data;

public static class DbInitializer
{
    public static async Task InitDb(this WebApplication app)
    {
        await DB.InitAsync("OWAdministrativeDb",
            MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("DefaultConnection")));

        await DB.Index<Form>()
            .Key(x => x.Email, KeyType.Text)
            .Key(x => x.FileName, KeyType.Text)
            .Key(x => x.CreatedAt, KeyType.Descending)
            .Key(x => x.UpdatedAt, KeyType.Descending)
            .Key(x => x.Type, KeyType.Text)
            .CreateAsync();
    }
}