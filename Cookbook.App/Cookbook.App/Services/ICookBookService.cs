﻿using Cookbook.App.Models;

namespace Cookbook.App.Services
{
    public interface ICookBookService
    {
        Task<User> GetUserAsync(User userModel);
        Task<List<Item>> GetInventroy();
        Task UpdateItemAsync(Item item, bool isInventory = false);
    }
}