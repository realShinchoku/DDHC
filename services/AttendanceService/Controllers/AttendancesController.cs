using ApplicationBase.Security;
using AttendanceService.DTOs;
using AttendanceService.Models;
using AutoMapper;
using Contracts.Attendances;
using Contracts.Lessons;
using Contracts.Notifications;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using MongoDB.Entities;

namespace AttendanceService.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AttendancesController(IUserAccessor userAccessor, IPublishEndpoint publishEndpoint, IMapper mapper)
    : ControllerBase
{
    [HttpGet("lesson/{room}")]
    [Authorize]
    [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    [AuthorizeForScopes(ScopeKeySection = "AzureAd:Scopes")]
    public async Task<IActionResult> GetLessonByRoom(string room)
    {
        var now = DateTime.UtcNow;
        var lesson = await DB.Find<Lesson>()
            .Match(x =>
                x.Room.ToLower() == room.ToLower()
                && x.StudentEmails.Contains(userAccessor.GetUserEmail())
                && x.StartTime <= now && now <= x.EndTime
            )
            .ExecuteFirstAsync();

        if (lesson == null)
            return NotFound("Lesson not found");

        return Ok(lesson);
    }

    [HttpPost]
    [Authorize]
    [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    [AuthorizeForScopes(ScopeKeySection = "AzureAd:Scopes")]
    public async Task<IActionResult> Attendance(AttendanceSendDto attendance)
    {
        Console.WriteLine("=>> Attendance form room: " + attendance.Room);
        var userEmail = userAccessor.GetUserEmail();
        var lesson = await DB.Find<Lesson>()
            .Match(x =>
                x.Room.ToLower().Contains(attendance.Room.ToLower())
                && x.StudentEmails.Contains(userEmail)
                && x.StartTime <= DateTime.UtcNow && DateTime.UtcNow <= x.EndTime
            )
            .ExecuteFirstAsync();

        if (lesson == null)
            return NotFound("Lesson not found");

        if (lesson.OpenAttendanceTime == null || lesson.CloseAttendanceTime == null)
            return BadRequest("Attendance time is not set");
        
        if (!IsSentTimeValid(attendance.SentTime))
            return BadRequest("Sent time is not valid");

        var bluetoothValid = ValidateBluetooth(attendance, lesson);

        var wifiValid = ValidateWifi(attendance, lesson);

        var attendanceCreated = new AttendanceCreated
        {
            LessonId = lesson.Id,
            StudentEmail = userEmail,
            Type = bluetoothValid || wifiValid ? AttendanceType.Presence : AttendanceType.Absent
        };

        if (!IsTimeValid(lesson, attendance.ScannedTime) && attendanceCreated.Type == AttendanceType.Presence)
            attendanceCreated.Type = AttendanceType.Late;

        await publishEndpoint.Publish(attendanceCreated);

        await publishEndpoint.Publish(mapper.Map<LessonWifiBluetoothUpdated>(lesson));

        await publishEndpoint.Publish(new NotificationCreated
        {
            Title = "Thông báo điểm danh",
            Email = userEmail,
            Message = "Điểm danh thành công.\nTrạng thái: " + (attendanceCreated.Type switch
            {
                AttendanceType.Presence => "Có mặt",
                AttendanceType.Late => "Muộn",
                _ => "Vắng"
            }),
            Type = NotificationType.Normal
        });

        var result = await DB.Update<Lesson>()
            .MatchID(lesson.Id)
            .Modify(x => x.Wifi, lesson.Wifi)
            .Modify(x => x.Bluetooth, lesson.Bluetooth)
            .ExecuteAsync();

        if (!result.IsAcknowledged)
            throw new MessageException(typeof(AttendanceCreated), "Problem saving Wifi and Bluetooth to mongoDB");

        return Ok(attendanceCreated);
    }


    private static bool ValidateBluetooth(AttendanceSendDto attendance, Lesson lesson)
    {
        if (lesson.Bluetooth == null || lesson.Bluetooth.Count == 0)
        {
            lesson.Bluetooth = attendance.Bluetooth.Select(b => new Bluetooth { Name = b }).ToList();
            return true;
        }

        foreach (var bluetooth in attendance.Bluetooth)
            if (!lesson.Bluetooth.Select(x => x.Name).Contains(bluetooth))
                lesson.Bluetooth.Add(new Bluetooth { Name = bluetooth });
            else
                lesson.Bluetooth.First(x => x.Name == bluetooth).Count+=1;

        return attendance.Bluetooth
            .Select(bluetooth => lesson.Bluetooth.FirstOrDefault(x => x.Name == bluetooth))
            .Any(existingBluetooth => existingBluetooth is { Count: >= 2 });
    }

    private static bool ValidateWifi(AttendanceSendDto attendance, Lesson lesson)
    {
        if (lesson.Wifi == null || lesson.Wifi.Count == 0)
        {
            lesson.Wifi = attendance.Wifi.Select(b => new Wifi { Name = b }).ToList();
            return true;
        }

        foreach (var wifi in attendance.Wifi)
            if (!lesson.Wifi.Select(x => x.Name).Contains(wifi))
                lesson.Wifi.Add(new Wifi { Name = wifi });
            else
                lesson.Wifi.First(x => x.Name == wifi).Count+=1;

        return attendance.Wifi
            .Select(wifi => lesson.Wifi.FirstOrDefault(x => x.Name == wifi))
            .Any(existingWifi => existingWifi is { Count: >= 2 });
    }

    private static bool IsTimeValid(Lesson lesson, DateTime scannedTime)
    {
        return new DateTime(DateOnly.FromDateTime(lesson.StartTime),
                   TimeOnly.FromDateTime(lesson.OpenAttendanceTime.Value)) < scannedTime
               &&
               scannedTime < new DateTime(DateOnly.FromDateTime(lesson.EndTime),
                   TimeOnly.FromDateTime(lesson.CloseAttendanceTime.Value));
    }

    private static bool IsSentTimeValid(DateTime sentTime)
    {
        return DateTime.UtcNow.AddMinutes(-10) <= sentTime && sentTime <= DateTime.UtcNow.AddMinutes(10);
    }
}