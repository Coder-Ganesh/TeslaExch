using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBetfair.Models
{
    public class ClientPLModel
    {
        public string description { get; set; }
        public string event_id { get; set; }
        public string market_id { get; set; }
        public DateTime created { get; set; }
        public Double debit { get; set; }
        public Double credit { get; set; }
        public Double total_pl { get; set; }
        public Double dl_pl { get; set; }
        public Double mdl_pl { get; set; }
        public Double admin_pl { get; set; }
        public Double total_plc { get; set; }
        public Double dl_plc { get; set; }
        public Double mdl_plc { get; set; }
        public Double admin_plc { get; set; }
        public string uname { get; set; }
    }
}