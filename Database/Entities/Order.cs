namespace Database.Entities;

public class Order : BaseEntity
{
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }

    public int CustomerId { get; set; }

    public virtual Customer Customer { get; set; }
}