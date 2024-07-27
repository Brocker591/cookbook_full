namespace Cookbook.App.Dtos;

public class IngredientDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string quantity { get; set; }
    public int RecipeId { get; set; }
}
