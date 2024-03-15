using AutoMapper;
using Contracts.Attendances;
using Contracts.Lessons;
using StudentService.DTOs;
using StudentService.Models;

namespace StudentService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Student, StudentDto>();

        CreateMap<StudentUpdateDto, Student>()
            .ForMember(x => x.BirthDay,
                opt => opt.MapFrom(s => DateOnly.FromDateTime(s.BirthDay)));

        CreateMap<Subject, SubjectDto>()
            .ForMember(x => x.DateStart, opt => opt.MapFrom(s => s.DateStart.ToDateTime(new TimeOnly(0, 0, 0))))
            .ForMember(x => x.DateEnd, opt => opt.MapFrom(s => s.DateEnd.ToDateTime(new TimeOnly(0, 0, 0))))
            .ForMember(x => x.StudentsCount, opt => opt.MapFrom(s => s.Students.Count))
            .ForMember(x => x.LessonsCount, opt => opt.MapFrom(s => s.Lessons.Count))
            .ForMember(x => x.CurrentLessonsCount,
                opt => opt.MapFrom(s => s.Lessons.Count(x => x.EndTime < DateTime.UtcNow)));

        CreateMap<Lesson, LessonDto>()
            .IncludeMembers(x => x.Subject)
            .ForMember(x => x.TeacherName, opt => opt.MapFrom(s => s.Subject.Teacher.Name))
            .ForMember(x => x.StudentEmails, opt => opt.MapFrom(s => s.Subject.Students.Select(x => x.Email)));

        CreateMap<Subject, LessonDto>();

        CreateMap<SubjectCreateDto, Subject>()
            .ForMember(x => x.DateStart, opt => opt.MapFrom(s => DateOnly.FromDateTime(s.DateStart)))
            .ForMember(x => x.DateEnd, opt => opt.MapFrom(s => DateOnly.FromDateTime(s.DateEnd)))
            .ForMember(d => d.Students, s => s.Ignore())
            .ForMember(d => d.Lessons, s => s.Ignore());

        CreateMap<StudentCreateDto, Student>();

        CreateMap<Lesson, LessonCreated>()
            .IncludeMembers(x => x.Subject)
            .ForMember(x => x.TeacherName, opt => opt.MapFrom(s => s.Subject.Teacher.Name))
            .ForMember(x => x.StudentEmails, opt => opt.MapFrom(s => s.Subject.Students.Select(x => x.Email)));
        CreateMap<Subject, LessonCreated>();

        CreateMap<Lesson, LessonUpdated>()
            .IncludeMembers(x => x.Subject)
            .ForMember(x => x.TeacherName, opt => opt.MapFrom(s => s.Subject.Teacher.Name))
            .ForMember(x => x.StudentEmails, opt => opt.MapFrom(s => s.Subject.Students.Select(x => x.Email)));
        CreateMap<Subject, LessonUpdated>();

        CreateMap<LessonWifiBluetoothUpdated, Lesson>();

        CreateMap<LessonUpdateDto, Lesson>();

        CreateMap<AttendanceCreated, Attendance>();

        CreateMap<Teacher, TeacherDto>();
        CreateMap<TeacherUpdateDto, Teacher>();

        CreateMap<Subject, SubjectDetailDto>()
            .ForMember(x => x.DateStart, opt => opt.MapFrom(s => s.DateStart.ToDateTime(new TimeOnly(0, 0, 0))))
            .ForMember(x => x.DateEnd, opt => opt.MapFrom(s => s.DateEnd.ToDateTime(new TimeOnly(0, 0, 0))))
            .ForMember(x => x.StudentsCount, opt => opt.MapFrom(s => s.Students.Count))
            .ForMember(x => x.LessonsCount, opt => opt.MapFrom(s => s.Lessons.Count))
            .ForMember(x => x.CurrentLessonsCount,
                opt => opt.MapFrom(s => s.Lessons.Count(x => x.EndTime < DateTime.UtcNow)));
    }
}