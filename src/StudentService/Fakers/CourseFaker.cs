using Bogus;
using StudentService.Entities;

namespace StudentService.Fakers;

public sealed class CourseFaker : Faker<Course>
{
    public CourseFaker(string locale = "vi") : base(locale)
    {
        RuleFor(x => x.Name, f => f.Lorem.Letter());
    }
}