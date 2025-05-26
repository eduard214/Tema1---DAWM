using Core.Dtos.Requests.Customers;
using Core.Dtos.Responses.Customers;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomersController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponse>>> GetCustomers([FromQuery] bool includeDeleted = false)
    {
        var customers = await _customerService.GetAllCustomersAsync(includeDeleted);
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponse>> GetCustomer(int id, [FromQuery] bool includeDeleted = false)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id, includeDeleted);

        if (customer == null) return NotFound();

        return Ok(customer);
    }

    [HttpGet("with-orders")]
    public async Task<ActionResult<IEnumerable<CustomerResponse>>> GetCustomersWithOrders(
        [FromQuery] bool includeDeleted = false)
    {
        var customers = await _customerService.GetCustomersWithOrdersAsync(includeDeleted);
        return Ok(customers);
    }

    [HttpGet("{id}/with-orders")]
    public async Task<ActionResult<CustomerResponse>> GetCustomerWithOrdersById(int id,
        [FromQuery] bool includeDeleted = false)
    {
        var customer = await _customerService.GetCustomerWithOrdersByIdAsync(id, includeDeleted);

        if (customer == null) return NotFound();

        return Ok(customer);
    }

    [HttpGet("by-email/{email}")]
    public async Task<ActionResult<CustomerResponse>> GetCustomerByEmail(string email,
        [FromQuery] bool includeDeleted = false)
    {
        var customer = await _customerService.GetCustomerByEmailAsync(email, includeDeleted);

        if (customer == null) return NotFound();

        return Ok(customer);
    }

    [HttpGet("by-email/{email}/with-orders")]
    public async Task<ActionResult<CustomerResponse>> GetCustomerByEmailWithOrders(string email,
        [FromQuery] bool includeDeleted = false)
    {
        var customer = await _customerService.GetCustomerByEmailAsync(email, includeDeleted);

        if (customer == null) return NotFound();

        return Ok(customer);
    }


    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> PostCustomer([FromBody] AddCustomerRequest customerRequest)
    {
        if (customerRequest == null) return BadRequest("Customer data is required.");

        var createdCustomer = await _customerService.CreateCustomerAsync(customerRequest);

        return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.Id }, createdCustomer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCustomer(int id, [FromBody] AddCustomerRequest customerRequest)
    {
        if (customerRequest == null) return BadRequest("Customer data is required.");

        var success = await _customerService.UpdateCustomerAsync(id, customerRequest);

        if (!success) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var success = await _customerService.DeleteCustomerAsync(id);

        if (!success) return NotFound();

        return NoContent();
    }

    [HttpPost("get-filtered-customers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetFilteredCustomers([FromBody] GetFilteredCustomersRequest payload)
    {
        if (payload == null) return BadRequest("Filter criteria is required.");

        var result = await _customerService.GetFilteredCustomersAsync(payload);
        return Ok(result);
    }
}