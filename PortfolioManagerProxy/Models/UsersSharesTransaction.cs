using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioManagerProxy.Models
{
    public class UsersSharesTransaction
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Share Share { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public bool TransactionType { get; set; }
        public int TransactionQuantity { get; set; }
    }
}