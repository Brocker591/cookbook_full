using Cookbook.App.Models;
using Cookbook.App.Services;
using System.Windows.Input;


namespace Cookbook.App.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly ICookBookService cookBookService;

        private UserModel user;
        public UserModel User
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
            User = new UserModel();
            User.UserName = "admin";
            User.Password = "e8v55pgEaZ7Jpm3";


        }

        public async Task LoginUserAsync()
        {
            User = await cookBookService.GetUserAsync(User);

            if (String.IsNullOrEmpty(User.Token))
            {
                IsLoggedIn = false;
                IsLoggedOut = true;
            }
            else
            {
                IsLoggedIn = true;
                IsLoggedOut = false;
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
