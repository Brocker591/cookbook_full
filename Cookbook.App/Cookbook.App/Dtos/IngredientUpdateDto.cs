namespace Cookbook.App.Dtos;

public class IngredientUpdateDto
{

    public int id { get; set; }
    public string name { get; set; } = default!;
    public string quantity { get; set; } = default!;
    public int recipe_id { get; set; }
}
