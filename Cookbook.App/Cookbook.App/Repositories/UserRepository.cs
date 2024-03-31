using Cookbook.App.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.App.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseSetting _databaseSetting;

        public UserRepository(DatabaseSetting databaseSetting)
        {
            _databaseSetting = databaseSetting;
        }

        public SQLiteAsyncConnection Database { get; set; }

        public async Task Init()
        {

            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(_databaseSetting.DatabasePath, _databaseSetting.Flags);
            var result = await Database.CreateTableAsync<User>();
        }

        public async Task<User> GetAsync()
        {
            await Init();
            return await Database.Table<User>().FirstOrDefaultAsync();
        }

        public async Task CreateAsync(User entity)
        {
            await Init();

            await Database.InsertAsync(entity);
        }

        public async Task UpdateAsync(User entity)
        {
            await Init();

            var existingEntity = await GetAsync();

            //Services verwendet nur Update
            if (existingEntity is null)
            {
                await CreateAsync(entity);
                return;
            }

            existingEntity.UserName = entity.UserName;
            existingEntity.Password = entity.Password;
            existingEntity.Token = entity.Token;
            existingEntity.ExpireDate = entity.ExpireDate;
            await Database.UpdateAsync(existingEntity);
        }

        public async Task RemoveAsync(Guid id)
        {
            await Init();

            var entity = await Database.Table<User>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
                await Database.DeleteAsync(entity);
        }


    }
}
