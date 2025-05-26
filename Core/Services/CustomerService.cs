using AutoMapper;
using Core.Dtos.Requests.Customers;
using Core.Dtos.Responses.Common;
using Core.Dtos.Responses.Customers;
using Database.Dtos;
using Database.Entities;
using Database.Repositories;

namespace Core.Services;

public class CustomerService
{
    private readonly CustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomerService(CustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerResponse>> GetAllCustomersAsync(bool includeDeleted = false)
    {
        var customers = await _customerRepository.GetAllAsync(includeDeleted);
        return _mapper.Map<IEnumerable<CustomerResponse>>(customers);
    }

    public async Task<CustomerResponse?> GetCustomerByIdAsync(int id, bool includeDeleted = false)
    {
        var customer = await _customerRepository.GetFirstOrDefaultAsync(id, includeDeleted);
        return customer == null ? null : _mapper.Map<CustomerResponse>(customer);
    }

    public async Task<IEnumerable<CustomerResponse>> GetCustomersWithOrdersAsync(bool includeDeleted = false)
    {
        var customers = await _customerRepository.GetCustomersWithOrdersAsync(includeDeleted);
        return _mapper.Map<IEnumerable<CustomerResponse>>(customers);
    }

    public async Task<CustomerResponse?> GetCustomerWithOrdersByIdAsync(int id, bool includeDeleted = false)
    {
        var customer = await _customerRepository.GetCustomerWithOrdersByIdAsync(id, includeDeleted);
        return customer == null ? null : _mapper.Map<CustomerResponse>(customer);
    }

    public async Task<CustomerResponse?> GetCustomerByEmailAsync(string email, bool includeDeleted = false)
    {
        var customer = await _customerRepository.GetCustomerByEmailAsync(email, includeDeleted);
        return customer == null ? null : _mapper.Map<CustomerResponse>(customer);
    }

    public async Task<CustomerResponse> CreateCustomerAsync(AddCustomerRequest request)
    {
        var customer = _mapper.Map<Customer>(request);
        _customerRepository.Insert(customer);
        await _customerRepository.SaveChangesAsync();
        return _mapper.Map<CustomerResponse>(customer);
    }

    public async Task<bool>
        UpdateCustomerAsync(int id,
            AddCustomerRequest request)
    {
        var existingCustomer = await _customerRepository.GetFirstOrDefaultAsync(id);
        if (existingCustomer == null) return false;

        _mapper.Map(request, existingCustomer);

        _customerRepository.Update(existingCustomer);
        await _customerRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var customer = await _customerRepository.GetFirstOrDefaultAsync(id);
        if (customer == null) return false;

        _customerRepository.SoftDelete(customer);
        await _customerRepository.SaveChangesAsync();
        return true;
    }

    public async Task<PagedResponse<CustomerResponse>> GetFilteredCustomersAsync(GetFilteredCustomersRequest request)
    {
        if (request == null || request.Filters == null || request.SortingOption == null)
            return new PagedResponse<CustomerResponse>
            {
                Items = new List<CustomerResponse>(),
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 10,
                TotalPages = 0
            };

        request.Pagination ??= new PaginationDto();

        var (customers, totalCount) = await _customerRepository.GetFilteredCustomersAsync(
            request.Filters,
            request.SortingOption,
            request.Pagination.PageNumber,
            request.Pagination.PageSize);

        var customerResponses = _mapper.Map<IEnumerable<CustomerResponse>>(customers);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.Pagination.PageSize);

        return new PagedResponse<CustomerResponse>
        {
            Items = customerResponses,
            TotalCount = totalCount,
            PageNumber = request.Pagination.PageNumber,
            PageSize = request.Pagination.PageSize,
            TotalPages = totalPages
        };
    }
}