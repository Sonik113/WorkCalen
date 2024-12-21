using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WorkCalendarik.Domain.Database;
using WorkCalendarik.Domain.Database.Entities;
using WorkCalendarik.Domain.Database.ModelsDb;
using WorkCalendarik.Domain.Database.Responses;
using WorkCalendarik.Domain.Interfaces;
using WorkCalendarik.Service.Interfaces;

namespace WorkCalendarik.Service.Realizations;

public class BronCalendarService : IBronCalendarService
{
    private readonly IBaseStorage<BronCalendarDb> _bronCalendarStorage;
    private readonly IMapper _mapper;

    public BronCalendarService(IBaseStorage<BronCalendarDb> bronCalendarStorage, IMapper mapper)
    {
        _bronCalendarStorage = bronCalendarStorage;
        _mapper = mapper;
    }

    public BaseResponse<List<BronCalendar>> GetAllBronCalendars()
    {
        try
        {
            var bronCalendarsDb = _bronCalendarStorage.GetAll()
                .OrderBy(p => p.CreatedAt)
                .ToList();

            var bronCalendars = _mapper.Map<List<BronCalendar>>(bronCalendarsDb);

            return new BaseResponse<List<BronCalendar>>()
            {
                Data = bronCalendars,
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<BronCalendar>>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<BaseResponse<BronCalendar>> GetBronCalendarById(Guid id)
    {
        try
        {
            var bronCalendarDb = await _bronCalendarStorage.Get(id);

            var result = _mapper.Map<BronCalendar>(bronCalendarDb);
            
            if (result == null)
            {
                return new BaseResponse<BronCalendar>()
                {
                    Description = "Найдено 0 элементов",
                    StatusCode = StatusCode.OK
                };
            }
            
            return new BaseResponse<BronCalendar>()
            {
                Data = result,
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception e)
        {
            return new BaseResponse<BronCalendar>()
            {
                Description = e.Message,
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    public BaseResponse<List<BronCalendar>> GetBronCalendarByFilter(Filter filter)
    {
        try
        {
            var bronCalendarsFilter = GetAllBronCalendars().Data;

            if (filter != null && bronCalendarsFilter != null)
            {
                if (filter.PriceMax != 1000000 || filter.PriceMin != 0)
                {
                    bronCalendarsFilter = bronCalendarsFilter.Where(p => p.Price <= filter.PriceMax && p.Price > filter.PriceMin)
                        .ToList();
                }

                // if (filter.FuelTypes.Count > 0)
                // {
                //     bronCalendarsFilter = bronCalendarsFilter.Where(p => filter.FuelTypes.Contains(p.Car.Fuel.ToString())).ToList();
                // }
            }

            return new BaseResponse<List<BronCalendar>>
            {
                Data = bronCalendarsFilter,
                Description = "Отфильтрованные данные",
                StatusCode = StatusCode.OK
            };

        }
        catch (Exception ex)
        {
            return new BaseResponse<List<BronCalendar>>
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalServerError
            };
        }
    }
}
