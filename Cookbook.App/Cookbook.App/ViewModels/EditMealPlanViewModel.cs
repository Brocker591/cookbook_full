using Cookbook.App.Models;
using Cookbook.App.PageModels;
using Cookbook.App.Repositories;
using Cookbook.App.Services;
using System.Collections.ObjectModel;

namespace Cookbook.App.ViewModels;

[QueryProperty(nameof(PageModels.MealPlanPageModel), "MealPlanPageModel")]
public class EditMealPlanViewModel : BaseViewModel
{
    private readonly ICookBookService _cookBookService;
    private readonly IUserRepository _userRepository;


    private MealPlanPageModel mealPlanPageModel;
    public MealPlanPageModel MealPlanPageModel
    {
        get => mealPlanPageModel;
        set
        {
            if (mealPlanPageModel == value)
                return;

            mealPlanPageModel = value;
            OnPropertyChanged();
        }
    }
    public ObservableCollection<Recipe> RecipeList { get; } = new();
    public Command LoadDataFromServerCommand { get; set; }
    public Command SaveMealPlanCommand { get; set; }
    public Command SetMealPlanNameFromRecipeCommand { get; set; }

    public EditMealPlanViewModel(ICookBookService cookBookService, IUserRepository userRepository)
    {
        _cookBookService = cookBookService;
        _userRepository = userRepository;
        LoadDataFromServerCommand = new Command(async () => await LoadDataFromServerAsync());
        SaveMealPlanCommand = new Command(async () => await SaveMealPlanDayAsync());
        SetMealPlanNameFromRecipeCommand = new Command(async (recipe) => await SetMealPlanNameFromRecipeAsync((Recipe)recipe));
        
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

            foreach (var recipe in recipes)
            {
                RecipeList.Add(recipe);
            }

            IsBusy = false;
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

    private async Task SetMealPlanNameFromRecipeAsync(Recipe recipe)
    {
        if (IsBusy)
            return;
        IsBusy = true;

        try
        {
            MealPlanPageModel.MealName = recipe.Name;

            IsBusy = false;
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

    private async Task SaveMealPlanDayAsync()
    {
        try
        {
            var user = await _userRepository.GetAsync();

            if (user is null)
                throw new Exception("Es Konnte kein User geladen werden.");

            var mealPlan = await _cookBookService.GetMealPlanAsync(user.UserId);

            if (mealPlan is null)
                mealPlan = new MealPlan();

            if (mealPlanPageModel.DayName == nameof(mealPlan.Monday))
                mealPlan.Monday = mealPlanPageModel.MealName;

            if (mealPlanPageModel.DayName == nameof(mealPlan.Tuesday))
                mealPlan.Tuesday = mealPlanPageModel.MealName;

            if (mealPlanPageModel.DayName == nameof(mealPlan.Wednesday))
                mealPlan.Wednesday = mealPlanPageModel.MealName;

            if (mealPlanPageModel.DayName == nameof(mealPlan.Thursday))
                mealPlan.Thursday = mealPlanPageModel.MealName;

            if (mealPlanPageModel.DayName == nameof(mealPlan.Friday))
                mealPlan.Friday = mealPlanPageModel.MealName;

            if (mealPlanPageModel.DayName == nameof(mealPlan.Saturday))
                mealPlan.Saturday = mealPlanPageModel.MealName;

            if (mealPlanPageModel.DayName == nameof(mealPlan.Sunday))
                mealPlan.Sunday = mealPlanPageModel.MealName;

            if(mealPlan.Id is 0)
            {
                mealPlan.Id = user.UserId;
                await _cookBookService.CreateMealPlanAsync(mealPlan);
            }
            else
            {
                await _cookBookService.UpdateMealPlanAsync(mealPlan);
            }
            
            await Shell.Current.Navigation.PopAsync();

        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
    }
}
