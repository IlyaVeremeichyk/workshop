using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortfolioManagerProxy.Models;
using PortfolioManagerProxy.Models.Context;

namespace PortfolioManagerProxy.Repositories
{
    public class UsersRepository
    {
        private readonly PortfolioManagerContext _context;

        public UsersRepository()
        {
            _context = new PortfolioManagerContext();
        }

        public void Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User GetById(int userId)
        {
            return _context.Users.SingleOrDefault(u => u.Id == userId);
        }
    }
}