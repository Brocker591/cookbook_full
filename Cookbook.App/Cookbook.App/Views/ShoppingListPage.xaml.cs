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

        ViewModel.CheckIsLoggedIn();
        if (ViewModel.IsLoggedIn)
        {
            if (ViewModel.FirstStart)
                ViewModel.LoadDataFromServerCommand.Execute(this);
            else
                ViewModel.LoadDataFromDatabaseCommand.Execute(this);
        }
        else
        {
            ViewModel.LoadDataFromDatabaseCommand.Execute(this);
        }
    }

    private async void Entry_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (ViewModel == null)
        {
            return;
        }
        if (MyFilterString is not null && !ViewModel.FirstStart)
        {
            await ViewModel.FilterUnusedItems(MyFilterString.Text);
        }

    }
}