using Cookbook.App.Models;
using SQLite;

namespace Cookbook.App.Repositories
{
    public interface IUserRepository
    {
        SQLiteAsyncConnection Database { get; set; }

        Task CreateAsync(User entity);
        Task<User> GetAsync();
        Task Init();
        Task RemoveAsync(Guid id);
        Task UpdateAsync(User entity);
    }
}