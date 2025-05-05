using Database.Context;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories;

public class OrderRepository : BaseRepository<Order>
{
    private readonly DatabaseContext _databaseContext;

    public OrderRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId, bool includeDeleted = false)
    {
        return GetRecords(includeDeleted)
            .Where(o => o.CustomerId == customerId)
            .ToListAsync();
    }

    public Task<List<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate,
        bool includeDeleted = false)
    {
        return GetRecords(includeDeleted)
            .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
            .ToListAsync();
    }

    public Task<List<Order>> GetOrdersWithCustomerAsync(bool includeDeleted = false)
    {
        return GetRecords(includeDeleted)
            .Include(o => o.Customer)
            .ToListAsync();
    }

    public Task<Order?> GetOrderWithCustomerByIdAsync(int orderId, bool includeDeleted = false)
    {
        return GetRecords(includeDeleted)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }
}