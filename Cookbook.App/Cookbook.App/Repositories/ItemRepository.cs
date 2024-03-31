using Cookbook.App.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.App.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly DatabaseSetting _databaseSetting;

        public ItemRepository(DatabaseSetting databaseSetting)
        {
            _databaseSetting = databaseSetting;
        }

        public SQLiteAsyncConnection Database { get; set; }

        public async Task Init()
        {

            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(_databaseSetting.DatabasePath, _databaseSetting.Flags);
            var result = await Database.CreateTableAsync<Item>();
        }

        public async Task<Item> GetAsync(int id)
        {
            await Init();
            return await Database.Table<Item>().FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<List<Item>> GetAllAsync()
        {
            await Init();
            return await Database.Table<Item>().ToListAsync();
        }

        public async Task CreateAsync(Item entity)
        {
            await Init();

            await Database.InsertAsync(entity);
        }

        public async Task UpdateAsync(Item entity)
        {
            await Init();

            var existingEntity = await GetAsync(entity.Id);

            if (existingEntity is null)
                return;

            existingEntity.Name = entity.Name;
            existingEntity.Quantity = entity.Quantity;
            existingEntity.Priority = entity.Priority;
            existingEntity.Inventory = entity.Inventory;

            await Database.UpdateAsync(existingEntity);
        }

        public async Task RemoveAsync(int id)
        {
            await Init();

            var entity = await GetAsync(id);

            if (entity is null)
                return;

            await Database.DeleteAsync(entity);
        }

        public async Task UpdateItemsFromServerAsync(List<Item> items)
        {
            await Init();

            var localItems = await GetAllAsync();

            foreach (var item in items)
            {
                var localItem = localItems.FirstOrDefault(x => x.Id == item.Id);

                if (localItem is null)
                {
                    await CreateAsync(item);
                    continue;
                }

                localItem.Name = item.Name;
                localItem.Quantity = item.Quantity;
                localItem.Priority = item.Priority;
                localItem.Inventory = item.Inventory;

                await UpdateAsync(localItem);
            }

            foreach (var item in localItems)
            {
                if (items.All(x => x.Id != item.Id))
                    await RemoveAsync(item.Id);
            }
        }
    }
}
