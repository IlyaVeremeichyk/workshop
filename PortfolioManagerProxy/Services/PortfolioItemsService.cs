using Newtonsoft.Json;
using PortfolioManagerProxy.Models;
using PortfolioManagerProxy.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
            return data;
        }

        /// <summary>
        /// Creates a portfolio item. UserId is taken from the model.
        /// </summary>
        /// <param name="item">The portfolio item to create.</param>
        public void CreateItem(PortfolioItemModel item)
        {
            _cloudRepository.CreateItem(item)
                .ContinueWith(prev =>
                {
                    Debug.WriteLine("===>>>Create item faulted with exception\n" + prev.Exception.ToString());
                    _repository.DeleteItem(item.ItemId);
                }, TaskContinuationOptions.OnlyOnFaulted);
            _repository.AddItem(item);
        }

        /// <summary>
        /// Updates a portfolio item.
        /// </summary>
        /// <param name="item">The portfolio item to update.</param>
        public void UpdateItem(PortfolioItemModel item)
        {
            var oldItem = _repository.GetItem(item.ItemId);
            _cloudRepository.UpdateItem(item)
                .ContinueWith(prev =>
                {
                    Debug.WriteLine("===>>>Update item faulted with exception\n" + prev.Exception.ToString());
                    _repository.UpdateItem(oldItem);
                }, TaskContinuationOptions.OnlyOnFaulted);
            _repository.UpdateItem(item);
        }

        /// <summary>
        /// Deletes a portfolio item.
        /// </summary>
        /// <param name="id">The portfolio item Id to delete.</param>
        public void DeleteItem(int id)
        {
            var oldItem = _repository.GetItem(id);
            _cloudRepository.DeleteItem(id)
                .ContinueWith(prev =>
                {
                    Debug.WriteLine("===>>>Delete item faulted with exception\n" + prev.Exception.ToString());
                    _repository.AddItem(oldItem);
                }, TaskContinuationOptions.OnlyOnFaulted);
            _repository.DeleteItem(id);
        }

        /// <summary>
        /// Gets the synchronized portfolio items for the user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The list of synchronized portfolio items.</returns>
        public IList<PortfolioItemModel> GetSynchronizedItems(int userId)
        {
            var data = _cloudRepository.GetItems(userId).Result;
            _repository.UpdateUser(userId, data);
            return data;
        }
    }
}
