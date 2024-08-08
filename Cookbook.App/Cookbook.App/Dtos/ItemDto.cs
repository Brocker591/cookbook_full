namespace Cookbook.App.Dtos;

public class ItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Quantity { get; set; } = default!;
    public int Priority { get; set; }
    public bool Inventory { get; set; } 
}
