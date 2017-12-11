using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioManagerProxy.Models
{
    public class SharesHistoricalPrice
    {
        public  int Id { get; set; }
        public  Share Share { get; set; }
        public  DateTime Date { get; set; }
        public  decimal OpenPrice { get; set; }
        public  decimal MinPrice { get; set; }
        public  decimal MaxPrice { get; set; }
        public  decimal ClosePrice { get; set; }
        public  int SaledNumber { get; set; }
    }
}