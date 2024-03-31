using Cookbook.App.Models;
using SQLite;

namespace Cookbook.App.Repositories
{
    public interface IItemRepository
    {
        SQLiteAsyncConnection Database { get; set; }

        Task CreateAsync(Item entity);
        Task<List<Item>> GetAllAsync();
        Task<Item> GetAsync(int id);
        Task Init();
        Task RemoveAsync(int id);
        Task UpdateAsync(Item entity);
        Task UpdateItemsFromServerAsync(List<Item> items);
    }
}