using Cookbook.App.Models;
using Cookbook.App.Repositories;
using Cookbook.App.Services;

namespace Cookbook.App.ViewModels;

public class MealPlanViewModel : BaseViewModel
{
    private readonly ICookBookService _cookBookService;
    private readonly IUserRepository _userRepository;
    public Command LoadDataCommand { get; set; }
    public Command SaveMealPlanCommand { get; set; }
    public User User { get; set; }

    private MealPlan mealPlan;
    public MealPlan MealPlan
    {
        get => mealPlan;
        set
        {
            if (mealPlan == value)
                return;

            mealPlan = value;
            OnPropertyChanged();
        }
    }
    public MealPlanViewModel(ICookBookService cookBookService, IUserRepository userRepository)
    {
        _cookBookService = cookBookService;
        _userRepository = userRepository;
        LoadDataCommand = new Command(async () => await LoadDataAsync());
        SaveMealPlanCommand = new Command(async () => await CreateOrUpdateMealPlan());
    }

    private async Task LoadDataAsync()
    {
        if (IsBusy)
            return;
        IsBusy = true;

        try
        {
            this.User = await _userRepository.GetAsync();

            if (this.User is null)
                throw new Exception("Es Konnte kein User geladen werden.");

            this.MealPlan = await _cookBookService.GetMealPlanAsync(this.User.UserId);

            if (this.MealPlan is null)
                this.MealPlan = new MealPlan();

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

    public async Task CreateOrUpdateMealPlan()
    {
        if (IsBusy)
            return;
        IsBusy = true;

        try
        {
            if(MealPlan.Id == 0)
            {
                MealPlan.Id = User.UserId;
                await _cookBookService.CreateMealPlanAsync(this.MealPlan);
            }
            else
            {
                await _cookBookService.UpdateMealPlanAsync(this.MealPlan);
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
}
