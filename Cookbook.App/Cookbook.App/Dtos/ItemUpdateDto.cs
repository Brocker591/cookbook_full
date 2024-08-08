namespace Cookbook.App.Dtos;

public class ItemUpdateDto
{
    public int id { get; set; }
    public string name { get; set; } = default!;
    public string quantity { get; set; } = default!;
    public int priority { get; set; }
    public bool inventory { get; set; } = false;
}
