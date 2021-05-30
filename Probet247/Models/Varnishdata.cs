using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBetfair.Models
{
    public class Varnishdata
    {
        public string SelectionId { get; set; }
        public string RunnerName { get; set; }
        public string LaySize1 { get; set; }
        public string BackSize1 { get; set; }
        public string BackPrice1 { get; set; }
        public string LayPrice1 { get; set; }
        public string GameStatus { get; set; }
    }
}