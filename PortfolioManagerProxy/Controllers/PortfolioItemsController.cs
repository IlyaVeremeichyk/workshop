using PortfolioManagerProxy.Models;
using PortfolioManagerProxy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PortfolioManagerProxy.Controllers
{
    [EnableCors("*", "*", "*")]
    public class PortfolioItemsController : ApiController
    {
        private readonly SharesService _sharesService = new SharesService();
     
        public IEnumerable<Share> Get(int userId)
        {
            return _sharesService.GetItems(userId);
        }
        
        //public void Post([FromBody]Share item)
        //{
        //    this._sharesService.CreateItem(item);
        //}

        [HttpPost]
        public void SetUserShare(int userId, string isin, int count)
        {
            _sharesService.SetUserShare(userId, isin, count);
        }

        //public void Delete(int id)
        //{
        //    this._sharesService.DeleteItem(id);
        //}
    }
}
