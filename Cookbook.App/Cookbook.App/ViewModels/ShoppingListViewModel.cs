using Cookbook.App.Models;
using Cookbook.App.Services;
using Cookbook.App.Views;
using System.Collections.ObjectModel;

namespace Cookbook.App.ViewModels
{
    public class ShoppingListViewModel : BaseViewModel
    {
        private readonly ICookBookService _cookBookService;
        public ObservableCollection<Item> ShoppingList { get; } = new();
        public ObservableCollection<Item> UnusedItems { get; set; } = new();
        public List<Item> UnusedItemList { get; set; } = new();
        public Command LoadData { get; set; }
        public Command CrossOutCommand { get; set; }
        public Command GoToCreateItemCommand { get; set; }
        public Command GoToUpdateItemCommand { get; set; }

        private string filterString = "";
        public string FilterString
        {
            get => filterString;
            set
            {
                if (filterString == value)
                    return;

                filterString = value;
                OnPropertyChanged();
            }
        }

        public ShoppingListViewModel(ICookBookService cookBookService)
        {
            _cookBookService = cookBookService;
            LoadData = new Command(async () => await LoadDataAsync());
            CrossOutCommand = new Command<Item>(async (item) => await CrossOutItemFromShoppingListAsync(item));
            GoToCreateItemCommand = new Command(async () => await GoToCreateItemAsync());
            GoToUpdateItemCommand = new Command<Item>(async (item) => await GoToUpdateItemAsync(item));

        }

        private async Task LoadDataAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            ShoppingList.Clear();
            UnusedItemList.Clear();
            try
            {
                var itemList = await _cookBookService.GetInventroy();
                foreach (var item in itemList)
                {
                    ShoppingList.Add(item);
                }
                IsBusy = false;


                //TEST DATEN
                //UnusedItemList.Add(new Item { Id = 5, Name = "Test1", Quantity = "" });
                //UnusedItemList.Add(new Item { Id = 6, Name = "Schinken", Quantity = "" });
                //UnusedItemList.Add(new Item { Id = 7, Name = "Obst", Quantity = "" });
                //UnusedItemList.Add(new Item { Id = 8, Name = "Salami", Quantity = "" });
                //UnusedItemList.Add(new Item { Id = 9, Name = "Frosta", Quantity = "6 Packungen" });
                //UnusedItemList.Add(new Item { Id = 10, Name = "Honig", Quantity = "1 Glas" });
                //UnusedItemList.Add(new Item { Id = 11, Name = "Kartoffeln", Quantity = "" });
                //UnusedItemList.Add(new Item { Id = 12, Name = "Nutella", Quantity = "" });
                //UnusedItemList.Add(new Item { Id = 13, Name = "Lauch", Quantity = "3 Stück" });
                //UnusedItemList.Add(new Item { Id = 14, Name = "Pizza", Quantity = "4 Stück" });
                var allItems = await _cookBookService.GetAllItemsAsync();
                UnusedItemList = allItems.Where(x => x.Inventory == false).ToList();
                FillUnusedItems(UnusedItemList);
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

        public async Task CrossOutItemFromShoppingListAsync(Item item)
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                await _cookBookService.UpdateItemAsync(item);

                ShoppingList.Remove(item);
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


        public async Task FilterUnusedItems(string filterString)
        {
            if (filterString is not null)
            {
                this.FilterString = filterString;
                List<Item> UnusedItemsSorted = UnusedItemList.OrderBy(x => x.Name).Where(x => x.Name.ToLower().Contains(filterString.ToLower())).ToList();
                await FillUnusedItems(UnusedItemsSorted);
            }


        }

        private async Task FillUnusedItems(List<Item> itemList)
        {

            UnusedItems.Clear();

            foreach (var item in itemList)
            {
                UnusedItems.Add(item);
            }
        }

        private async Task GoToCreateItemAsync()
        {
            if(IsBusy)
                return;
            IsBusy = true;

            Item item = new Item();
            item.Name = FilterString;

            try
            {
                await Shell.Current.GoToAsync(nameof(AddOrCreateItemPage), true, new Dictionary<string, object>
                {
                    {"Item", item }
                });
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

        private async Task GoToUpdateItemAsync(Item item)
        {
            if (IsBusy && item is null)
                return;
            IsBusy = true;

            try
            {
                await Shell.Current.GoToAsync(nameof(AddOrCreateItemPage), true, new Dictionary<string, object>
                {
                    {"Item", item }
                });
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
