using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBetfair.Models
{
    public class dlanalysisdata
    {
        public string GetSp { get; set; }
        public string betfair_id { get; set; }
        public string lg_id { get; set; }
        public string book_id { get; set; }
        public string ev_code { get; set; }
        public string un_ev_code { get; set; }
        public string Spname { get; set; }
        public string match_ttime { get; set; }
        public string market_name { get; set; }
        public Double teamAWD { get; set; }
        public Double teamBWD { get; set; }
        public Double teamCWD { get; set; }
    }
}