namespace Cookbook.App.Dtos;

public class RecipeUpdateDto
{
    public int id { get; set; }
    public string name { get; set; } = default!;
    public string preparation { get; set; } = default!;
    public List<IngredientUpdateDto> ingredients { get; set; } = default!;
}
