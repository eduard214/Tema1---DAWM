using Database.Context;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories;

public class BaseRepository<T>(DatabaseContext databaseContext) where T : BaseEntity
{
    protected readonly DatabaseContext _dbContext = databaseContext;
    private DbSet<T> DbSet { get; } = databaseContext.Set<T>();

    public Task<List<T>> GetAllAsync(bool includeDeletedEntities = false)
    {
        return GetRecords(includeDeletedEntities).ToListAsync();
    }

    public Task<T?> GetFirstOrDefaultAsync(int primaryKey, bool includeDeletedEntities = false)
    {
        var records = GetRecords(includeDeletedEntities);

        return records.FirstOrDefaultAsync(record => record.Id == primaryKey);
    }

    public void Insert(params T[] records)
    {
        DbSet.AddRange(records);
    }

    public void Update(params T[] records)
    {
        foreach (var baseEntity in records) baseEntity.ModifiedAt = DateTime.UtcNow;

        DbSet.UpdateRange(records);
    }

    public void SoftDelete(params T[] records)
    {
        foreach (var baseEntity in records) baseEntity.DeletedAt = DateTime.UtcNow;

        Update(records);
    }

    public Task SaveChangesAsync()
    {
        return databaseContext.SaveChangesAsync();
    }

    protected IQueryable<T> GetRecords(bool includeDeletedEntities = false)
    {
        var result = DbSet.AsQueryable();

        if (includeDeletedEntities is false) result = result.Where(r => r.DeletedAt == null);

        return result;
    }

    public async Task<(List<T> Items, int TotalCount)> GetPagedData(IQueryable<T> query, int pageNumber, int pageSize)
    {
        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}