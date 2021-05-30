using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetBarter.Models
{
    public class UserRagister
    {
        public string Status { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string Currency { get; set; }
        public string hash_key { get; set; }
        public string OddsUrl { get; set; }
        public string SessUrl { get; set; }
        public int uid { get; set; }

        public Double balance { get; set; }
        public Double exposure { get; set; }

    }
}