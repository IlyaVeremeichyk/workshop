using PortfolioManagerProxy.Models;
using PortfolioManagerProxy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PortfolioManagerProxy.Controllers
{
    public class PortfolioItemsController : ApiController
    {
        private readonly PortfolioItemsService _portfolioItemsService = new PortfolioItemsService();
     
        public IEnumerable<PortfolioItemModel> Get(int id)
        {
            return this._portfolioItemsService.GetItems(id);
        }
        
        public void Post([FromBody]PortfolioItemModel item)
        {
            this._portfolioItemsService.CreateItem(item);
        }
        
        public void Put([FromBody]PortfolioItemModel value)
        {
            this._portfolioItemsService.UpdateItem(value);
        }
        
        public void Delete(int id)
        {
            this._portfolioItemsService.DeleteItem(id);
        }

        [Route("api/portfolioitems/GetSynchronizedData/{id}")]
        public IEnumerable<PortfolioItemModel> GetSynchronizedData(int id)
        {
            return this._portfolioItemsService.GetSynchronizedItems(id);
        }
    }
}
