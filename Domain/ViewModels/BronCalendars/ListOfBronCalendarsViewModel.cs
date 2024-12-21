using WorkCalendarik.Domain.Database.Entities;
using WorkCalendarik.Domain.Database.ModelsDb;

namespace WorkCalendarik.Domain.ViewModels.BronCalendars;

public class ListOfBronCalendarsViewModel
{
    public List<BronCalendarForBronCalendarsViewModel> BronCalendars { get; set; }
}

public class BronCalendarForBronCalendarsViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public List<string> ImagesPaths { get; set; }

    public string Info { get; set; }

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }
}