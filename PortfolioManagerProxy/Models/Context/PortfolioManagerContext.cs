using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PortfolioManagerProxy.Models.Context
{
    public class PortfolioManagerContext : DbContext
    {
        public PortfolioManagerContext()
            :base("DbConnection")
        { }

        public DbSet<PortfolioItemModel> PortfolioItems { get; set; }

        public DbSet<Share> Shares { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SharesHistoricalPrice> SharesHistoricalPrices { get; set; }
        public DbSet<UsersPortfolio> UsersPortfolios { get; set; }
        public DbSet<UsersSharesTransaction> UsersSharesTransactions { get; set; }
    }
}