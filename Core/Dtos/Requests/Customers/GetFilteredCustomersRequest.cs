using Database.Dtos;

namespace Core.Dtos.Requests.Customers;

public class GetFilteredCustomersRequest
{
    public CustomerFilteringDto Filters { get; set; }
    public CustomerSortingDto SortingOption { get; set; }
    public PaginationDto Pagination { get; set; } = new();
}