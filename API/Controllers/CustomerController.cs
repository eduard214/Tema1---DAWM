// API/Controllers/CustomerController.cs

using Core.Dtos.Requests.Customers;
using Core.Dtos.Responses.Customers;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
// Add using for request DTO
// Add using for response DTO
// Add using for the service

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    // Inject CustomerService instead of CustomerRepository
    private readonly CustomerService _customerService;

    public CustomersController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    // Return CustomerResponse DTO
    public async Task<ActionResult<IEnumerable<CustomerResponse>>> GetCustomers([FromQuery] bool includeDeleted = false)
    {
        // Call service method
        var customers = await _customerService.GetAllCustomersAsync(includeDeleted);
        return Ok(customers);
    }

    [HttpGet("{id}")]
    // Return CustomerResponse DTO
    public async Task<ActionResult<CustomerResponse>> GetCustomer(int id, [FromQuery] bool includeDeleted = false)
    {
        // Call service method
        var customer = await _customerService.GetCustomerByIdAsync(id, includeDeleted);

        if (customer == null) return NotFound();

        return Ok(customer);
    }

    [HttpGet("with-orders")]
    // Return CustomerResponse DTO
    public async Task<ActionResult<IEnumerable<CustomerResponse>>> GetCustomersWithOrders(
        [FromQuery] bool includeDeleted = false)
    {
        // Call service method
        var customers = await _customerService.GetCustomersWithOrdersAsync(includeDeleted);
        return Ok(customers);
    }

    [HttpGet("{id}/with-orders")]
    // Return CustomerResponse DTO
    public async Task<ActionResult<CustomerResponse>> GetCustomerWithOrdersById(int id,
        [FromQuery] bool includeDeleted = false)
    {
        // Call service method
        var customer = await _customerService.GetCustomerWithOrdersByIdAsync(id, includeDeleted);

        if (customer == null) return NotFound();

        return Ok(customer);
    }

    [HttpGet("by-email/{email}")]
    // Return CustomerResponse DTO
    public async Task<ActionResult<CustomerResponse>> GetCustomerByEmail(string email,
        [FromQuery] bool includeDeleted = false)
    {
        // Call service method
        var customer = await _customerService.GetCustomerByEmailAsync(email, includeDeleted);

        if (customer == null) return NotFound();

        return Ok(customer);
    }

    // Note: CustomerService doesn't have GetCustomerByEmailWithOrdersAsync.
    // You might need to add it or adjust GetCustomerByEmailAsync mapping.
    // This example assumes GetCustomerByEmailAsync is sufficient for now.
    [HttpGet("by-email/{email}/with-orders")]
    public async Task<ActionResult<CustomerResponse>> GetCustomerByEmailWithOrders(string email,
        [FromQuery] bool includeDeleted = false)
    {
        // Placeholder: Call appropriate service method when available
        // For now, using GetCustomerByEmailAsync as an example
        var customer = await _customerService.GetCustomerByEmailAsync(email, includeDeleted); // Adjust if needed

        if (customer == null) return NotFound();

        return Ok(customer);
    }


    [HttpPost]
    // Accept AddCustomerRequest DTO, Return CustomerResponse DTO
    public async Task<ActionResult<CustomerResponse>> PostCustomer([FromBody] AddCustomerRequest customerRequest)
    {
        if (customerRequest == null) return BadRequest("Customer data is required.");

        // Call service method
        var createdCustomer = await _customerService.CreateCustomerAsync(customerRequest);

        // Use the ID from the response DTO
        return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.Id }, createdCustomer);
    }

    [HttpPut("{id}")]
    // Accept AddCustomerRequest DTO (or a specific Update DTO)
    public async Task<IActionResult> PutCustomer(int id, [FromBody] AddCustomerRequest customerRequest)
    {
        if (customerRequest == null) return BadRequest("Customer data is required.");

        // Call service method
        var success = await _customerService.UpdateCustomerAsync(id, customerRequest);

        if (!success) return NotFound(); // Service returns false if customer not found

        return NoContent(); // Return 204 No Content on successful update
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        // Call service method
        var success = await _customerService.DeleteCustomerAsync(id);

        if (!success) return NotFound(); // Service returns false if customer not found

        return NoContent(); // Return 204 No Content on successful delete
    }
}