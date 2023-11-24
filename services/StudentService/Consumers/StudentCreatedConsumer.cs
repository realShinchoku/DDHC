using Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using StudentService.Data;
using StudentService.Models;

namespace StudentService.Consumers;

public class StudentCreatedConsumer : IConsumer<StudentCreated>
{
    private readonly DataContext _dbContext;

    public StudentCreatedConsumer(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<StudentCreated> context)
    {
        Console.WriteLine("==> Consuming StudentCreated event");

        var student = await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == context.Message.Id);
        if (student != null)
            return;

        var grade = await _dbContext.Grades.FirstOrDefaultAsync(x => x.Name == context.Message.GradeName) ?? new Grade
        {
            Name = context.Message.GradeName,
        };

        var department =
            await _dbContext.Departments.FirstOrDefaultAsync(x => x.Name == context.Message.DepartmentName) ??
            new Department
            {
                Name = context.Message.DepartmentName,
            };

        var @class = await _dbContext.Classes.FirstOrDefaultAsync(x => x.Name == context.Message.ClassName) ?? new Class
        {
            Name = context.Message.ClassName,
            Department = department,
            Grade = grade,
        };

        student = new Student
        {
            Id = context.Message.Id,
            Name = context.Message.Name,
            Email = context.Message.Email,
            Phone = context.Message.Phone,
            StudentCode = context.Message.StudentCode,
            Class = @class
        };

        _dbContext.Add(student);

        var result = await _dbContext.SaveChangesAsync() > 0;

        if (!result)
            throw new DbUpdateException("Error saving student");
    }
}