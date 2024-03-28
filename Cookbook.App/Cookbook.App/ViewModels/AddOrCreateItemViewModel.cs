using Cookbook.App.Models;
using Cookbook.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.App.ViewModels
{
    [QueryProperty(nameof(Item), "Item")]
    public class AddOrCreateItemViewModel : BaseViewModel
    {
        private readonly ICookBookService _cookBookService;
        private Item item;
        public Item Item
        {
            get => item;
            set
            {
                if (item == value)
                    return;

                item = value;
                OnPropertyChanged();
            }
        }

        public Command AddOrCreateItemCommand { get; set; }


        public AddOrCreateItemViewModel(ICookBookService cookBookService)
        {
            _cookBookService = cookBookService;
            AddOrCreateItemCommand = new Command(async() => await AddOrCreateItemAsync());
        }

        public async Task AddOrCreateItemAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                if (Item.Id == 0)
                {
                    //TODO: Wenn Prüfung stattgefunden hat, muss mit den Items in der Datenbank verglichen werden ob es schon existiert

                    await _cookBookService.CreateItemAsync(Item);
                }
                else
                {
                    await _cookBookService.UpdateItemAsync(Item, isInventory: true);
                }
                IsBusy = false;
                await Shell.Current.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
