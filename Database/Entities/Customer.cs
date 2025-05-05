namespace Database.Entities;

public class Customer : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}