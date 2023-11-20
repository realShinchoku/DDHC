using Bogus;
using StudentService.Entities;

namespace StudentService.Fakers;

public sealed class DepartmentFaker : Faker<Department>
{
    public DepartmentFaker(string locale = "vi") : base(locale)
    {
        RuleFor(x => x.Name, f => f.Lorem.Letter());
    }
}