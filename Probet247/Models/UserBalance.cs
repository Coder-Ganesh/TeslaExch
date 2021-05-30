using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBetfair.Models
{
    public class UserBalance
    {
        public Double UBalance { get; set; }
        public Double UExposure { get; set; }
        public string UserName { get; set; }
        public string FUserName { get; set; }
    }
}