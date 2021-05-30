using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBetfair.Models
{
    public class MatchedClientBetList
    {
        public int betid { get; set; }
        public string EventTime { get; set; }
        public string ucname { get; set; }
        public int uid { get; set; }
        public string Description { get; set; }
        public string GetEventTName { get; set; }
        public string GetMarketName { get; set; }
        public string Field { get; set; }
        public string Field_pos { get; set; }
        public Double Rate { get; set; }
        public Double Stakes { get; set; }
        public Double Stakes1 { get; set; }
        public Double PL { get; set; }
        public Double OddsReq { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string betfairid { get; set; }
        public string event_idsend { get; set; }
        public string odds_match { get; set; }
    }
}