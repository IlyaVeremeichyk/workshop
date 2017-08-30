using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace PortfolioManagerProxy.Services
{
    public class UserService
    {
        private readonly string _serviceApiUrl = ConfigurationManager.AppSettings["PortfolioManagerServiceUrl"] + "Users";

        private readonly HttpClient _httpClient;

        public UserService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public int CreateUser(string userName)
        {
            var response = _httpClient.PostAsJsonAsync(_serviceApiUrl, userName).Result;
            response.EnsureSuccessStatusCode();
            var result = response.Content.ReadAsAsync<int>().Result;
            return result;
        }
    }
}