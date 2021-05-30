using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBetfair.Models
{
    public class Databasedl
    {
        public int user_id { get; set; }
        public Double stakes { get; set; }
        public Double total_value { get; set; }
        public string field { get; set; }
        public Double rate { get; set; }
    }
}