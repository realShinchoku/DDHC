using Bogus;
using StudentService.Models;

namespace StudentService.Fakers;

public sealed class SubjectFaker : Faker<Subject>
{
    public SubjectFaker() : base("vi")
    {
        RuleFor(x => x.Room, f => f.Random.String2(10));
        RuleFor(x => x.Name, f => f.Music.Genre());
        RuleFor(x => x.Code, f => f.Random.String2(10));
        RuleFor(x => x.Lessons, f => new LessonFaker().Generate(5));
        RuleFor(x => x.DateStart, f => f.Date.PastDateOnly());
        RuleFor(x => x.DateEnd, f => f.Date.FutureDateOnly());
        RuleFor(x => x.Students, f => new StudentFaker().Generate(5));
    }
}