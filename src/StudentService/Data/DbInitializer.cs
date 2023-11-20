using Microsoft.EntityFrameworkCore;
using StudentService.Fakers;

namespace StudentService.Data;

public static class DbInitializer
{
    public static async Task InitDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await SeeData(scope.ServiceProvider.GetService<DataContext>());
    }

    private static async Task SeeData(DataContext context)
    {
        await context.Database.MigrateAsync();

        if (!context.Departments.Any())
        {
            Console.WriteLine("==> Seeding Departments");
            await context.AddRangeAsync(new DepartmentFaker().Generate(10));
        }

        if (!context.Courses.Any())
        {
            Console.WriteLine("==> Seeding Courses");
            await context.AddRangeAsync(new CourseFaker().Generate(10));
        }

        await context.SaveChangesAsync();
    }
}