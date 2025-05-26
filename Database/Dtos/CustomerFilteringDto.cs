using Database.Enums;

namespace Database.Dtos;

public class CustomerFilteringDto
{
    public FilterType FilterType { get; set; } = FilterType.Name;

    public string? NameContains { get; set; }

    public string? EmailContains { get; set; }

    public DateTime? CreatedAfter { get; set; }
    public DateTime? CreatedBefore { get; set; }

    public int? MinOrderCount { get; set; }

    public decimal? MinTotalSpent { get; set; }

    public bool? IncludeDeleted { get; set; }
}