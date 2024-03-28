using Cookbook.App.ViewModels;

namespace Cookbook.App.Views;

public partial class ShoppingListPage : ContentPage
{
    public ShoppingListViewModel ViewModel;
    public ShoppingListPage(ShoppingListViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        BindingContext = ViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        ViewModel.LoadData.Execute(this);
    }

    private async void Entry_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (MyFilterString is not null)
        {
            await ViewModel.FilterUnusedItems(MyFilterString.Text);
        }

    }
}