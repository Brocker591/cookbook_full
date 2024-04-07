using Cookbook.App.ViewModels;

namespace Cookbook.App.Views;

public partial class RecipeListPage : ContentPage
{
    public RecipeListViewModel ViewModel;
    public RecipeListPage(RecipeListViewModel viewModel)
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