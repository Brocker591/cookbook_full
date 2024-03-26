using Cookbook.App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }


        public async Task<UserModel> GetUserAsync(UserModel userModel)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("username", userModel.UserName);
            dict.Add("password", userModel.Password);;
            using var req = new HttpRequestMessage(HttpMethod.Post, "/login") { Content = new FormUrlEncodedContent(dict) };


            using var response = await Client.SendAsync(req);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<UserResponse>(json);

                userModel.Token = userResponse.access_token;
                userModel.ExpireDate = DateTime.Now.AddMinutes(userResponse.expiresIn);
            }
            return userModel;
        }
    }
}
