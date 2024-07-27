using Cookbook.App.Models;
using SQLite;

namespace Cookbook.App.Repositories;

public class ItemToSendRepository : IItemToSendRepository
{
    private readonly DatabaseSetting _databaseSetting;

    public ItemToSendRepository(DatabaseSetting databaseSetting)
    {
        _databaseSetting = databaseSetting;
    }

    public SQLiteAsyncConnection Database { get; set; }

    public async Task Init()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(_databaseSetting.DatabasePath, _databaseSetting.Flags);
        var result = await Database.CreateTableAsync<ItemToSend>();
    }

    public async Task<ItemToSend> GetAsync(Guid id)
    {
        await Init();
        return await Database.Table<ItemToSend>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<ItemToSend>> GetAllAsync()
    {
        await Init();
        return await Database.Table<ItemToSend>().ToListAsync();
    }

    public async Task CreateAsync(ItemToSend entity)
    {
        await Init();

        await Database.InsertAsync(entity);
    }

    public async Task UpdateAsync(ItemToSend entity)
    {
        await Init();

        var existingEntity = await GetAsync(entity.Id);

        if (existingEntity is null)
            return;

        existingEntity.ItemId = entity.ItemId;
        existingEntity.Name = entity.Name;
        existingEntity.Quantity = entity.Quantity;
        existingEntity.Priority = entity.Priority;
        existingEntity.Inventory = entity.Inventory;

        await Database.UpdateAsync(existingEntity);
    }

    public async Task RemoveAsync(Guid id)
    {
        await Init();

        var entity = await Database.Table<ItemToSend>().FirstOrDefaultAsync(x => x.Id == id);
        if (entity != null)
            await Database.DeleteAsync(entity);
    }
}
