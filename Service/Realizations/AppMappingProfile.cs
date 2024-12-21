using AutoMapper;
using WorkCalendarik.Domain.Database.Entities;
using WorkCalendarik.Domain.Database.ModelsDb;
using WorkCalendarik.Domain.ViewModels.BronCalendars;
using WorkCalendarik.Domain.ViewModels.LogAndReg;

namespace WorkCalendarik.Service.Realizations;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<User, UserDb>().ReverseMap();
        CreateMap<User, LoginViewModel>().ReverseMap();
        CreateMap<User, RegisterViewModel>().ReverseMap();
        CreateMap<RegisterViewModel, ConfirmEmailViewModel>().ReverseMap();
        CreateMap<User, ConfirmEmailViewModel>().ReverseMap();
        CreateMap<BronCalendar, BronCalendarDb>().ReverseMap();
        CreateMap<BronCalendar, BronCalendarPageViewModel>().ReverseMap();
        CreateMap<BronCalendar, BronCalendarForBronCalendarsViewModel>().ReverseMap();
    }
}