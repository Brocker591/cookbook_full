namespace Cookbook.App.Dtos;

public class RecipeCreateDto
{
    public string name { get; set; } = default!;
    public string preparation { get; set; } = default!;
    public List<IngredientCreateDto> ingredients { get; set; } = default!;
}
