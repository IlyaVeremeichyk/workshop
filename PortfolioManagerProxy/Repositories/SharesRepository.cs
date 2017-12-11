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
    public class SharesRepository
    {
        private readonly PortfolioManagerContext _context;

        public SharesRepository()
        {
            _context = new PortfolioManagerContext();
        }

        public IQueryable<Share> GetItems(int userId)
        {
            var sharesUsers = _context.UsersPortfolios.Where(s => s.User.Id == userId).Select(s => s.Share.ISIN).ToList();

            return _context.Shares.Where(s => sharesUsers.Contains(s.ISIN));
        }

        public Share GetById(string isin)
        {
            return _context.Shares.SingleOrDefault(m => m.ISIN == isin);
        }

        //public void AddItem(Share model)
        //{
        //    _context.PortfolioItems.Add(model);
        //    _context.SaveChanges();
        //}

        //public void UpdateItem(Share model)
        //{
        //    _context.SaveChanges();
        //}

        //public void DeleteItem(int itemId)
        //{
        //    var item = _context.PortfolioItems.Where(m => m.ItemId == itemId).SingleOrDefault();
        //    if (item != null)
        //    {
        //        _context.PortfolioItems.Remove(item);
        //        _context.SaveChanges();
        //    }
        //}
        
    }
}