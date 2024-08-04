using Cookbook.App.Dtos;
using Cookbook.App.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;


namespace Cookbook.App.Services;

public class CookBookService : ICookBookService
{
    HttpClient Client;


    public CookBookService()
    {
        Client = new HttpClient();
        Client.Timeout = TimeSpan.FromSeconds(10);
    }


    public async Task<User> GetUserAsync(User userModel)
    {
        var dict = new Dictionary<string, string>();
        dict.Add("username", userModel.UserName);
        dict.Add("password", userModel.Password);;
        using var request = new HttpRequestMessage(HttpMethod.Post, Settings.ApiUrl + "/login") { Content = new FormUrlEncodedContent(dict) };


        using var response = await Client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var userResponse = JsonConvert.DeserializeObject<UserDto>(json);

            userModel.UserId = userResponse.UserId;
            userModel.Token = userResponse.access_token;
            userModel.ExpireDate = DateTime.Now.AddMinutes(userResponse.expiresIn);
        }
        return userModel;
    }


    public async Task<List<Item>> GetAllItemsAsync()
    {
        if (string.IsNullOrEmpty(Settings.Token))
            return new List<Item>();

        using var request = new HttpRequestMessage(HttpMethod.Get, Settings.ApiUrl + "/items");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);

        using var response = await Client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var itemsDto = JsonConvert.DeserializeObject<List<ItemDto>>(json);
            List<Item> items = itemsDto.Select(x => x.ToModel()).ToList();

            return items;
        }
        return new List<Item>();
    }

    public async Task UpdateItemAsync(Item item)
    {
        if (string.IsNullOrEmpty(Settings.Token))
            return;
        var itemUpdateDto = new ItemUpdateDto
        {
            id = item.Id,
            name = item.Name,
            quantity = item.Quantity,
            priority = item.Priority,
            inventory = item.Inventory
        };

        using var request = new HttpRequestMessage(HttpMethod.Put, Settings.ApiUrl + "/items");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);

        request.Content = new StringContent(JsonConvert.SerializeObject(itemUpdateDto), Encoding.UTF8, "application/json");


        using var response = await Client.SendAsync(request);
        if (response.IsSuccessStatusCode)
            return;
        else
            throw new HttpRequestException("Error updating item");
    }

    public async Task<Item> CreateItemAsync(Item item)
    {
        if (string.IsNullOrEmpty(Settings.Token))
            throw new HttpRequestException("No Token");
           
            
        var itemCreateDto = new ItemCreateDto
        {
            name = item.Name,
            quantity = item.Quantity,
        };


        using var request = new HttpRequestMessage(HttpMethod.Post, Settings.ApiUrl + "/items");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);

        request.Content = new StringContent(JsonConvert.SerializeObject(itemCreateDto), Encoding.UTF8, "application/json");


        using var response = await Client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var itemDto = JsonConvert.DeserializeObject<ItemDto>(json);
            return itemDto.ToModel();
        }
        else
            throw new HttpRequestException("Error creating item");

    }

    public async Task<List<Recipe>> GetAllRecipesAsync()
    {
        if (string.IsNullOrEmpty(Settings.Token))
            throw new HttpRequestException("No Token");

        using var request = new HttpRequestMessage(HttpMethod.Get, Settings.ApiUrl + "/recipes");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);

        using var response = await Client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var recipesDto = JsonConvert.DeserializeObject<List<RecipeDto>>(json);
            List<Recipe> recipes = recipesDto.Select(x => x.ToModel()).ToList();

            return recipes;
        }
        else
            throw new HttpRequestException("Error getting recipes");

    }

    public async Task<Recipe> CreateRecipeAsync(Recipe recipe)
    {
        if (string.IsNullOrEmpty(Settings.Token))
            throw new HttpRequestException("No Token");

        var recipeCreateDto = recipe.ToCreateDto();

        using var request = new HttpRequestMessage(HttpMethod.Post, Settings.ApiUrl + "/recipes");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);

        request.Content = new StringContent(JsonConvert.SerializeObject(recipeCreateDto), Encoding.UTF8, "application/json");

        using var response = await Client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var recipeDto = JsonConvert.DeserializeObject<RecipeDto>(json);
            return recipeDto.ToModel();
        }
        else
            throw new HttpRequestException("Error creating recipe");
    }

    public async Task UpdateRecipeAsync(Recipe recipe)
    {
        if (string.IsNullOrEmpty(Settings.Token))
            throw new HttpRequestException("No Token");

        var recipeUpdateDto = recipe.ToUpdateDto();

        using var request = new HttpRequestMessage(HttpMethod.Put, Settings.ApiUrl + "/recipes");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);

        request.Content = new StringContent(JsonConvert.SerializeObject(recipeUpdateDto), Encoding.UTF8, "application/json");

        using var response = await Client.SendAsync(request);
        if (response.IsSuccessStatusCode)
            return;
        else
            throw new HttpRequestException("Error updating recipe");
    }
    
    public async Task AddIngredientToShoplist(int recipeId)
    {
        if (string.IsNullOrEmpty(Settings.Token))
            throw new HttpRequestException("No Token");

        using var request = new HttpRequestMessage(HttpMethod.Post, Settings.ApiUrl + $"/recipes/shoplist/{recipeId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);

        using var response = await Client.SendAsync(request);
        if (response.IsSuccessStatusCode)
            return;
        else
            throw new HttpRequestException("Error updating recipe");
    }

    public async Task<MealPlan?> GetMealPlanAsync(int userId)
    {
        if (string.IsNullOrEmpty(Settings.Token))
            return null;

        using var request = new HttpRequestMessage(HttpMethod.Get, Settings.ApiUrl + $"/mealplans/{userId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);

        using var response = await Client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<MealPlanDto>(json);
            return dto.ToModel();
        }
        return null;
    }


    public async Task<MealPlan> CreateMealPlanAsync(MealPlan mealplan)
    {
        if (string.IsNullOrEmpty(Settings.Token))
            throw new HttpRequestException("No Token");


        using var request = new HttpRequestMessage(HttpMethod.Post, Settings.ApiUrl + "/mealplans");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);

        request.Content = new StringContent(JsonConvert.SerializeObject(mealplan.ToMealPlanDto()), Encoding.UTF8, "application/json");


        using var response = await Client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<MealPlanDto>(json);
            return dto.ToModel();
        }
        else
            throw new HttpRequestException("Error creating MealPlan");

    }


    public async Task UpdateMealPlanAsync(MealPlan mealPlan)
    {
        if (string.IsNullOrEmpty(Settings.Token))
            return;


        using var request = new HttpRequestMessage(HttpMethod.Put, Settings.ApiUrl + "/mealplans");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);

        request.Content = new StringContent(JsonConvert.SerializeObject(mealPlan.ToMealPlanDto()), Encoding.UTF8, "application/json");


        using var response = await Client.SendAsync(request);
        if (response.IsSuccessStatusCode)
            return;
        else
            throw new HttpRequestException("Error updating MealPlan");
    }
}
