using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBetfair.Models
{
    public class BookViewAg
    {
        public int user_id { get; set; }
        public string cname { get; set; }
        public Double teamAWD { get; set; }
        public Double teamBWD { get; set; }
        public Double teamCWD { get; set; }
        public Double teamAWDDL { get; set; }
        public Double teamBWDDL { get; set; }
        public Double teamCWDDL { get; set; }
        public Double ag_rate { get; set; }
        public Double agm_rate { get; set; }
        public Double ad_rate { get; set; }
    }
}