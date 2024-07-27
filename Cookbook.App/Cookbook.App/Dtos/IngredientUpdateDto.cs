namespace Cookbook.App.Dtos;

public class IngredientUpdateDto
{

    public int id { get; set; }
    public string name { get; set; }
    public string quantity { get; set; }
    public int recipe_id { get; set; }
}
