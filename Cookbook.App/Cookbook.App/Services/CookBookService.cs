using Cookbook.App.Dtos;
using Cookbook.App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.App.Services
{
    public class CookBookService : ICookBookService
    {
        HttpClient Client;


        public CookBookService()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(Settings.ApiUrl);
            Client.Timeout = TimeSpan.FromSeconds(10);
        }


        public async Task<User> GetUserAsync(User userModel)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("username", userModel.UserName);
            dict.Add("password", userModel.Password);;
            using var request = new HttpRequestMessage(HttpMethod.Post, "/login") { Content = new FormUrlEncodedContent(dict) };


            using var response = await Client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<UserDto>(json);

                userModel.Token = userResponse.access_token;
                userModel.ExpireDate = DateTime.Now.AddMinutes(userResponse.expiresIn);
            }
            return userModel;
        }

        //public async Task<List<Item>> GetInventroy()
        //{
        //    if(string.IsNullOrEmpty(Settings.Token))
        //        return new List<Item>();

        //    using var request = new HttpRequestMessage(HttpMethod.Get, "/inventory");
        //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);

        //    using var response = await Client.SendAsync(request);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var json = await response.Content.ReadAsStringAsync();
        //        var itemsDto = JsonConvert.DeserializeObject<ItemListDto>(json);
        //        List<Item> items = itemsDto.Items.Select(x => x.ToModel()).ToList();

        //        return items;
        //    }
        //    return new List<Item>();
        //}

        public async Task<List<Item>> GetAllItemsAsync()
        {
            if (string.IsNullOrEmpty(Settings.Token))
                return new List<Item>();

            using var request = new HttpRequestMessage(HttpMethod.Get, "/items");
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

            using var request = new HttpRequestMessage(HttpMethod.Put, "/items");
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


            using var request = new HttpRequestMessage(HttpMethod.Post, "/items");
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
    }
}
