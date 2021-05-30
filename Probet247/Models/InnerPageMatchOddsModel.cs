﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BertfairLive1.Models
{
    public class InnerPageMatchOddsModel
    {
        public int id { get; set; }
        public string back1 { get; set; }
        public string lay1 { get; set; }
        public string Runnername { get; set; }
        public string back11 { get; set; }
        public string back22 { get; set; }
        public string lay11 { get; set; }
        public string lay22 { get; set; }
        public string RunnernameB { get; set; }
        public string EventCode { get; set; }
        public string BetfairId { get; set; }
        public string back1size { get; set; }
        public string back2size { get; set; }
        public string back3size { get; set; }
        public string lay1size { get; set; }
        public string lay2size { get; set; }
        public string lay3size { get; set; }
        public string totalMatched { get; set; }
        public string Event_Type_Id { get; set; }
        public string MatchTime { get; set; }
    }
}