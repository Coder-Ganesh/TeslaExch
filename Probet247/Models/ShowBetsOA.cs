using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Probet247.Models
{
    public class ShowBetsOA
    {
        public int betid { get; set; }
        public string Selection { get; set; }
        public Double rate { get; set; }
        public Double Stake { get; set; }
        public Double profit_loss { get; set; }
        public string Type { get; set; }
        public string Placed { get; set; }
    }
}