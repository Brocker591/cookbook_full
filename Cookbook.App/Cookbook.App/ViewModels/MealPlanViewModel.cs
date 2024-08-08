using Cookbook.App.Models;
using Cookbook.App.PageModels;
using Cookbook.App.Repositories;
using Cookbook.App.Services;
using Cookbook.App.Views;
using System.Collections.ObjectModel;


namespace Cookbook.App.ViewModels;

public class MealPlanViewModel : BaseViewModel
{
    private readonly ICookBookService _cookBookService;
    private readonly IUserRepository _userRepository;
    public Command LoadDataCommand { get; set; }
    public Command SaveMealPlanCommand { get; set; }
    public Command MealPlanDetailsCommand { get; set; }

    public User User { get; set; }

    public ObservableCollection<MealPlanPageModel> MealPlanPageModelList { get; set; }

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
        MealPlanDetailsCommand = new Command(async (mealPlanPageModel) => await MealPlanDetails((MealPlanPageModel)mealPlanPageModel));
        MealPlanPageModelList = new ObservableCollection<MealPlanPageModel>();
    }

    private async Task LoadDataAsync()
    {
        if (IsBusy)
            return;
        IsBusy = true;

        try
        {
            MealPlanPageModelList.Clear();
            this.User = await _userRepository.GetAsync();

            if (this.User is null)
                throw new Exception("Es Konnte kein User geladen werden.");

            this.MealPlan = await _cookBookService.GetMealPlanAsync(this.User.UserId);

            if (this.MealPlan is null)
                this.MealPlan = new MealPlan();

            MealPlanPageModel monday = new() { DayName = nameof(MealPlan.Monday), MealName = MealPlan.Monday };
            MealPlanPageModel tuesday = new() { DayName = nameof(MealPlan.Tuesday), MealName = MealPlan.Tuesday };
            MealPlanPageModel wednesday = new() { DayName = nameof(MealPlan.Wednesday), MealName = MealPlan.Wednesday };
            MealPlanPageModel thursday = new() { DayName = nameof(MealPlan.Thursday), MealName = MealPlan.Thursday };
            MealPlanPageModel friday = new() { DayName = nameof(MealPlan.Friday), MealName = MealPlan.Friday };
            MealPlanPageModel saturday = new() { DayName = nameof(MealPlan.Saturday), MealName = MealPlan.Saturday };
            MealPlanPageModel Sunday = new() { DayName = nameof(MealPlan.Sunday), MealName = MealPlan.Sunday };

            MealPlanPageModelList.Add(monday);
            MealPlanPageModelList.Add(tuesday);
            MealPlanPageModelList.Add(wednesday);
            MealPlanPageModelList.Add(thursday);
            MealPlanPageModelList.Add(friday);
            MealPlanPageModelList.Add(saturday);
            MealPlanPageModelList.Add(Sunday);


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

    public async Task MealPlanDetails(MealPlanPageModel mealPlanPageModel)
    {
        await Shell.Current.GoToAsync(nameof(EditMealPlanPage), true, new Dictionary<string, object>
        {
            {nameof(MealPlanPageModel), mealPlanPageModel }
        });
    }
}
