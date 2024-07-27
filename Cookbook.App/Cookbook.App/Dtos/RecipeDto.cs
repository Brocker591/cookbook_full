namespace Cookbook.App.Dtos;

public class RecipeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Preparation { get; set; }
    public List<IngredientDto> Ingredients { get; set; }
}
