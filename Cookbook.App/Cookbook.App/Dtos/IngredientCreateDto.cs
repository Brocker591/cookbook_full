namespace Cookbook.App.Dtos;

public class IngredientCreateDto
{
    public string name { get; set; }
    public string quantity { get; set; }
    public int? recipe_id { get; set; }
}
