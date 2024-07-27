using Cookbook.App.Models;
using SQLite;

namespace Cookbook.App.Repositories;

public interface IItemToSendRepository
{
    SQLiteAsyncConnection Database { get; set; }

    Task CreateAsync(ItemToSend entity);
    Task<List<ItemToSend>> GetAllAsync();
    Task<ItemToSend> GetAsync(Guid id);
    Task Init();
    Task UpdateAsync(ItemToSend entity);
    Task RemoveAsync(Guid id);
}