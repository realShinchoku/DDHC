using AttendanceService.Models;
using AutoMapper;
using Contracts.Lessons;

namespace AttendanceService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Lesson, Lesson>();
        CreateMap<LessonCreated, Lesson>();
        CreateMap<LessonUpdated, Lesson>();
        CreateMap<Lesson, LessonWifiBluetoothUpdated>();
    }
}