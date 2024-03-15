using Bogus;
using Contracts.Attendances;
using StudentService.Models;

namespace StudentService.Fakers;

public sealed class AttendanceFaker : Faker<Attendance>
{
    public AttendanceFaker(Lesson lesson, Student student) : base("vi")
    {
        RuleFor(x => x.Lesson, lesson);
        RuleFor(x => x.Student, student);
        RuleFor(x => x.Type, f => f.Random.Enum<AttendanceType>());
        RuleFor(x => x.AttendedTime, f => f.Date.Future());
    }
}