using Newtonsoft.Json;
using PortfolioManagerProxy.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace PortfolioManagerProxy.Repositories
{
    public class CloudSynchronizationRepository
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
        public CloudSynchronizationRepository()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Gets all portfolio items for the user.
        /// </summary>
        /// <param name="userId">The User Id.</param>
        /// <returns>The list of portfolio items.</returns>
        public Task<IList<PortfolioItemModel>> GetItems(int userId)
        {
            return Task.Run(() =>
            {
                var dataAsString = _httpClient.GetStringAsync(string.Format(_serviceApiUrl + GetAllUrl, userId)).Result;
                return JsonConvert.DeserializeObject<IList<PortfolioItemModel>>(dataAsString);
            });
        }

        /// <summary>
        /// Creates a portfolio item. UserId is taken from the model.
        /// </summary>
        /// <param name="item">The portfolio item to create.</param>
        public async Task CreateItem(PortfolioItemModel item)
        {
            (await ItemActionTry(_httpClient.PostAsJsonAsync(_serviceApiUrl + CreateUrl, item), 3))
                .EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Updates a portfolio item.
        /// </summary>
        /// <param name="item">The portfolio item to update.</param>
        public async Task UpdateItem(PortfolioItemModel item)
        {
            (await ItemActionTry(_httpClient.PutAsJsonAsync(_serviceApiUrl + UpdateUrl, item), 3))
                .EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Deletes a portfolio item.
        /// </summary>
        /// <param name="id">The portfolio item Id to delete.</param>
        public async Task DeleteItem(int id)
        {
            (await ItemActionTry(_httpClient.DeleteAsync(string.Format(_serviceApiUrl + DeleteUrl, id)), 3))
                .EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Try to make an action a <see cref="triesRemaining"/> times.
        /// </summary>
        /// <param name="task">The task to try.</param>
        /// <param name="triesRemaining">The tries remaining.</param>
        /// <returns>Action response.</returns>
        private async Task<HttpResponseMessage> ItemActionTry(Task<HttpResponseMessage> task, int triesRemaining)
        {
            try
            {
                return await task;
            }
            catch
            {
                if (triesRemaining < 0)
                {
                    throw;
                }

                else
                {
                    return await ItemActionTry(task, triesRemaining - 1);
                }
            }
        }
    }
}