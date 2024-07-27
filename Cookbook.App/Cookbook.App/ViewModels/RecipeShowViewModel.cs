using Cookbook.App.Models;
using Cookbook.App.Services;
using Cookbook.App.Views;

namespace Cookbook.App.ViewModels;

[QueryProperty(nameof(Models.Recipe), "Recipe")]
public class RecipeShowViewModel : BaseViewModel
{
    private readonly ICookBookService _cookBookService;
    private readonly ShoppingListViewModel ShoppingListViewModel;
    private Recipe recipe;
    public Recipe Recipe
    {
        get => recipe;
        set
        {
            if (recipe == value)
                return;

            recipe = value;
            OnPropertyChanged();
        }
    }


    public Command GoToRecipeManageCommand { get; set; }
    public Command GoPageBackCommand { get; set; }
    public Command AddIngredientToShoplist { get; set; }


    public RecipeShowViewModel(ICookBookService cookBookService, ShoppingListViewModel shoppingListViewModel)
    {
        _cookBookService = cookBookService;
        GoToRecipeManageCommand = new Command(async () => await GoToRecipeManageAsync());
        GoPageBackCommand = new Command(async () => await GoPageBack());
        AddIngredientToShoplist = new Command(async () => await AddIngredientToShoplistAsync());
        ShoppingListViewModel = shoppingListViewModel;
    }

    public async Task GoToRecipeManageAsync()
    {
        await Shell.Current.GoToAsync(nameof(RecipeManagePage), true, new Dictionary<string, object>
        {
            {nameof(Models.Recipe), Recipe }
        });
    }

    public async Task AddIngredientToShoplistAsync()
    {

        if (IsBusy)
            return;

        IsBusy = true;

        try
        {
            await _cookBookService.AddIngredientToShoplist(Recipe.Id);
            IsBusy = false;
            ShoppingListViewModel.FirstStart = true;
            await Shell.Current.Navigation.PopAsync();
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
}
