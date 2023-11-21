using AttendanceService.DTOs;
using AttendanceService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendancesController : ControllerBase
{
    private readonly GrpcStudentClient _grpcClient;

    public AttendancesController(GrpcStudentClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    [HttpPost]
    public async Task<IActionResult> Attendance(AttendanceSendDto attendanceSendDto)
    {
        _grpcClient.GetStudent(attendanceSendDto.ToString());
        return Ok(attendanceSendDto);
    }
}