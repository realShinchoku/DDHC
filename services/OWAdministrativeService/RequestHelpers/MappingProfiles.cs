using System.Text.Json;
using AutoMapper;
using OWAdministrativeService.DTOs;
using OWAdministrativeService.Models;

namespace OWAdministrativeService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Form, FormDto>()
            .ForMember(d => d.Body,
                s => s.MapFrom(x => JsonSerializer.Deserialize<object>(x.Body, JsonSerializerOptions.Default)));
        CreateMap<StudentCardFormDto, StudentCardForm>()
            .ForMember(s => s.BirthDay, opt => opt.MapFrom(s => s.BirthDay.ToString("dd-MM-yyyy")))
            .ForMember(s => s.Photo3X4, opt => opt.Ignore())
            .ForMember(s => s.FrontIdPhoto, opt => opt.Ignore())
            .ForMember(s => s.BackIdPhoto, opt => opt.Ignore());
        CreateMap<StudentVerifyFormDto, StudentVerifyForm>()
            .ForMember(x => x.BirthDay, opt => opt.MapFrom(x => x.BirthDay.ToString("dd-MM-yyyy")))
            .ForMember(x => x.IdDateIssued, opt => opt.MapFrom(x => x.IdDateIssued.ToString("dd-MM-yyyy")));
    }
}