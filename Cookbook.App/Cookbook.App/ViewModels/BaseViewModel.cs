using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.App.ViewModels
{
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
    }
}
