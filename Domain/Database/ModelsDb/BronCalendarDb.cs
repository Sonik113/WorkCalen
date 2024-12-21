using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkCalendarik.Domain.Database.ModelsDb;

[Table("broncalendars2")]
public class BronCalendarDb
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("imagespaths")]
    public List<string> ImagesPaths { get; set; }

    [Column("info")]
    public string Info { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

    [Column("createdat", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }
}
