using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBetfair.Models
{
    public class AccountStatement_DL
    {
        public DateTime DTime { get; set; }
        public Double Deposit { get; set; }
        public Double Withdraw { get; set; }
        public Double Balance { get; set; }
        public string Remark { get; set; }
        public string Desc { get; set; }
        public string time { get; set; }
    }
}