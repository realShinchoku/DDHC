namespace AttendanceService.DTOs;

public class AttendanceSendDto
{
    public Guid RoomId { get; set; }
    public Guid StudentId { get; set; }
    public DateTime ScanTime { get; set; }
    public DateTime SendTime { get; set; }
    public ICollection<string> WifiNames { get; set; }
    public ICollection<string> BluetoothNames { get; set; }
}