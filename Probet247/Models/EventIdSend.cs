using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BertfairLive1.Models
{
    public class EventIdSend
    {
        public string event_code { get; set; }
        public string mk_code { get; set; }
        public string Betfair_id_For_OMarkets { get; set; }
        public string Betfair_Id_MView { get; set; }
        public string league_id_MList { get; set; }
        public string Sport_id_MList { get; set; }
        public string League_Name_MList { get; set; }
        public string Pagenumber { get; set; }
        public int PagenumberLast { get; set; }
    }
}