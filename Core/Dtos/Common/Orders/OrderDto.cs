namespace Core.Dtos.Common.Orders;

public class OrderDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int CustomerId { get; set; }
    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }
}