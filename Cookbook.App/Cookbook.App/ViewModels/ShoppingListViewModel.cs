using Cookbook.App.Models;
using Cookbook.App.Services;
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
                UnusedItemList.Add(new Item { Id = 5, Name = "Test1", Quantity = "" });
                UnusedItemList.Add(new Item { Id = 6, Name = "Schinken", Quantity = "" });
                UnusedItemList.Add(new Item { Id = 7, Name = "Obst", Quantity = "" });
                UnusedItemList.Add(new Item { Id = 8, Name = "Salami", Quantity = "" });
                UnusedItemList.Add(new Item { Id = 9, Name = "Frosta", Quantity = "6 Packungen" });
                UnusedItemList.Add(new Item { Id = 10, Name = "Honig", Quantity = "1 Glas" });
                UnusedItemList.Add(new Item { Id = 11, Name = "Kartoffeln", Quantity = "" });
                UnusedItemList.Add(new Item { Id = 12, Name = "Nutella", Quantity = "" });
                UnusedItemList.Add(new Item { Id = 13, Name = "Lauch", Quantity = "3 Stück" });
                UnusedItemList.Add(new Item { Id = 14, Name = "Pizza", Quantity = "4 Stück" });
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
        public async Task FilterUnusedItems(string filterString)
        {
            if (filterString is not null)
            {
                List<Item> UnusedItemsSorted = UnusedItemList.OrderBy(x => x.Name).Where(x => x.Name.Contains(filterString)).ToList();
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
    }
}
