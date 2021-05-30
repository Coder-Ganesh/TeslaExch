using RBetfair.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Probet247.Models
{
    public class SuperadminanalysisRepositary
    {
        DateTime time = DateTime.Now;
        string format1 = "yyyy-MM-dd HH:mm:ss";

        public IEnumerable<dlanalysisdata> GetAllMessages()
        {
            var AllSportsAddM = new List<dlanalysisdata>();
            try
            {
                string user_ids = (string)System.Web.HttpContext.Current.Session["SuperAdmin_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT distinct(betfair_id) , event_id from live_bet where sup_id='" + user_ids + "' AND status='' AND odds_type='MO' "))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            int ii = 0;
                            while (reader.Read())
                            {
                                Double teamAWD = 0;
                                Double teamBWD = 0;
                                Double teamCWD = 0;
                                string betfair_id = (string)reader["betfair_id"];
                                string event_id = (string)reader["event_id"];
                                using (var cmd1 = new SqlCommand("SELECT book_id,match_title,betfair_id,event_code,match_time,sport_id,league_id from matches where event_code='" + event_id + "' AND status='OPEN' ", con))
                                {
                                    var reader1 = cmd1.ExecuteReader();
                                    if (reader1.HasRows)
                                    {
                                        reader1.Read();
                                        string SportsNameForShowAllSports = (string)reader1["match_title"];
                                        string lg_id = (string)reader1["league_id"];
                                        string book_id = (string)reader1["book_id"];
                                        if (book_id == betfair_id)
                                        {
                                            lg_id = "88";
                                        }
                                        string event_code = (string)reader1["event_code"];
                                        DateTime match_t_time = (DateTime)reader1["match_time"];
                                        string match_ttime = match_t_time.ToString("yyyy-MM-dd HH:mm");
                                        string sport_id = (string)reader1["sport_id"];
                                        string jhg = GetSportname(sport_id);
                                        string market_name = MessagesRepository.GetMArketName(event_id, betfair_id);
                                        using (var cmd2 = new SqlCommand("select amount,runner_pos from runner_cal where sup_id='" + user_ids + "' AND market_id='" + betfair_id + "'", con))
                                        {
                                            var reader2 = cmd2.ExecuteReader();
                                            if (reader2.HasRows)
                                            {
                                                while (reader2.Read())
                                                {
                                                    Double amountt = (Double)reader2["amount"];
                                                    int runner_posrd = (Int32)reader2["runner_pos"];
                                                    if (runner_posrd == 1)
                                                    {
                                                        teamAWD = teamAWD + amountt;
                                                    }
                                                    else if (runner_posrd == 2)
                                                    {
                                                        teamBWD = teamBWD + amountt;
                                                    }
                                                    else if (runner_posrd == 3)
                                                    {
                                                        teamCWD = teamCWD + amountt;
                                                    }
                                                    else
                                                    {

                                                    }
                                                }
                                            }
                                        }

                                        AllSportsAddM.Add(item: new dlanalysisdata
                                        {
                                            GetSp = SportsNameForShowAllSports,
                                            betfair_id = betfair_id,
                                            ev_code = event_code,
                                            un_ev_code = event_code + ii.ToString(),
                                            lg_id = lg_id,
                                            book_id = book_id,
                                            Spname = jhg,
                                            match_ttime = match_ttime,
                                            market_name = market_name,
                                            teamAWD = teamAWD,
                                            teamBWD = teamBWD,
                                            teamCWD = teamCWD
                                        });
                                    }
                                }

                                ii++;
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return AllSportsAddM;
        }

        public string GetSportname(string Sports_ID)
        {
            string sport_name = "";
            if (Sports_ID == "4")
            {
                sport_name = "Cricket";
            }
            else if (Sports_ID == "2")
            {
                sport_name = "Tennis";
            }
            else if (Sports_ID == "1")
            {
                sport_name = "Soccer";
            }
            else if (Sports_ID == "7888")
            {
                sport_name = "Live Casino";
            }
            /*using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                string stmt = "SELECT sport_name from [sports] where sport_id='" + Sports_ID + "'";
                using (SqlCommand cmdCount1 = new SqlCommand(stmt, con))
                {
                    var reader1 = cmdCount1.ExecuteReader();
                    if (reader1.HasRows)
                    {
                        reader1.Read();
                        sport_name = (string)reader1["sport_name"];
                    }
                }
                con.Close();
            }*/
            return sport_name;

        }
    }
}