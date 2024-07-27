using SQLite;


namespace Cookbook.App.Models;

public class Recipe : BaseModel
{
    private int id;
    [PrimaryKey]
    public int Id
    {
        get => id;
        set
        {
            if (id == value)
                return;
            id = value;
            this.OnPropertyChanged();
        }
    }

    private string name;
    public string Name
    {
        get => name;
        set
        {
            if (name == value)
                return;
            name = value;
            this.OnPropertyChanged();
        }
    }

    private string preparation;
    public string Preparation
    {
        get => preparation;
        set
        {
            if (preparation == value)
                return;
            preparation = value;
            this.OnPropertyChanged();
        }
    }

    private List<Ingredient> ingredients;
    public List<Ingredient> Ingredients
    {
        get => ingredients;
        set
        {
            if (ingredients == value)
                return;
            ingredients = value;
            this.OnPropertyChanged();
        }
    }
}
