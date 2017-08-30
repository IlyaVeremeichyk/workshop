using PortfolioManagerProxy.Models;
using PortfolioManagerProxy.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioManagerProxy.Repositories
{
    public class PortfolioItemsDatabaseRepository
    {
        private readonly PortfolioManagerContext _context;

        public PortfolioItemsDatabaseRepository()
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

        public void UpdateItem(PortfolioItemModel model)
        {
            var origin = _context.PortfolioItems.Find(model.ItemId);
            UpdateOriginalItem(origin, model);
            _context.SaveChanges();
        }

        public void DeleteItem(int itemId)
        {
            _context.PortfolioItems.Remove(_context.PortfolioItems.Find(itemId));
            _context.SaveChanges();
        }

        public void UpdateUser(int userId, IEnumerable<PortfolioItemModel> items)
        {
            foreach (var item in _context.PortfolioItems.Where(m => m.UserId == userId))
            {
                _context.PortfolioItems.Remove(item);
            }
            foreach (var item in items)
            {
                _context.PortfolioItems.Add(item);
            }
            _context.SaveChanges();
        }

        private void UpdateOriginalItem(PortfolioItemModel origin, PortfolioItemModel newItem) {
            origin.SharesNumber = newItem.SharesNumber;
            origin.Symbol = newItem.Symbol;
            origin.UserId = newItem.UserId;
        }
    }
}