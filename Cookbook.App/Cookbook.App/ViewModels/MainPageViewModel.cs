using Cookbook.App.Models;
using Cookbook.App.Services;
using System.Windows.Input;


namespace Cookbook.App.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly ICookBookService cookBookService;

        private User user;
        public User User
        {
            get => user;
            set
            {
                if (user == value)
                    return;

                user = value;
                OnPropertyChanged();
            }
        }

        private bool isLoggedIn = false;
        public bool IsLoggedIn
        {
            get => isLoggedIn;
            set
            {
                if (isLoggedIn == value)
                    return;

                isLoggedIn = value;
                OnPropertyChanged();
            }
        }

        private bool isLoggedOut = false;
        public bool IsLoggedOut
        {
            get => isLoggedOut;
            set
            {
                if (isLoggedOut == value)
                    return;

                isLoggedOut = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginUserCommand { private set; get; }
        public ICommand LoadUserCommand { private set; get; }
        public ICommand LogoutUserCommand { private set; get; }

        public MainPageViewModel(ICookBookService cookBookService)
        {
            this.cookBookService = cookBookService;
            LoginUserCommand = new Command(async () => await LoginUserAsync());
            LoadUserCommand = new Command(async () => await LoadUser());
            LogoutUserCommand = new Command(async () => await LogoutUserAsync());

            IsLoggedIn = false;
            IsLoggedOut = true;
        }

        public async Task LoadUser()
        {
            User = new User();
            User.UserName = "admin";
            User.Password = "e8v55pgEaZ7Jpm3";

            if(this.User == null)
                User = new User();

            if (User.UserName != "" && User.Password != "")
                await LoginUserAsync();
        }

        public async Task LoginUserAsync()
        {
            if (IsBusy)
                return;

            try
            {
                User = await cookBookService.GetUserAsync(User);

                if (string.IsNullOrEmpty(User.Token))
                {
                    Settings.Token = "";
                    IsLoggedIn = false;
                    IsLoggedOut = true;
                }
                else
                {
                    Settings.Token = User.Token;
                    IsLoggedIn = true;
                    IsLoggedOut = false;
                }
                IsBusy = false;

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
                IsBusy = false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task LogoutUserAsync()
        {

            IsLoggedIn = false;
            IsLoggedOut = true;

            User.UserName = "";
            User.Password = "";

        }


    }
}
