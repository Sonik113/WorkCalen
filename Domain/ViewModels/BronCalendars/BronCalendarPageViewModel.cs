using WorkCalendarik.Domain.Database.Entities;

namespace WorkCalendarik.Domain.ViewModels.BronCalendars;

public class BronCalendarPageViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public List<string> ImagesPaths { get; set; }

    public string Info { get; set; }

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }
}