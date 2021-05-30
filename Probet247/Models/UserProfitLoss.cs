using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetBarter.Models
{
    public class UserProfitLoss
    {
        public string desc { get; set; }
        public string settle { get; set; }
        public string market_id { get; set; }
        public string event_id { get; set; }
        public string netpl { get; set; }


    }
}