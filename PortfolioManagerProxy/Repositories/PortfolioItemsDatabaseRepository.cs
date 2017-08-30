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

        public PortfolioItemModel GetItem(int itemId)
        {
            return _context.PortfolioItems.Where(m => m.ItemId == itemId).SingleOrDefault();
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
            UpdatePortfolioItem(model);
            _context.SaveChanges();
        }

        public void DeleteItem(int itemId)
        {
            var item = _context.PortfolioItems.Where(m => m.ItemId == itemId).SingleOrDefault();
            if (item != null)
            {
                _context.PortfolioItems.Remove(item);
                _context.SaveChanges();
            }
        }

        public void UpdateUser(int userId, IList<PortfolioItemModel> receivedItems)
        {
            receivedItems = receivedItems.ToList();
            var equalityComparer = new PortfolioItemModelComparer();
            var localItems = _context.PortfolioItems.Where(m => m.UserId == userId).ToList();

            UpdateDublicateData(localItems, receivedItems, equalityComparer);

            foreach (var localItem in localItems)
            {
                _context.PortfolioItems.Remove(localItem);
            }

            foreach (var newItem in receivedItems)
            {
                _context.PortfolioItems.Add(newItem);
            }

            _context.SaveChanges();
        }

        private void UpdateDublicateData(List<PortfolioItemModel> localItems, IList<PortfolioItemModel> receivedItems, IEqualityComparer<PortfolioItemModel> equalityComparer)
        {
            foreach (var receivedItem in receivedItems.ToList())
            {
                var localDublicate = localItems.Where(m => m.ItemId == receivedItem.ItemId).SingleOrDefault();
                if (localDublicate == null)
                {
                    continue;
                }
                if (equalityComparer.GetHashCode(receivedItem) != equalityComparer.GetHashCode(localDublicate)
                    || !(equalityComparer.Equals(receivedItem, localDublicate)))
                {
                    UpdatePortfolioItem(receivedItem);
                }

                localItems.Remove(localDublicate);
                receivedItems.Remove(receivedItem);
            }
        }

        private void UpdatePortfolioItem(PortfolioItemModel newItem)
        {
            var origin = _context.PortfolioItems.Where(m => m.ItemId == newItem.ItemId).Single();
            origin.SharesNumber = newItem.SharesNumber;
            origin.Symbol = newItem.Symbol;
            origin.UserId = newItem.UserId;
        }
    }
}