 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBetfair.Models
{
    public class DL_UserBetList
    {
        public DateTime EventTime { get; set; }
        public string Description { get; set; }
        public string Field { get; set; }
        public Double Rate { get; set; }
        public Double Stakes { get; set; }
        public Double PL { get; set; }
    }
}