using Bogus;
using StudentService.Models;

namespace StudentService.Fakers;

public sealed class StudentFaker : Faker<Student>
{
    public StudentFaker(string locale = "vi") : base(locale)
    {
        RuleFor(x => x.Name, f => f.Person.FullName);
        RuleFor(x => x.Email, f => f.Person.Email);
        RuleFor(x => x.Phone, f => f.Person.Phone);
        RuleFor(x => x.StudentCode, f => f.Random.String2(10));
    }
}