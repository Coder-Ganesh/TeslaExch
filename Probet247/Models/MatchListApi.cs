using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetBarter.Models
{
    public class MatchListApi
    {
        public string name { get; set; }
        public string time { get; set; }
        public string sport_id { get; set; }
        public string is_access { get; set; }
        public string xmarket_code { get; set; }
        public string betfair_id { get; set; }
        public string event_code { get; set; }
        public string inplay { get; set; }
        public string premium { get; set; }
        public string is_tv { get; set; }
        public string pro { get; set; }
        public string bookfancy { get; set; }
        public string prifancy { get; set; }
    }
}