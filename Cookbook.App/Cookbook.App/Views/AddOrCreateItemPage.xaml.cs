using Cookbook.App.ViewModels;

namespace Cookbook.App.Views;

public partial class AddOrCreateItemPage : ContentPage
{
    public AddOrCreateItemViewModel ViewModel;
    public AddOrCreateItemPage(AddOrCreateItemViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        BindingContext = ViewModel;
    }
}