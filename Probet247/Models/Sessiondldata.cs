using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBetfair.Models
{
    public class Sessiondldata
    {
        public int client_id { get; set; }
        public string client_name { get; set; }
        public Double DlRatioA { get; set; }
        public Double DlRatioB { get; set; }
        public Double DlRatioC { get; set; }
        public Double DlRatioD { get; set; }
        public Double DlRatioE { get; set; }
        public Double DlRatioF { get; set; }
        public string marketna { get; set; }
        public Double Back1 { get; set; }
        public Double Back2 { get; set; }
        public Double Back3 { get; set; }
        public Double Backsize1 { get; set; }
        public Double Laysize1 { get; set; }
        public Double Lay1 { get; set; }
        public Double Lay2 { get; set; }
        public Double Lay3 { get; set; }
        public Double ag_rate { get; set; }
        public Double agm_rate { get; set; }
        public Double ad_rate { get; set; }
    }
}