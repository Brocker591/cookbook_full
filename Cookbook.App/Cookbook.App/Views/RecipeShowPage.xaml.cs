using Cookbook.App.ViewModels;

namespace Cookbook.App.Views;

public partial class RecipeShowPage : ContentPage
{
    public RecipeShowViewModel ViewModel;
    public RecipeShowPage(RecipeShowViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        BindingContext = viewModel;
    }
}