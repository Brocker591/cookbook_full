namespace Cookbook.App.Dtos;

public class IngredientDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string quantity { get; set; } = default!;
    public int RecipeId { get; set; } = default!;
}
