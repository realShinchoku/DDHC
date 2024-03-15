using AutoMapper;
using Contracts.Notifications;
using NotificationService.Models;

namespace NotificationService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<NotificationCreated, Notification>();
    }
}