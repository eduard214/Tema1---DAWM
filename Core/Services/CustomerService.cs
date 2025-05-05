// Core/Services/CustomerService.cs

using AutoMapper;
using Core.Dtos.Requests.Customers;
using Core.Dtos.Responses.Customers;
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
        // Ensure your MappingProfile handles mapping Orders within CustomerResponse if needed
        var customers = await _customerRepository.GetCustomersWithOrdersAsync(includeDeleted);
        return _mapper.Map<IEnumerable<CustomerResponse>>(customers);
    }

    public async Task<CustomerResponse?> GetCustomerWithOrdersByIdAsync(int id, bool includeDeleted = false)
    {
        // Ensure your MappingProfile handles mapping Orders within CustomerResponse if needed
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
        // BaseRepository handles CreatedAt implicitly if not set here
        _customerRepository.Insert(customer);
        await _customerRepository.SaveChangesAsync();
        return _mapper.Map<CustomerResponse>(customer); // Map the created entity back to response DTO
    }

    public async Task<bool>
        UpdateCustomerAsync(int id,
            AddCustomerRequest request) // Assuming AddCustomerRequest can be used for updates too, or create an UpdateCustomerRequest
    {
        var existingCustomer = await _customerRepository.GetFirstOrDefaultAsync(id);
        if (existingCustomer == null) return false; // Customer not found

        // Map updated fields from request to existing entity
        _mapper.Map(request, existingCustomer);
        // BaseRepository handles ModifiedAt in Update method

        _customerRepository.Update(existingCustomer);
        await _customerRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var customer = await _customerRepository.GetFirstOrDefaultAsync(id);
        if (customer == null) return false; // Customer not found

        _customerRepository.SoftDelete(customer);
        await _customerRepository.SaveChangesAsync();
        return true;
    }
}