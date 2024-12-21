using WorkCalendarik.Domain.Database.Entities;
using WorkCalendarik.Domain.Database.ModelsDb;
using WorkCalendarik.Domain.Database.Responses;

namespace WorkCalendarik.Service.Interfaces;

public interface IBronCalendarService
{
    BaseResponse<List<BronCalendar>> GetAllBronCalendars();

    Task<BaseResponse<BronCalendar>> GetBronCalendarById(Guid id);
}