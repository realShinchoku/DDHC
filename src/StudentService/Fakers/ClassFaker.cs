using Bogus;
using StudentService.Entities;

namespace StudentService.Fakers;

public sealed class ClassFaker : Faker<Class>
{
    public ClassFaker(string locale = "vi") : base(locale)
    {
        RuleFor(x => x.Name, f => f.Random.String2(10));
        RuleFor(x => x.Students, f => new StudentFaker().GenerateBetween(1, f.Random.Int(40, 80)));
    }
}