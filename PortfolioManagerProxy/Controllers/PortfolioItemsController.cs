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
     
        // GET api/values/5
        public IEnumerable<PortfolioItemModel> Get(int id)
        {
            return this._portfolioItemsService.GetItems(id);
        }

        // POST api/values
        public void Post([FromBody]PortfolioItemModel item)
        {
            this._portfolioItemsService.CreateItem(item);
        }

        // PUT api/values/5
        public void Put([FromBody]PortfolioItemModel value)
        {
            this._portfolioItemsService.UpdateItem(value);
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            this._portfolioItemsService.DeleteItem(id);
        }
    }
}
