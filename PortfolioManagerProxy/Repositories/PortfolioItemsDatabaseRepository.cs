using PortfolioManagerProxy.Models;
using PortfolioManagerProxy.Models.Comparers;
using PortfolioManagerProxy.Models.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public PortfolioItemModel GetItem(int id) {
            return _context.PortfolioItems.Find(id);
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
            var equalityComparer = new PortfolioItemModelComparer();
            var localItems = _context.PortfolioItems.Where(m => m.UserId == userId).ToList();
            var receivedItems = items.ToList();

            foreach (var receivedItem in receivedItems)
            {
                var localItem = localItems.Where(m => m.Id == receivedItem.Id).SingleOrDefault();

                if (equalityComparer.GetHashCode(receivedItem) == equalityComparer.GetHashCode(localItem)
                    && equalityComparer.Equals(receivedItem, localItem))
                {
                    localItems.Remove(localItem);
                    receivedItems.Remove(receivedItem);
                }
                else
                {
                    if(localItem.ItemId == receivedItem.ItemId)
                    {
                        UpdateItem(receivedItem);
                    }
                    else
                    {
                        _context.PortfolioItems.Add(receivedItem);
                    }                    
                }
            }

            foreach (var localItem in localItems)
            {
                _context.PortfolioItems.Remove(localItem);
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