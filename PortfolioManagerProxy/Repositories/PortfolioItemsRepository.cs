using PortfolioManagerProxy.Models;
using PortfolioManagerProxy.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioManagerProxy.Repositories
{
    public class PortfolioItemsRepository
    {
        private readonly PortfolioManagerContext _context;

        public PortfolioItemsRepository()
        {
            _context = new PortfolioManagerContext();
        }

        public IQueryable<PortfolioItemModel> GetItems(int userId)
        {
            return _context.PortfolioItems.Where(m => m.UserId == userId);
        }

        public void AddItem(PortfolioItemModel model)
        {
            _context.PortfolioItems.Add(model);
            _context.SaveChanges();
        }
    }
}