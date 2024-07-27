using Cookbook.App.Models;
using Cookbook.App.Repositories;
using Cookbook.App.Services;
using Cookbook.App.Views;
using System.Collections.ObjectModel;


namespace Cookbook.App.ViewModels;

public class ShoppingListViewModel : BaseViewModel
{
    private readonly ICookBookService _cookBookService;
    private readonly IItemRepository _itemRepository;
    private readonly IItemToSendRepository _itemToSendRepository;

    private PeriodicTimer? _periodicTimer;

    public ObservableCollection<Item> ShoppingList { get; } = new();
    public ObservableCollection<Item> UnusedItems { get; set; } = new();
    public List<Item> UnusedItemList { get; set; } = new();
    public Command LoadDataFromDatabaseCommand { get; set; }
    public Command LoadDataFromServerCommand { get; set; }
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
    private bool firstStart = true;
    public bool FirstStart
    {
        get => firstStart;
        set
        {
            if (firstStart == value)
                return;

            firstStart = value;
            OnPropertyChanged();
        }
    }

    public ShoppingListViewModel(ICookBookService cookBookService, IItemRepository itemRepository, IItemToSendRepository itemToSendRepository)
    {
        _cookBookService = cookBookService;
        _itemRepository = itemRepository;
        LoadDataFromDatabaseCommand = new Command(async () => await LoadDataFromDatabaseAsync());
        LoadDataFromServerCommand = new Command(async () => await LoadDataFromServerAsync());
        CrossOutCommand = new Command<Item>(async (item) => await CrossOutItemFromShoppingListAsync(item));
        GoToCreateItemCommand = new Command(async () => await GoToCreateItemAsync());
        GoToUpdateItemCommand = new Command<Item>(async (item) => await GoToUpdateItemAsync(item));
        _itemToSendRepository = itemToSendRepository;
        StartTimer();
        //if (FirstStart)
        //{

        //    LoadDataFromServerCommand.Execute(this);
        //    FirstStart = false;
        //}

    }

    public async Task StartTimer()
    {
        _periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(360));

        while (await _periodicTimer.WaitForNextTickAsync())
        {
            try
            {
                var getAllItemsToSend = await _itemToSendRepository.GetAllAsync();

                foreach (var itemToSend in getAllItemsToSend)
                {
                    Item item = new()
                    {
                        Id = itemToSend.ItemId,
                        Name = itemToSend.Name,
                        Priority = itemToSend.Priority,
                        Quantity = itemToSend.Quantity,
                        Inventory = itemToSend.Inventory,
                    };

                    if (itemToSend.ItemId == 0)
                    {
                        var newITem = await _cookBookService.CreateItemAsync(item);

                        if (newITem is not null)
                            await _itemRepository.CreateAsync(newITem);
                    }
                    else
                    {
                        item.Inventory = true;
                        await _cookBookService.UpdateItemAsync(item);
                        await _itemRepository.UpdateAsync(item);
                    }
                    await _itemToSendRepository.RemoveAsync(itemToSend.Id);
                }

                if(getAllItemsToSend is not null)
                    await LoadDataFromServerAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }

        }
    }

    private async Task LoadDataFromDatabaseAsync()
    {
        if (IsBusy)
            return;
        IsBusy = true;

        ShoppingList.Clear();
        UnusedItemList.Clear();
        try
        {
            var itemList = await _itemRepository.GetAllAsync();
            foreach (var item in itemList)
            {
                if (item.Inventory)
                    ShoppingList.Add(item);
                else
                    UnusedItemList.Add(item);

            }
            IsBusy = false;

            await FillUnusedItems(UnusedItemList);

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

    private async Task LoadDataFromServerAsync()
    {
        if (IsBusy)
            return;
        IsBusy = true;

        try
        {
            var itemList = await _cookBookService.GetAllItemsAsync();
            await _itemRepository.UpdateItemsFromServerAsync(itemList);
            FirstStart = false;
            IsBusy = false;
            await LoadDataFromDatabaseAsync();


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
            item.Inventory = false;
            await _cookBookService.UpdateItemAsync(item);
            await _itemRepository.UpdateAsync(item);
            IsBusy = false;
            await LoadDataFromDatabaseAsync();
            //ShoppingList.Remove(item);

        }
        catch (HttpRequestException)
        {
            await _itemToSendRepository.CreateAsync(new ItemToSend
            {
                Id = Guid.NewGuid(),
                ItemId = item.Id,
                Name = item.Name,
                Quantity = item.Quantity,
                Priority = item.Priority,
                Inventory = item.Inventory,
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


    public async Task FilterUnusedItems(string filterString)
    {
        if (filterString is not null)
        {
            if (filterString != "")
            {
                this.FilterString = filterString;
                List<Item> UnusedItemsSorted = UnusedItemList.OrderBy(x => x.Name).Where(x => x.Name.ToLower().Contains(filterString.ToLower())).ToList();
                await FillUnusedItems(UnusedItemsSorted);
            }
            else
            {
                await FillUnusedItems(UnusedItemList);
            }
        }

    }


    private async Task FillUnusedItems(List<Item> itemList)
    {

        UnusedItems.Clear();

        var sortetItemList = itemList.OrderByDescending(x => x.Priority).ToList();

        foreach (var item in sortetItemList)
        {
            UnusedItems.Add(item);
        }
    }

    private async Task GoToCreateItemAsync()
    {
        if (IsBusy)
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
