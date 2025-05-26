namespace Database.Dtos;

public class CustomerSortingDto
{
    public string? SortBy { get; set; }
    public bool IsDescending { get; set; } = false;
}