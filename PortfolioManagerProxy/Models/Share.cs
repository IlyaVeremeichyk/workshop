using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortfolioManagerProxy.Models
{
    public class Share
    {
        [Key]
        public string ISIN { get; set; }
        public  string ShareSymbol { get; set; }
        public  string Market { get; set; }
        public int Volume { get; set; }
        public  string Currency { get; set; }
    }
}