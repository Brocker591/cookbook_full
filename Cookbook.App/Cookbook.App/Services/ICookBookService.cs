using Cookbook.App.Models;

namespace Cookbook.App.Services
{
    public interface ICookBookService
    {
        Task<User> GetUserAsync(User userModel);
        //Task<List<Item>> GetInventroy();
        Task<List<Item>> GetAllItemsAsync();
        Task UpdateItemAsync(Item item);
        Task<Item> CreateItemAsync(Item item);
    }
}