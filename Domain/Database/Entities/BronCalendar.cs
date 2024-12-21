using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkCalendarik.Domain.Database.ModelsDb;

public class BronCalendar
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public List<string> ImagesPaths { get; set; }

    public string Info { get; set; }

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }
}
