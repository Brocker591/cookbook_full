using Cookbook.App.ViewModels;

namespace Cookbook.App.Views;

public partial class MealPlanPage : ContentPage
{
    public MealPlanViewModel ViewModel;
    public MealPlanPage(MealPlanViewModel viewModel)
	{

        InitializeComponent();
        ViewModel = viewModel;
        BindingContext = ViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        ViewModel.LoadDataCommand.Execute(this);

        ViewModel.CheckIsLoggedIn();
    }
}