using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BertfairLive1.Models
{
    public class BetPlaceD
    {
        public string BetfairId { get; set; }
        public string EventCode { get; set; }
        public string stackValue { get; set; }
        public string OddsValue { get; set; }
        public int RunnerIndex { get; set; }
        public string BoxType { get; set; }
        public string RunnerNameget { get; set; }
        public string MarketName { get; set; }
        public string EventType { get; set; }
        public string OddsVolume { get; set; }
        public string SeBFI { get; set; }
    }
}