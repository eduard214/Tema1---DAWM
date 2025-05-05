using Database.Context;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

// Add this using

namespace Database.Repositories;

public class CustomerRepository : BaseRepository<Customer>
{
    public CustomerRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }

    public Task<List<Customer>> GetCustomersWithOrdersAsync(bool includeDeleted = false)
    {
        return GetRecords(includeDeleted)
            .Include(c => c.Orders)
            .ToListAsync();
    }

    public Task<Customer?> GetCustomerWithOrdersByIdAsync(int customerId, bool includeDeleted = false)
    {
        return GetRecords(includeDeleted)
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Id == customerId);
    }

    public Task<Customer?> GetCustomerByEmailAsync(string email, bool includeDeleted = false)
    {
        return GetRecords(includeDeleted)
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public Task<Customer?> GetCustomerByEmailWithOrdersAsync(string email, bool includeDeleted = false)
    {
        return GetRecords(includeDeleted)
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Email == email);
    }
}