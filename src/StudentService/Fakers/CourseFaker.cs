using Bogus;
using StudentService.Entities;

namespace StudentService.Fakers;

public sealed class CourseFaker : Faker<Course>
{
    public CourseFaker(string locale = "vi") : base(locale)
    {
        RuleFor(x => x.Name, f => f.Random.String2(10));
        RuleFor(x => x.Classes, f => new ClassFaker().GenerateBetween(1, f.Random.Int(2, 10)));
    }
}