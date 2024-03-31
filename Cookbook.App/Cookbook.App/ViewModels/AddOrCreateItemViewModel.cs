using Cookbook.App.Models;
using Cookbook.App.Repositories;
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
        private readonly IItemRepository _itemRepository;
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


        public AddOrCreateItemViewModel(ICookBookService cookBookService, IItemRepository itemRepository)
        {
            _cookBookService = cookBookService;
            _itemRepository = itemRepository;
            AddOrCreateItemCommand = new Command(async () => await AddOrCreateItemAsync());
            
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
                    var newITem = await _cookBookService.CreateItemAsync(Item);

                    if (newITem is not null)
                        await _itemRepository.CreateAsync(newITem);
                }
                else
                {
                    Item.Inventory = true;
                    await _cookBookService.UpdateItemAsync(Item);
                    await _itemRepository.UpdateAsync(Item);
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
