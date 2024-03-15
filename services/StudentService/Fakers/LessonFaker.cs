using Bogus;
using Contracts.Lessons;
using Lesson = StudentService.Models.Lesson;

namespace StudentService.Fakers;

public sealed class LessonFaker : Faker<Lesson>
{
    public LessonFaker() : base("vi")
    {
        RuleFor(x => x.StartTime, f => f.Date.Future());
        RuleFor(x => x.EndTime, f => f.Date.Future());
        RuleFor(x => x.Wifi, f => new WifiFaker().Generate(f.Random.Int(1, 5)));
        RuleFor(x => x.Bluetooth, f => new BluetoothFaker().Generate(f.Random.Int(1, 5)));
    }
}

public sealed class WifiFaker : Faker<Wifi>
{
    public WifiFaker()
    {
        RuleFor(x => x.Name, f => f.Internet.Mac());
        RuleFor(x => x.Count, f => f.Random.Int(1, 5));
    }
}

public sealed class BluetoothFaker : Faker<Bluetooth>
{
    public BluetoothFaker()
    {
        RuleFor(x => x.Name, f => f.Internet.Mac());
        RuleFor(x => x.Count, f => f.Random.Int(1, 5));
    }
}