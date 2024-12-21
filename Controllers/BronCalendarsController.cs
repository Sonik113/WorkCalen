using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WorkCalendarik.Domain.Database;
using WorkCalendarik.Domain.Database.Entities;
using WorkCalendarik.Domain.Database.ModelsDb;
using WorkCalendarik.Domain.Database.Storage;
using WorkCalendarik.Domain.Interfaces;
using WorkCalendarik.Domain.ViewModels.BronCalendars;
using WorkCalendarik.Service.Interfaces;
using WorkCalendarik.Service.Realizations;

namespace WorkCalendarik.Controllers;

[Route("BronCalendars/[action]")]
public class BronCalendarsController : Controller
{
    private readonly IBronCalendarService _bronCalendarService;
   
    private IMapper _mapper { get; set; }

    private MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
    {
        p.AddProfile<AppMappingProfile>();
    });
    
    public BronCalendarsController(IBronCalendarService bronCalendarService, IBaseStorage<BronCalendarDb> bronCalStorage, IMapper mapper)
    {
        _bronCalendarService = bronCalendarService;
        _mapper = mapperConfiguration.CreateMapper();
    }
    
    public IActionResult ListOfBronCalendars(Guid id)
        {
        var result = _bronCalendarService.GetAllBronCalendars();

        var viewModel = new ListOfBronCalendarsViewModel
        {
            BronCalendars = result.Data.Select(bronCalendar => new BronCalendarForBronCalendarsViewModel
            {
                Id = bronCalendar.Id,
                Info = bronCalendar.Info,
                Price = bronCalendar.Price,
                Name = bronCalendar.Name,
                CreatedAt = bronCalendar.CreatedAt,
                ImagesPaths = bronCalendar.ImagesPaths,
            }).ToList(),
        };

        return View(viewModel);
    }

    public async Task<IActionResult> BronCalendarPage(Guid id)
    {
        var resultBronCalendar = await _bronCalendarService.GetBronCalendarById(id);

        BronCalendarPageViewModel bronCalendarPage = _mapper.Map<BronCalendarPageViewModel>(resultBronCalendar.Data);
        
        return View(bronCalendarPage);
    }

    //[HttpPost]
    //public async Task<IActionResult> Filter([FromBody] Filter filter)
    //{
    //    var result = _bronCalendarService.GetBronCalendarByFilter(filter);
        
    //    var filteredBronCalendars = _mapper.Map<List<BronCalendarForBronCalendarsViewModel>>(result.Data);

    //    return Json(filteredBronCalendars);
    //}
}
