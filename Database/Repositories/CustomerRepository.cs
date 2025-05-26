using Database.Context;
using Database.Dtos;
using Database.Entities;
using Database.Enums;
using Microsoft.EntityFrameworkCore;

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

    public async Task<(List<Customer> Items, int TotalCount)> GetFilteredCustomersAsync(
        CustomerFilteringDto filters,
        CustomerSortingDto sortingOption,
        int pageNumber,
        int pageSize)
    {
        var query = GetRecords(filters.IncludeDeleted ?? false);

        switch (filters.FilterType)
        {
            case FilterType.Name:
                if (!string.IsNullOrEmpty(filters.NameContains))
                    query = query.Where(c => c.Name.Contains(filters.NameContains));
                break;

            case FilterType.Email:
                if (!string.IsNullOrEmpty(filters.EmailContains))
                    query = query.Where(c => c.Email.Contains(filters.EmailContains));
                break;

            case FilterType.CreatedDate:
                if (filters.CreatedAfter.HasValue)
                    query = query.Where(c => c.CreatedAt >= filters.CreatedAfter.Value);

                if (filters.CreatedBefore.HasValue)
                    query = query.Where(c => c.CreatedAt <= filters.CreatedBefore.Value);
                break;

            case FilterType.OrderCount:
                if (filters.MinOrderCount.HasValue)
                    query = query.Include(c => c.Orders)
                        .Where(c => c.Orders.Count >= filters.MinOrderCount.Value);
                break;

            case FilterType.TotalSpent:
                if (filters.MinTotalSpent.HasValue)
                    query = query.Include(c => c.Orders)
                        .Where(c => c.Orders.Sum(o => o.TotalAmount) >= filters.MinTotalSpent.Value);
                break;
        }

        if (!string.IsNullOrEmpty(sortingOption.SortBy))
            query = sortingOption.SortBy.ToLower() switch
            {
                "name" => sortingOption.IsDescending
                    ? query.OrderByDescending(c => c.Name)
                    : query.OrderBy(c => c.Name),
                "email" => sortingOption.IsDescending
                    ? query.OrderByDescending(c => c.Email)
                    : query.OrderBy(c => c.Email),
                "createdat" => sortingOption.IsDescending
                    ? query.OrderByDescending(c => c.CreatedAt)
                    : query.OrderBy(c => c.CreatedAt),
                "ordercount" => sortingOption.IsDescending
                    ? query.OrderByDescending(c => c.Orders.Count)
                    : query.OrderBy(c => c.Orders.Count),
                "totalspent" => sortingOption.IsDescending
                    ? query.OrderByDescending(c => c.Orders.Sum(o => o.TotalAmount))
                    : query.OrderBy(c => c.Orders.Sum(o => o.TotalAmount)),
                _ => query.OrderBy(c => c.Id)
            };
        else
            query = query.OrderBy(c => c.Id);

        return await GetPagedData(query, pageNumber, pageSize);
    }
}