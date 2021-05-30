using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBetfair.Models
{
    public class PlaceBetsList
    {
        public string EventName { get; set; }
        public string BetName { get; set; }
        public string BetType { get; set; }
        public string RunnerName { get; set; }
        public string event_market_field { get; set; }
        public Double Price { get; set; }
        public Double Qty { get; set; }
        public Double PL { get; set; }
        public string MatchOddsId { get; set; }
        public string RunnerId { get; set; }
        public string created { get; set; }
    }
}