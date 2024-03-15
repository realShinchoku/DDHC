using Microsoft.EntityFrameworkCore;
using StudentService.Fakers;
using StudentService.Models;

namespace StudentService.Data;

public static class DbInitializer
{
    public static async Task InitDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        await context.Database.MigrateAsync();
    }
}