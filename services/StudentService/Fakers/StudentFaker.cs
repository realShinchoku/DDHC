using Bogus;
using StudentService.Models;

namespace StudentService.Fakers;

public sealed class StudentFaker : Faker<Student>
{
    public StudentFaker() : base("vi")
    {
        RuleFor(x => x.Name, f => f.Person.FullName);
        RuleFor(x => x.Email, f => f.Person.Email + f.Random.String2(7));
        RuleFor(x => x.Phone, f => f.Person.Phone);
        RuleFor(x => x.StudentCode, f => f.Random.String2(10));
        RuleFor(x => x.Class, f => f.Random.String2(10));
        RuleFor(x => x.Grade, f => f.Random.String2(10));
        RuleFor(x => x.Faculty, f => f.Company.CompanyName());
        RuleFor(x => x.Status, f => f.PickRandom<UserStatus>());
    }
}