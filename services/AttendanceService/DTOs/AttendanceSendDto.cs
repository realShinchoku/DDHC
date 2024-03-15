namespace AttendanceService.DTOs;

public class AttendanceSendDto
{
    public string Room { get; set; }
    public DateTime ScannedTime { get; set; }
    public DateTime SentTime { get; set; }
    public ICollection<string> Wifi { get; set; }
    public ICollection<string> Bluetooth { get; set; }
}