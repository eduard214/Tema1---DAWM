namespace Core.Dtos.Responses.Customers;

public class CustomerResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }
}