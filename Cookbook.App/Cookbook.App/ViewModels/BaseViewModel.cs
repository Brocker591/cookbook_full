using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Cookbook.App.ViewModels;

public partial class BaseViewModel : INotifyPropertyChanged
{
    public bool isBusy;
    public bool IsBusy
    {
        get => isBusy;
        set
        {
            if (isBusy == value)
                return;
            isBusy = value;
            this.IsNotBusy = !isBusy;
            this.OnPropertyChanged();
        }
    }
    public bool isNotBusy;
    public bool IsNotBusy
    {
        get => isNotBusy;
        set
        {
            if (isNotBusy == value)
                return;
            isNotBusy = value;
            this.OnPropertyChanged();
        }
    }

    public bool isLoggedIn;
    public bool IsLoggedIn
    {
        get => isLoggedIn;
        set
        {
            if (isLoggedIn == value)
                return;
            isLoggedIn = value;
            this.IsNotLoggedIn = !isLoggedIn;
            this.OnPropertyChanged();
        }
    }
    public bool isNotLoggedIn = true;
    public bool IsNotLoggedIn
    {
        get => isNotLoggedIn;
        set
        {
            if (isNotLoggedIn == value)
                return;
            isNotLoggedIn = value;
            this.OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public async Task GoPageBack()
    {

        try
        {
            await Shell.Current.Navigation.PopAsync();

        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
    }

    public Task CheckIsLoggedIn()
    {
        if (string.IsNullOrEmpty(Settings.Token))
            IsLoggedIn = false;
        else
            IsLoggedIn = true;

        return Task.CompletedTask;
    }
}
