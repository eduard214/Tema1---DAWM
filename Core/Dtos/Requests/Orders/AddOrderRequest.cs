using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Requests.Orders;

public class AddOrderRequest
{
    [Required(ErrorMessage = "Order date is required.")]
    public DateTime OrderDate { get; set; }

    [Required(ErrorMessage = "Total amount is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")] // Example validation
    public decimal TotalAmount { get; set; }

    [Required(ErrorMessage = "Customer ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid Customer ID.")] // Ensure CustomerId is positive
    public int CustomerId { get; set; }
}