using Cookbook.App.ViewModels;

namespace Cookbook.App.Views;

public partial class RecipeManagePage : ContentPage
{
    public RecipeManageViewModel ViewModel;
    public RecipeManagePage(RecipeManageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        BindingContext = viewModel;
    }
}