using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBetfair.Models
{
    public class NotSettleMN
    {
        public string Marketname { get; set; }
        public string event_code { get; set; }
        public string betfair_id { get; set; }
        public string type { get; set; }
        public string event_name { get; set; }
        public string created { get; set; }
        public string sport_id { get; set; }
    }
}