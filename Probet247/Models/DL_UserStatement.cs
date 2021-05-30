using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBetfair.Models
{
    public class DL_UserStatement
    {
        public int Client_Id { get; set; }
        public int dl_Id { get; set; }
        public int md_Id { get; set; }
        public string Client_Username { get; set; }
        public string Client_name { get; set; }
        public Double Client_balance { get; set; }
        public Double Client_avl_balance { get; set; }
        public Double Client_ttl_balance { get; set; }
        public Double Client_exposure { get; set; }
        public Double Client_ref_profit_loss { get; set; }
        public Double Client_profit_loss { get; set; }
        public Double Coin_Rate { get; set; }
        public Double MA_balance { get; set; }
        public Double PL_balance { get; set; }
        public Double Client_lib { get; set; }
        public string Client_status { get; set; }
        public string Client_pas { get; set; }
        public string is_bet { get; set; }
        public Double DLPROF_LOSS { get; set; }
        public Double credit_ref { get; set; }
        public Double exposure_limit { get; set; }
    }
}