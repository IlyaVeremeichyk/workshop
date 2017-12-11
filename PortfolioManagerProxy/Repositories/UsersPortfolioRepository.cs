using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortfolioManagerProxy.Models;
using PortfolioManagerProxy.Models.Context;

namespace PortfolioManagerProxy.Repositories
{
    public class UsersPortfolioRepository
    {
        private readonly PortfolioManagerContext _context;
        public UsersPortfolioRepository()
        {
            _context = new PortfolioManagerContext();
        }

        public void Add(UsersPortfolio model)
        {
            _context.UsersPortfolios.Add(model);
            _context.SaveChanges();
        }
    }
}