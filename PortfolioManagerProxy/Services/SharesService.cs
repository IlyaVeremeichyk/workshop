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
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;

namespace PortfolioManagerProxy.Services
{
    /// <summary>
    /// Works with portfolio backend.
    /// </summary>
    public class SharesService
    {
        private readonly SharesRepository _repository;
        private readonly UsersRepository _usersRepository;
        private readonly UsersPortfolioRepository _usersPortfolioRepository;

        /// <summary>
        /// Creates the service.
        /// </summary>
        public SharesService()
        {
            _repository = new SharesRepository();
            _usersRepository = new UsersRepository();
            _usersPortfolioRepository = new UsersPortfolioRepository();
        }

        /// <summary>
        /// Gets all portfolio items for the user.
        /// </summary>
        /// <param name="userId">The User Id.</param>
        /// <returns>The list of portfolio items.</returns>
        public IList<Share> GetItems(int userId)
        {
            var data = _repository.GetItems(userId).ToList();
            return data;
        }
        
        //public void CreateItem(Share item)
        //{
        //    _repository.AddItem(item);
        //}
        
        //public void UpdateItem(Share item)
        //{
        //    var oldItem = _repository.GetItem(item.ItemId);
        //    _repository.UpdateItem(item);
        //}
        
        //public void DeleteItem(int id)
        //{
        //    var oldItem = _repository.GetItem(id);
        //    _repository.DeleteItem(id);
        //}
        
        public void SetUserShare(int userId, string isin, int count)
        {
            var share = _repository.GetById(isin);
            var user = _usersRepository.GetById(userId);
            var userShare = new UsersPortfolio()
            {
                Share = share,
                User = user,
                SharesNumber = count
            };
            
            _usersPortfolioRepository.Add(userShare);
        }
    }
}
