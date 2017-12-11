using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioManagerProxy.Models
{
    public class UsersPortfolio
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Share Share { get; set; }
        public int SharesNumber { get; set; }
    }
}