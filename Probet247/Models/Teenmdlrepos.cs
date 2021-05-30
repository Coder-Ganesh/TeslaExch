using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RBetfair.Models
{
    public class Teenmdlrepos
    {
        DateTime time = DateTime.Now;
        string format1 = "yyyy-MM-dd HH:mm:ss";

        public IEnumerable<dlanalysisdata> GetAllMessages()
        {
            var AllSportsAddM = new List<dlanalysisdata>();
            try
            {
                string user_ids = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT distinct(betfair_id) , event_id from live_bet where md_id='" + user_ids + "' AND status='' AND odds_type='TP'", con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            int ii = 0;
                            while (reader.Read())
                            {
                                string betfair_id = (string)reader["betfair_id"];
                                string event_id = (string)reader["event_id"];
                                using (var cmd1 = new SqlCommand("SELECT created , market_name from markets where betfair_id='" + betfair_id + "' AND status='activate' ", con))
                                {
                                    var reader1 = cmd1.ExecuteReader();
                                    if (reader1.HasRows)
                                    {
                                        reader1.Read();
                                        DateTime match_t_time = (DateTime)reader1["created"];
                                        string match_ttime = match_t_time.ToString("yyyy-MM-dd HH:mm");
                                        string sport = "Live Casino";
                                        string market_name = (string)reader1["market_name"];
                                        string match_title = GetTeenTitle(event_id);

                                        Double TotalamountA = 0;
                                        Double TotalamountB = 0;
                                        Double TotalamountC = 0;
                                        SqlCommand sqlLost = new SqlCommand("SELECT stakes,total_value,runner_posi,event_id FROM live_bet WHERE betfair_id='" + betfair_id + "' AND md_id='" + user_ids + "' ", con);
                                        var dataLost = sqlLost.ExecuteReader();
                                        if (dataLost.HasRows)
                                        {
                                            while (dataLost.Read())
                                            {
                                                Double stakes = (Double)dataLost["stakes"];
                                                Double total_value = (Double)dataLost["total_value"];
                                                int runner_pos = (int)dataLost["runner_posi"];
                                                if (runner_pos == 1)
                                                {
                                                    TotalamountA = TotalamountA + total_value;
                                                    TotalamountB = TotalamountB - stakes;
                                                    if (event_id == "51515151")
                                                    {
                                                        TotalamountC = TotalamountC - stakes;
                                                    }
                                                }
                                                else if (runner_pos == 2 || runner_pos == 21)
                                                {
                                                    TotalamountA = TotalamountA - stakes;
                                                    TotalamountB = TotalamountB + total_value;
                                                    if (event_id == "51515151")
                                                    {
                                                        TotalamountC = TotalamountC - stakes;
                                                    }
                                                }
                                                else if (runner_pos == 41)
                                                {
                                                    TotalamountA = TotalamountA - stakes;
                                                    TotalamountB = TotalamountB - stakes;
                                                    TotalamountC = TotalamountC + total_value;
                                                }
                                            }
                                        }
                                        AllSportsAddM.Add(item: new dlanalysisdata
                                        {
                                            GetSp = match_title,
                                            betfair_id = betfair_id,
                                            ev_code = event_id,
                                            un_ev_code = event_id + ii.ToString(),
                                            lg_id = event_id,
                                            book_id = event_id,
                                            Spname = sport,
                                            match_ttime = match_ttime,
                                            market_name = market_name,
                                            teamAWD = TotalamountA,
                                            teamBWD = TotalamountB,
                                            teamCWD = TotalamountC
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

            }
            return AllSportsAddM;
        }
        public string GetTeenTitle(string event_code)
        {
            string sport_name = "";
            if (event_code == "20202020")
            {
                sport_name = "Teen Patti T20";
            }
            else if (event_code == "30303030")
            {
                sport_name = "Teen Patti T20";
            }
            else if (event_code == "31313131")
            {
                sport_name = "Teen Patti OneDay";
            }
            else if (event_code == "40404040")
            {
                sport_name = "Poker T20";
            }
            else if (event_code == "50505050")
            {
                sport_name = "Dragon Tiger T20";
            }
            else if (event_code == "51515151")
            {
                sport_name = "Dragon Tiger Lion";
            }
            else if (event_code == "60606060")
            {
                sport_name = "Lucky 7A";
            }
            else if (event_code == "41414141")
            {
                sport_name = "Poker One Day";
            }
            else if (event_code == "52525252")
            {
                sport_name = "Dragon Tiger OneDay";
            }
            else if (event_code == "61616161")
            {
                sport_name = "Lucky 7B";
            }
            return sport_name;

        }
    }
}