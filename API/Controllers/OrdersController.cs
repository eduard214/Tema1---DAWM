using Core.Dtos.Requests.Orders;
using Core.Dtos.Responses.Orders;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrders([FromQuery] bool includeDeleted = false)
    {
        var orders = await _orderService.GetAllOrdersAsync(includeDeleted);
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponse>> GetOrder(int id, [FromQuery] bool includeDeleted = false)
    {
        var order = await _orderService.GetOrderByIdAsync(id, includeDeleted);

        if (order == null) return NotFound();

        return Ok(order);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByCustomer(int customerId,
        [FromQuery] bool includeDeleted = false)
    {
        var orders = await _orderService.GetOrdersByCustomerIdAsync(customerId, includeDeleted);
        return Ok(orders);
    }

    [HttpGet("date-range")]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByDateRange(
        [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] bool includeDeleted = false)
    {
        var orders = await _orderService.GetOrdersByDateRangeAsync(startDate, endDate, includeDeleted);
        return Ok(orders);
    }

    [HttpGet("with-customer")]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersWithCustomer(
        [FromQuery] bool includeDeleted = false)
    {
        var orders = await _orderService.GetOrdersWithCustomerAsync(includeDeleted);
        return Ok(orders);
    }

    [HttpGet("{orderId}/with-customer")]
    public async Task<ActionResult<OrderResponse>> GetOrderWithCustomerById(int orderId,
        [FromQuery] bool includeDeleted = false)
    {
        var order = await _orderService.GetOrderWithCustomerByIdAsync(orderId, includeDeleted);

        if (order == null) return NotFound();

        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> PostOrder([FromBody] AddOrderRequest orderRequest)
    {
        if (orderRequest == null) return BadRequest("Order data is required.");

        var createdOrder = await _orderService.CreateOrderAsync(orderRequest);

        if (createdOrder == null)
            return BadRequest("Invalid Customer ID or failed to create order.");

        return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutOrder(int id, [FromBody] AddOrderRequest orderRequest)
    {
        if (orderRequest == null) return BadRequest("Order data is required.");

        var success = await _orderService.UpdateOrderAsync(id, orderRequest);

        if (!success)
            return NotFound("Order not found or update failed (e.g., invalid Customer ID).");

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var success = await _orderService.DeleteOrderAsync(id);

        if (!success) return NotFound();

        return NoContent();
    }
}