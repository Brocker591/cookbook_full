namespace Cookbook.App.Dtos;

public class IngredientCreateDto
{
    public string name { get; set; } = default!;
    public string quantity { get; set; } = default!;
    public int? recipe_id { get; set; } = default!;
}
