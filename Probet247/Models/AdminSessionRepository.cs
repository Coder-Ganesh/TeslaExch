﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RBetfair.Models
{
    public class AdminSessionRepository
    {
        public IEnumerable<dlanalysisdataS> GetAllMessagesS()
        {
            Double teamAWD = 0;
            Double teamBWD = 0;
            Double teamCWD = 0;
            var AllSportsAddM = new List<dlanalysisdataS>();
            try
            {
                string user_ids = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT distinct event_id,betfair_id from live_bet_new where admin_id='" + user_ids + "' AND status='' AND odds_type='sess' ", con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string event_id = (string)reader["event_id"];
                            string bf_id = (string)reader["betfair_id"];
                            string marketn = GetSessionname(event_id, bf_id);
                            using (var cmd1 = new SqlCommand("SELECT match_title , match_time from matches where event_code='" + event_id + "' AND status='OPEN' ", con))
                            {
                                var reader1 = cmd1.ExecuteReader();
                                reader1.Read();
                                string SportsNameForShowAllSports = (string)reader1["match_title"];
                                DateTime match_t_time = (DateTime)reader1["match_time"];
                                string match_ttime = match_t_time.ToString("yyyy-MM-dd HH:mm");

                                AllSportsAddM.Add(item: new dlanalysisdataS
                                {
                                    GetSp = SportsNameForShowAllSports,
                                    betfair_id = bf_id,
                                    ev_code = event_id,
                                    Spname = "Cricket",
                                    match_ttime = match_ttime,
                                    teamAWD = teamAWD,
                                    teamBWD = teamBWD,
                                    teamCWD = teamCWD,
                                    mname = marketn
                                });
                            }


                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return AllSportsAddM;
        }
        public string GetSessionname(string event_code, string bf_id)
        {
            string spname = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("select market_name from markets where event_code='" + event_code + "' AND betfair_id='" + bf_id + "'"))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    spname = (string)reader["market_name"];
                    con.Close();
                }
            }
            return spname;
        }
    }
}