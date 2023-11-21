using Bogus;
using StudentService.Models;

namespace StudentService.Fakers;

public sealed class DepartmentFaker : Faker<Department>
{
    public DepartmentFaker(string locale = "vi") : base(locale)
    {
        RuleFor(x => x.Name, f => f.Lorem.Word());
    }
}