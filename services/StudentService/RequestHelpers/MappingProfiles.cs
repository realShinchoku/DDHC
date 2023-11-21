using AutoMapper;
using StudentService.DTOs;
using StudentService.Models;

namespace StudentService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Student, StudentDto>()
            .IncludeMembers(x => x.Class)
            .ForMember(x => x.DepartmentId, o => o.MapFrom(s => s.Class.DepartmentId))
            .ForMember(x => x.DepartmentName, o => o.MapFrom(s => s.Class.Department.Name))
            .ForMember(x => x.GradeId, o => o.MapFrom(s => s.Class.CourseId))
            .ForMember(x => x.GradeName, o => o.MapFrom(s => s.Class.Grade.Name));
        CreateMap<Class, StudentDto>();
        CreateMap<Grade, StudentDto>();
        CreateMap<Department, StudentDto>();
    }
}