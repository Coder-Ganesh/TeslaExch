using BertfairLive1.Models;
using Newtonsoft.Json;
using Probet247.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Probet247.Controllers
{
    public class OtherController : Controller
    {
        // GET: Other
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult QuickLinks()
        {
            var products = "";
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://strandsweave.betfair.com/api/ems/homepage/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("v4?_ak=ajhWsonjIgux55OJ&exchangeIds=%5B1%5D").Result;  // Blocking call! 
                    products = response.Content.ReadAsStringAsync().Result;
                }
                catch (Exception ex)
                {
                }
            }
            catch (TaskCanceledException ex)
            {

            }
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        public String QuickCricketX()
        {
            var products = "";
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("gully/add_bf_matches.php").Result;  // Blocking call! 
                    products = response.Content.ReadAsStringAsync().Result;
                }
                catch (Exception ex)
                {
                }
            }
            catch (TaskCanceledException ex)
            {

            }
            return products;
        }

        public JsonResult OtherMarketData()
        {

            List<OtherMarketList> messages = new List<OtherMarketList>();
            messages = GetAllMessagesNew11();
            return Json(messages, JsonRequestBehavior.AllowGet);

        }

        public List<OtherMarketList> GetAllMessagesNew11()
        {
            ArrayList OthermarketBfIds = new ArrayList();
            var messages = new List<OtherMarketList>();
            string event_codess = "29488999";
            string event_type_ids = "1";
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://strandsweave.betfair.com/api/ems/multi-market-page/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = client.GetAsync("v1?_ak=ajhWsonjIgux55OJ&eventId=" + event_codess + "&eventTypeId=" + event_type_ids).Result;  // Blocking call! 

                    var products = response.Content.ReadAsStringAsync().Result;
                    dynamic responseJson = JsonConvert.DeserializeObject(products);
                    var jhvg = responseJson.allMarketsList;
                    for (int i1 = 0; i1 < jhvg.Count; i1++)
                    {
                        OthermarketBfIds.Add(jhvg[i1].marketId);
                    }
                    var Otherallmlist = responseJson.markets;
                    for (int i = 0; i < OthermarketBfIds.Count; i++)
                    {
                        string hghj = responseJson.events[event_codess].name;
                        string toBeSearched = " v ";
                        string run2 = hghj.Substring(hghj.IndexOf(toBeSearched) + toBeSearched.Length);
                        string run1 = hghj.Replace(" v " + run2, "");

                        string OtherBf_id_obj = OthermarketBfIds[i].ToString();
                        string OtherName = Otherallmlist[OtherBf_id_obj].name;
                        string OtherBf_id = Otherallmlist[OtherBf_id_obj].id;
                        if (OtherName.Contains("Over/Under") == true && OtherName.Contains("Goals") == true && OtherName.Contains("Match Odds and ") == false && OtherName.Contains(run1) == false && OtherName.Contains(run2) == false)
                        {
                            messages.Add(item: new OtherMarketList
                            {
                                OtherName = OtherName,
                                OtherBf_id = OtherBf_id
                            });
                        }
                    }

                }
                catch (Exception ex)
                {

                }
            }
            catch (TaskCanceledException ex)
            {

            }


            return messages;
        }

        public ActionResult Indeso()
        {
            string type = "";
            if (Request.QueryString["type"] != null)
            {
                type = Request.QueryString["type"];
            }

            string spi_id = "0";
            if (Request.QueryString["spi_id"] != null)
            {
                spi_id = Request.QueryString["spi_id"];
            }

            string comp_id = "0";
            if (Request.QueryString["comp_id"] != null)
            {
                comp_id = Request.QueryString["comp_id"];
            }

            string mat_id = "0";
            if (Request.QueryString["mat_id"] != null)
            {
                mat_id = Request.QueryString["mat_id"];
            }

            string mrkt_id = "0";
            if (Request.QueryString["mrkt_id"] != null)
            {
                mrkt_id = Request.QueryString["mrkt_id"];
            }

            string limit = "0";
            if (Request.QueryString["limit"] != null)
            {
                limit = Request.QueryString["limit"];
            }

            string evv_co = "";
            if (Request.QueryString["evv_co"] != null)
            {
                evv_co = Request.QueryString["evv_co"];
            }
            string evv_st = "";
            if (Request.QueryString["evv_st"] != null)
            {
                evv_st = Request.QueryString["evv_st"];
            }

            if (type == "league")
            {
                string selleague = "SELECT [sport_id] FROM sports where status='activate' AND sport_id='" + spi_id + "' ";
                if (spi_id == "all")
                {
                    selleague = "SELECT [sport_id] FROM sports where status='activate' AND sport_id in(4,2,1) order by id desc ";
                }
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(selleague))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string EventTypeID = (string)reader["sport_id"];
                            InsertLeague(EventTypeID);
                        }
                        con.Close();
                    }
                }

            }

            if (type == "allleague")
            {
                string selleague = "SELECT [sport_id] FROM sports where status='activate' ";
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(selleague))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string EventTypeID = (string)reader["sport_id"];
                            InsertLeague(EventTypeID);
                        }
                        con.Close();
                    }
                }

            }

            else if (type == "dleague")
            {
                string selleague = "SELECT [sport_id],[league_id] FROM league where status='activate' AND sport_id='" + spi_id + "' ";
                if (spi_id == "all")
                {
                    selleague = "SELECT [sport_id],[league_id] FROM league where status='activate' order by id desc ";
                }
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(selleague))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string EventTypeID = (string)reader["sport_id"];
                            string LeagueID = (string)reader["league_id"];
                            dleague(EventTypeID, LeagueID);
                        }
                        con.Close();
                    }
                }

            }

            else if (type == "match")
            {
                string selleague = "SELECT [sport_id],[league_id] FROM league where status='activate' AND sport_id='" + spi_id + "' ";
                if (spi_id == "all")
                {
                    selleague = "SELECT [sport_id],[league_id] FROM league where status='activate' AND sport_id in(4,2,1) order by id desc ";
                }
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(selleague))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string EventTypeID = (string)reader["sport_id"];
                            string LeagueID = (string)reader["league_id"];
                            InsertMatches(EventTypeID, LeagueID);
                        }
                        con.Close();
                    }
                }

            }

            else if (type == "allmatch")
            {
                string selleague = "SELECT [sport_id],[league_id] FROM league where status='activate' AND sport_id NOT in(4,2,1) order by id desc ";
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(selleague))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string EventTypeID = (string)reader["sport_id"];
                            string LeagueID = (string)reader["league_id"];
                            InsertMatches(EventTypeID, LeagueID);
                        }
                        con.Close();
                    }
                }

            }

            else if (type == "match_gc")
            {
                InsertMatches_gc();

            }

            else if (type == "match_X")
            {
                InsertMatches_X();

            }

            else if (type == "market")
            {
                string selleague = "SELECT [sport_id],[league_id],[event_code] FROM matches where status='OPEN' AND sport_id='" + spi_id + "' ";
                if (mat_id == "all")
                {
                    selleague = "SELECT [sport_id],[league_id],[event_code] FROM match where status='OPEN' AND sport_id in(4,2,1) order by id desc ";
                }
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(selleague))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string event_code = (string)reader["event_code"];
                            string LeagueIdGet = (string)reader["league_id"];
                            string EventTypeID = (string)reader["sport_id"];
                            InsertMarkets(event_code, LeagueIdGet, EventTypeID);
                        }
                        con.Close();
                    }
                }

            }

            else if (type == "runner")
            {
                string selleague = "SELECT [betfair_id] FROM markets WHERE runner_name_done !='done' AND status='activate' AND sport_id='" + spi_id + "' ";
                if (mrkt_id == "all")
                {
                    selleague = "SELECT [betfair_id] FROM markets WHERE runner_name_done !='done' AND status='activate' AND sport_id in(4,2,1) order by id desc ";
                }
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(selleague))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string BetfairId = (string)reader["betfair_id"];

                            AddRunnerNameForMarkets(BetfairId);
                        }
                        con.Close();
                    }
                }

            }

            else if (type == "status")
            {
                DateTime times = DateTime.Now;              // Use current time
                string format = "yyyy-MM-dd HH:mm:ss";

                string selleague = "SELECT [betfair_id] FROM matches where status='OPEN' AND betfair_id!='0' AND sport_id='" + spi_id + "' AND match_time < '" + times.ToString(format) + "' ";
                if (spi_id == "all")
                {
                    selleague = "SELECT [betfair_id] FROM matches where status='OPEN' AND betfair_id!='0' AND sport_id in(4,2,1) AND match_time < '" + times.ToString(format) + "' ";
                }
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(selleague))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string betfair_idst = (string)reader["betfair_id"];
                            CheckStatus(betfair_idst);
                        }
                        con.Close();
                    }
                }

            }

            else if (type == "allstatus")
            {
                DateTime times = DateTime.Now;              // Use current time
                string format = "yyyy-MM-dd HH:mm:ss";

                string selleague = "SELECT [betfair_id] FROM matches where status='OPEN' AND betfair_id!='0' AND match_time < '" + times.ToString(format) + "' ";
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(selleague))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string betfair_idst = (string)reader["betfair_id"];
                            CheckStatus(betfair_idst);
                        }
                        con.Close();
                    }
                }

            }

            else if (type == "evv_st")
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (SqlCommand cmd_updd = new SqlCommand("Update matches SET status='" + evv_st + "' where event_code='" + evv_co + "' ", con))
                    {
                        con.Open();
                        cmd_updd.ExecuteNonQuery();
                        con.Close();
                    }
                }

            }

            return View();
        }

        public ActionResult InsertLeague(String EventTypeIDGet)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://www.api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("league.php?event_type_id=" + EventTypeIDGet).Result;
                    var products = response.Content.ReadAsStringAsync().Result;
                    dynamic responseJson = JsonConvert.DeserializeObject(products);
                    for (int i = 0; i < responseJson.Count; i++)
                    {
                        string Id = responseJson[i].id;
                        string name = responseJson[i].name;
                        name = name.Replace("'", "''");
                        string competitionRegion = "GBP";//responseJson[i].competitionRegion;

                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            con.Open();
                            string stmt = "SELECT [status],[id] FROM league WHERE league_id='" + Id + "' ";
                            using (SqlCommand cmdCount = new SqlCommand(stmt, con))
                            {
                                var datamat = cmdCount.ExecuteReader();
                                if (datamat.HasRows)
                                {
                                    datamat.Read();
                                    string status = (string)datamat["status"];
                                    int id = (Int32)datamat["id"];
                                    if (status == "deactivate")
                                    {
                                        SqlCommand cmd6new = new SqlCommand("UPDATE league SET status ='activate'  WHERE id = '" + id + "' ", con);
                                        cmd6new.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    DateTime time = DateTime.Now;              // Use current time
                                    string format = "yyyy-MM-dd HH:mm:ss";

                                    SqlCommand cmd2 = new SqlCommand("INSERT INTO  league (sport_id,league_name,league_id,status,created,competitionRegion) " +
                                   "VALUES ('" + EventTypeIDGet + "','" + name + "','" + Id + "','activate','" + time.ToString(format) + "','" + competitionRegion + "') ", con);
                                    cmd2.ExecuteNonQuery();
                                }
                            }
                            con.Close();
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (TaskCanceledException ex)
            {

            }
            return View();
        }

        public ActionResult dleague(String EventTypeIDGet, string LeagueID)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://www.api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("league.php?event_type_id=" + EventTypeIDGet).Result;

                    var products = response.Content.ReadAsStringAsync().Result;
                    dynamic responseJson = JsonConvert.DeserializeObject(products);
                    string Delleague = "Ram";
                    for (int i = 0; i < responseJson.Count; i++)
                    {
                        string Id = responseJson[i].id;
                        if (LeagueID == Id)
                        {
                            Delleague = "Sita";
                        }
                    }
                    if (Delleague == "Ram")
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            con.Open();
                            SqlCommand cmd6new = new SqlCommand("UPDATE league SET status='deactivate' WHERE league_id='" + LeagueID + "' ", con);
                            cmd6new.ExecuteNonQuery();
                            con.Close();
                        }

                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (TaskCanceledException ex)
            {

            }
            return View();
        }

        public ActionResult InsertMatches(string EventTypeIDGet, string LeagueIDGet)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://www.api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("match.php?league_id=" + LeagueIDGet).Result;

                    var products = response.Content.ReadAsStringAsync().Result;
                    dynamic responseJson = JsonConvert.DeserializeObject(products);
                    for (int i = 0; i < responseJson.Count; i++)
                    {
                        string EventIDGet = responseJson[i]["id"];
                        string name = responseJson[i]["name"];
                        name = name.Replace("'", "''");
                        DateTime openDate = responseJson[i]["openDate"];
                        openDate = openDate.AddMinutes(330);

                        if (name != "Set 01" && name != "Set 02" && name != "Set 03")
                        {

                            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                            {
                                con.Open();
                                string stmt = "SELECT [match_time] FROM matches where event_code='" + EventIDGet + "'";
                                using (SqlCommand dataCount = new SqlCommand(stmt, con))
                                {
                                    string dateformat = "yyyy-MM-dd HH:mm:ss";
                                    var datamat = dataCount.ExecuteReader();
                                    if (datamat.HasRows)
                                    {
                                        datamat.Read();
                                        DateTime match_time = (DateTime)datamat["match_time"];
                                        string cric_match_time1 = match_time.ToString("yyyy-MM-dd HH:mm:ss");
                                        string apidate = openDate.ToString(dateformat);
                                        if (cric_match_time1 != apidate)
                                        {
                                            SqlCommand cmd6new = new SqlCommand("UPDATE matches SET match_time ='" + apidate + "'  WHERE event_code = '" + EventIDGet + "' ", con);
                                            cmd6new.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        string format = "yyyy-MM-dd HH:mm:ss";

                                        SqlCommand cmd2 = new SqlCommand("INSERT INTO  matches (sport_id,match_title,league_id,event_code,status,match_time,betfair_id,teama,teamb) " +
                                        " VALUES ('" + EventTypeIDGet + "','" + name + "','" + LeagueIDGet + "','" + EventIDGet + "','deactivate','" + openDate.ToString(format) + "','0','nikhil','rakesh') ", con);
                                        int runquery = cmd2.ExecuteNonQuery();
                                        if (runquery == 1)
                                        {
                                            HttpResponseMessage response1 = client.GetAsync("markets.php?event_type_id=" + EventTypeIDGet + "&event_code=" + EventIDGet).Result;

                                            var products1 = response1.Content.ReadAsStringAsync().Result;
                                            dynamic responseJson1 = JsonConvert.DeserializeObject(products1);
                                            for (int i1 = 0; i1 < responseJson1.Count; i1++)
                                            {
                                                string BetfairId = responseJson1[i1].marketId;
                                                string marketName = responseJson1[i1].marketName;

                                                if (marketName == "Match Odds" || marketName == "Moneyline")
                                                {
                                                    marketName = "Match Odds";
                                                    string stmt1 = "SELECT COUNT(*) FROM markets where event_code='" + BetfairId + "' ";
                                                    int countmktm = 0;
                                                    using (SqlCommand cmdCount = new SqlCommand(stmt1, con))
                                                    {
                                                        countmktm = (int)cmdCount.ExecuteScalar();
                                                    }
                                                    if (countmktm < 1)
                                                    {

                                                        DateTime time = DateTime.Now;              // Use current time
                                                        string format1 = "yyyy-MM-dd HH:mm:ss";

                                                        SqlCommand cmd3 = new SqlCommand("INSERT INTO  markets (market_name,back1,lay1,back2,lay2,back3,lay3,status,result_value," +
                                                        "market_settle,created,league_id,sport_id,betfair_id,event_code,nested_market,runner_name_done) " +
                                                        "VALUES ('" + marketName + "' ,'' ,'' ,'' ,'' ,'' ,'' ,'activate' ,'' ,'' ,'" + time.ToString(format1) + "' ,'" + LeagueIDGet + "' ,'" + EventTypeIDGet + "' ,'" + BetfairId + "' ,'" + EventIDGet + "' ,'' ,'undone') ", con);
                                                        int runquery1 = cmd3.ExecuteNonQuery();
                                                        if (runquery1 == 1)
                                                        {
                                                            SqlCommand cmd4 = new SqlCommand("UPDATE matches SET betfair_id ='" + BetfairId + "' WHERE event_code = '" + EventIDGet + "' ", con);
                                                            int runquery2 = cmd4.ExecuteNonQuery();
                                                            if (runquery2 == 1)
                                                            {
                                                                HttpResponseMessage response2 = client.GetAsync("selection.php?betfair_id=" + BetfairId).Result;

                                                                var products2 = response2.Content.ReadAsStringAsync().Result;
                                                                dynamic responseJson2 = JsonConvert.DeserializeObject(products2);

                                                                for (int i2 = 0; i2 < responseJson2.Count; i2++)
                                                                {
                                                                    var runners = responseJson2[i2].runners;
                                                                    string marketId = responseJson2[i2].marketId;
                                                                    string marketName1 = responseJson2[i2].marketName;
                                                                    string Team_A = "";
                                                                    string Team_B = "";
                                                                    for (int i3 = 0; i3 < runners.Count; i3++)
                                                                    {
                                                                        string selectionId = runners[i3].selectionId;
                                                                        string runnerName = runners[i3].runnerName;
                                                                        runnerName = runnerName.Replace("'", "''");
                                                                        string sortPriority = runners[i3].sortPriority;
                                                                        if (i3 == 0)
                                                                        {
                                                                            Team_A = runnerName;
                                                                        }
                                                                        else if (i3 == 1)
                                                                        {
                                                                            Team_B = runnerName;
                                                                        }
                                                                        string stmtr = "SELECT COUNT(*) FROM runner_market where market_id='" + BetfairId + "' AND sortPriority='" + sortPriority + "' ";
                                                                        int countmktmr = 0;
                                                                        using (SqlCommand cmdCount = new SqlCommand(stmtr, con))
                                                                        {
                                                                            countmktmr = (int)cmdCount.ExecuteScalar();
                                                                        }
                                                                        if (countmktmr < 1)
                                                                        {
                                                                            SqlCommand cmd5 = new SqlCommand("INSERT INTO  runner_market (runner_name,back1,back2,back3,lay1,lay2,lay3,status,selection_id,market_id,sortPriority) " +
                                                                            "VALUES ('" + runnerName + "' , '0' , '0' , '0' , '0' , '0' , '0' , 'activate' ,'" + selectionId + "' ,'" + marketId + "' ,'" + sortPriority + "') ", con);
                                                                            cmd5.ExecuteNonQuery();
                                                                            SqlCommand cmd6 = new SqlCommand("UPDATE matches SET teama ='" + Team_A + "' , teamb ='" + Team_B + "' WHERE betfair_id = '" + marketId + "' ", con);
                                                                            cmd6.ExecuteNonQuery();
                                                                            SqlCommand cmd7 = new SqlCommand("UPDATE markets SET runner_name_done ='done' WHERE runner_name_done='undone' AND betfair_id = '" + marketId + "' ", con);
                                                                            cmd7.ExecuteNonQuery();

                                                                        }
                                                                    }
                                                                }

                                                            }

                                                        }
                                                    }

                                                    con.Close();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (TaskCanceledException ex)
            {

            }

            return View();
        }

        public ActionResult InsertMatches_gc()
        {
            string EventTypeIDGet = "508";
            string LeagueIDGet = "508";
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("gc_matches.php").Result;

                    var products = response.Content.ReadAsStringAsync().Result;
                    dynamic responseJson = JsonConvert.DeserializeObject(products);
                    for (int i = 0; i < responseJson.Count; i++)
                    {
                        string EventIDGet = responseJson[i]["event_code"];
                        string betfair_id = responseJson[i]["betfair_id"];
                        string is_access = responseJson[i]["is_access"];
                        string is_tv = responseJson[i]["is_tv"];
                        string name = responseJson[i]["name"];
                        name = name.Replace("'", "''");
                        string openDate = responseJson[i]["time"];

                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            con.Open();
                            string stmt = "SELECT [match_time],[x_type],[tv_ch] FROM matches where event_code='" + EventIDGet + "'";
                            using (SqlCommand dataCount = new SqlCommand(stmt, con))
                            {
                                var datamat = dataCount.ExecuteReader();
                                if (datamat.HasRows)
                                {
                                    datamat.Read();
                                    DateTime match_time = (DateTime)datamat["match_time"];
                                    string x_type = (string)datamat["x_type"];
                                    string tv_ch = (string)datamat["tv_ch"];
                                    string cric_match_time1 = match_time.ToString("yyyy-MM-dd HH:mm:ss");
                                    if (cric_match_time1 != openDate || x_type != is_access || tv_ch != is_tv)
                                    {
                                        SqlCommand cmd6new = new SqlCommand("UPDATE matches SET match_time ='" + openDate + "' ,x_type ='" + is_access + "',tv_ch ='" + is_tv + "'  WHERE event_code = '" + EventIDGet + "' ", con);
                                        cmd6new.ExecuteNonQuery();
                                    }
                                }
                                else
                                {

                                    SqlCommand cmd2 = new SqlCommand("INSERT INTO  matches (sport_id,match_title,league_id,event_code,status,match_time,betfair_id,teama,teamb) " +
                                    " VALUES ('" + EventTypeIDGet + "','" + name + "','" + LeagueIDGet + "','" + EventIDGet + "','OPEN','" + openDate + "','0','nikhil','rakesh') ", con);
                                    int runquery = cmd2.ExecuteNonQuery();
                                    if (runquery == 1)
                                    {
                                        HttpResponseMessage response1 = client.GetAsync("gc_mo.php?betfair_id=" + betfair_id).Result;

                                        var products1 = response1.Content.ReadAsStringAsync().Result;
                                        dynamic responseJson1 = JsonConvert.DeserializeObject(products1);
                                        string BetfairId = responseJson1.marketId;
                                        string marketName = responseJson1.market;
                                        var runners = responseJson1.runner;

                                        if (marketName == "Match Odds")
                                        {
                                            marketName = "Match Odds";
                                            string stmt1 = "SELECT COUNT(*) FROM markets where event_code='" + BetfairId + "' ";
                                            int countmktm = 0;
                                            using (SqlCommand cmdCount = new SqlCommand(stmt1, con))
                                            {
                                                countmktm = (int)cmdCount.ExecuteScalar();
                                            }
                                            if (countmktm < 1)
                                            {

                                                DateTime time = DateTime.Now;              // Use current time
                                                string format1 = "yyyy-MM-dd HH:mm:ss";

                                                SqlCommand cmd3 = new SqlCommand("INSERT INTO  markets (market_name,back1,lay1,back2,lay2,back3,lay3,status,result_value," +
                                                "market_settle,created,league_id,sport_id,betfair_id,event_code,nested_market,runner_name_done) " +
                                                "VALUES ('" + marketName + "' ,'' ,'' ,'' ,'' ,'' ,'' ,'activate' ,'' ,'' ,'" + time.ToString(format1) + "' ,'" + LeagueIDGet + "' ,'" + EventTypeIDGet + "' ,'" + BetfairId + "' ,'" + EventIDGet + "' ,'' ,'undone') ", con);
                                                int runquery1 = cmd3.ExecuteNonQuery();
                                                if (runquery1 == 1)
                                                {
                                                    SqlCommand cmd4 = new SqlCommand("UPDATE matches SET betfair_id ='" + BetfairId + "' WHERE event_code = '" + EventIDGet + "' ", con);
                                                    int runquery2 = cmd4.ExecuteNonQuery();
                                                    if (runquery2 == 1)
                                                    {
                                                        string Team_A = "";
                                                        string Team_B = "";
                                                        for (int i3 = 0; i3 < runners.Count; i3++)
                                                        {
                                                            string runnerName = runners[i3].runnerName;
                                                            runnerName = runnerName.Replace("'", "''");
                                                            string sortPriority = runners[i3].sortPriority;
                                                            string selectionId = "508508" + sortPriority;
                                                            if (i3 == 0)
                                                            {
                                                                Team_A = runnerName;
                                                            }
                                                            else if (i3 == 1)
                                                            {
                                                                Team_B = runnerName;
                                                            }
                                                            string stmtr = "SELECT COUNT(*) FROM runner_market where market_id='" + BetfairId + "' AND sortPriority='" + sortPriority + "' ";
                                                            int countmktmr = 0;
                                                            using (SqlCommand cmdCount = new SqlCommand(stmtr, con))
                                                            {
                                                                countmktmr = (int)cmdCount.ExecuteScalar();
                                                            }
                                                            if (countmktmr < 1)
                                                            {
                                                                SqlCommand cmd5 = new SqlCommand("INSERT INTO  runner_market (runner_name,back1,back2,back3,lay1,lay2,lay3,status,selection_id,market_id,sortPriority) " +
                                                                "VALUES ('" + runnerName + "' , '0' , '0' , '0' , '0' , '0' , '0' , 'activate' ,'" + selectionId + "' ,'" + betfair_id + "' ,'" + sortPriority + "') ", con);
                                                                cmd5.ExecuteNonQuery();
                                                                SqlCommand cmd6 = new SqlCommand("UPDATE matches SET teama ='" + Team_A + "' , teamb ='" + Team_B + "' WHERE betfair_id = '" + betfair_id + "' ", con);
                                                                cmd6.ExecuteNonQuery();
                                                                SqlCommand cmd7 = new SqlCommand("UPDATE markets SET runner_name_done ='done' WHERE runner_name_done='undone' AND betfair_id = '" + betfair_id + "' ", con);
                                                                cmd7.ExecuteNonQuery();

                                                            }
                                                        }

                                                    }

                                                }
                                            }

                                            con.Close();
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (TaskCanceledException ex)
            {

            }

            return View();
        }

        public ActionResult InsertMatches_X()
        {
            string EventTypeIDGet = "66";
            string LeagueIDGet = "66";
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("gully/add_bf_matches.php").Result;

                    var products = response.Content.ReadAsStringAsync().Result;
                    dynamic responseJson = JsonConvert.DeserializeObject(products);
                    for (int i = 0; i < responseJson.Count; i++)
                    {
                        string EventIDGet = responseJson[i]["event_code"];
                        string betfair_id = responseJson[i]["betfair_id"];
                        string is_access = responseJson[i]["is_access"];
                        string is_tv = responseJson[i]["is_tv"];
                        string name = responseJson[i]["name"];
                        string xmarket_code = responseJson[i]["xmarket_code"];
                        name = name.Replace("'", "''");
                        string openDate = responseJson[i]["time"];

                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            con.Open();
                            string stmt = "SELECT [match_time],[x_type] FROM matches where event_code='" + EventIDGet + "'";
                            using (SqlCommand dataCount = new SqlCommand(stmt, con))
                            {
                                var datamat = dataCount.ExecuteReader();
                                if (datamat.HasRows)
                                {
                                    datamat.Read();
                                    DateTime match_time = (DateTime)datamat["match_time"];
                                    string x_type = (string)datamat["x_type"];
                                    string cric_match_time1 = match_time.ToString("yyyy-MM-dd HH:mm:ss");
                                    if (cric_match_time1 != openDate || x_type != is_access)
                                    {
                                        SqlCommand cmd6new = new SqlCommand("UPDATE matches SET match_time ='" + openDate + "' ,x_type ='" + is_access + "'  WHERE event_code = '" + EventIDGet + "' ", con);
                                        cmd6new.ExecuteNonQuery();
                                    }
                                }
                                else
                                {

                                    SqlCommand cmd2 = new SqlCommand("INSERT INTO  matches (sport_id,match_title,league_id,event_code,status,match_time,betfair_id,x_code,teama,teamb) " +
                                    " VALUES ('" + EventTypeIDGet + "','" + name + "','" + LeagueIDGet + "','" + EventIDGet + "','OPEN','" + openDate + "','0','" + xmarket_code + "','nikhil','rakesh') ", con);
                                    int runquery = cmd2.ExecuteNonQuery();
                                    if (runquery == 1)
                                    {
                                        HttpResponseMessage response1 = client.GetAsync("gully/add_bf_mo.php?betfair_id=" + betfair_id).Result;

                                        var products1 = response1.Content.ReadAsStringAsync().Result;
                                        dynamic responseJson1 = JsonConvert.DeserializeObject(products1);
                                        string BetfairId = responseJson1.marketId;
                                        string marketName = responseJson1.market;
                                        var runners = responseJson1.runner;

                                        if (marketName == "Match Odds")
                                        {
                                            marketName = "Match Odds";
                                            string stmt1 = "SELECT COUNT(*) FROM markets where event_code='" + BetfairId + "' ";
                                            int countmktm = 0;
                                            using (SqlCommand cmdCount = new SqlCommand(stmt1, con))
                                            {
                                                countmktm = (int)cmdCount.ExecuteScalar();
                                            }
                                            if (countmktm < 1)
                                            {

                                                DateTime time = DateTime.Now;              // Use current time
                                                string format1 = "yyyy-MM-dd HH:mm:ss";

                                                SqlCommand cmd3 = new SqlCommand("INSERT INTO  markets (market_name,back1,lay1,back2,lay2,back3,lay3,status,result_value," +
                                                "market_settle,created,league_id,sport_id,betfair_id,event_code,nested_market,runner_name_done) " +
                                                "VALUES ('" + marketName + "' ,'' ,'' ,'' ,'' ,'' ,'' ,'activate' ,'' ,'' ,'" + time.ToString(format1) + "' ,'" + LeagueIDGet + "' ,'" + EventTypeIDGet + "' ,'" + BetfairId + "' ,'" + EventIDGet + "' ,'' ,'undone') ", con);
                                                int runquery1 = cmd3.ExecuteNonQuery();
                                                if (runquery1 == 1)
                                                {
                                                    SqlCommand cmd4 = new SqlCommand("UPDATE matches SET betfair_id ='" + BetfairId + "' WHERE event_code = '" + EventIDGet + "' ", con);
                                                    int runquery2 = cmd4.ExecuteNonQuery();
                                                    if (runquery2 == 1)
                                                    {
                                                        string Team_A = "";
                                                        string Team_B = "";
                                                        for (int i3 = 0; i3 < runners.Count; i3++)
                                                        {
                                                            string runnerName = runners[i3].runnerName;
                                                            runnerName = runnerName.Replace("'", "''");
                                                            string sortPriority = runners[i3].sortPriority;
                                                            string selectionId = "666666" + sortPriority;
                                                            if (i3 == 0)
                                                            {
                                                                Team_A = runnerName;
                                                            }
                                                            else if (i3 == 1)
                                                            {
                                                                Team_B = runnerName;
                                                            }
                                                            string stmtr = "SELECT COUNT(*) FROM runner_market where market_id='" + BetfairId + "' AND sortPriority='" + sortPriority + "' ";
                                                            int countmktmr = 0;
                                                            using (SqlCommand cmdCount = new SqlCommand(stmtr, con))
                                                            {
                                                                countmktmr = (int)cmdCount.ExecuteScalar();
                                                            }
                                                            if (countmktmr < 1)
                                                            {
                                                                SqlCommand cmd5 = new SqlCommand("INSERT INTO  runner_market (runner_name,back1,back2,back3,lay1,lay2,lay3,status,selection_id,market_id,sortPriority) " +
                                                                "VALUES ('" + runnerName + "' , '0' , '0' , '0' , '0' , '0' , '0' , 'activate' ,'" + selectionId + "' ,'" + betfair_id + "' ,'" + sortPriority + "') ", con);
                                                                cmd5.ExecuteNonQuery();
                                                                SqlCommand cmd6 = new SqlCommand("UPDATE matches SET teama ='" + Team_A + "' , teamb ='" + Team_B + "' WHERE betfair_id = '" + betfair_id + "' ", con);
                                                                cmd6.ExecuteNonQuery();
                                                                SqlCommand cmd7 = new SqlCommand("UPDATE markets SET runner_name_done ='done' WHERE runner_name_done='undone' AND betfair_id = '" + betfair_id + "' ", con);
                                                                cmd7.ExecuteNonQuery();

                                                            }
                                                        }

                                                    }

                                                }
                                            }

                                            con.Close();
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (TaskCanceledException ex)
            {

            }

            return View();
        }
        public ActionResult InsertMarkets(string EventIDGet, string LeagueIDGet, string EventTypeIDGet)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://www.api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response1 = client.GetAsync("markets.php?event_type_id=" + EventTypeIDGet + "&event_code==" + EventIDGet).Result;

                    var products1 = response1.Content.ReadAsStringAsync().Result;
                    dynamic responseJson1 = JsonConvert.DeserializeObject(products1);
                    for (int i1 = 0; i1 < responseJson1.Count; i1++)
                    {
                        string BetfairId = responseJson1[i1].marketId;
                        string marketName = responseJson1[i1].marketName;

                        if (marketName == "Match Odds" || marketName == "Moneyline")
                        {
                            marketName = "Match Odds";
                            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                            {
                                con.Open();
                                string stmt1 = "SELECT COUNT(*) FROM markets where event_code='" + BetfairId + "' ";
                                int countmktm = 0;
                                using (SqlCommand cmdCount = new SqlCommand(stmt1, con))
                                {
                                    countmktm = (int)cmdCount.ExecuteScalar();
                                }
                                if (countmktm < 1)
                                {

                                    DateTime time = DateTime.Now;              // Use current time
                                    string format1 = "yyyy-MM-dd HH:mm:ss";

                                    SqlCommand cmd3 = new SqlCommand("INSERT INTO  markets (market_name,back1,lay1,back2,lay2,back3,lay3,status,result_value," +
                                    "market_settle,created,league_id,sport_id,betfair_id,event_code,nested_market,runner_name_done) " +
                                   "VALUES ('" + marketName + "' ,'' ,'' ,'' ,'' ,'' ,'' ,'activate' ,'' ,'' ,'" + time.ToString(format1) + "' ,'" + LeagueIDGet + "' ,'" + EventTypeIDGet + "' ,'" + BetfairId + "' ,'" + EventIDGet + "' ,'' ,'undone') ", con);
                                    int runquery1 = cmd3.ExecuteNonQuery();
                                    if (runquery1 == 1)
                                    {
                                        SqlCommand cmd4 = new SqlCommand("UPDATE matches SET betfair_id ='" + BetfairId + "' WHERE event_code = '" + EventIDGet + "' ", con);
                                        int runquery2 = cmd4.ExecuteNonQuery();
                                        if (runquery2 == 1)
                                        {
                                            HttpResponseMessage response2 = client.GetAsync("selection.php?betfair_id=" + BetfairId).Result;

                                            var products2 = response2.Content.ReadAsStringAsync().Result;
                                            dynamic responseJson2 = JsonConvert.DeserializeObject(products2);

                                            for (int i2 = 0; i2 < responseJson2.Count; i2++)
                                            {
                                                var runners = responseJson2[i2].runners;
                                                string marketId = responseJson2[i2].marketId;
                                                string marketName1 = responseJson2[i2].marketName;
                                                string Team_A = "";
                                                string Team_B = "";
                                                for (int i3 = 0; i3 < runners.Count; i3++)
                                                {
                                                    string selectionId = runners[i3].selectionId;
                                                    string runnerName = runners[i3].runnerName;
                                                    runnerName = runnerName.Replace("'", "''");
                                                    string sortPriority = runners[i3].sortPriority;
                                                    if (i3 == 0)
                                                    {
                                                        Team_A = runnerName;
                                                    }
                                                    else if (i3 == 1)
                                                    {
                                                        Team_B = runnerName;
                                                    }
                                                    string stmtr = "SELECT COUNT(*) FROM runner_market where market_id='" + BetfairId + "' AND sortPriority='" + sortPriority + "' ";
                                                    int countmktmr = 0;
                                                    using (SqlCommand cmdCount = new SqlCommand(stmtr, con))
                                                    {
                                                        countmktmr = (int)cmdCount.ExecuteScalar();
                                                    }
                                                    if (countmktmr < 1)
                                                    {
                                                        SqlCommand cmd5 = new SqlCommand("INSERT INTO  runner_market (runner_name,back1,back2,back3,lay1,lay2,lay3,status,selection_id,market_id,sortPriority) " +
                                                       "VALUES ('" + runnerName + "' , '0' , '0' , '0' , '0' , '0' , '0' , 'activate' ,'" + selectionId + "' ,'" + marketId + "' ,'" + sortPriority + "') ", con);
                                                        cmd5.ExecuteNonQuery();
                                                        SqlCommand cmd6 = new SqlCommand("UPDATE matches SET teama ='" + Team_A + "' , teamb ='" + Team_B + "' WHERE betfair_id = '" + marketId + "' ", con);
                                                        cmd6.ExecuteNonQuery();
                                                        SqlCommand cmd7 = new SqlCommand("UPDATE markets SET runner_name_done ='done' WHERE runner_name_done='undone' AND betfair_id = '" + marketId + "' ", con);
                                                        cmd7.ExecuteNonQuery();

                                                    }
                                                }
                                            }

                                        }

                                    }
                                }

                                con.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (TaskCanceledException ex)
            {

            }
            return View();
        }

        public ActionResult AddRunnerNameForMarkets(string BetfairId)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://www.api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("selection.php?betfair_id=" + BetfairId).Result;

                    var products = response.Content.ReadAsStringAsync().Result;
                    dynamic responseJson = JsonConvert.DeserializeObject(products);

                    for (int i = 0; i < responseJson.Count; i++)
                    {
                        var runners = responseJson[i].runners;
                        string marketId = responseJson[i].marketId;
                        string marketName = responseJson[i].marketName;
                        string Team_A = "";
                        string Team_B = "";
                        for (int i1 = 0; i1 < runners.Count; i1++)
                        {
                            string selectionId = responseJson[i].runners[i1].selectionId;
                            string runnerName = responseJson[i].runners[i1].runnerName;
                            runnerName = runnerName.Replace("'", "''");
                            string sortPriority = responseJson[i].runners[i1].sortPriority;
                            if (i1 == 0)
                            {
                                Team_A = runnerName;
                            }
                            else if (i1 == 1)
                            {
                                Team_B = runnerName;
                            }
                            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                            {
                                con.Open();
                                string stmtr = "SELECT COUNT(*) FROM runner_market where market_id='" + BetfairId + "' AND sortPriority='" + sortPriority + "' ";
                                int countmktmr = 0;
                                using (SqlCommand cmdCount = new SqlCommand(stmtr, con))
                                {
                                    countmktmr = (int)cmdCount.ExecuteScalar();
                                }
                                if (countmktmr < 1)
                                {
                                    SqlCommand cmd2 = new SqlCommand("INSERT INTO  runner_market (runner_name,back1,back2,back3,lay1,lay2,lay3,status,selection_id,market_id,sortPriority) " +
                                   "VALUES ('" + runnerName + "' , '0' , '0' , '0' , '0' , '0' , '0' , 'activate' ,'" + selectionId + "' ,'" + marketId + "' ,'" + sortPriority + "') ", con);
                                    cmd2.ExecuteNonQuery();
                                    if (marketName == "Match Odds")
                                    {
                                        SqlCommand cmd3 = new SqlCommand("UPDATE matches SET teama ='" + Team_A + "' , teamb ='" + Team_B + "' WHERE betfair_id = '" + marketId + "' ", con);
                                        cmd3.ExecuteNonQuery();
                                    }
                                    SqlCommand cmd4 = new SqlCommand("UPDATE markets SET runner_name_done ='done' WHERE runner_name_done='undone' AND betfair_id = '" + marketId + "' ", con);
                                    cmd4.ExecuteNonQuery();

                                }
                                con.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (TaskCanceledException ex)
            {

            }
            return View();
        }

        public void CheckStatus(string BfId)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://www.betfair.com/www/sports/exchange/readonly/v1/bymarket");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("?_ak=nzIFcwyWhrlwYMrh&alt=json&currencyCode=USD&locale=en&marketIds=" + BfId + "&rollupLimit=25&rollupModel=STAKE&types=MARKET_STATE").Result;

                    var products = response.Content.ReadAsStringAsync().Result;
                    dynamic responseJson = JsonConvert.DeserializeObject(products);

                    if (responseJson.eventTypes != null || responseJson.eventTypes != "")
                    {
                        try
                        {
                            if (responseJson.eventTypes[0].eventNodes[0].marketNodes[0].state.status != null)
                            {
                                string STATUS = responseJson.eventTypes[0].eventNodes[0].marketNodes[0].state.status;
                                string event_code = responseJson.eventTypes[0].eventNodes[0].eventId;
                                if (STATUS != "OPEN")
                                {
                                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                                    {
                                        con.Open();
                                        string stmtcli = "SELECT COUNT(id) FROM live_bet where event_id = '" + event_code + "' AND status='' ";
                                        int countinplaycli = 0;
                                        using (SqlCommand cmdCountcli = new SqlCommand(stmtcli, con))
                                        {
                                            countinplaycli = (int)cmdCountcli.ExecuteScalar();
                                            if (countinplaycli == 0)
                                            {
                                                SqlCommand sqlLivematch = new SqlCommand("UPDATE matches SET status='" + STATUS + "' WHERE event_code='" + event_code + "' ", con);
                                                sqlLivematch.ExecuteNonQuery();
                                            }
                                        }
                                        con.Close();
                                    }
                                }
                            }
                            else
                            {

                            }
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (TaskCanceledException ex)
            {

            }
        }

        static public string Submitdfx(string id, string market_id)
        {
            string ReturnMessage = "Error";
            DateTime time = DateTime.Now;
            string format = "yyyy-MM-dd HH:mm:ss";
            string marketName = "Round " + market_id;
            if (market_id != "" && id != "")
            {
                int player1 = 0;
                int player2 = 0;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    SqlCommand cmd3 = new SqlCommand("INSERT INTO  markets (market_name,back1,lay1,back2,lay2,back3,lay3,status,result_value," +
                            "market_settle,created,league_id,sport_id,betfair_id,event_code,nested_market,runner_name_done,type) " +
                           "VALUES ('" + marketName + "' ,'' ,'' ,'' ,'' ,'' ,'' ,'activate' ,'' ,'' ,'" + time.ToString(format) + "' ,'" + id + "' ,'7888' ,'" + market_id + "' ,'" + id + "' ,'' ,'done','TP') ", con);
                    int runquery1 = cmd3.ExecuteNonQuery();
                    if (runquery1 == 1)
                    {
                        SqlCommand cmd5 = new SqlCommand("INSERT INTO  runner_market (runner_name,back1,back2,back3,lay1,lay2,lay3,status,selection_id,market_id,sortPriority) " +
                               "VALUES ('PlayerA' , '1.98' , '0' , '0' , '0' , '0' , '0' , 'activate' ,'20202020 ' ,'" + market_id + "' ,'1') ", con);
                        player1 = cmd5.ExecuteNonQuery();
                        SqlCommand cmd51 = new SqlCommand("INSERT INTO  runner_market (runner_name,back1,back2,back3,lay1,lay2,lay3,status,selection_id,market_id,sortPriority) " +
                               "VALUES ('PlayerB' , '1.98' , '0' , '0' , '0' , '0' , '0' , 'activate' ,'21212121' ,'" + market_id + "' ,'2') ", con);
                        player2 = cmd51.ExecuteNonQuery();

                    }
                    if (player1 > 0 && player2 > 0)
                    {
                        ReturnMessage = "Success";
                    }
                    con.Close();
                }

            }
            else
            {
                ReturnMessage = "Data not coming";
            }
            return ReturnMessage;
        }


        static public string Submitdfxs(string id, string market_id)
        {
            string ReturnMessage = "Error";
            DateTime time = DateTime.Now;
            string format = "yyyy-MM-dd HH:mm:ss";
            string marketName = "Round " + market_id;
            if (market_id != "" && id != "")
            {
                int player1 = 0;
                int player2 = 0;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionBFPBF"].ToString()))
                {
                    con.Open();
                    SqlCommand cmd3 = new SqlCommand("INSERT INTO  markets (market_name,back1,lay1,back2,lay2,back3,lay3,status,result_value," +
                            "market_settle,created,league_id,sport_id,betfair_id,event_code,nested_market,runner_name_done,type) " +
                           "VALUES ('" + marketName + "' ,'' ,'' ,'' ,'' ,'' ,'' ,'activate' ,'' ,'' ,'" + time.ToString(format) + "' ,'" + id + "' ,'7888' ,'" + market_id + "' ,'" + id + "' ,'' ,'done','TP') ", con);
                    int runquery1 = cmd3.ExecuteNonQuery();
                    if (runquery1 == 1)
                    {
                        SqlCommand cmd5 = new SqlCommand("INSERT INTO  runner_market (runner_name,back1,back2,back3,lay1,lay2,lay3,status,selection_id,market_id,sortPriority) " +
                               "VALUES ('PlayerA' , '1.98' , '0' , '0' , '0' , '0' , '0' , 'activate' ,'20202020 ' ,'" + market_id + "' ,'1') ", con);
                        player1 = cmd5.ExecuteNonQuery();
                        SqlCommand cmd51 = new SqlCommand("INSERT INTO  runner_market (runner_name,back1,back2,back3,lay1,lay2,lay3,status,selection_id,market_id,sortPriority) " +
                               "VALUES ('PlayerB' , '1.98' , '0' , '0' , '0' , '0' , '0' , 'activate' ,'21212121' ,'" + market_id + "' ,'2') ", con);
                        player2 = cmd51.ExecuteNonQuery();

                    }
                    if (player1 > 0 && player2 > 0)
                    {
                        ReturnMessage = "Success";
                    }
                    con.Close();
                }

            }
            else
            {
                ReturnMessage = "Data not coming";
            }
            return ReturnMessage;
        }

        static public string Submitdfxsb(string id, string market_id)
        {
            string ReturnMessage = "Error";
            DateTime time = DateTime.Now;
            string format = "yyyy-MM-dd HH:mm:ss";
            string marketName = "Round " + market_id;
            if (market_id != "" && id != "")
            {
                int player1 = 0;
                int player2 = 0;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionBFF"].ToString()))
                {
                    con.Open();
                    SqlCommand cmd3 = new SqlCommand("INSERT INTO  markets (market_name,back1,lay1,back2,lay2,back3,lay3,status,result_value," +
                            "market_settle,created,league_id,sport_id,betfair_id,event_code,nested_market,runner_name_done,type) " +
                           "VALUES ('" + marketName + "' ,'' ,'' ,'' ,'' ,'' ,'' ,'activate' ,'' ,'' ,'" + time.ToString(format) + "' ,'" + id + "' ,'7888' ,'" + market_id + "' ,'" + id + "' ,'' ,'done','TP') ", con);
                    int runquery1 = cmd3.ExecuteNonQuery();
                    if (runquery1 == 1)
                    {
                        SqlCommand cmd5 = new SqlCommand("INSERT INTO  runner_market (runner_name,back1,back2,back3,lay1,lay2,lay3,status,selection_id,market_id,sortPriority) " +
                               "VALUES ('PlayerA' , '1.98' , '0' , '0' , '0' , '0' , '0' , 'activate' ,'20202020 ' ,'" + market_id + "' ,'1') ", con);
                        player1 = cmd5.ExecuteNonQuery();
                        SqlCommand cmd51 = new SqlCommand("INSERT INTO  runner_market (runner_name,back1,back2,back3,lay1,lay2,lay3,status,selection_id,market_id,sortPriority) " +
                               "VALUES ('PlayerB' , '1.98' , '0' , '0' , '0' , '0' , '0' , 'activate' ,'21212121' ,'" + market_id + "' ,'2') ", con);
                        player2 = cmd51.ExecuteNonQuery();

                    }
                    if (player1 > 0 && player2 > 0)
                    {
                        ReturnMessage = "Success";
                    }
                    con.Close();
                }

            }
            else
            {
                ReturnMessage = "Data not coming";
            }
            return ReturnMessage;
        } 
        static public string Submitdfxsbar(string id, string market_id)
        {
            string ReturnMessage = "Error";
            DateTime time = DateTime.Now;
            string format = "yyyy-MM-dd HH:mm:ss";
            string marketName = "Round " + market_id;
            if (market_id != "" && id != "")
            {
                int player1 = 0;
                int player2 = 0;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionBart"].ToString()))
                {
                    con.Open();
                    SqlCommand cmd3 = new SqlCommand("INSERT INTO  markets (market_name,back1,lay1,back2,lay2,back3,lay3,status,result_value," +
                            "market_settle,created,league_id,sport_id,betfair_id,event_code,nested_market,runner_name_done,type) " +
                           "VALUES ('" + marketName + "' ,'' ,'' ,'' ,'' ,'' ,'' ,'activate' ,'' ,'' ,'" + time.ToString(format) + "' ,'" + id + "' ,'7888' ,'" + market_id + "' ,'" + id + "' ,'' ,'done','TP') ", con);
                    int runquery1 = cmd3.ExecuteNonQuery();
                    if (runquery1 == 1)
                    {
                        SqlCommand cmd5 = new SqlCommand("INSERT INTO  runner_market (runner_name,back1,back2,back3,lay1,lay2,lay3,status,selection_id,market_id,sortPriority) " +
                               "VALUES ('PlayerA' , '1.98' , '0' , '0' , '0' , '0' , '0' , 'activate' ,'20202020 ' ,'" + market_id + "' ,'1') ", con);
                        player1 = cmd5.ExecuteNonQuery();
                        SqlCommand cmd51 = new SqlCommand("INSERT INTO  runner_market (runner_name,back1,back2,back3,lay1,lay2,lay3,status,selection_id,market_id,sortPriority) " +
                               "VALUES ('PlayerB' , '1.98' , '0' , '0' , '0' , '0' , '0' , 'activate' ,'21212121' ,'" + market_id + "' ,'2') ", con);
                        player2 = cmd51.ExecuteNonQuery();

                    }
                    if (player1 > 0 && player2 > 0)
                    {
                        ReturnMessage = "Success";
                    }
                    con.Close();
                }

            }
            else
            {
                ReturnMessage = "Data not coming";
            }
            return ReturnMessage;
        }

        public string change_stat(string id, string status)
        {
            string ReturnMessage = "Error";
            DateTime time = DateTime.Now;
            string format = "yyyy-MM-dd HH:mm:ss";
            if (status != "" && id != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    SqlCommand cmd3 = new SqlCommand("UPDATE markets SET status='" + status + "' where betfair_id='" + id + "' ", con);
                    int runquery1 = cmd3.ExecuteNonQuery();
                    if (runquery1 == 1)
                    {
                        SqlCommand cmd4 = new SqlCommand("UPDATE runner_market SET status='" + status + "' where market_id='" + id + "' ", con);
                        cmd4.ExecuteNonQuery();
                        ReturnMessage = "Success";
                    }
                    con.Close();
                }
            }
            else
            {
                ReturnMessage = "Data not coming";
            }
            return ReturnMessage;
        }

        public string Set_winnerT(string id, string event_code, string winner)
        {
            string ReturnMessage = "Error";
            try
            {
                DateTime time = DateTime.Now;
                string format = "yyyy-MM-dd HH:mm:ss";
                string betfair_id = id;
                string eventSportName = FunctionDataController.GetSportsNameFromEvent(event_code);
                string EventName = FunctionDataController.getEventName(event_code);
                string market_name = "Round : " + id;

                if (event_code != "" && betfair_id != "" && winner != "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        SqlCommand sqlFancy = new SqlCommand("SELECT DISTINCT user_id FROM live_bet WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_code + "' ", con);
                        var rowFancy = sqlFancy.ExecuteReader();
                        if (!rowFancy.HasRows)
                        {
                            SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='1' ,status='deactivate' , result_value='" + winner + "' WHERE status!='deactivate' AND result_value='' AND market_settle!='1' AND betfair_id = '" + betfair_id + "' AND event_code = '" + event_code + "' ", con);
                            int sqlMarketSettleUpdatedone = sqlMarketSettleUpdate.ExecuteNonQuery();
                            if (sqlMarketSettleUpdatedone > 0)
                            {
                                SqlCommand cmd4 = new SqlCommand("UPDATE runner_market SET status='deactivate' where market_id='" + betfair_id + "' ", con);
                                cmd4.ExecuteNonQuery();
                                con.Close();
                                return "Market Settle SuccessFully";
                            }
                            else
                            {
                                con.Close();
                                return "Market Already Settled";
                            }
                        }
                        else
                        {
                            SqlCommand marketde = new SqlCommand("UPDATE markets SET status='deactivate' WHERE status!='deactivate' AND market_settle!='1' AND betfair_id = '" + betfair_id + "' AND event_code = '" + event_code + "' ", con);
                            int sqlMarketUpdatedone = marketde.ExecuteNonQuery();
                            SqlCommand cmd41 = new SqlCommand("UPDATE runner_market SET status='deactivate' where market_id='" + betfair_id + "' ", con);
                            cmd41.ExecuteNonQuery();
                            if (sqlMarketUpdatedone > 0)
                            {
                                while (rowFancy.Read())
                                {
                                    int uid = (int)rowFancy["user_id"];
                                    int user_id = uid;
                                    Double total_liability = 0;
                                    Double availableLiability = 0;

                                    SqlCommand sql_new = new SqlCommand("SELECT [id] FROM live_bet WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_code + "' ", con);
                                    var result_new = sql_new.ExecuteReader();
                                    if (result_new.HasRows)
                                    {
                                        SqlCommand sqlUser2 = new SqlCommand("SELECT [balance],[exposure],[profit_loss],[admin_id],[mdl_id],[dl_id] FROM users_client WHERE id='" + user_id + "' ", con);
                                        var dataUser2 = sqlUser2.ExecuteReader();
                                        dataUser2.Read();
                                        Double UserBalance = (Double)dataUser2["balance"];
                                        Double UserLiability = (Double)dataUser2["exposure"];
                                        Double ClientPL = (Double)dataUser2["profit_loss"];
                                        int Admin_id = (int)dataUser2["admin_id"];
                                        int UserMDId = (int)dataUser2["mdl_id"];
                                        int UserDistId = (int)dataUser2["dl_id"];
                                        availableLiability = UserLiability;

                                        Double TotalPLWon = 0;
                                        SqlCommand sqlWon = new SqlCommand("SELECT COALESCE(SUM(total_value),0) as win_val FROM live_bet WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND user_id='" + uid + "' AND runner_posi = '" + winner + "' ", con);
                                        var dataWon = sqlWon.ExecuteReader();
                                        if (dataWon.HasRows)
                                        {
                                            dataWon.Read();
                                            TotalPLWon = (Double)dataWon["win_val"];
                                        }

                                        Double TotalStakesLost = 0;
                                        SqlCommand sqlLost = new SqlCommand("SELECT COALESCE(SUM(stakes),0) as lost_val FROM live_bet WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND user_id='" + uid + "' AND runner_posi != '" + winner + "' ", con);
                                        var dataLost = sqlLost.ExecuteReader();
                                        if (dataLost.HasRows)
                                        {
                                            dataLost.Read();
                                            TotalStakesLost = (Double)dataLost["lost_val"];
                                        }

                                        SqlCommand sqlFancyExch = new SqlCommand("SELECT COALESCE(SUM(stakes),0) as tot_stak FROM live_bet WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND user_id='" + uid + "' ", con);
                                        var dataFancyExch = sqlFancyExch.ExecuteReader();
                                        if (dataFancyExch.HasRows)
                                        {
                                            dataFancyExch.Read();
                                            total_liability = (Double)dataFancyExch["tot_stak"];
                                        }
                                        availableLiability = UserLiability - total_liability;

                                        Double NetWinLost = TotalPLWon - TotalStakesLost;
                                        Double debit_balance = 0;
                                        Double credit_balance = 0;
                                        if (NetWinLost < 0)
                                        {
                                            debit_balance = -NetWinLost;
                                            credit_balance = 0;
                                        }
                                        else
                                        {
                                            debit_balance = 0;
                                            credit_balance = NetWinLost;
                                        }

                                        Double ClientNetBalance = UserBalance + NetWinLost + total_liability;
                                        Double user_updated_net_balance = ClientNetBalance + availableLiability;

                                        Double NetWinLoss = NetWinLost;
                                        Double NetClientPL = ClientPL + NetWinLoss;
                                        SqlCommand sqlLiveBetWon = new SqlCommand("UPDATE live_bet SET status='won' WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND runner_posi='" + winner + "' ", con);
                                        sqlLiveBetWon.ExecuteNonQuery();

                                        SqlCommand sqlLiveBetLost = new SqlCommand("UPDATE live_bet SET status='lost' WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND runner_posi!='" + winner + "' ", con);
                                        sqlLiveBetLost.ExecuteNonQuery();

                                        SqlCommand sqlUserBalanceUpdate = new SqlCommand("UPDATE users_client SET balance='" + ClientNetBalance + "' , exposure='" + availableLiability + "' , profit_loss='" + NetClientPL + "' WHERE id = '" + user_id + "' ", con);
                                        sqlUserBalanceUpdate.ExecuteNonQuery();

                                        string AccStatementDescription = eventSportName + " / " + EventName + " / " + market_name;

                                        SqlCommand sqlInsertUser = new SqlCommand("INSERT INTO user_account_statements (admin_id , md_id , dist_id , user_id , acc_stat_type ,  description ,  remark ,  debit ,  credit ,  balance ,  market_id ,  event_id ,  created , match_odds ) VALUES ('" + Admin_id + "','" + UserMDId + "','" + UserDistId + "','" + user_id + "','pl_coins','" + AccStatementDescription + "','','" + debit_balance + "','" + credit_balance + "','" + user_updated_net_balance + "','" + betfair_id + "','" + event_code + "','" + time.ToString(format) + "','2')", con);
                                        sqlInsertUser.ExecuteNonQuery();
                                    }

                                    SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='1' , result_value='" + winner + "' WHERE status='deactivate' AND betfair_id = '" + betfair_id + "' AND event_code='" + event_code + "' AND result_value='' AND market_settle!='1'  ", con);
                                    sqlMarketSettleUpdate.ExecuteNonQuery();

                                    con.Close();
                                    return "Market Settle SuccessFully";
                                }
                            }
                            else
                            {
                                con.Close();
                                return "Erro in Market Settle";
                            }
                        }
                        con.Close();
                    }
                }

                else
                {
                    ReturnMessage = "Data not coming";
                }
            }
            catch (Exception ex)
            {
                ReturnMessage = "Error";
            }
            return ReturnMessage;
        }

        static public string AddTPData(string id, string market_id)
        {
            string ReturnMessage = "Error";
            DateTime time = DateTime.Now;
            string format = "yyyy-MM-dd HH:mm:ss";
            string marketName = "Round " + market_id;
            if (market_id != "" && id != "")
            {
                string evv_name = "";
                if (id == "40404040")
                {
                    evv_name = "TeenPatiT20Poker";
                }
                else if (id == "50505050")
                {
                    evv_name = "dt20";
                }
                else if (id == "51515151")
                {
                    evv_name = "dtl20";
                }
                else if (id == "60606060")
                {
                    evv_name = "lucky7";
                }
                else
                {

                }
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("t20api.php?type=" + evv_name).Result;
                    var products = response.Content.ReadAsStringAsync().Result;
                    dynamic responseJson = JsonConvert.DeserializeObject(products);
                    Boolean successs = responseJson.success;
                    if (successs == true)
                    {
                        var res_data = responseJson.data.t2;
                        int vjhvh = res_data.Count;
                        if (vjhvh > 0)
                        {
                            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                            {
                                con.Open();
                                SqlCommand cmd3 = new SqlCommand("INSERT INTO  markets (market_name,back1,lay1,back2,lay2,back3,lay3,status,result_value," +
                                        "market_settle,created,league_id,sport_id,betfair_id,event_code,nested_market,runner_name_done,type) " +
                                       "VALUES ('" + marketName + "' ,'' ,'' ,'' ,'' ,'' ,'' ,'activate' ,'' ,'' ,'" + time.ToString(format) + "' ,'" + id + "' ,'7888' ,'" + market_id + "' ,'" + id + "' ,'' ,'done','TP') ", con);
                                int runquery1 = cmd3.ExecuteNonQuery();
                                if (runquery1 == 1)
                                {
                                    int uhiu = 1;
                                    for (int i = 0; i < res_data.Count; i++)
                                    {
                                        string nation = "";
                                        if (id == "40404040")
                                        {
                                            nation = res_data[i]["nation"];
                                            if (i == 0)
                                            {
                                                nation = nation + " (Player A)";
                                            }
                                            else if (i == 1)
                                            {
                                                nation = nation + " (Player B)";
                                            }
                                        }
                                        else
                                        {
                                            nation = res_data[i]["nat"];
                                        }

                                        string rate = res_data[i]["rate"];
                                        string sid = res_data[i]["sid"];
                                        string sidss = id + "_" + sid;
                                        SqlCommand cmd5 = new SqlCommand("INSERT INTO  runner_market (runner_name,back1,back2,back3,lay1,lay2,lay3,status,selection_id,market_id,sortPriority) " +
                                           "VALUES ('" + nation + "' , '" + rate + "' , '0' , '0' , '0' , '0' , '0' , 'activate' ,'" + sidss + " ' ,'" + market_id + "' ,'" + sid + "') ", con);
                                        cmd5.ExecuteNonQuery();
                                        uhiu++;
                                    }

                                    if (uhiu == vjhvh)
                                    {
                                        ReturnMessage = "Success";
                                    }
                                }
                                con.Close();
                            }
                        }


                    }

                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                ReturnMessage = "Data not coming";
            }
            return ReturnMessage;
        }

        public ActionResult AddMatch()
        {
            var Livebetpp = new List<LiveBetPr>();
            string spidd = "0";
            string eidd = "0";
            string midd = "0";
            string mst = "";
            if (Request.QueryString["sport_id"] != null)
            {
                spidd = Request.QueryString["sport_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT sport_name FROM sports where sport_id='" + spidd + "' "))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        reader.Read();
                        if (reader.HasRows)
                        {
                            ViewBag.spNamea = (string)reader["sport_name"];
                        }

                        con.Close();
                    }
                }
                ArrayList MatchForShowAllSportsL = new ArrayList();
                ArrayList MatchIDForShowAllSportsL = new ArrayList();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT match_title,event_code,match_time,status FROM matches WHERE sport_id='" + spidd + "'AND status='deactivate' order by match_time "))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                MatchForShowAllSportsL.Add((string)reader["match_title"]);
                                MatchIDForShowAllSportsL.Add((string)reader["event_code"]);
                                Livebetpp.Add(item: new LiveBetPr
                                {
                                    time = (DateTime)reader["match_time"],
                                    matchTitle = (string)reader["match_title"],
                                    status = (string)reader["status"],
                                    event_code = (string)reader["event_code"]
                                });
                            }
                        }

                        con.Close();
                    }
                }
                ViewBag.MaName = MatchForShowAllSportsL;
                ViewBag.MaId = MatchIDForShowAllSportsL;
            }

            ArrayList SportsNameForShowAllSportsL = new ArrayList();
            ArrayList SportsIDForShowAllSportsL = new ArrayList();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT * FROM sports"))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SportsNameForShowAllSportsL.Add((string)reader["sport_name"]);
                        SportsIDForShowAllSportsL.Add((string)reader["sport_id"]);
                    }
                    con.Close();
                }
            }
            ViewBag.SpName = SportsNameForShowAllSportsL;
            ViewBag.SpId = SportsIDForShowAllSportsL;

            ViewBag.sport_id = spidd;
            ViewBag.match_id = eidd;
            ViewBag.market_id = midd;
            ViewBag.status = mst;
            return View(Livebetpp);
        }
        public ActionResult FinishMatch()
        {
            var Livebetpp = new List<LiveBetPr>();
            string spidd = "0";
            string eidd = "0";
            string midd = "0";
            string mst = "";
            if (Request.QueryString["sport_id"] != null)
            {
                spidd = Request.QueryString["sport_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT sport_name FROM sports where sport_id='" + spidd + "' "))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        reader.Read();
                        if (reader.HasRows)
                        {
                            ViewBag.spNamea = (string)reader["sport_name"];
                        }

                        con.Close();
                    }
                }
                ArrayList MatchForShowAllSportsL = new ArrayList();
                ArrayList MatchIDForShowAllSportsL = new ArrayList();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT match_title,event_code,match_time,status FROM matches WHERE sport_id='" + spidd + "'AND status='OPEN' order by match_time "))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                MatchForShowAllSportsL.Add((string)reader["match_title"]);
                                MatchIDForShowAllSportsL.Add((string)reader["event_code"]);
                                Livebetpp.Add(item: new LiveBetPr
                                {
                                    time = (DateTime)reader["match_time"],
                                    matchTitle = (string)reader["match_title"],
                                    status = (string)reader["status"],
                                    event_code = (string)reader["event_code"]


                                });
                            }
                        }

                        con.Close();
                    }
                }
                ViewBag.MaName = MatchForShowAllSportsL;
                ViewBag.MaId = MatchIDForShowAllSportsL;
            }

            ArrayList SportsNameForShowAllSportsL = new ArrayList();
            ArrayList SportsIDForShowAllSportsL = new ArrayList();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT * FROM sports"))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SportsNameForShowAllSportsL.Add((string)reader["sport_name"]);
                        SportsIDForShowAllSportsL.Add((string)reader["sport_id"]);
                    }
                    con.Close();
                }
            }
            ViewBag.SpName = SportsNameForShowAllSportsL;
            ViewBag.SpId = SportsIDForShowAllSportsL;

            ViewBag.sport_id = spidd;
            ViewBag.match_id = eidd;
            ViewBag.market_id = midd;
            ViewBag.status = mst;
            return View(Livebetpp);
        }
        public ActionResult OpenMatch()
        {
            var Livebetpp = new List<LiveBetPr>();
            string spidd = "0";
            string eidd = "0";
            string midd = "0";
            string mst = "";
            if (Request.QueryString["sport_id"] != null)
            {
                spidd = Request.QueryString["sport_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT sport_name FROM sports where sport_id='" + spidd + "' "))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        reader.Read();
                        if (reader.HasRows)
                        {
                            ViewBag.spNamea = (string)reader["sport_name"];
                        }

                        con.Close();
                    }
                }
                ArrayList MatchForShowAllSportsL = new ArrayList();
                ArrayList MatchIDForShowAllSportsL = new ArrayList();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT Top(50) match_title,event_code,match_time,status FROM matches WHERE sport_id='" + spidd + "'AND status='CLOSED' order by match_time desc "))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                MatchForShowAllSportsL.Add((string)reader["match_title"]);
                                MatchIDForShowAllSportsL.Add((string)reader["event_code"]);
                                Livebetpp.Add(item: new LiveBetPr
                                {
                                    time = (DateTime)reader["match_time"],
                                    matchTitle = (string)reader["match_title"],
                                    status = (string)reader["status"],
                                    event_code = (string)reader["event_code"]


                                });
                            }
                        }

                        con.Close();
                    }
                }
                ViewBag.MaName = MatchForShowAllSportsL;
                ViewBag.MaId = MatchIDForShowAllSportsL;
            }

            ArrayList SportsNameForShowAllSportsL = new ArrayList();
            ArrayList SportsIDForShowAllSportsL = new ArrayList();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT * FROM sports"))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SportsNameForShowAllSportsL.Add((string)reader["sport_name"]);
                        SportsIDForShowAllSportsL.Add((string)reader["sport_id"]);
                    }
                    con.Close();
                }
            }
            ViewBag.SpName = SportsNameForShowAllSportsL;
            ViewBag.SpId = SportsIDForShowAllSportsL;

            ViewBag.sport_id = spidd;
            ViewBag.match_id = eidd;
            ViewBag.market_id = midd;
            ViewBag.status = mst;
            return View(Livebetpp);
        }
        public string MatchOpenXA(string event_code, string status)
        {
            string ResponseS = "Failed";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                SqlCommand cmd4 = new SqlCommand("UPDATE matches SET status='" + status + "' where event_code='" + event_code + "' ", con);
                int result = cmd4.ExecuteNonQuery();
                if (result > 0)
                {
                    ResponseS = "Success";
                }
                con.Close();
            }
            return ResponseS;
        }


        public String xcodeupdate(string x_type, string x_code, string event_code)
        {
            string SenR = "Failed";
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    string stmt = "SELECT id FROM matches where event_code='" + event_code + "'";
                    using (SqlCommand dataCount = new SqlCommand(stmt, con))
                    {
                        var datamat = dataCount.ExecuteReader();
                        if (datamat.HasRows)
                        {
                            SqlCommand cmd1 = new SqlCommand("UPDATE matches SET x_type ='" + x_type + "' , x_code ='" + x_code + "' WHERE event_code = '" + event_code + "' ", con);
                            cmd1.ExecuteNonQuery();

                            SenR = "X Code Update SuccessFully";
                        }
                        else
                        {
                            SenR = "Error";

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return SenR;
        }
        public String isfancyupdate(string e_sport, string is_fancy, string event_code)
        {
            string SenR = "Failed";
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    string stmt = "SELECT id FROM matches where event_code='" + event_code + "'";
                    using (SqlCommand dataCount = new SqlCommand(stmt, con))
                    {
                        var datamat = dataCount.ExecuteReader();
                        if (datamat.HasRows)
                        {
                            SqlCommand cmd1 = new SqlCommand("UPDATE matches SET e_sport ='" + e_sport + "' , is_fancy ='" + is_fancy + "' WHERE event_code = '" + event_code + "' ", con);
                            cmd1.ExecuteNonQuery();

                            SenR = "Is Fancy Update SuccessFully";
                        }
                        else
                        {
                            SenR = "Error";

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return SenR;
        }
        public ActionResult SetLimit()
        {
            string idd = "";
            Double mo_limit = 0;
            Double bm_limit = 0;
            Double sess_limit = 0;
            Double mo_limit_bp = 0;
            Double sess_limit_bp = 0;
            if (Request.QueryString["id"] != null)
            {
                idd = Request.QueryString["id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    string stmt = "SELECT max_limit_mo,max_limit_mo_bm,max_limit_sess,max_limit_mo_bp,max_limit_sess_bp FROM matches where event_code='" + idd + "'";
                    using (SqlCommand dataCount = new SqlCommand(stmt, con))
                    {
                        var datamat = dataCount.ExecuteReader();
                        if (datamat.HasRows)
                        {
                            datamat.Read();
                            mo_limit = (Double)datamat["max_limit_mo"];
                            bm_limit = (Double)datamat["max_limit_mo_bm"];
                            sess_limit = (Double)datamat["max_limit_sess"];
                            mo_limit_bp = (Double)datamat["max_limit_mo_bp"];
                            sess_limit_bp = (Double)datamat["max_limit_sess_bp"];
                        }
                    }
                }
            }
            ViewBag.id = idd;
            ViewBag.mo_limit = mo_limit;
            ViewBag.bm_limit = bm_limit;
            ViewBag.sess_limit = sess_limit;
            ViewBag.mo_limit_bp = mo_limit_bp;
            ViewBag.sess_limit_bp = sess_limit_bp;
            return View();
        }
        public String UpdateLimit(string mo_limit, string bm_limit, string sess_limit, string mo_limit_bp, string sess_limit_bp, string event_code)
        {
            string SenR = "Failed";
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    string stmt = "SELECT id FROM matches where event_code='" + event_code + "'";
                    using (SqlCommand dataCount = new SqlCommand(stmt, con))
                    {
                        var datamat = dataCount.ExecuteReader();
                        if (datamat.HasRows)
                        {
                            SqlCommand cmd1 = new SqlCommand("UPDATE matches SET max_limit_mo ='" + mo_limit + "' ,max_limit_mo_bm ='" + bm_limit + "' , max_limit_sess ='" + sess_limit + "',max_limit_mo_bp ='" + mo_limit_bp + "' , max_limit_sess_bp ='" + sess_limit_bp + "' WHERE event_code = '" + event_code + "' ", con);
                            cmd1.ExecuteNonQuery();

                            SenR = "Limit Update SuccessFully";
                        }
                        else
                        {
                            SenR = "Error";

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return SenR;
        }
        public ActionResult BetDelay()
        {
            string idd = "";
            int mo_delay = 0;
            int sess_delay = 0;
            int bm_delay = 0;
            if (Request.QueryString["id"] != null)
            {
                idd = Request.QueryString["id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    string stmt = "SELECT mo_delay,sess_delay,bm_delay FROM matches where event_code='" + idd + "'";
                    using (SqlCommand dataCount = new SqlCommand(stmt, con))
                    {
                        var datamat = dataCount.ExecuteReader();
                        if (datamat.HasRows)
                        {
                            datamat.Read();
                            mo_delay = (int)datamat["mo_delay"];
                            sess_delay = (int)datamat["sess_delay"];
                            bm_delay = (int)datamat["bm_delay"];
                        }
                    }
                }
            }
            ViewBag.id = idd;
            ViewBag.mo_delay = mo_delay;
            ViewBag.sess_delay = sess_delay;
            ViewBag.bm_delay = bm_delay;
            return View();
        }

        public String UpdateDelay(string mo_delay, string sess_delay, string bm_delay, string event_code)
        {
            string SenR = "Failed";
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    string stmt = "SELECT id FROM matches where event_code='" + event_code + "'";
                    using (SqlCommand dataCount = new SqlCommand(stmt, con))
                    {
                        var datamat = dataCount.ExecuteReader();
                        if (datamat.HasRows)
                        {
                            SqlCommand cmd1 = new SqlCommand("UPDATE matches SET mo_delay ='" + mo_delay + "' ,sess_delay ='" + sess_delay + "' , bm_delay ='" + bm_delay + "' WHERE event_code = '" + event_code + "' ", con);
                            cmd1.ExecuteNonQuery();

                            SenR = "Bet Delay Update SuccessFully";
                        }
                        else
                        {
                            SenR = "Error";

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return SenR;
        }

        public ActionResult AddTVID()
        {
            string idd = "";
            if (Request.QueryString["id"] != null)
            {
                idd = Request.QueryString["id"];
            }
            ViewBag.id = idd;
            return View();
        }
        public String xidupdate(string xid, string event_code)
        {
            string SenR = "Failed";
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    string stmt = "SELECT id FROM matches where event_code='" + event_code + "'";
                    using (SqlCommand dataCount = new SqlCommand(stmt, con))
                    {
                        var datamat = dataCount.ExecuteReader();
                        if (datamat.HasRows)
                        {
                            SqlCommand cmd1 = new SqlCommand("UPDATE matches SET x_type ='LiveFeed' , x_code ='" + xid + "' , event_code ='" + xid + "' , betfair_id ='" + xid + "' WHERE event_code = '" + event_code + "' ", con);
                            cmd1.ExecuteNonQuery();
                            SqlCommand cmd2 = new SqlCommand("UPDATE markets SET event_code ='" + xid + "' , betfair_id ='" + xid + "' WHERE betfair_id = '" + event_code + "' ", con);
                            cmd2.ExecuteNonQuery();
                            SqlCommand cmd6 = new SqlCommand("UPDATE markets SET event_code ='" + xid + "' WHERE event_code = '" + event_code + "' ", con);
                            cmd6.ExecuteNonQuery();
                            SqlCommand cmd3 = new SqlCommand("UPDATE runner_market SET market_id ='" + xid + "' WHERE market_id = '" + event_code + "' ", con);
                            cmd3.ExecuteNonQuery();
                            SqlCommand cmd4 = new SqlCommand("UPDATE runner_cal SET market_id='" + xid + "', market_code='" + xid + "' , event_code='" + xid + "', event_id='" + xid + "' WHERE market_id = '" + event_code + "' ", con);
                            cmd4.ExecuteNonQuery();
                            SqlCommand cmd5 = new SqlCommand("UPDATE live_bet set event_id='" + xid + "', betfair_id='" + xid + "' where betfair_id='" + event_code + "' ", con);
                            cmd5.ExecuteNonQuery();
                            SqlCommand cmd7 = new SqlCommand("UPDATE live_bet set event_id='" + xid + "' where event_id='" + event_code + "' ", con);
                            cmd7.ExecuteNonQuery();
                            SqlCommand cmd8 = new SqlCommand("UPDATE live_bet_new set event_id='" + xid + "' where event_id='" + event_code + "' ", con);
                            cmd8.ExecuteNonQuery();
                            SqlCommand cmd9 = new SqlCommand("UPDATE fancy_exchange set event_id='" + xid + "' where event_id='" + event_code + "' ", con);
                            cmd9.ExecuteNonQuery();
                            SenR = "XiD and LiveFeed Update SuccessFully";
                        }
                        else
                        {
                            SenR = "Error";

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return SenR;
        }
        public String tvupdate(string tv, string event_code)
        {
            string SenR = "Failed";
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    string stmt = "SELECT league_id FROM matches where event_code='" + event_code + "'";
                    using (SqlCommand dataCount = new SqlCommand(stmt, con))
                    {
                        var datamat = dataCount.ExecuteReader();
                        if (datamat.HasRows)
                        {
                            SqlCommand cmd1 = new SqlCommand("UPDATE matches SET tv_ch ='1' , video_l ='" + tv + "' WHERE event_code = '" + event_code + "' ", con);
                            cmd1.ExecuteNonQuery();
                            SenR = "TV Url Update SuccessFully";
                        }
                        else
                        {
                            SenR = "Error";

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return SenR;
        }
        public String booklimit(string book_limit, string event_code)
        {
            string SenR = "Failed";
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response1 = client.GetAsync("sky_mo.php?event_id=" + event_code).Result;
                    var products1 = response1.Content.ReadAsStringAsync().Result;
                    dynamic responseJson1 = JsonConvert.DeserializeObject(products1);
                    string BetfairId = responseJson1.marketId;
                    string eventTypeId = responseJson1.eventTypeId;
                    string betfair_id = BetfairId;
                    string marketName = "Bookmaker";
                    var runners = responseJson1.runner;

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        SqlCommand cmd4 = new SqlCommand("UPDATE matches SET max_limit_mo_bm='" + book_limit + "' , book_id ='" + BetfairId + "' WHERE event_code = '" + event_code + "' ", con);
                        int runquery2 = cmd4.ExecuteNonQuery();
                        if (runquery2 == 1)
                        {
                            string stmt1 = "SELECT COUNT(id) FROM markets where betfair_id='" + BetfairId + "' ";
                            int countmktm = 0;
                            using (SqlCommand cmdCount = new SqlCommand(stmt1, con))
                            {
                                countmktm = (int)cmdCount.ExecuteScalar();
                            }
                            if (countmktm < 1)
                            {
                                DateTime time = DateTime.Now;              // Use current time
                                string format1 = "yyyy-MM-dd HH:mm:ss";

                                SqlCommand cmd3 = new SqlCommand("INSERT INTO  markets (market_name,back1,lay1,back2,lay2,back3,lay3,status,result_value," +
                                "market_settle,created,league_id,sport_id,betfair_id,event_code,nested_market,runner_name_done) " +
                                "VALUES ('" + marketName + "' ,'' ,'' ,'' ,'' ,'' ,'' ,'activate' ,'' ,'' ,'" + time.ToString(format1) + "' ,'" + 88 + "' ,'" + eventTypeId + "' ,'" + BetfairId + "' ,'" + event_code + "' ,'' ,'undone') ", con);
                                int runquery1 = cmd3.ExecuteNonQuery();
                                if (runquery1 == 1)
                                {
                                    for (int i3 = 0; i3 < runners.Count; i3++)
                                    {
                                        string runnerName = runners[i3].runnerName;
                                        runnerName = runnerName.Replace("'", "''");
                                        string sortPriority = runners[i3].sortPriority;
                                        string selectionId = runners[i3].selectionId;
                                        string stmtr = "SELECT COUNT(*) FROM runner_market where market_id='" + BetfairId + "' AND sortPriority='" + sortPriority + "' ";
                                        int countmktmr = 0;
                                        using (SqlCommand cmdCount = new SqlCommand(stmtr, con))
                                        {
                                            countmktmr = (int)cmdCount.ExecuteScalar();
                                        }
                                        if (countmktmr < 1)
                                        {
                                            SqlCommand cmd5 = new SqlCommand("INSERT INTO  runner_market (runner_name,back1,back2,back3,lay1,lay2,lay3,status,selection_id,market_id,sortPriority) " +
                                            "VALUES ('" + runnerName + "' , '0' , '0' , '0' , '0' , '0' , '0' , 'activate' ,'" + selectionId + "' ,'" + betfair_id + "' ,'" + sortPriority + "') ", con);
                                            cmd5.ExecuteNonQuery();
                                            SqlCommand cmd7 = new SqlCommand("UPDATE markets SET runner_name_done ='done' WHERE runner_name_done='undone' AND betfair_id = '" + betfair_id + "' ", con);
                                            cmd7.ExecuteNonQuery();
                                            SenR = "Book Added & Limit Updated Successfully";

                                        }
                                    }
                                }
                            }
                            else
                            {
                                SenR = "Book Limit Updated SuccessFully";
                            }
                        }
                        con.Close();
                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (TaskCanceledException ex)
            {

            }

            return SenR;
        }
        public ActionResult AddM()
        {
            return View();
        }

        public String AddMa(string title, string mtime, string type, string e_sport, string is_fancy, string event_code)
        {
            string SenR = "Failed";
            string EventTypeIDGet = "4";
            string LeagueIDGet = "88";
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    string EventIDGet = event_code;
                    string name = title;
                    name = name.Replace("'", "''");
                    string openDate = mtime;

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        string stmt = "SELECT id FROM matches where event_code='" + EventIDGet + "'";
                        using (SqlCommand dataCount = new SqlCommand(stmt, con))
                        {
                            var datamat = dataCount.ExecuteReader();
                            if (datamat.HasRows)
                            {
                                
                            }
                            else
                            {

                                SqlCommand cmd2 = new SqlCommand("INSERT INTO  matches (sport_id,match_title,league_id,event_code,status,match_time,betfair_id,teama,teamb,site_t,e_sport,is_fancy) " +
                                " VALUES ('" + EventTypeIDGet + "','" + name + "','" + LeagueIDGet + "','" + EventIDGet + "','OPEN','" + openDate + "','0','nikhil','rakesh',2,'" + e_sport + "','" + is_fancy + "') ", con);
                                int runquery = cmd2.ExecuteNonQuery();
                                if (runquery == 1)
                                {
                                    HttpResponseMessage response1 = client.GetAsync("sky_mo.php?event_id=" + EventIDGet).Result;

                                    var products1 = response1.Content.ReadAsStringAsync().Result;
                                    dynamic responseJson1 = JsonConvert.DeserializeObject(products1);
                                    string BetfairId = responseJson1.marketId;
                                    string betfair_id = BetfairId;
                                    string marketName = responseJson1.market;
                                    var runners = responseJson1.runner;

                                    if (marketName == "Match Odds")
                                    {
                                        marketName = "Match Odds";
                                        string stmt1 = "SELECT COUNT(*) FROM markets where event_code='" + BetfairId + "' ";
                                        int countmktm = 0;
                                        using (SqlCommand cmdCount = new SqlCommand(stmt1, con))
                                        {
                                            countmktm = (int)cmdCount.ExecuteScalar();
                                        }
                                        if (countmktm < 1)
                                        {

                                            DateTime time = DateTime.Now;              // Use current time
                                            string format1 = "yyyy-MM-dd HH:mm:ss";

                                            SqlCommand cmd3 = new SqlCommand("INSERT INTO  markets (market_name,back1,lay1,back2,lay2,back3,lay3,status,result_value," +
                                            "market_settle,created,league_id,sport_id,betfair_id,event_code,nested_market,runner_name_done) " +
                                            "VALUES ('" + marketName + "' ,'' ,'' ,'' ,'' ,'' ,'' ,'activate' ,'' ,'' ,'" + time.ToString(format1) + "' ,'" + LeagueIDGet + "' ,'" + EventTypeIDGet + "' ,'" + BetfairId + "' ,'" + EventIDGet + "' ,'' ,'undone') ", con);
                                            int runquery1 = cmd3.ExecuteNonQuery();
                                            if (runquery1 == 1)
                                            {
                                                SqlCommand cmd4 = new SqlCommand("UPDATE matches SET betfair_id ='" + BetfairId + "' WHERE event_code = '" + EventIDGet + "' ", con);
                                                int runquery2 = cmd4.ExecuteNonQuery();
                                                if (runquery2 == 1)
                                                {
                                                    string Team_A = "";
                                                    string Team_B = "";
                                                    for (int i3 = 0; i3 < runners.Count; i3++)
                                                    {
                                                        string runnerName = runners[i3].runnerName;
                                                        runnerName = runnerName.Replace("'", "''");
                                                        string sortPriority = runners[i3].sortPriority;
                                                        string selectionId = runners[i3].selectionId;
                                                        if (i3 == 0)
                                                        {
                                                            Team_A = runnerName;
                                                        }
                                                        else if (i3 == 1)
                                                        {
                                                            Team_B = runnerName;
                                                        }
                                                        string stmtr = "SELECT COUNT(*) FROM runner_market where market_id='" + BetfairId + "' AND sortPriority='" + sortPriority + "' ";
                                                        int countmktmr = 0;
                                                        using (SqlCommand cmdCount = new SqlCommand(stmtr, con))
                                                        {
                                                            countmktmr = (int)cmdCount.ExecuteScalar();
                                                        }
                                                        if (countmktmr < 1)
                                                        {
                                                            SqlCommand cmd5 = new SqlCommand("INSERT INTO  runner_market (runner_name,back1,back2,back3,lay1,lay2,lay3,status,selection_id,market_id,sortPriority) " +
                                                            "VALUES ('" + runnerName + "' , '0' , '0' , '0' , '0' , '0' , '0' , 'activate' ,'" + selectionId + "' ,'" + betfair_id + "' ,'" + sortPriority + "') ", con);
                                                            cmd5.ExecuteNonQuery();
                                                            SqlCommand cmd6 = new SqlCommand("UPDATE matches SET teama ='" + Team_A + "' , teamb ='" + Team_B + "' WHERE betfair_id = '" + betfair_id + "' ", con);
                                                            cmd6.ExecuteNonQuery();
                                                            SqlCommand cmd7 = new SqlCommand("UPDATE markets SET runner_name_done ='done' WHERE runner_name_done='undone' AND betfair_id = '" + betfair_id + "' ", con);
                                                            cmd7.ExecuteNonQuery();
                                                            SenR = "Success";

                                                        }
                                                    }

                                                }

                                            }
                                        }

                                        con.Close();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (TaskCanceledException ex)
            {

            }

            return SenR;
        }
        public ActionResult AddMX()
        {
            return View();
        }

        public String AddMaX(string title,string teamA,string teamB, string mtime, string sport, string type, string e_sport, string is_fancy, string event_code)
        {
            string SenR = "Failed";
            string EventTypeIDGet = sport;
            string LeagueIDGet = "0";
            if (EventTypeIDGet == "4")
            {
                LeagueIDGet = "99887766";
            }
            else if (EventTypeIDGet == "1")
            {
                LeagueIDGet = "999888777";
            }
            else if (EventTypeIDGet == "2")
            {
                LeagueIDGet = "888777666";
            }
            else if (EventTypeIDGet == "10")
            {
                LeagueIDGet = "1010101010";
            }
            else if (EventTypeIDGet == "3503")
            {
                LeagueIDGet = "35033503";
            }
            else if (EventTypeIDGet == "98765")
            {
                LeagueIDGet = "98765987";
            }
            else if (EventTypeIDGet == "56789")
            {
                LeagueIDGet = "56789567";
            }
            try
            {
                try
                {
                    string name = title;
                    name = name.Replace("'", "''");
                    string openDate = mtime;

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        string stmt = "SELECT id FROM matches where event_code='" + event_code + "'";
                        using (SqlCommand dataCount = new SqlCommand(stmt, con))
                        {
                            var datamat = dataCount.ExecuteReader();
                            if (!datamat.HasRows)
                            {
                                SqlCommand cmd2 = new SqlCommand("INSERT INTO  matches (sport_id,match_title,league_id,event_code,status,match_time,betfair_id,teama,teamb,x_type,x_code,site_t,e_sport,is_fancy) " +
                                " VALUES ('" + EventTypeIDGet + "','" + name + "','" + LeagueIDGet + "','" + event_code + "','OPEN','" + openDate + "','" + event_code + "','" + teamA + "','" + teamB + "','" + type + "','" + event_code + "',2,'" + e_sport + "','" + is_fancy + "') ", con);
                                int runquery = cmd2.ExecuteNonQuery();
                                if (runquery == 1)
                                {
                                    string BetfairId = event_code;
                                    string marketName = "Match Odds";

                                    marketName = "Match Odds";
                                    string stmt1 = "SELECT COUNT(*) FROM markets where event_code='" + BetfairId + "' ";
                                    int countmktm = 0;
                                    using (SqlCommand cmdCount = new SqlCommand(stmt1, con))
                                    {
                                        countmktm = (int)cmdCount.ExecuteScalar();
                                    }
                                    if (countmktm < 1)
                                    {

                                        DateTime time = DateTime.Now;              // Use current time
                                        string format1 = "yyyy-MM-dd HH:mm:ss";

                                        SqlCommand cmd3 = new SqlCommand("INSERT INTO  markets (market_name,back1,lay1,back2,lay2,back3,lay3,status,result_value," +
                                        "market_settle,created,league_id,sport_id,betfair_id,event_code,nested_market,runner_name_done) " +
                                        "VALUES ('" + marketName + "' ,'' ,'' ,'' ,'' ,'' ,'' ,'activate' ,'' ,'' ,'" + time.ToString(format1) + "' ,'" + LeagueIDGet + "' ,'" + EventTypeIDGet + "' ,'" + BetfairId + "' ,'" + event_code + "' ,'' ,'undone') ", con);
                                        int runquery1 = cmd3.ExecuteNonQuery();
                                        if (runquery1 == 1)
                                        {

                                            string stmtr = "SELECT COUNT(*) FROM runner_market where market_id='" + BetfairId + "' ";
                                            int countmktmr = 0;
                                            using (SqlCommand cmdCount = new SqlCommand(stmtr, con))
                                            {
                                                countmktmr = (int)cmdCount.ExecuteScalar();
                                            }
                                            if (countmktmr < 1)
                                            {
                                                SqlCommand cmd5 = new SqlCommand("INSERT INTO  runner_market (runner_name,back1,back2,back3,lay1,lay2,lay3,status,selection_id,market_id,sortPriority) " +
                                                "VALUES ('" + teamA + "' , '0' , '0' , '0' , '0' , '0' , '0' , 'activate' ,'" + event_code + "_" + 1 + "' ,'" + BetfairId + "' ,'1') ", con);
                                                cmd5.ExecuteNonQuery();
                                                SqlCommand cmd8 = new SqlCommand("INSERT INTO  runner_market (runner_name,back1,back2,back3,lay1,lay2,lay3,status,selection_id,market_id,sortPriority) " +
                                                "VALUES ('" + teamB + "' , '0' , '0' , '0' , '0' , '0' , '0' , 'activate' ,'" + event_code + "_" + 2 + "' ,'" + BetfairId + "' ,'2') ", con);
                                                cmd8.ExecuteNonQuery();
                                                SqlCommand cmd7 = new SqlCommand("UPDATE markets SET runner_name_done ='done' WHERE runner_name_done='undone' AND betfair_id = '" + BetfairId + "' ", con);
                                                cmd7.ExecuteNonQuery();
                                                SenR = "Success";

                                            }

                                        }
                                    }

                                    con.Close();
                                }
                            }
                            else
                            {
                                SenR = "Match Already Added";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (TaskCanceledException ex)
            {

            }

            return SenR;
        }

    }
}