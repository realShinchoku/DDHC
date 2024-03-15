namespace Contracts.Lessons;

public class LessonWifiBluetoothUpdated
{
    public ICollection<Wifi> Wifi { get; set; }
    public ICollection<Bluetooth> Bluetooth { get; set; }
    public Guid Id { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}