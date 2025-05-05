// API/Controllers/OrdersController.cs

using Core.Dtos.Requests.Orders;
using Core.Dtos.Responses.Orders;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
// Add using for request DTO
// Add using for response DTO
// Add using for the service

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    // Inject OrderService instead of OrderRepository
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    // Return OrderResponse DTO
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrders([FromQuery] bool includeDeleted = false)
    {
        // Call service method
        var orders = await _orderService.GetAllOrdersAsync(includeDeleted);
        return Ok(orders);
    }

    [HttpGet("{id}")]
    // Return OrderResponse DTO
    public async Task<ActionResult<OrderResponse>> GetOrder(int id, [FromQuery] bool includeDeleted = false)
    {
        // Call service method
        var order = await _orderService.GetOrderByIdAsync(id, includeDeleted);

        if (order == null) return NotFound();

        return Ok(order);
    }

    [HttpGet("customer/{customerId}")]
    // Return OrderResponse DTO
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByCustomer(int customerId,
        [FromQuery] bool includeDeleted = false)
    {
        // Call service method
        var orders = await _orderService.GetOrdersByCustomerIdAsync(customerId, includeDeleted);
        // Consider checking if the customer exists first if needed, or handle empty list
        return Ok(orders);
    }

    [HttpGet("date-range")]
    // Return OrderResponse DTO
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByDateRange(
        [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] bool includeDeleted = false)
    {
        // Call service method
        var orders = await _orderService.GetOrdersByDateRangeAsync(startDate, endDate, includeDeleted);
        return Ok(orders);
    }

    [HttpGet("with-customer")]
    // Return OrderResponse DTO
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersWithCustomer(
        [FromQuery] bool includeDeleted = false)
    {
        // Call service method
        var orders = await _orderService.GetOrdersWithCustomerAsync(includeDeleted);
        return Ok(orders);
    }

    [HttpGet("{orderId}/with-customer")]
    // Return OrderResponse DTO
    public async Task<ActionResult<OrderResponse>> GetOrderWithCustomerById(int orderId,
        [FromQuery] bool includeDeleted = false)
    {
        // Call service method
        var order = await _orderService.GetOrderWithCustomerByIdAsync(orderId, includeDeleted);

        if (order == null) return NotFound();

        return Ok(order);
    }

    [HttpPost]
    // Accept AddOrderRequest DTO, Return OrderResponse DTO
    public async Task<ActionResult<OrderResponse>> PostOrder([FromBody] AddOrderRequest orderRequest)
    {
        if (orderRequest == null) return BadRequest("Order data is required.");

        // Call service method
        var createdOrder = await _orderService.CreateOrderAsync(orderRequest);

        if (createdOrder == null)
            // Service returns null if customer doesn't exist (based on current service implementation)
            return BadRequest("Invalid Customer ID or failed to create order.");

        // Use the ID from the response DTO
        return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
    }

    [HttpPut("{id}")]
    // Accept AddOrderRequest DTO (or a specific Update DTO)
    public async Task<IActionResult> PutOrder(int id, [FromBody] AddOrderRequest orderRequest)
    {
        if (orderRequest == null) return BadRequest("Order data is required.");

        // Optional: Add validation if ID in route must match something in body,
        // although it's often better to just use the ID from the route.

        // Call service method
        var success = await _orderService.UpdateOrderAsync(id, orderRequest);

        if (!success)
            // Could be NotFound (order or customer) or other validation failure in service
            return NotFound("Order not found or update failed (e.g., invalid Customer ID).");

        return NoContent(); // Return 204 No Content on successful update
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        // Call service method
        var success = await _orderService.DeleteOrderAsync(id);

        if (!success) return NotFound(); // Service returns false if order not found

        return NoContent(); // Return 204 No Content on successful delete
    }
}