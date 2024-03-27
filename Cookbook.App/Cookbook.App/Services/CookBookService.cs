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

        public async Task<List<Item>> GetInventroy()
        {
            if(string.IsNullOrEmpty(Settings.Token))
                return new List<Item>();

            using var request = new HttpRequestMessage(HttpMethod.Get, "/inventory");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);

            using var response = await Client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var itemsDto = JsonConvert.DeserializeObject<ItemListDto>(json);
                List<Item> items = itemsDto.Items.Select(x => x.ToModel()).ToList();

                return items;
            }
            return new List<Item>();
        }
    }
}
