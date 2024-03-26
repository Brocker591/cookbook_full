using Cookbook.App.Models;

namespace Cookbook.App.Services
{
    public interface ICookBookService
    {
        Task<UserModel> GetUserAsync(UserModel userModel);
    }
}