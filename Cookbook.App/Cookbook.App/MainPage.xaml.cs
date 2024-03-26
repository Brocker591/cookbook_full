using Cookbook.App.ViewModels;

namespace Cookbook.App
{
    public partial class MainPage : ContentPage
    {
        public MainPageViewModel ViewModel;

        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            BindingContext = viewModel;

            ViewModel.LoadUserCommand.Execute(this);
        }

    }

}
