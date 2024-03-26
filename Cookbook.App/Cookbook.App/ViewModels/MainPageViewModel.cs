using Cookbook.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Cookbook.App.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
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

        public ICommand LoginUserCommand { private set; get; }
        public ICommand LoadUserCommand { private set; get; }

        public MainPageViewModel()
        {
            LoginUserCommand = new Command(async () => await LoginUserAsync());
            LoadUserCommand = new Command(async () => await LoadUser());
            
        }

        public async Task LoadUser()
        {
            User = new UserModel();
            User.UserName = "admin";
            User.Password = "e8v55pgEaZ7Jpm3";
            

        }

        public async Task LoginUserAsync()
        {
            await Task.CompletedTask;
        }


    }
}
