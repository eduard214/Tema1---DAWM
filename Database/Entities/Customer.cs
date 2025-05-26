using System.ComponentModel.DataAnnotations;

namespace Database.Entities;

public class Customer : BaseEntity
{
    [Required] [MaxLength(100)] public string Name { get; set; }

    [Required] [MaxLength(150)] public string Email { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}