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
    }
}