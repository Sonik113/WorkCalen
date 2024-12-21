using Microsoft.EntityFrameworkCore;
using WorkCalendarik.Domain.Database.ModelsDb;
using WorkCalendarik.Domain.Interfaces;

namespace WorkCalendarik.Domain.Database.Storage;

public class BronCalendarStorage : IBaseStorage<BronCalendarDb>
{
    public readonly ApplicationDbContext _db;

    public BronCalendarStorage(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Add(BronCalendarDb item)
    {
        await _db.AddAsync(item);
        await _db.SaveChangesAsync();
    }
    
    public async Task Delete(BronCalendarDb item)
    {
        _db.Remove(item);
        await _db.SaveChangesAsync();
    }
    
    public async Task<BronCalendarDb> Get(Guid id)
    {
        return await _db.BronCalendarsDb.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public IQueryable<BronCalendarDb> GetAll()
    {
        return _db.BronCalendarsDb;
    }

    public async Task<BronCalendarDb> Update(BronCalendarDb item)
    {
        _db.BronCalendarsDb.Update(item);
        await _db.SaveChangesAsync();
        
        return item;
    }
}