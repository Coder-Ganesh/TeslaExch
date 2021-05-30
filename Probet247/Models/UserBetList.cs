using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetBarter.Models
{
    public class UserBetList
    {
        public string market_title { get; set; }
        public string created { get; set; }
        public string field { get; set; }
        public string market_data { get; set; }

        public Double profit_loss { get; set; }
    }
}