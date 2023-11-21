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

        if (!context.Departments.Any() && !context.Grades.Any())
        {
            var departments = new DepartmentFaker().Generate(20);
            var courses = new CourseFaker().Generate(20);
            await context.AddRangeAsync(departments);
            await context.AddRangeAsync(courses);
            await context.SaveChangesAsync();
            if (!context.Classes.Any())
            {
                var classes = new ClassFaker().Generate(2000);
                var random = new Random();
                for (var i = 0; i < 20; i++)
                for (var j = 0; j < 10; j++)
                {
                    var idx = random.Next(0, classes.Count - 1);
                    departments[i].Classes.Add(classes[idx]);
                    courses[i].Classes.Add(classes[idx]);
                    classes.Remove(classes[idx]);
                }

                await context.SaveChangesAsync();
            }
        }

        await context.SaveChangesAsync();
    }
}