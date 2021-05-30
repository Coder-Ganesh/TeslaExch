using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBetfair.Models
{
    public class clientprofitlossstat
    {
        public string user_id { get; set; }
        public string sport { get; set; }
        public string match { get; set; }
        public string market { get; set; }
        public string event_id { get; set; }
        public string market_id { get; set; }
        public String created { get; set; }
        public String total_pl { get; set; }
        public string total_pl_color { get; set; }
    }
}