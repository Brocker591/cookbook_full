using SQLite;

namespace Cookbook.App.Models;

public class ItemToSend
{
    [PrimaryKey]
    public Guid Id { get; set; }
    public int ItemId { get; set; }
    public string Name { get; set; } = default!;
    public string Quantity { get; set; } = default!;
    public int Priority { get; set; }
    public bool Inventory { get; set; }
}
