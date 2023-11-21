using Bogus;
using StudentService.Models;

namespace StudentService.Fakers;

public sealed class CourseFaker : Faker<Grade>
{
    public CourseFaker(string locale = "vi") : base(locale)
    {
        RuleFor(x => x.Name, f => f.Lorem.Word());
    }
}