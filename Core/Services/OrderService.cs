using AutoMapper;
using Core.Dtos.Requests.Orders;
using Core.Dtos.Responses.Orders;
using Database.Entities;
using Database.Repositories;

namespace Core.Services;

public class OrderService
{
    private readonly CustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly OrderRepository _orderRepository;

    public OrderService(OrderRepository orderRepository, CustomerRepository customerRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync(bool includeDeleted = false)
    {
        var orders = await _orderRepository.GetAllAsync(includeDeleted);
        return _mapper.Map<IEnumerable<OrderResponse>>(orders);
    }

    public async Task<OrderResponse?> GetOrderByIdAsync(int id, bool includeDeleted = false)
    {
        var order = await _orderRepository.GetFirstOrDefaultAsync(id, includeDeleted);
        return order == null ? null : _mapper.Map<OrderResponse>(order);
    }

    public async Task<IEnumerable<OrderResponse>> GetOrdersByCustomerIdAsync(int customerId,
        bool includeDeleted = false)
    {
        var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId, includeDeleted);
        return _mapper.Map<IEnumerable<OrderResponse>>(orders);
    }

    public async Task<IEnumerable<OrderResponse>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate,
        bool includeDeleted = false)
    {
        var orders = await _orderRepository.GetOrdersByDateRangeAsync(startDate, endDate, includeDeleted);
        return _mapper.Map<IEnumerable<OrderResponse>>(orders);
    }

    public async Task<IEnumerable<OrderResponse>> GetOrdersWithCustomerAsync(bool includeDeleted = false)
    {
        var orders = await _orderRepository.GetOrdersWithCustomerAsync(includeDeleted);
        return _mapper.Map<IEnumerable<OrderResponse>>(orders);
    }

    public async Task<OrderResponse?> GetOrderWithCustomerByIdAsync(int orderId, bool includeDeleted = false)
    {
        var order = await _orderRepository.GetOrderWithCustomerByIdAsync(orderId, includeDeleted);
        return order == null ? null : _mapper.Map<OrderResponse>(order);
    }


    public async Task<OrderResponse?> CreateOrderAsync(AddOrderRequest request)
    {
        var customerExists = await _customerRepository.GetFirstOrDefaultAsync(request.CustomerId);
        if (customerExists == null)
            return null;

        var order = _mapper.Map<Order>(request);
        _orderRepository.Insert(order);
        await _orderRepository.SaveChangesAsync();
        return _mapper.Map<OrderResponse>(order);
    }

    public async Task<bool>
        UpdateOrderAsync(int id, AddOrderRequest request)
    {
        var existingOrder = await _orderRepository.GetFirstOrDefaultAsync(id);
        if (existingOrder == null) return false;

        var customerExists = await _customerRepository.GetFirstOrDefaultAsync(request.CustomerId);
        if (customerExists == null)
            return false;

        _mapper.Map(request, existingOrder);

        _orderRepository.Update(existingOrder);
        await _orderRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        var order = await _orderRepository.GetFirstOrDefaultAsync(id);
        if (order == null) return false;

        _orderRepository.SoftDelete(order);
        await _orderRepository.SaveChangesAsync();
        return true;
    }
}