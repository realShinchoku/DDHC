using AutoMapper;
using StudentService.DTOs;
using StudentService.Entities;

namespace StudentService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Student, StudentDto>()
            .IncludeMembers(x => x.Class)
            .IncludeMembers(x => x.Class.Course)
            .IncludeMembers(x => x.Class.Department);
        CreateMap<Class, StudentDto>();
        CreateMap<Course, StudentDto>();
        CreateMap<Department, StudentDto>();
    }
}