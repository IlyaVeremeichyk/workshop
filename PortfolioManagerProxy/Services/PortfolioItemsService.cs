using Newtonsoft.Json;
using PortfolioManagerProxy.Models;
using PortfolioManagerProxy.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace PortfolioManagerProxy.Services
{
    /// <summary>
    /// Works with portfolio backend.
    /// </summary>
    public class PortfolioItemsService
    {
        private readonly HttpClient _httpClient;

        private readonly PortfolioItemsDatabaseRepository _repository;

        private readonly CloudSynchronizationRepository _cloudRepository;

        /// <summary>
        /// Creates the service.
        /// </summary>
        public PortfolioItemsService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _repository = new PortfolioItemsDatabaseRepository();
            _cloudRepository = new CloudSynchronizationRepository();
        }

        /// <summary>
        /// Gets all portfolio items for the user.
        /// </summary>
        /// <param name="userId">The User Id.</param>
        /// <returns>The list of portfolio items.</returns>
        public IList<PortfolioItemModel> GetItems(int userId)
        {
            var data = _repository.GetItems(userId).ToList();
            UpdateItems(userId);
            return data;
        }

        /// <summary>
        /// Creates a portfolio item. UserId is taken from the model.
        /// </summary>
        /// <param name="item">The portfolio item to create.</param>
        public void CreateItem(PortfolioItemModel item)
        {
            _cloudRepository.CreateItem(item).ContinueWith(prev => UpdateItems(item.UserId));
            _repository.AddItem(item);
        }

        /// <summary>
        /// Updates a portfolio item.
        /// </summary>
        /// <param name="item">The portfolio item to update.</param>
        public void UpdateItem(PortfolioItemModel item)
        {
            _cloudRepository.UpdateItem(item).ContinueWith(prev => UpdateItems(item.UserId));
            _repository.UpdateItem(item);
        }

        /// <summary>
        /// Deletes a portfolio item.
        /// </summary>
        /// <param name="id">The portfolio item Id to delete.</param>
        public void DeleteItem(int id)
        {
            var userId = _repository.GetItem(id).UserId;
            _cloudRepository.DeleteItem(id).ContinueWith(prev => UpdateItems(userId));
            _repository.DeleteItem(id);
        }

        private async Task UpdateItems(int userId) {
            await _cloudRepository.GetItems(userId)
                .ContinueWith(prev => _repository.UpdateUser(userId, prev.Result));
        }
    }
}
