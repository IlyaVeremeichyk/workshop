using Newtonsoft.Json;
using PortfolioManagerProxy.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace PortfolioManagerProxy.Services
{
    /// <summary>
    /// Works with portfolio backend.
    /// </summary>
    public class PortfolioItemsService
    {
        /// <summary>
        /// The url for getting all portfolio items.
        /// </summary>
        private const string GetAllUrl = "PortfolioItems?userId={0}";

        /// <summary>
        /// The url for updating a portfolio item.
        /// </summary>
        private const string UpdateUrl = "PortfolioItems";

        /// <summary>
        /// The url for a portfolio item's creation.
        /// </summary>
        private const string CreateUrl = "PortfolioItems";

        /// <summary>
        /// The url for a portfolio item's deletion.
        /// </summary>
        private const string DeleteUrl = "PortfolioItems/{0}";

        /// <summary>
        /// The service URL.
        /// </summary>
        private readonly string _serviceApiUrl = ConfigurationManager.AppSettings["PortfolioManagerServiceUrl"];

        private readonly HttpClient _httpClient;

        /// <summary>
        /// Creates the service.
        /// </summary>
        public PortfolioItemsService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Gets all portfolio items for the user.
        /// </summary>
        /// <param name="userId">The User Id.</param>
        /// <returns>The list of portfolio items.</returns>
        public IList<PortfolioItemModel> GetItems(int userId)
        {
            var dataAsString = _httpClient.GetStringAsync(string.Format(_serviceApiUrl + GetAllUrl, userId)).Result;
            return JsonConvert.DeserializeObject<IList<PortfolioItemModel>>(dataAsString);
        }

        /// <summary>
        /// Creates a portfolio item. UserId is taken from the model.
        /// </summary>
        /// <param name="item">The portfolio item to create.</param>
        public void CreateItem(PortfolioItemModel item)
        {
            _httpClient.PostAsJsonAsync(_serviceApiUrl + CreateUrl, item)
                .Result.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Updates a portfolio item.
        /// </summary>
        /// <param name="item">The portfolio item to update.</param>
        public void UpdateItem(PortfolioItemModel item)
        {
            _httpClient.PutAsJsonAsync(_serviceApiUrl + UpdateUrl, item)
                .Result.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Deletes a portfolio item.
        /// </summary>
        /// <param name="id">The portfolio item Id to delete.</param>
        public void DeleteItem(int id)
        {
            _httpClient.DeleteAsync(string.Format(_serviceApiUrl + DeleteUrl, id))
                .Result.EnsureSuccessStatusCode();
        }
    }
}
