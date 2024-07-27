using Cookbook.App.Models;
using Cookbook.App.Services;
using Cookbook.App.Views;
using System.Collections.ObjectModel;


namespace Cookbook.App.ViewModels;

public class RecipeListViewModel : BaseViewModel
{
    private readonly ICookBookService _cookBookService;
    public ObservableCollection<Recipe> RecipeList { get; } = new();
    public Command LoadDataFromServerCommand { get; set; }
    public Command ShowRecipeCommand { get; set; }
    public Command CreateNewRecipeCommand { get; set; }


    public RecipeListViewModel(ICookBookService cookBookService)
    {
        _cookBookService = cookBookService;
        LoadDataFromServerCommand = new Command(async () => await LoadDataFromServerAsync());
        ShowRecipeCommand = new Command(async (recipe) => await ShowRecipeAsync((Recipe)recipe));
        CreateNewRecipeCommand = new Command(async async => await CreateNewRecipe());

        
    }

    private async Task LoadDataFromServerAsync()
    {
        if (IsBusy)
            return;
        IsBusy = true;

        try
        {
            RecipeList.Clear();
            var recipes = await _cookBookService.GetAllRecipesAsync();

            foreach(var recipe in recipes)
            {
                RecipeList.Add(recipe);
            }
            //await _itemRepository.UpdateItemsFromServerAsync(itemList);
            IsBusy = false;
            //await LoadDataFromDatabaseAsync();


        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task ShowRecipeAsync(Recipe recipe)
    {
        await Shell.Current.GoToAsync(nameof(RecipeShowPage), true, new Dictionary<string, object>
        {
            {nameof(Recipe), recipe }
        });
    }

    private async Task CreateNewRecipe()
    {
        Recipe recipe = new Recipe();
        recipe.Ingredients = new List<Ingredient>();

        await Shell.Current.GoToAsync(nameof(RecipeManagePage), true, new Dictionary<string, object>
        {
            {nameof(Recipe), recipe }
        });
    }

}
