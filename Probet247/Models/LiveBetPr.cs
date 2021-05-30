using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Probet247.Models
{
    public class LiveBetPr
    {
        public DateTime time { get; set; }
        public string matchTitle { get; set; }
        public string status { get; set; }
        public string event_code { get; set; }
    }
}