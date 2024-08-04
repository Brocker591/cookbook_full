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
        Task<List<Recipe>> GetAllRecipesAsync();
        Task<Recipe> CreateRecipeAsync(Recipe recipe);
        Task UpdateRecipeAsync(Recipe recipe);
        Task AddIngredientToShoplist(int recipeId);
        Task<MealPlan?> GetMealPlanAsync(int userIdd);
        Task<MealPlan> CreateMealPlanAsync(MealPlan mealplan);
        Task UpdateMealPlanAsync(MealPlan mealPlan);

    }
}