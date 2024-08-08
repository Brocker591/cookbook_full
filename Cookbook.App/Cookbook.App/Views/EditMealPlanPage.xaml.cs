using Cookbook.App.ViewModels;

namespace Cookbook.App.Views;

public partial class EditMealPlanPage : ContentPage
{
    public EditMealPlanViewModel ViewModel;
    public EditMealPlanPage(EditMealPlanViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        BindingContext = ViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        ViewModel.LoadDataFromServerCommand.Execute(this);

        ViewModel.CheckIsLoggedIn();
    }
}