//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Probet247.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class live_bet
    {
        public int id { get; set; }
        public Nullable<int> sup_id { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> dist_id { get; set; }
        public Nullable<int> md_id { get; set; }
        public Nullable<int> admin_id { get; set; }
        public string event_id { get; set; }
        public string betfair_id { get; set; }
        public string field { get; set; }
        public Nullable<double> rate { get; set; }
        public Nullable<double> stakes { get; set; }
        public Nullable<double> total_value { get; set; }
        public Nullable<double> session_rate { get; set; }
        public string logic { get; set; }
        public string field_pos { get; set; }
        public string team_name { get; set; }
        public Nullable<int> runner_posi { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> place_time { get; set; }
        public Nullable<System.DateTime> settled_time { get; set; }
        public string odds_type { get; set; }
        public Nullable<double> input_stakes { get; set; }
        public Nullable<double> input_pl { get; set; }
        public Nullable<double> before_bal { get; set; }
        public Nullable<double> after_bal { get; set; }
        public Nullable<double> before_exp { get; set; }
        public Nullable<double> after_exp { get; set; }
    }
}
