using Cookbook.App.Models;
using Cookbook.App.Repositories;
using Cookbook.App.Services;
using System.Windows.Input;


namespace Cookbook.App.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly ICookBookService _cookBookService;
        private readonly IUserRepository _userRepository;

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

        public MainPageViewModel(ICookBookService cookBookService, IUserRepository userRepository)
        {
            _cookBookService = cookBookService;
            _userRepository = userRepository;
            LoginUserCommand = new Command(async () => await LoginUserAsync());
            LoadUserCommand = new Command(async () => await LoadUser());
            LogoutUserCommand = new Command(async () => await LogoutUserAsync());

            IsLoggedIn = false;
            IsLoggedOut = true;

        }

        public async Task LoadUser()
        {
            var user = await _userRepository.GetAsync();

            if (user != null)
                User = user;
            else
                User = new User();

            //User.UserName = "admin";
            //User.Password = "e8v55pgEaZ7Jpm3";


            if (!string.IsNullOrEmpty(User.UserName) && !string.IsNullOrEmpty(User.Password))
                await LoginUserAsync();
        }

        public async Task LoginUserAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (User.ExpireDate != null && User.ExpireDate > DateTime.Now)
                {
                    Settings.Token = User.Token;
                    IsLoggedIn = true;
                    IsLoggedOut = false;
                    IsBusy = false;
                    return;
                }
                else
                {
                    User = await _cookBookService.GetUserAsync(User);
                    if (string.IsNullOrEmpty(User.Token))
                    {
                        Settings.Token = "";
                        IsLoggedIn = false;
                        IsLoggedOut = true;
                        await Shell.Current.DisplayAlert("Error!", "Username oder Password falsch", "OK");
                    }
                    else
                    {
                        Settings.Token = User.Token;
                        IsLoggedIn = true;
                        IsLoggedOut = false;

                        if(User.Id == Guid.Empty)
                            User.Id = Guid.NewGuid();

                        await _userRepository.UpdateAsync(User);
                    }

                }

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
