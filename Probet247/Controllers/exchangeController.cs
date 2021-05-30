using BertfairLive1.Controllers;
using BertfairLive1.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Probet247.Models;
using RBetfair.Models;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace Probet247.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class exchangeController : Controller
    {

        private SqlConnection con;
        static string SportsidForShowAllSports = "";
        static string SportsNameForShowAllSports = "";
        ArrayList AllSportsSideNavbar = new ArrayList();
        readonly string _connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        static string GetBetfairidForInnerPage = "";
        static string GetEventCodeForInnerPage = "";
        static string League_Name_SendTop = "";
        static string GetSportIDOutsideU = "";
        static string GetLeagueIdOutsideU = "";
        static string GetMarketIdOutsideU = "";

        string check_sess_id = (string)System.Web.HttpContext.Current.Session["UIDS"];
        string check_hash_key = (string)System.Web.HttpContext.Current.Session["hash_keyS"];
        string check_ag_id = (string)System.Web.HttpContext.Current.Session["AgentID"];
        string check_agm_id = (string)System.Web.HttpContext.Current.Session["AgentMID"];
        string check_ad_id = (string)System.Web.HttpContext.Current.Session["AdminID"];
        string is_panel_log = (string)System.Web.HttpContext.Current.Session["is_panel_log"];
        DateTime todaydate = DateTime.Now;
        string today = "yyyy-MM-dd";

        DateTime CurrentTime = DateTime.Now;
        public ActionResult Index(EventIdSend eventIdSend, string sportid, string leagueid, string eventcode, string pagenlo)
        {
            var AllSportsAddM = new List<NavbarListAllSports>();
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
            {
                return RedirectToAction("MMatchList/4", "exchange");
            }
            else
            {
                try
                {
                    if (check_hash_key != null && check_hash_key != "")
                    {
                        CULL();
                    }
                    string dl_block_sports_string = FunctionDataController.getblocksport();
                    string dl_block_leagues_string = FunctionDataController.getblockleague();
                    string dl_block_events_string = FunctionDataController.getblockevent();

                    string lega_query = "";
                    if (leagueid != null && leagueid != "0" && leagueid != "")
                    {
                        lega_query = " AND league_id=" + leagueid;
                    }
                    DateTime time = DateTime.Now;
                    string format = "yyyy-MM-dd HH:mm:ss";

                    string spo_id = "0";
                    GetSportIDOutsideU = sportid;
                    GetLeagueIdOutsideU = leagueid;
                    GetMarketIdOutsideU = eventcode;
                    if (sportid != null)
                    {
                        spo_id = sportid;
                    }
                    ViewBag.RunSports = spo_id;

                    ArrayList list = new ArrayList();
                    ArrayList list1 = new ArrayList();
                    try
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            using (var cmd = new SqlCommand("SELECT sport_name,sport_id FROM sports where sport_id Not IN(" + dl_block_sports_string + ") order by id asc"))
                            {
                                cmd.Connection = con;
                                con.Open();
                                var reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    SportsNameForShowAllSports = (string)reader["sport_name"];
                                    SportsidForShowAllSports = (string)reader["sport_id"];
                                    list.Add(SportsNameForShowAllSports);
                                    list1.Add(SportsidForShowAllSports);
                                    AllSportsAddM.Add(item: new NavbarListAllSports
                                    {
                                        SportsName = SportsNameForShowAllSports,
                                        SportsId = SportsidForShowAllSports,
                                        LaegueId = "0",
                                        EventId = "0",
                                        MarketId = "0",
                                        ForTest = "0"
                                    });
                                }
                                con.Close();
                            }
                        }
                        string cric_bf_id = "";
                        string tenn_bf_id = "";
                        string socc_bf_id = "";
                        using (var connection = new SqlConnection(_connString))
                        {

                            string cric_query = "SELECT Top (5) [betfair_id] FROM matches where sport_id='878454'";
                            string socc_query = "SELECT Top (5) [betfair_id]  FROM matches where sport_id='878454'";
                            string tenn_query = "SELECT Top (5) [betfair_id]  FROM matches where sport_id='878454'";
                            if (spo_id == "inplay")
                            {
                                cric_query = "SELECT Top (5) [betfair_id] FROM matches where sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND sport_id = 4 " + lega_query + " AND betfair_id!='0' AND teama!='nikhil' AND status='OPEN' AND match_time < '" + time.ToString(format) + "' order by match_time ";
                                socc_query = "SELECT Top (5) [betfair_id] FROM matches where sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND sport_id = 1 " + lega_query + " AND betfair_id!='0' AND teama!='nikhil' AND status='OPEN' AND match_time < '" + time.ToString(format) + "' order by match_time ";
                                tenn_query = "SELECT Top (5) [betfair_id] FROM matches where sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND sport_id = 2 " + lega_query + " AND betfair_id!='0' AND teama!='nikhil' AND status='OPEN' AND match_time < '" + time.ToString(format) + "' order by match_time ";
                            }
                            else
                            {
                                cric_query = "SELECT Top (4) [betfair_id] FROM matches where sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND sport_id = 4 " + lega_query + " AND betfair_id!='0' AND teama!='nikhil' AND status='OPEN' order by match_time ";
                                socc_query = "SELECT Top (4) [betfair_id] FROM matches where sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND sport_id = 1 " + lega_query + " AND betfair_id!='0' AND teama!='nikhil' AND status='OPEN' order by match_time ";
                                tenn_query = "SELECT Top (4) [betfair_id] FROM matches where sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND sport_id = 2 " + lega_query + " AND betfair_id!='0' AND teama!='nikhil' AND status='OPEN' order by match_time ";

                            }
                            connection.Open();
                            using (var command = new SqlCommand(cric_query, connection))
                            {

                                var reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    string betfair_code = (string)reader["betfair_id"];
                                    if (betfair_code != "0")
                                    {
                                        cric_bf_id += betfair_code + ",";
                                    }
                                }
                            }
                            connection.Close();
                            connection.Open();
                            using (var commandt = new SqlCommand(tenn_query, connection))
                            {

                                var reader = commandt.ExecuteReader();
                                while (reader.Read())
                                {
                                    string betfair_code = (string)reader["betfair_id"];
                                    if (betfair_code != "0")
                                    {
                                        tenn_bf_id += betfair_code + ",";
                                    }
                                }
                            }
                            connection.Close();
                            connection.Open();
                            using (var commands = new SqlCommand(socc_query, connection))
                            {
                                var reader = commands.ExecuteReader();
                                while (reader.Read())
                                {
                                    string betfair_code = (string)reader["betfair_id"];
                                    if (betfair_code != "0")
                                    {
                                        socc_bf_id += betfair_code + ",";
                                    }
                                }
                            }
                            connection.Close();
                            connection.Open();
                            string stmts = "SELECT COUNT(id) FROM matches where sport_id!='7888' AND sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND sport_id!=7888 AND betfair_id != '0' AND teama != 'nikhil' AND status='OPEN' AND match_time < '" + time.ToString(format) + "' ";
                            int countinplay = 0;
                            using (SqlCommand cmdCounts = new SqlCommand(stmts, connection))
                            {
                                countinplay = (int)cmdCounts.ExecuteScalar();
                            }
                            connection.Close();

                            ViewBag.countinplay = countinplay;
                            ViewBag.cricket_MIDS = cric_bf_id;
                            ViewBag.soccer_MIDS = socc_bf_id;
                            ViewBag.tennis_MIDS = tenn_bf_id;
                        }
                        ViewBag.sp_name = list;
                        ViewBag.sp_id = list1;
                    }
                    catch (SqlException ex)
                    {

                    }

                }
                catch (Exception ex)
                {

                }
            }

            return View(AllSportsAddM);
        }

        public ActionResult Sport(EventIdSend eventIdSend, string sportid, string leagueid, string eventcode, string pagenlo)
        {
            var AllSportsAddM = new List<NavbarListAllSports>();
            try
            {
                if (check_hash_key != null && check_hash_key != "")
                {
                    CULL();
                }
                string dl_block_sports_string = FunctionDataController.getblocksport();
                string dl_block_leagues_string = FunctionDataController.getblockleague();
                string dl_block_events_string = FunctionDataController.getblockevent();
                string lega_query = "";
                if (leagueid != null && leagueid != "0" && leagueid != "")
                {
                    lega_query = " AND league_id=" + leagueid;
                }
                string Page_number = eventIdSend.Pagenumber;
                int myvalue = 0;
                if (Page_number != null)
                {
                    myvalue = Int32.Parse(Page_number);
                }
                DateTime time = DateTime.Now;
                string format = "yyyy-MM-dd HH:mm:ss";

                int min_page = 0 + myvalue;
                int max_page = 20 + myvalue;
                ViewBag.PageNumber = myvalue;

                string spo_id = "0";
                string SportsName = "";
                GetSportIDOutsideU = sportid;
                GetLeagueIdOutsideU = leagueid;
                GetMarketIdOutsideU = eventcode;
                if (sportid != null)
                {
                    spo_id = sportid;
                    SportsName = FunctionDataController.GetSportsNameById(spo_id);
                }
                ViewBag.RunSports = spo_id;
                ViewBag.SportsName = SportsName;
                try
                {
                    if (sportid != null && sportid != "0" && leagueid == null)
                    {
                        string Getdata = GetSportsNameForClick(sportid);
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            using (var cmd = new SqlCommand("SELECT league_name,league_id FROM league WHERE sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND sport_id = '" + sportid + "' AND EXISTS (SELECT event_code FROM matches  WHERE event_code Not IN(" + dl_block_events_string + ") AND sport_id='" + sportid + "' AND league.league_id = matches.league_id AND betfair_id!='0' AND teama!='nikhil' AND status = 'OPEN') "))
                            {
                                cmd.Connection = con;
                                con.Open();
                                var reader = cmd.ExecuteReader();
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        string GSportsNameForShowAllSports = (string)reader["league_name"];
                                        string GSportsidForShowAllSports = (string)reader["league_id"];
                                        AllSportsAddM.Add(item: new NavbarListAllSports
                                        {
                                            SportsName = GSportsNameForShowAllSports,
                                            SportsId = sportid,
                                            LaegueId = GSportsidForShowAllSports,
                                            EventId = "0",
                                            MarketId = "0",
                                            GetPreviousName = Getdata,
                                            ForTest = "0"
                                        });
                                    }
                                }
                                else
                                {
                                    AllSportsAddM.Add(item: new NavbarListAllSports
                                    {
                                        SportsName = "",
                                        SportsId = sportid,
                                        LaegueId = "101",
                                        EventId = "0",
                                        MarketId = "0",
                                        GetPreviousName = Getdata,
                                        ForTest = "0"
                                    });
                                }

                                con.Close();
                            }
                        }
                    }
                    else if (sportid != null && sportid != "0" && leagueid != null && eventcode == null)
                    {
                        string Getdata = GetSportsNameForClick(sportid);
                        string GetLeagueNameS = GetLeagueName(leagueid);

                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            using (var cmd = new SqlCommand("SELECT match_title,betfair_id,event_code FROM matches where sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND league_id= '" + leagueid + "' AND betfair_id!='0' AND teama!='nikhil' AND status = 'OPEN' "))
                            {
                                cmd.Connection = con;
                                con.Open();
                                var reader = cmd.ExecuteReader();
                                string Gevent_code = "";
                                while (reader.Read())
                                {
                                    string GSportsNameForShowAllSports = (string)reader["match_title"];
                                    string GSportsidForShowAllSports = (string)reader["betfair_id"];
                                    Gevent_code = (string)reader["event_code"];
                                    //string matchTt = GetMatchTitleN(Gevent_code);
                                    AllSportsAddM.Add(item: new NavbarListAllSports
                                    {
                                        SportsName = GSportsNameForShowAllSports,
                                        SportsId = sportid,
                                        LaegueId = leagueid,
                                        EventId = Gevent_code,
                                        MarketId = GSportsidForShowAllSports,
                                        GetPreviousName = Getdata,
                                        GetPreviousLName = GetLeagueNameS,
                                        ForTest = "0"
                                    });
                                }

                                con.Close();
                            }
                        }
                    }
                    else
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            using (var cmd = new SqlCommand("SELECT sport_name,sport_id FROM sports where sport_id Not IN(" + dl_block_sports_string + ") order by id asc"))
                            {
                                cmd.Connection = con;
                                con.Open();
                                var reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    SportsNameForShowAllSports = (string)reader["sport_name"];
                                    SportsidForShowAllSports = (string)reader["sport_id"];
                                    AllSportsAddM.Add(item: new NavbarListAllSports
                                    {
                                        SportsName = SportsNameForShowAllSports,
                                        SportsId = SportsidForShowAllSports,
                                        LaegueId = "0",
                                        EventId = "0",
                                        MarketId = "0",
                                        ForTest = "0"
                                    });
                                }
                                con.Close();
                            }
                        }
                    }

                    string sport_bf_id = "";
                    using (var connection = new SqlConnection(_connString))
                    {

                        string cric_query = "SELECT Top (5) [betfair_id] FROM matches where sport_id='878454'";
                        if (spo_id != null)
                        {
                            cric_query = "SELECT betfair_id FROM (select [betfair_id] ,ROW_NUMBER() over(order by match_time) as row from matches where sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND sport_id = '" + sportid + "' " + lega_query + " AND betfair_id!='0' AND teama!='nikhil' AND status = 'OPEN') a where a.row>'" + min_page + "'  and a.row<='" + max_page + "'";
                        }
                        connection.Open();
                        using (var command = new SqlCommand(cric_query, connection))
                        {
                            var reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                string betfair_code = (string)reader["betfair_id"];
                                if (betfair_code != "0")
                                {
                                    sport_bf_id += betfair_code + ",";
                                }
                            }
                        }
                        connection.Close();
                        connection.Open();
                        string stmts = "SELECT COUNT(id) FROM matches where sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND sport_id!=7888 AND betfair_id != '0' AND teama != 'nikhil' AND status='OPEN' AND match_time < '" + time.ToString(format) + "' ";
                        int countinplay = 0;
                        using (SqlCommand cmdCounts = new SqlCommand(stmts, connection))
                        {
                            countinplay = (int)cmdCounts.ExecuteScalar();
                        }
                        connection.Close();

                        ViewBag.countinplay = countinplay;
                        ViewBag.sport_MIDS = sport_bf_id;
                    }
                }
                catch (SqlException ex)
                {

                }
            }
            catch (Exception ex)
            {

            }
            return View(AllSportsAddM);
        }

        public ActionResult Menu()
        {
            var AllSportsAddM = new List<NavbarListAllSports>();
            try
            {
                if (check_hash_key != null && check_hash_key != "")
                {
                    CULL();
                }
                string dl_block_sports_string = FunctionDataController.getblocksport();

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT sport_name,sport_id FROM sports where sport_id Not IN(" + dl_block_sports_string + ") order by id asc"))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            SportsNameForShowAllSports = (string)reader["sport_name"];
                            SportsidForShowAllSports = (string)reader["sport_id"];
                            AllSportsAddM.Add(item: new NavbarListAllSports
                            {
                                SportsName = SportsNameForShowAllSports,
                                SportsId = SportsidForShowAllSports
                            });
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View(AllSportsAddM);
        }

        public JsonResult getSportMat(string sprt_ids)
        {

            List<OddsGetOne> messages = new List<OddsGetOne>();
            if (check_sess_id != null && check_sess_id != "")
            {
                messages = getSportMatData(sprt_ids);
            }
            else
            {
                messages = getSportMatDatabf(sprt_ids);
            }
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<OddsGetOne> getSportMatData(string sprt_ids)
        {
            var messages = new List<OddsGetOne>();
            if (sprt_ids != "")
            {
                ArrayList cricket_bfid_array = new ArrayList();
                ArrayList cric_runerteamA = new ArrayList();
                ArrayList cric_runerteamB = new ArrayList();
                ArrayList cric_eventCode = new ArrayList();
                ArrayList cric_matchTime = new ArrayList();
                try
                {
                    HttpClient client1 = new HttpClient();
                    client1.BaseAddress = new Uri("https://ero.betfair.com/www/sports/exchange/readonly/v1/bymarket");
                    client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response1 = client1.GetAsync("?_ak=nzIFcwyWhrlwYMrh&alt=json&marketIds=" + sprt_ids + "&types=EVENT,MARKET_STATE,RUNNER_DESCRIPTION").Result;
                        if (response1 != null)
                        {
                            var products1 = response1.Content.ReadAsStringAsync().Result;
                            dynamic responseJson1 = JsonConvert.DeserializeObject(products1);
                            var Getmatchdata = responseJson1.eventTypes[0].eventNodes;
                            string cric_eve_code = "";
                            cric_eventCode.Clear();
                            cric_runerteamA.Clear();
                            cric_runerteamB.Clear();
                            cricket_bfid_array.Clear();
                            cric_matchTime.Clear();

                            for (int i1 = 0; i1 < Getmatchdata.Count; i1++)
                            {
                                cric_eve_code = Getmatchdata[i1].eventId;
                                var GetRunnernamearray = Getmatchdata[i1]["marketNodes"][0]["runners"];
                                DateTime cric_match_time = Getmatchdata[i1]["event"]["openDate"];
                                cric_match_time = cric_match_time.AddMinutes(330);
                                string cric_match_time1 = cric_match_time.ToString("yyyy/MM/dd HH:mm:ss");
                                string betfair_code = Getmatchdata[i1]["marketNodes"][0]["marketId"];
                                string teamA = GetRunnernamearray[0]["description"]["runnerName"];
                                string teamB = GetRunnernamearray[1]["description"]["runnerName"];

                                cricket_bfid_array.Add(betfair_code);
                                cric_runerteamA.Add(teamA);
                                cric_runerteamB.Add(teamB);
                                cric_matchTime.Add(cric_match_time1);
                                cric_eventCode.Add(cric_eve_code);
                            }
                        }
                    }
                    catch (Exception ex) { }
                }
                catch (TaskCanceledException ex)
                {
                }
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://api_link.com");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("market/2/29278528/" + sprt_ids).Result;

                        // Blocking call! 
                        if (response != null)
                        {
                            var products = response.Content.ReadAsStringAsync().Result;
                            dynamic responseJson = JsonConvert.DeserializeObject(products);
                            for (int i = 0; i < responseJson.Count; i++)
                            {
                                var GetRunner = responseJson[i].runners;
                                int CheckRunner = GetRunner.Count;

                                string priceb11 = "";
                                string priceb12 = "";
                                string priceb13 = "";

                                string pricel11 = "";
                                string pricel12 = "";
                                string pricel13 = "";

                                string sizeb11 = "";
                                string sizeb12 = "";
                                string sizeb13 = "";

                                string sizel11 = "";
                                string sizel12 = "";
                                string sizel13 = "";
                                string MatchStatus = responseJson[i].runners[0].status;
                                string totalMatched = "$" + (int)responseJson[i].totalMatched;
                                string EvnetMarketId = responseJson[i].marketId;
                                //string RunnerNameAQ = "";

                                if (GetRunner[0].ex.availableToBack.Count > 0)
                                {
                                    priceb11 = GetRunner[0].ex.availableToBack[0].price;
                                    sizeb11 = "$" + (int)GetRunner[0].ex.availableToBack[0].size;
                                }

                                if (GetRunner[0].ex.availableToLay.Count > 0)
                                {
                                    pricel11 = GetRunner[0].ex.availableToLay[0].price;
                                    sizel11 = "$" + (int)GetRunner[0].ex.availableToLay[0].size;
                                }

                                if (GetRunner[1].ex.availableToBack.Count > 0)
                                {
                                    priceb12 = GetRunner[1].ex.availableToBack[0].price;
                                    sizeb12 = "$" + (int)GetRunner[1].ex.availableToBack[0].size;
                                }

                                if (GetRunner[1].ex.availableToLay.Count > 0)
                                {
                                    pricel12 = GetRunner[1].ex.availableToLay[0].price;
                                    sizel12 = "$" + (int)GetRunner[1].ex.availableToLay[0].size;
                                }

                                if (CheckRunner > 2)
                                {
                                    if (GetRunner[2].ex.availableToBack.Count > 0)
                                    {
                                        priceb13 = GetRunner[2].ex.availableToBack[0].price;
                                        sizeb13 = "$" + (int)GetRunner[2].ex.availableToBack[0].size;
                                    }

                                    if (GetRunner[2].ex.availableToLay.Count > 0)
                                    {
                                        pricel13 = GetRunner[2].ex.availableToLay[0].price;
                                        sizel13 = "$" + (int)GetRunner[2].ex.availableToLay[0].size;
                                    }
                                }

                                string runteama = "";
                                string runteamb = "";
                                string event_code1 = "";
                                string MatchTTime = "";
                                string BetfairMarketid = "";
                                for (int iy = 0; iy < cric_runerteamA.Count; iy++)
                                {
                                    if (cricket_bfid_array[iy].ToString() == EvnetMarketId)
                                    {
                                        runteama = cric_runerteamA[iy].ToString();
                                        runteamb = cric_runerteamB[iy].ToString();
                                        event_code1 = cric_eventCode[iy].ToString();
                                        MatchTTime = cric_matchTime[iy].ToString();
                                        BetfairMarketid = EvnetMarketId;
                                    }
                                }

                                messages.Add(item: new OddsGetOne
                                {
                                    EventCode = event_code1,
                                    back1 = priceb11,
                                    lay1 = pricel11,
                                    back11 = priceb12,
                                    lay11 = pricel12,
                                    back22 = priceb13,
                                    lay22 = pricel13,
                                    Runnername = runteama,
                                    RunnernameB = runteamb,
                                    BetfairId = BetfairMarketid,
                                    MatchTime = MatchTTime,
                                    back1size = sizeb11,
                                    back2size = sizeb12,
                                    back3size = sizeb13,
                                    lay1size = sizel11,
                                    lay2size = sizel12,
                                    lay3size = sizel13,
                                    MatchedAmt = totalMatched,
                                    status = MatchStatus
                                });
                            }
                        }
                        else
                        {
                            messages.Add(item: new OddsGetOne
                            {
                                EventCode = "",
                                back1 = "",
                                lay1 = "",
                                back11 = "",
                                lay11 = "",
                                back22 = "",
                                lay22 = "",
                                Runnername = "",
                                RunnernameB = "",
                                BetfairId = "",
                                MatchTime = "",
                                back1size = "",
                                back2size = "",
                                back3size = "",
                                lay1size = "",
                                lay2size = "",
                                lay3size = "",
                                MatchedAmt = "",
                                status = "SUSPENDED"
                            });
                        }
                    }
                    catch (Exception ex) { }
                }
                catch (TaskCanceledException ex)
                {
                }
            }

            return messages;
        }

        public List<OddsGetOne> getSportMatDatabf(string sprt_ids)
        {
            var messages = new List<OddsGetOne>();
            if (sprt_ids != "")
            {
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("https://ero.betfair.com/www/sports/exchange/readonly/v1/bymarket");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("?_ak=nzIFcwyWhrlwYMrh&alt=json&marketIds=" + sprt_ids + "&types=EVENT,MARKET_STATE,RUNNER_EXCHANGE_PRICES_BEST,RUNNER_DESCRIPTION").Result;

                        // Blocking call! 
                        if (response != null)
                        {
                            var products = response.Content.ReadAsStringAsync().Result;
                            dynamic responseJson1 = JsonConvert.DeserializeObject(products);
                            var responseJson = responseJson1.eventTypes[0].eventNodes;

                            for (int i = 0; i < responseJson.Count; i++)
                            {
                                string EvnetMarketId = responseJson[i].marketNodes[0].marketId;
                                string totalMatched = "$" + (int)responseJson[i].marketNodes[0].state.totalMatched;
                                string MatchStatus = responseJson[i].marketNodes[0].state.status;

                                string event_code1 = responseJson[i].eventId;
                                DateTime cric_match_time = responseJson[i]["event"]["openDate"];
                                cric_match_time = cric_match_time.AddMinutes(330);
                                string MatchTTime = cric_match_time.ToString("yyyy/MM/dd HH:mm:ss");
                                string BetfairMarketid = EvnetMarketId;

                                if (MatchStatus == "OPEN")
                                {
                                    MatchStatus = "ACTIVE";
                                }
                                var GetRunner = responseJson[i].marketNodes[0].runners;
                                string runteama = GetRunner[0]["description"]["runnerName"];
                                string runteamb = GetRunner[1]["description"]["runnerName"];

                                int CheckRunner = GetRunner.Count;

                                string priceb11 = "";
                                string priceb12 = "";
                                string priceb13 = "";

                                string pricel11 = "";
                                string pricel12 = "";
                                string pricel13 = "";

                                string sizeb11 = "";
                                string sizeb12 = "";
                                string sizeb13 = "";

                                string sizel11 = "";
                                string sizel12 = "";
                                string sizel13 = "";
                                //string RunnerNameAQ = "";
                                if (GetRunner[0].exchange.availableToBack != null)
                                {
                                    if (GetRunner[0].exchange.availableToBack.Count > 0)
                                    {
                                        priceb11 = GetRunner[0].exchange.availableToBack[0].price;
                                        sizeb11 = "$" + (int)GetRunner[0].exchange.availableToBack[0].size;
                                    }
                                }

                                if (GetRunner[0].exchange.availableToLay != null)
                                {
                                    if (GetRunner[0].exchange.availableToLay.Count > 0)
                                    {
                                        pricel11 = GetRunner[0].exchange.availableToLay[0].price;
                                        sizel11 = "$" + (int)GetRunner[0].exchange.availableToLay[0].size;
                                    }
                                }

                                if (GetRunner[1].exchange.availableToBack != null)
                                {
                                    if (GetRunner[1].exchange.availableToBack.Count > 0)
                                    {
                                        priceb12 = GetRunner[1].exchange.availableToBack[0].price;
                                        sizeb12 = "$" + (int)GetRunner[1].exchange.availableToBack[0].size;
                                    }
                                }

                                if (GetRunner[1].exchange.availableToLay != null)
                                {
                                    if (GetRunner[1].exchange.availableToLay.Count > 0)
                                    {
                                        pricel12 = GetRunner[1].exchange.availableToLay[0].price;
                                        sizel12 = "$" + (int)GetRunner[1].exchange.availableToLay[0].size;
                                    }
                                }

                                if (CheckRunner > 2)
                                {
                                    if (GetRunner[2].exchange.availableToBack != null)
                                    {
                                        if (GetRunner[2].exchange.availableToBack.Count > 0)
                                        {
                                            priceb13 = GetRunner[2].exchange.availableToBack[0].price;
                                            sizeb13 = "$" + (int)GetRunner[2].exchange.availableToBack[0].size;
                                        }
                                    }
                                    if (GetRunner[2].exchange.availableToLay != null)
                                    {
                                        if (GetRunner[2].exchange.availableToLay.Count > 0)
                                        {
                                            pricel13 = GetRunner[2].exchange.availableToLay[0].price;
                                            sizel13 = "$" + (int)GetRunner[2].exchange.availableToLay[0].size;
                                        }
                                    }
                                }

                                messages.Add(item: new OddsGetOne
                                {
                                    EventCode = event_code1,
                                    back1 = priceb11,
                                    lay1 = pricel11,
                                    back11 = priceb12,
                                    lay11 = pricel12,
                                    back22 = priceb13,
                                    lay22 = pricel13,
                                    Runnername = runteama,
                                    RunnernameB = runteamb,
                                    BetfairId = BetfairMarketid,
                                    MatchTime = MatchTTime,
                                    back1size = sizeb11,
                                    back2size = sizeb12,
                                    back3size = sizeb13,
                                    lay1size = sizel11,
                                    lay2size = sizel12,
                                    lay3size = sizel13,
                                    MatchedAmt = totalMatched,
                                    status = MatchStatus
                                });
                            }
                        }
                        else
                        {
                            messages.Add(item: new OddsGetOne
                            {
                                EventCode = "",
                                back1 = "",
                                lay1 = "",
                                back11 = "",
                                lay11 = "",
                                back22 = "",
                                lay22 = "",
                                Runnername = "",
                                RunnernameB = "",
                                BetfairId = "",
                                MatchTime = "",
                                back1size = "",
                                back2size = "",
                                back3size = "",
                                lay1size = "",
                                lay2size = "",
                                lay3size = "",
                                MatchedAmt = "",
                                status = "SUSPENDED"
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("ErrorC" + ex.ToString());
                    }
                }
                catch (TaskCanceledException ex)
                {
                    System.Diagnostics.Debug.WriteLine("ErrorC" + ex.ToString());
                }
            }

            return messages;
        }

        public JsonResult getCricketMat(string cric_ids)
        {

            List<OddsGetOne> messages = new List<OddsGetOne>();
            if (check_sess_id != null && check_sess_id != "")
            {
                messages = getCricketMatData(cric_ids);
            }
            else
            {
                messages = getCricketMatDatabf(cric_ids);
            }
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<OddsGetOne> getCricketMatData(string cric_ids)
        {
            var messages = new List<OddsGetOne>();
            if (cric_ids != "")
            {
                ArrayList cricket_bfid_array = new ArrayList();
                ArrayList cric_runerteamA = new ArrayList();
                ArrayList cric_runerteamB = new ArrayList();
                ArrayList cric_eventCode = new ArrayList();
                ArrayList cric_matchTime = new ArrayList();
                try
                {
                    HttpClient client1 = new HttpClient();
                    client1.BaseAddress = new Uri("https://ero.betfair.com/www/sports/exchange/readonly/v1/bymarket");
                    client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response1 = client1.GetAsync("?_ak=nzIFcwyWhrlwYMrh&alt=json&marketIds=" + cric_ids + "&types=EVENT,MARKET_STATE,RUNNER_DESCRIPTION").Result;
                        if (response1 != null)
                        {
                            var products1 = response1.Content.ReadAsStringAsync().Result;
                            dynamic responseJson1 = JsonConvert.DeserializeObject(products1);
                            var Getmatchdata = responseJson1.eventTypes[0].eventNodes;
                            string cric_eve_code = "";
                            cric_eventCode.Clear();
                            cric_runerteamA.Clear();
                            cric_runerteamB.Clear();
                            cricket_bfid_array.Clear();
                            cric_matchTime.Clear();

                            for (int i1 = 0; i1 < Getmatchdata.Count; i1++)
                            {
                                cric_eve_code = Getmatchdata[i1].eventId;
                                var GetRunnernamearray = Getmatchdata[i1]["marketNodes"][0]["runners"];
                                DateTime cric_match_time = Getmatchdata[i1]["event"]["openDate"];
                                cric_match_time = cric_match_time.AddMinutes(330);
                                string cric_match_time1 = cric_match_time.ToString("yyyy/MM/dd HH:mm:ss");
                                string betfair_code = Getmatchdata[i1]["marketNodes"][0]["marketId"];
                                string teamA = GetRunnernamearray[0]["description"]["runnerName"];
                                string teamB = GetRunnernamearray[1]["description"]["runnerName"];

                                cricket_bfid_array.Add(betfair_code);
                                cric_runerteamA.Add(teamA);
                                cric_runerteamB.Add(teamB);
                                cric_matchTime.Add(cric_match_time1);
                                cric_eventCode.Add(cric_eve_code);
                            }
                        }
                    }
                    catch (Exception ex) { }
                }
                catch (TaskCanceledException ex)
                {
                    System.Diagnostics.Debug.WriteLine("ErrorC" + ex.ToString());
                }
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://api_link.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("exchange/odds/market/2/29278528/" + cric_ids).Result;

                        // Blocking call! 
                        if (response != null)
                        {
                            var products = response.Content.ReadAsStringAsync().Result;
                            dynamic responseJson = JsonConvert.DeserializeObject(products);
                            for (int i = 0; i < responseJson.Count; i++)
                            {
                                var GetRunner = responseJson[i].runners;
                                int CheckRunner = GetRunner.Count;

                                string priceb11 = "";
                                string priceb12 = "";
                                string priceb13 = "";

                                string pricel11 = "";
                                string pricel12 = "";
                                string pricel13 = "";

                                string sizeb11 = "";
                                string sizeb12 = "";
                                string sizeb13 = "";

                                string sizel11 = "";
                                string sizel12 = "";
                                string sizel13 = "";
                                string MatchStatus = responseJson[i].runners[0].status;
                                string totalMatched = "$" + (int)responseJson[i].totalMatched;
                                string EvnetMarketId = responseJson[i].marketId;
                                //string RunnerNameAQ = "";

                                if (GetRunner[0].ex.availableToBack.Count > 0)
                                {
                                    priceb11 = GetRunner[0].ex.availableToBack[0].price;
                                    sizeb11 = "$" + (int)GetRunner[0].ex.availableToBack[0].size;
                                }

                                if (GetRunner[0].ex.availableToLay.Count > 0)
                                {
                                    pricel11 = GetRunner[0].ex.availableToLay[0].price;
                                    sizel11 = "$" + (int)GetRunner[0].ex.availableToLay[0].size;
                                }

                                if (GetRunner[1].ex.availableToBack.Count > 0)
                                {
                                    priceb12 = GetRunner[1].ex.availableToBack[0].price;
                                    sizeb12 = "$" + (int)GetRunner[1].ex.availableToBack[0].size;
                                }

                                if (GetRunner[1].ex.availableToLay.Count > 0)
                                {
                                    pricel12 = GetRunner[1].ex.availableToLay[0].price;
                                    sizel12 = "$" + (int)GetRunner[1].ex.availableToLay[0].size;
                                }

                                if (CheckRunner > 2)
                                {
                                    if (GetRunner[2].ex.availableToBack.Count > 0)
                                    {
                                        priceb13 = GetRunner[2].ex.availableToBack[0].price;
                                        sizeb13 = "$" + (int)GetRunner[2].ex.availableToBack[0].size;
                                    }

                                    if (GetRunner[2].ex.availableToLay.Count > 0)
                                    {
                                        pricel13 = GetRunner[2].ex.availableToLay[0].price;
                                        sizel13 = "$" + (int)GetRunner[2].ex.availableToLay[0].size;
                                    }
                                }

                                string runteama = "";
                                string runteamb = "";
                                string event_code1 = "";
                                string MatchTTime = "";
                                string BetfairMarketid = "";
                                for (int iy = 0; iy < cric_runerteamA.Count; iy++)
                                {
                                    if (cricket_bfid_array[iy].ToString() == EvnetMarketId)
                                    {
                                        runteama = cric_runerteamA[iy].ToString();
                                        runteamb = cric_runerteamB[iy].ToString();
                                        event_code1 = cric_eventCode[iy].ToString();
                                        MatchTTime = cric_matchTime[iy].ToString();
                                        BetfairMarketid = EvnetMarketId;
                                    }
                                }

                                messages.Add(item: new OddsGetOne
                                {
                                    EventCode = event_code1,
                                    back1 = priceb11,
                                    lay1 = pricel11,
                                    back11 = priceb12,
                                    lay11 = pricel12,
                                    back22 = priceb13,
                                    lay22 = pricel13,
                                    Runnername = runteama,
                                    RunnernameB = runteamb,
                                    BetfairId = BetfairMarketid,
                                    MatchTime = MatchTTime,
                                    back1size = sizeb11,
                                    back2size = sizeb12,
                                    back3size = sizeb13,
                                    lay1size = sizel11,
                                    lay2size = sizel12,
                                    lay3size = sizel13,
                                    MatchedAmt = totalMatched,
                                    status = MatchStatus
                                });
                            }
                        }
                        else
                        {
                            messages.Add(item: new OddsGetOne
                            {
                                EventCode = "",
                                back1 = "",
                                lay1 = "",
                                back11 = "",
                                lay11 = "",
                                back22 = "",
                                lay22 = "",
                                Runnername = "",
                                RunnernameB = "",
                                BetfairId = "",
                                MatchTime = "",
                                back1size = "",
                                back2size = "",
                                back3size = "",
                                lay1size = "",
                                lay2size = "",
                                lay3size = "",
                                MatchedAmt = "",
                                status = "SUSPENDED"
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("ErrorC" + ex.ToString());
                    }
                }
                catch (TaskCanceledException ex)
                {
                    System.Diagnostics.Debug.WriteLine("ErrorC" + ex.ToString());
                }
            }

            return messages;
        }

        public List<OddsGetOne> getCricketMatDatabf(string cric_ids)
        {
            var messages = new List<OddsGetOne>();
            if (cric_ids != "")
            {
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("https://ero.betfair.com/www/sports/exchange/readonly/v1/bymarket");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("?_ak=nzIFcwyWhrlwYMrh&alt=json&marketIds=" + cric_ids + "&types=EVENT,MARKET_STATE,RUNNER_EXCHANGE_PRICES_BEST,RUNNER_DESCRIPTION").Result;

                        // Blocking call! 
                        if (response != null)
                        {
                            var products = response.Content.ReadAsStringAsync().Result;
                            dynamic responseJson1 = JsonConvert.DeserializeObject(products);
                            var responseJson = responseJson1.eventTypes[0].eventNodes;

                            for (int i = 0; i < responseJson.Count; i++)
                            {
                                string EvnetMarketId = responseJson[i].marketNodes[0].marketId;
                                string totalMatched = "$" + (int)responseJson[i].marketNodes[0].state.totalMatched;
                                string MatchStatus = responseJson[i].marketNodes[0].state.status;

                                string event_code1 = responseJson[i].eventId;
                                DateTime cric_match_time = responseJson[i]["event"]["openDate"];
                                cric_match_time = cric_match_time.AddMinutes(330);
                                string MatchTTime = cric_match_time.ToString("yyyy/MM/dd HH:mm:ss");
                                string BetfairMarketid = EvnetMarketId;

                                if (MatchStatus == "OPEN")
                                {
                                    MatchStatus = "ACTIVE";
                                }
                                var GetRunner = responseJson[i].marketNodes[0].runners;
                                string runteama = GetRunner[0]["description"]["runnerName"];
                                string runteamb = GetRunner[1]["description"]["runnerName"];

                                int CheckRunner = GetRunner.Count;

                                string priceb11 = "";
                                string priceb12 = "";
                                string priceb13 = "";

                                string pricel11 = "";
                                string pricel12 = "";
                                string pricel13 = "";

                                string sizeb11 = "";
                                string sizeb12 = "";
                                string sizeb13 = "";

                                string sizel11 = "";
                                string sizel12 = "";
                                string sizel13 = "";
                                //string RunnerNameAQ = "";
                                if (GetRunner[0].exchange.availableToBack != null)
                                {
                                    if (GetRunner[0].exchange.availableToBack.Count > 0)
                                    {
                                        priceb11 = GetRunner[0].exchange.availableToBack[0].price;
                                        sizeb11 = "$" + (int)GetRunner[0].exchange.availableToBack[0].size;
                                    }
                                }

                                if (GetRunner[0].exchange.availableToLay != null)
                                {
                                    if (GetRunner[0].exchange.availableToLay.Count > 0)
                                    {
                                        pricel11 = GetRunner[0].exchange.availableToLay[0].price;
                                        sizel11 = "$" + (int)GetRunner[0].exchange.availableToLay[0].size;
                                    }
                                }

                                if (GetRunner[1].exchange.availableToBack != null)
                                {
                                    if (GetRunner[1].exchange.availableToBack.Count > 0)
                                    {
                                        priceb12 = GetRunner[1].exchange.availableToBack[0].price;
                                        sizeb12 = "$" + (int)GetRunner[1].exchange.availableToBack[0].size;
                                    }
                                }

                                if (GetRunner[1].exchange.availableToLay != null)
                                {
                                    if (GetRunner[1].exchange.availableToLay.Count > 0)
                                    {
                                        pricel12 = GetRunner[1].exchange.availableToLay[0].price;
                                        sizel12 = "$" + (int)GetRunner[1].exchange.availableToLay[0].size;
                                    }
                                }

                                if (CheckRunner > 2)
                                {
                                    if (GetRunner[2].exchange.availableToBack != null)
                                    {
                                        if (GetRunner[2].exchange.availableToBack.Count > 0)
                                        {
                                            priceb13 = GetRunner[2].exchange.availableToBack[0].price;
                                            sizeb13 = "$" + (int)GetRunner[2].exchange.availableToBack[0].size;
                                        }
                                    }
                                    if (GetRunner[2].exchange.availableToLay != null)
                                    {
                                        if (GetRunner[2].exchange.availableToLay.Count > 0)
                                        {
                                            pricel13 = GetRunner[2].exchange.availableToLay[0].price;
                                            sizel13 = "$" + (int)GetRunner[2].exchange.availableToLay[0].size;
                                        }
                                    }
                                }

                                messages.Add(item: new OddsGetOne
                                {
                                    EventCode = event_code1,
                                    back1 = priceb11,
                                    lay1 = pricel11,
                                    back11 = priceb12,
                                    lay11 = pricel12,
                                    back22 = priceb13,
                                    lay22 = pricel13,
                                    Runnername = runteama,
                                    RunnernameB = runteamb,
                                    BetfairId = BetfairMarketid,
                                    MatchTime = MatchTTime,
                                    back1size = sizeb11,
                                    back2size = sizeb12,
                                    back3size = sizeb13,
                                    lay1size = sizel11,
                                    lay2size = sizel12,
                                    lay3size = sizel13,
                                    MatchedAmt = totalMatched,
                                    status = MatchStatus
                                });
                            }
                        }
                        else
                        {
                            messages.Add(item: new OddsGetOne
                            {
                                EventCode = "",
                                back1 = "",
                                lay1 = "",
                                back11 = "",
                                lay11 = "",
                                back22 = "",
                                lay22 = "",
                                Runnername = "",
                                RunnernameB = "",
                                BetfairId = "",
                                MatchTime = "",
                                back1size = "",
                                back2size = "",
                                back3size = "",
                                lay1size = "",
                                lay2size = "",
                                lay3size = "",
                                MatchedAmt = "",
                                status = "SUSPENDED"
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("ErrorC" + ex.ToString());
                    }
                }
                catch (TaskCanceledException ex)
                {
                    System.Diagnostics.Debug.WriteLine("ErrorC" + ex.ToString());
                }
            }

            return messages;
        }

        public JsonResult getSoccerMat(string socc_ids)
        {

            List<OddsGetOne> messages = new List<OddsGetOne>();
            if (check_sess_id != null && check_sess_id != "")
            {
                messages = getSoccerMatData(socc_ids);
            }
            else
            {
                messages = getSoccerMatDatabf(socc_ids);
            }
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<OddsGetOne> getSoccerMatData(string socc_ids)
        {
            var messages = new List<OddsGetOne>();
            if (socc_ids != "")
            {
                ArrayList soccer_bfid_array = new ArrayList();
                ArrayList socc_runerteamA = new ArrayList();
                ArrayList socc_runerteamB = new ArrayList();
                ArrayList socc_eventCode = new ArrayList();
                ArrayList socc_matchTime = new ArrayList();
                try
                {
                    HttpClient client1 = new HttpClient();
                    client1.BaseAddress = new Uri("https://ero.betfair.com/www/sports/exchange/readonly/v1/bymarket");
                    client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response1 = client1.GetAsync("?_ak=nzIFcwyWhrlwYMrh&alt=json&marketIds=" + socc_ids + "&types=EVENT,MARKET_STATE,RUNNER_DESCRIPTION").Result;
                        if (response1 != null)
                        {
                            var products1 = response1.Content.ReadAsStringAsync().Result;
                            dynamic responseJson1 = JsonConvert.DeserializeObject(products1);
                            var Getmatchdata = responseJson1.eventTypes[0].eventNodes;
                            string socc_eve_code = "";
                            socc_eventCode.Clear();
                            socc_runerteamA.Clear();
                            socc_runerteamB.Clear();
                            soccer_bfid_array.Clear();
                            socc_matchTime.Clear();

                            for (int i1 = 0; i1 < Getmatchdata.Count; i1++)
                            {
                                socc_eve_code = Getmatchdata[i1].eventId;
                                var GetRunnernamearray = Getmatchdata[i1]["marketNodes"][0]["runners"];
                                DateTime socc_match_time = Getmatchdata[i1]["event"]["openDate"];
                                socc_match_time = socc_match_time.AddMinutes(330);
                                string socc_match_time1 = socc_match_time.ToString("yyyy/MM/dd HH:mm:ss");
                                string betfair_code = Getmatchdata[i1]["marketNodes"][0]["marketId"];
                                string teamA = GetRunnernamearray[0]["description"]["runnerName"];
                                string teamB = GetRunnernamearray[1]["description"]["runnerName"];

                                soccer_bfid_array.Add(betfair_code);
                                socc_runerteamA.Add(teamA);
                                socc_runerteamB.Add(teamB);
                                socc_matchTime.Add(socc_match_time1);
                                socc_eventCode.Add(socc_eve_code);
                            }
                        }
                    }
                    catch (Exception ex) { }
                }
                catch (TaskCanceledException ex)
                {
                }
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://api_link.com");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    try
                    {
                        HttpResponseMessage response = client.GetAsync("market/2/29278528/" + socc_ids).Result;

                        // Blocking call! 
                        if (response != null)
                        {
                            var products = response.Content.ReadAsStringAsync().Result;
                            dynamic responseJson = JsonConvert.DeserializeObject(products);

                            for (int i = 0; i < responseJson.Count; i++)
                            {

                                var GetRunner = responseJson[i].runners;
                                int CheckRunner = GetRunner.Count;

                                string priceb11 = "";
                                string priceb12 = "";
                                string priceb13 = "";

                                string pricel11 = "";
                                string pricel12 = "";
                                string pricel13 = "";

                                string sizeb11 = "";
                                string sizeb12 = "";
                                string sizeb13 = "";

                                string sizel11 = "";
                                string sizel12 = "";
                                string sizel13 = "";
                                string MatchStatus = responseJson[0].runners[0].status;
                                string totalMatched = "$" + (int)responseJson[i].totalMatched;
                                string EvnetMarketId = responseJson[i].marketId;

                                if (GetRunner[0].ex.availableToBack.Count > 0)
                                {
                                    priceb11 = GetRunner[0].ex.availableToBack[0].price;
                                    sizeb11 = "$" + (int)GetRunner[0].ex.availableToBack[0].size;
                                }

                                if (GetRunner[0].ex.availableToLay.Count > 0)
                                {
                                    pricel11 = GetRunner[0].ex.availableToLay[0].price;
                                    sizel11 = "$" + (int)GetRunner[0].ex.availableToLay[0].size;
                                }

                                if (GetRunner[1].ex.availableToBack.Count > 0)
                                {
                                    priceb12 = GetRunner[1].ex.availableToBack[0].price;
                                    sizeb12 = "$" + (int)GetRunner[1].ex.availableToBack[0].size;
                                }

                                if (GetRunner[1].ex.availableToLay.Count > 0)
                                {
                                    pricel12 = GetRunner[1].ex.availableToLay[0].price;
                                    sizel12 = "$" + (int)GetRunner[1].ex.availableToLay[0].size;
                                }

                                if (CheckRunner > 2)
                                {
                                    if (GetRunner[2].ex.availableToBack.Count > 0)
                                    {
                                        priceb13 = GetRunner[2].ex.availableToBack[0].price;
                                        sizeb13 = "$" + (int)GetRunner[2].ex.availableToBack[0].size;
                                    }

                                    if (GetRunner[2].ex.availableToLay.Count > 0)
                                    {
                                        pricel13 = GetRunner[2].ex.availableToLay[0].price;
                                        sizel13 = "$" + (int)GetRunner[2].ex.availableToLay[0].size;
                                    }
                                }


                                string runteama = "";
                                string runteamb = "";
                                string event_code1 = "";
                                string MatchTTime = "";
                                string BetfairMarketid = "";
                                for (int iy = 0; iy < socc_runerteamA.Count; iy++)
                                {

                                    if (soccer_bfid_array[iy].ToString() == EvnetMarketId)
                                    {
                                        runteama = socc_runerteamA[iy].ToString();
                                        runteamb = socc_runerteamB[iy].ToString();
                                        event_code1 = socc_eventCode[iy].ToString();
                                        MatchTTime = socc_matchTime[iy].ToString();
                                        BetfairMarketid = EvnetMarketId;
                                    }
                                }


                                messages.Add(item: new OddsGetOne
                                {
                                    EventCode = event_code1,
                                    back1 = priceb11,
                                    lay1 = pricel11,
                                    back11 = priceb12,
                                    lay11 = pricel12,
                                    back22 = priceb13,
                                    lay22 = pricel13,
                                    Runnername = runteama,
                                    RunnernameB = runteamb,
                                    BetfairId = BetfairMarketid,
                                    MatchTime = MatchTTime,
                                    back1size = sizeb11,
                                    back2size = sizeb12,
                                    back3size = sizeb13,
                                    lay1size = sizel11,
                                    lay2size = sizel12,
                                    lay3size = sizel13,
                                    MatchedAmt = totalMatched,
                                    status = MatchStatus
                                });

                            }
                        }
                        else
                        {
                            messages.Add(item: new OddsGetOne
                            {
                                EventCode = "",
                                back1 = "",
                                lay1 = "",
                                back11 = "",
                                lay11 = "",
                                back22 = "",
                                lay22 = "",
                                Runnername = "",
                                RunnernameB = "",
                                BetfairId = "",
                                MatchTime = "",
                                back1size = "",
                                back2size = "",
                                back3size = "",
                                lay1size = "",
                                lay2size = "",
                                lay3size = "",
                                MatchedAmt = "",
                                status = "SUSPENDED"
                            });
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                }
                catch (TaskCanceledException f)
                {

                }
            }


            return messages;
            // return messages;
        }

        public List<OddsGetOne> getSoccerMatDatabf(string socc_ids)
        {
            var messages = new List<OddsGetOne>();
            if (socc_ids != "")
            {
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("https://ero.betfair.com/www/sports/exchange/readonly/v1/bymarket");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("?_ak=nzIFcwyWhrlwYMrh&alt=json&marketIds=" + socc_ids + "&types=EVENT,MARKET_STATE,RUNNER_EXCHANGE_PRICES_BEST,RUNNER_DESCRIPTION").Result;

                        // Blocking call! 
                        if (response != null)
                        {
                            var products = response.Content.ReadAsStringAsync().Result;
                            dynamic responseJson1 = JsonConvert.DeserializeObject(products);
                            var responseJson = responseJson1.eventTypes[0].eventNodes;

                            for (int i = 0; i < responseJson.Count; i++)
                            {
                                string EvnetMarketId = responseJson[i].marketNodes[0].marketId;
                                string totalMatched = "$" + (int)responseJson[i].marketNodes[0].state.totalMatched;
                                string MatchStatus = responseJson[i].marketNodes[0].state.status;

                                string event_code1 = responseJson[i].eventId;
                                DateTime cric_match_time = responseJson[i]["event"]["openDate"];
                                cric_match_time = cric_match_time.AddMinutes(330);
                                string MatchTTime = cric_match_time.ToString("yyyy/MM/dd HH:mm:ss");
                                string BetfairMarketid = EvnetMarketId;

                                if (MatchStatus == "OPEN")
                                {
                                    MatchStatus = "ACTIVE";
                                }
                                var GetRunner = responseJson[i].marketNodes[0].runners;
                                string runteama = GetRunner[0]["description"]["runnerName"];
                                string runteamb = GetRunner[1]["description"]["runnerName"];

                                int CheckRunner = GetRunner.Count;

                                string priceb11 = "";
                                string priceb12 = "";
                                string priceb13 = "";

                                string pricel11 = "";
                                string pricel12 = "";
                                string pricel13 = "";

                                string sizeb11 = "";
                                string sizeb12 = "";
                                string sizeb13 = "";

                                string sizel11 = "";
                                string sizel12 = "";
                                string sizel13 = "";

                                if (GetRunner[0].exchange.availableToBack != null)
                                {
                                    if (GetRunner[0].exchange.availableToBack.Count > 0)
                                    {
                                        priceb11 = GetRunner[0].exchange.availableToBack[0].price;
                                        sizeb11 = "$" + (int)GetRunner[0].exchange.availableToBack[0].size;
                                    }
                                }

                                if (GetRunner[0].exchange.availableToLay != null)
                                {
                                    if (GetRunner[0].exchange.availableToLay.Count > 0)
                                    {
                                        pricel11 = GetRunner[0].exchange.availableToLay[0].price;
                                        sizel11 = "$" + (int)GetRunner[0].exchange.availableToLay[0].size;
                                    }
                                }

                                if (GetRunner[1].exchange.availableToBack != null)
                                {
                                    if (GetRunner[1].exchange.availableToBack.Count > 0)
                                    {
                                        priceb12 = GetRunner[1].exchange.availableToBack[0].price;
                                        sizeb12 = "$" + (int)GetRunner[1].exchange.availableToBack[0].size;
                                    }
                                }

                                if (GetRunner[1].exchange.availableToLay != null)
                                {
                                    if (GetRunner[1].exchange.availableToLay.Count > 0)
                                    {
                                        pricel12 = GetRunner[1].exchange.availableToLay[0].price;
                                        sizel12 = "$" + (int)GetRunner[1].exchange.availableToLay[0].size;
                                    }
                                }

                                if (CheckRunner > 2)
                                {
                                    if (GetRunner[2].exchange.availableToBack != null)
                                    {
                                        if (GetRunner[2].exchange.availableToBack.Count > 0)
                                        {
                                            priceb13 = GetRunner[2].exchange.availableToBack[0].price;
                                            sizeb13 = "$" + (int)GetRunner[2].exchange.availableToBack[0].size;
                                        }
                                    }
                                    if (GetRunner[2].exchange.availableToLay != null)
                                    {
                                        if (GetRunner[2].exchange.availableToLay.Count > 0)
                                        {
                                            pricel13 = GetRunner[2].exchange.availableToLay[0].price;
                                            sizel13 = "$" + (int)GetRunner[2].exchange.availableToLay[0].size;
                                        }
                                    }
                                }

                                messages.Add(item: new OddsGetOne
                                {
                                    EventCode = event_code1,
                                    back1 = priceb11,
                                    lay1 = pricel11,
                                    back11 = priceb12,
                                    lay11 = pricel12,
                                    back22 = priceb13,
                                    lay22 = pricel13,
                                    Runnername = runteama,
                                    RunnernameB = runteamb,
                                    BetfairId = BetfairMarketid,
                                    MatchTime = MatchTTime,
                                    back1size = sizeb11,
                                    back2size = sizeb12,
                                    back3size = sizeb13,
                                    lay1size = sizel11,
                                    lay2size = sizel12,
                                    lay3size = sizel13,
                                    MatchedAmt = totalMatched,
                                    status = MatchStatus
                                });

                            }
                        }
                        else
                        {
                            messages.Add(item: new OddsGetOne
                            {
                                EventCode = "",
                                back1 = "",
                                lay1 = "",
                                back11 = "",
                                lay11 = "",
                                back22 = "",
                                lay22 = "",
                                Runnername = "",
                                RunnernameB = "",
                                BetfairId = "",
                                MatchTime = "",
                                back1size = "",
                                back2size = "",
                                back3size = "",
                                lay1size = "",
                                lay2size = "",
                                lay3size = "",
                                MatchedAmt = "",
                                status = "SUSPENDED"
                            });
                        }

                    }
                    catch (Exception f)
                    {

                    }
                }
                catch (TaskCanceledException f)
                {

                }
            }


            return messages;
            // return messages;
        }

        public JsonResult getTennisMat(string tenn_ids)
        {
            List<OddsGetOne> messages = new List<OddsGetOne>();
            if (check_sess_id != null && check_sess_id != "")
            {
                messages = getTennisMatData(tenn_ids);
            }
            else
            {
                messages = getTennisMatDatabf(tenn_ids);
            }
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<OddsGetOne> getTennisMatData(string tenn_ids)
        {
            var messages = new List<OddsGetOne>();
            if (tenn_ids != "")
            {
                ArrayList tennis_bfid_array = new ArrayList();
                ArrayList tenn_runerteamA = new ArrayList();
                ArrayList tenn_runerteamB = new ArrayList();
                ArrayList tenn_eventCode = new ArrayList();
                ArrayList tenn_matchTime = new ArrayList();
                try
                {
                    HttpClient client1 = new HttpClient();
                    client1.BaseAddress = new Uri("https://ero.betfair.com/www/sports/exchange/readonly/v1/bymarket");
                    client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client1.GetAsync("?_ak=nzIFcwyWhrlwYMrh&alt=json&marketIds=" + tenn_ids + "&types=EVENT,MARKET_STATE,RUNNER_DESCRIPTION").Result;
                        if (response != null)
                        {
                            var products = response.Content.ReadAsStringAsync().Result;
                            dynamic responseJson = JsonConvert.DeserializeObject(products);
                            var Getmatchdata = responseJson.eventTypes[0].eventNodes;
                            string tenn_eve_code = "";
                            tenn_eventCode.Clear();
                            tenn_runerteamA.Clear();
                            tenn_runerteamB.Clear();
                            tennis_bfid_array.Clear();
                            tenn_matchTime.Clear();

                            for (int i1 = 0; i1 < Getmatchdata.Count; i1++)
                            {
                                tenn_eve_code = Getmatchdata[i1].eventId;
                                var GetRunnernamearray = Getmatchdata[i1]["marketNodes"][0]["runners"];
                                DateTime tenn_match_time = Getmatchdata[i1]["event"]["openDate"];
                                tenn_match_time = tenn_match_time.AddMinutes(330);
                                string tenn_match_time1 = tenn_match_time.ToString("yyyy/MM/dd HH:mm:ss");
                                string betfair_code = Getmatchdata[i1]["marketNodes"][0]["marketId"];
                                string teamA = GetRunnernamearray[0]["description"]["runnerName"];
                                string teamB = GetRunnernamearray[1]["description"]["runnerName"];

                                tennis_bfid_array.Add(betfair_code);
                                tenn_runerteamA.Add(teamA);
                                tenn_runerteamB.Add(teamB);
                                tenn_matchTime.Add(tenn_match_time1);
                                tenn_eventCode.Add(tenn_eve_code);
                            }
                        }
                    }
                    catch (Exception ex) { }
                }
                catch (TaskCanceledException f)
                {

                }
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://api_link.com");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("market/2/29278528/" + tenn_ids).Result;

                        // Blocking call! 

                        if (response != null)
                        {
                            var products = response.Content.ReadAsStringAsync().Result;
                            dynamic responseJson = JsonConvert.DeserializeObject(products);

                            for (int i = 0; i < responseJson.Count; i++)
                            {

                                var GetRunner = responseJson[i].runners;
                                int CheckRunner = GetRunner.Count;

                                string priceb11 = "";
                                string priceb12 = "";
                                string priceb13 = "";

                                string pricel11 = "";
                                string pricel12 = "";
                                string pricel13 = "";

                                string sizeb11 = "";
                                string sizeb12 = "";
                                string sizeb13 = "";

                                string sizel11 = "";
                                string sizel12 = "";
                                string sizel13 = "";
                                string MatchStatus = responseJson[i].runners[0].status;
                                string totalMatched = "$" + (int)responseJson[i].totalMatched;
                                string EvnetMarketId = responseJson[i].marketId;

                                if (GetRunner[0].ex.availableToBack.Count > 0)
                                {
                                    priceb11 = GetRunner[0].ex.availableToBack[0].price;
                                    sizeb11 = "$" + (int)GetRunner[0].ex.availableToBack[0].size;
                                }

                                if (GetRunner[0].ex.availableToLay.Count > 0)
                                {
                                    pricel11 = GetRunner[0].ex.availableToLay[0].price;
                                    sizel11 = "$" + (int)GetRunner[0].ex.availableToLay[0].size;
                                }

                                if (GetRunner[1].ex.availableToBack.Count > 0)
                                {
                                    priceb12 = GetRunner[1].ex.availableToBack[0].price;
                                    sizeb12 = "$" + (int)GetRunner[1].ex.availableToBack[0].size;
                                }

                                if (GetRunner[1].ex.availableToLay.Count > 0)
                                {
                                    pricel12 = GetRunner[1].ex.availableToLay[0].price;
                                    sizel12 = "$" + (int)GetRunner[1].ex.availableToLay[0].size;
                                }

                                string runteama = "";
                                string runteamb = "";
                                string event_code1 = "";
                                string MatchTTime = "";
                                string BetfairMarketid = "";
                                for (int iy = 0; iy < tenn_runerteamA.Count; iy++)
                                {

                                    if (tennis_bfid_array[iy].ToString() == EvnetMarketId)
                                    {
                                        runteama = tenn_runerteamA[iy].ToString();
                                        runteamb = tenn_runerteamB[iy].ToString();
                                        event_code1 = tenn_eventCode[iy].ToString();
                                        MatchTTime = tenn_matchTime[iy].ToString();
                                        BetfairMarketid = EvnetMarketId;
                                    }
                                }


                                messages.Add(item: new OddsGetOne
                                {
                                    EventCode = event_code1,
                                    back1 = priceb11,
                                    lay1 = pricel11,
                                    back11 = priceb12,
                                    lay11 = pricel12,
                                    back22 = priceb13,
                                    lay22 = pricel13,
                                    Runnername = runteama,
                                    RunnernameB = runteamb,
                                    BetfairId = BetfairMarketid,
                                    MatchTime = MatchTTime,
                                    back1size = sizeb11,
                                    back2size = sizeb12,
                                    back3size = sizeb13,
                                    lay1size = sizel11,
                                    lay2size = sizel12,
                                    lay3size = sizel13,
                                    MatchedAmt = totalMatched,
                                    status = MatchStatus
                                });

                            }
                        }
                        else
                        {
                            messages.Add(item: new OddsGetOne
                            {
                                EventCode = "",
                                back1 = "",
                                lay1 = "",
                                back11 = "",
                                lay11 = "",
                                back22 = "",
                                lay22 = "",
                                Runnername = "",
                                RunnernameB = "",
                                BetfairId = "",
                                MatchTime = "",
                                back1size = "",
                                back2size = "",
                                back3size = "",
                                lay1size = "",
                                lay2size = "",
                                lay3size = "",
                                MatchedAmt = "",
                                status = "SUSPENDED"
                            });
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }
                catch (TaskCanceledException f)
                {

                }
            }

            return messages;
            // return messages;
        }
        public List<OddsGetOne> getTennisMatDatabf(string tenn_ids)
        {
            var messages = new List<OddsGetOne>();
            if (tenn_ids != "")
            {
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("https://ero.betfair.com/www/sports/exchange/readonly/v1/bymarket");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("?_ak=nzIFcwyWhrlwYMrh&alt=json&marketIds=" + tenn_ids + "&types=EVENT,MARKET_STATE,RUNNER_EXCHANGE_PRICES_BEST,RUNNER_DESCRIPTION").Result;

                        // Blocking call! 
                        if (response != null)
                        {
                            var products = response.Content.ReadAsStringAsync().Result;
                            dynamic responseJson1 = JsonConvert.DeserializeObject(products);
                            var responseJson = responseJson1.eventTypes[0].eventNodes;

                            for (int i = 0; i < responseJson.Count; i++)
                            {
                                string EvnetMarketId = responseJson[i].marketNodes[0].marketId;
                                string totalMatched = "$" + (int)responseJson[i].marketNodes[0].state.totalMatched;
                                string MatchStatus = responseJson[i].marketNodes[0].state.status;
                                string event_code1 = responseJson[i].eventId;
                                DateTime cric_match_time = responseJson[i]["event"]["openDate"];
                                cric_match_time = cric_match_time.AddMinutes(330);
                                string MatchTTime = cric_match_time.ToString("yyyy/MM/dd HH:mm:ss");
                                string BetfairMarketid = EvnetMarketId;

                                if (MatchStatus == "OPEN")
                                {
                                    MatchStatus = "ACTIVE";
                                }
                                var GetRunner = responseJson[i].marketNodes[0].runners;
                                string runteama = GetRunner[0]["description"]["runnerName"];
                                string runteamb = GetRunner[1]["description"]["runnerName"];
                                int CheckRunner = GetRunner.Count;

                                string priceb11 = "";
                                string priceb12 = "";
                                string priceb13 = "";

                                string pricel11 = "";
                                string pricel12 = "";
                                string pricel13 = "";

                                string sizeb11 = "";
                                string sizeb12 = "";
                                string sizeb13 = "";

                                string sizel11 = "";
                                string sizel12 = "";
                                string sizel13 = "";

                                if (GetRunner[0].exchange.availableToBack != null)
                                {
                                    if (GetRunner[0].exchange.availableToBack.Count > 0)
                                    {
                                        priceb11 = GetRunner[0].exchange.availableToBack[0].price;
                                        sizeb11 = "$" + (int)GetRunner[0].exchange.availableToBack[0].size;
                                    }
                                }

                                if (GetRunner[0].exchange.availableToLay != null)
                                {
                                    if (GetRunner[0].exchange.availableToLay.Count > 0)
                                    {
                                        pricel11 = GetRunner[0].exchange.availableToLay[0].price;
                                        sizel11 = "$" + (int)GetRunner[0].exchange.availableToLay[0].size;
                                    }
                                }

                                if (GetRunner[1].exchange.availableToBack != null)
                                {
                                    if (GetRunner[1].exchange.availableToBack.Count > 0)
                                    {
                                        priceb12 = GetRunner[1].exchange.availableToBack[0].price;
                                        sizeb12 = "$" + (int)GetRunner[1].exchange.availableToBack[0].size;
                                    }
                                }

                                if (GetRunner[1].exchange.availableToLay != null)
                                {
                                    if (GetRunner[1].exchange.availableToLay.Count > 0)
                                    {
                                        pricel12 = GetRunner[1].exchange.availableToLay[0].price;
                                        sizel12 = "$" + (int)GetRunner[1].exchange.availableToLay[0].size;
                                    }
                                }

                                messages.Add(item: new OddsGetOne
                                {
                                    EventCode = event_code1,
                                    back1 = priceb11,
                                    lay1 = pricel11,
                                    back11 = priceb12,
                                    lay11 = pricel12,
                                    back22 = priceb13,
                                    lay22 = pricel13,
                                    Runnername = runteama,
                                    RunnernameB = runteamb,
                                    BetfairId = BetfairMarketid,
                                    MatchTime = MatchTTime,
                                    back1size = sizeb11,
                                    back2size = sizeb12,
                                    back3size = sizeb13,
                                    lay1size = sizel11,
                                    lay2size = sizel12,
                                    lay3size = sizel13,
                                    MatchedAmt = totalMatched,
                                    status = MatchStatus
                                });

                            }
                        }
                        else
                        {
                            messages.Add(item: new OddsGetOne
                            {
                                EventCode = "",
                                back1 = "",
                                lay1 = "",
                                back11 = "",
                                lay11 = "",
                                back22 = "",
                                lay22 = "",
                                Runnername = "",
                                RunnernameB = "",
                                BetfairId = "",
                                MatchTime = "",
                                back1size = "",
                                back2size = "",
                                back3size = "",
                                lay1size = "",
                                lay2size = "",
                                lay3size = "",
                                MatchedAmt = "",
                                status = "SUSPENDED"
                            });
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }
                catch (TaskCanceledException f)
                {

                }
            }

            return messages;
            // return messages;
        }

        public JsonResult GetMessages1MView(string Mmatch_ids)
        {

            List<OddsGetOne> messages = new List<OddsGetOne>();
            if (check_sess_id != null && check_sess_id != "")
            {
                messages = GetAllMessagesNewMMVIew(Mmatch_ids);
            }
            else
            {
                messages = GetAllMessagesNewMMVIewbf(Mmatch_ids);
            }
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<OddsGetOne> GetAllMessagesNewMMVIew(string Mmatch_ids)
        {
            var messages = new List<OddsGetOne>();
            try
            {
                ArrayList Mcricket_bfid_array = new ArrayList();
                ArrayList Mcric_runerteamA = new ArrayList();
                ArrayList Mcric_runerteamB = new ArrayList();
                ArrayList Mcric_eventCode = new ArrayList();
                ArrayList Mcric_matchTime = new ArrayList();
                try
                {
                    HttpClient client1 = new HttpClient();
                    client1.BaseAddress = new Uri("https://ero.betfair.com/www/sports/exchange/readonly/v1/bymarket");
                    client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response1 = client1.GetAsync("?_ak=nzIFcwyWhrlwYMrh&alt=json&marketIds=" + Mmatch_ids + "&types=EVENT,MARKET_STATE,RUNNER_DESCRIPTION").Result;
                        if (response1 != null)
                        {
                            var products1 = response1.Content.ReadAsStringAsync().Result;
                            dynamic responseJson1 = JsonConvert.DeserializeObject(products1);
                            var Getmatchdata = responseJson1.eventTypes[0].eventNodes;
                            string Mcric_eve_code = "";
                            Mcric_eventCode.Clear();
                            Mcric_runerteamA.Clear();
                            Mcric_runerteamB.Clear();
                            Mcricket_bfid_array.Clear();
                            Mcric_matchTime.Clear();

                            for (int i1 = 0; i1 < Getmatchdata.Count; i1++)
                            {
                                Mcric_eve_code = Getmatchdata[i1].eventId;
                                var GetRunnernamearray = Getmatchdata[i1]["marketNodes"][0]["runners"];
                                DateTime Mcric_match_time = Getmatchdata[i1]["event"]["openDate"];
                                Mcric_match_time = Mcric_match_time.AddMinutes(330);
                                string Mcric_match_time1 = Mcric_match_time.ToString("yyyy/MM/dd HH:mm:ss");
                                string betfair_code = Getmatchdata[i1]["marketNodes"][0]["marketId"];
                                string teamA = GetRunnernamearray[0]["description"]["runnerName"];
                                string teamB = GetRunnernamearray[1]["description"]["runnerName"];

                                Mcricket_bfid_array.Add(betfair_code);
                                Mcric_runerteamA.Add(teamA);
                                Mcric_runerteamB.Add(teamB);
                                Mcric_matchTime.Add(Mcric_match_time1);
                                Mcric_eventCode.Add(Mcric_eve_code);
                            }
                        }
                    }
                    catch (Exception ex) { }
                }
                catch (TaskCanceledException f)
                {

                }
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://api_link.com");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("market/2/29278528/" + Mmatch_ids).Result;

                        // Blocking call! 
                        if (response != null)
                        {
                            var products = response.Content.ReadAsStringAsync().Result;
                            dynamic responseJson = JsonConvert.DeserializeObject(products);
                            for (int i = 0; i < responseJson.Count; i++)
                            {
                                var GetRunner = responseJson[i].runners;
                                int CheckRunner = GetRunner.Count;

                                string priceb11 = "";
                                string priceb12 = "";
                                string priceb13 = "";

                                string pricel11 = "";
                                string pricel12 = "";
                                string pricel13 = "";

                                string sizeb11 = "";
                                string sizeb12 = "";
                                string sizeb13 = "";

                                string sizel11 = "";
                                string sizel12 = "";
                                string sizel13 = "";
                                string MatchStatus = responseJson[i].runners[0].status;
                                string totalMatched = "$" + (int)responseJson[i].totalMatched;
                                string EvnetMarketId = responseJson[i].marketId;
                                //string RunnerNameAQ = "";

                                if (GetRunner[0].ex.availableToBack.Count > 0)
                                {
                                    priceb11 = GetRunner[0].ex.availableToBack[0].price;
                                    sizeb11 = "$" + (int)GetRunner[0].ex.availableToBack[0].size;
                                }

                                if (GetRunner[0].ex.availableToLay.Count > 0)
                                {
                                    pricel11 = GetRunner[0].ex.availableToLay[0].price;
                                    sizel11 = "$" + (int)GetRunner[0].ex.availableToLay[0].size;
                                }

                                if (GetRunner[1].ex.availableToBack.Count > 0)
                                {
                                    priceb12 = GetRunner[1].ex.availableToBack[0].price;
                                    sizeb12 = "$" + (int)GetRunner[1].ex.availableToBack[0].size;
                                }

                                if (GetRunner[1].ex.availableToLay.Count > 0)
                                {
                                    pricel12 = GetRunner[1].ex.availableToLay[0].price;
                                    sizel12 = "$" + (int)GetRunner[1].ex.availableToLay[0].size;
                                }

                                if (CheckRunner > 2)
                                {
                                    if (GetRunner[2].ex.availableToBack.Count > 0)
                                    {
                                        priceb13 = GetRunner[2].ex.availableToBack[0].price;
                                        sizeb13 = "$" + (int)GetRunner[2].ex.availableToBack[0].size;
                                    }

                                    if (GetRunner[2].ex.availableToLay.Count > 0)
                                    {
                                        pricel13 = GetRunner[2].ex.availableToLay[0].price;
                                        sizel13 = "$" + (int)GetRunner[2].ex.availableToLay[0].size;
                                    }
                                }

                                string runteama = "";
                                string runteamb = "";
                                string event_code1 = "";
                                string MatchTTime = "";

                                for (int iya = 0; iya < Mcric_runerteamA.Count; iya++)
                                {
                                    if (Mcricket_bfid_array[iya].ToString() == EvnetMarketId)
                                    {
                                        runteama = Mcric_runerteamA[iya].ToString();
                                        runteamb = Mcric_runerteamB[iya].ToString();
                                        event_code1 = Mcric_eventCode[iya].ToString();
                                        MatchTTime = Mcric_matchTime[iya].ToString();
                                    }
                                }

                                messages.Add(item: new OddsGetOne
                                {
                                    EventCode = event_code1,
                                    back1 = priceb11,
                                    lay1 = pricel11,
                                    back11 = priceb12,
                                    lay11 = pricel12,
                                    back22 = priceb13,
                                    lay22 = pricel13,
                                    Runnername = runteama,
                                    RunnernameB = runteamb,
                                    BetfairId = EvnetMarketId,
                                    MatchTime = MatchTTime,
                                    back1size = sizeb11,
                                    back2size = sizeb12,
                                    back3size = sizeb13,
                                    lay1size = sizel11,
                                    lay2size = sizel12,
                                    lay3size = sizel13,
                                    MatchedAmt = totalMatched,
                                    status = MatchStatus
                                });
                            }
                        }
                        else
                        {
                            messages.Add(item: new OddsGetOne
                            {
                                EventCode = "",
                                back1 = "",
                                lay1 = "",
                                back11 = "",
                                lay11 = "",
                                back22 = "",
                                lay22 = "",
                                Runnername = "",
                                RunnernameB = "",
                                BetfairId = "",
                                MatchTime = "",
                                back1size = "",
                                back2size = "",
                                back3size = "",
                                lay1size = "",
                                lay2size = "",
                                lay3size = "",
                                MatchedAmt = "",
                                status = "SUSPENDED"
                            });
                        }
                    }
                    catch (Exception f)
                    {

                    }
                }
                catch (TaskCanceledException f)
                {

                }

            }
            catch (TaskCanceledException ex)
            {

            }


            return messages;
        }

        public List<OddsGetOne> GetAllMessagesNewMMVIewbf(string Mmatch_ids)
        {
            var messages = new List<OddsGetOne>();
            if (Mmatch_ids != "")
            {
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("https://ero.betfair.com/www/sports/exchange/readonly/v1/bymarket");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("?_ak=nzIFcwyWhrlwYMrh&alt=json&marketIds=" + Mmatch_ids + "&types=EVENT,MARKET_STATE,RUNNER_EXCHANGE_PRICES_BEST,RUNNER_DESCRIPTION").Result;

                        // Blocking call! 
                        if (response != null)
                        {
                            var products = response.Content.ReadAsStringAsync().Result;
                            dynamic responseJson1 = JsonConvert.DeserializeObject(products);
                            var responseJson = responseJson1.eventTypes[0].eventNodes;

                            for (int i = 0; i < responseJson.Count; i++)
                            {
                                string EvnetMarketId = responseJson[i].marketNodes[0].marketId;
                                string totalMatched = "$" + (int)responseJson[i].marketNodes[0].state.totalMatched;
                                string MatchStatus = responseJson[i].marketNodes[0].state.status;
                                string event_code1 = responseJson[i].eventId;
                                DateTime cric_match_time = responseJson[i]["event"]["openDate"];
                                cric_match_time = cric_match_time.AddMinutes(330);
                                string MatchTTime = cric_match_time.ToString("yyyy/MM/dd HH:mm:ss");
                                string BetfairMarketid = EvnetMarketId;

                                if (MatchStatus == "OPEN")
                                {
                                    MatchStatus = "ACTIVE";
                                }
                                var GetRunner = responseJson[i].marketNodes[0].runners;
                                string runteama = GetRunner[0]["description"]["runnerName"];
                                string runteamb = GetRunner[1]["description"]["runnerName"];
                                int CheckRunner = GetRunner.Count;

                                string priceb11 = "";
                                string priceb12 = "";
                                string priceb13 = "";

                                string pricel11 = "";
                                string pricel12 = "";
                                string pricel13 = "";

                                string sizeb11 = "";
                                string sizeb12 = "";
                                string sizeb13 = "";

                                string sizel11 = "";
                                string sizel12 = "";
                                string sizel13 = "";
                                //string RunnerNameAQ = "";

                                if (GetRunner[0].exchange.availableToBack != null)
                                {
                                    if (GetRunner[0].exchange.availableToBack.Count > 0)
                                    {
                                        priceb11 = GetRunner[0].exchange.availableToBack[0].price;
                                        sizeb11 = "$" + (int)GetRunner[0].exchange.availableToBack[0].size;
                                    }
                                }

                                if (GetRunner[0].exchange.availableToLay != null)
                                {
                                    if (GetRunner[0].exchange.availableToLay.Count > 0)
                                    {
                                        pricel11 = GetRunner[0].exchange.availableToLay[0].price;
                                        sizel11 = "$" + (int)GetRunner[0].exchange.availableToLay[0].size;
                                    }
                                }

                                if (GetRunner[1].exchange.availableToBack != null)
                                {
                                    if (GetRunner[1].exchange.availableToBack.Count > 0)
                                    {
                                        priceb12 = GetRunner[1].exchange.availableToBack[0].price;
                                        sizeb12 = "$" + (int)GetRunner[1].exchange.availableToBack[0].size;
                                    }
                                }

                                if (GetRunner[1].exchange.availableToLay != null)
                                {
                                    if (GetRunner[1].exchange.availableToLay.Count > 0)
                                    {
                                        pricel12 = GetRunner[1].exchange.availableToLay[0].price;
                                        sizel12 = "$" + (int)GetRunner[1].exchange.availableToLay[0].size;
                                    }
                                }

                                if (CheckRunner > 2)
                                {
                                    if (GetRunner[2].exchange.availableToBack != null)
                                    {
                                        if (GetRunner[2].exchange.availableToBack.Count > 0)
                                        {
                                            priceb13 = GetRunner[2].exchange.availableToBack[0].price;
                                            sizeb13 = "$" + (int)GetRunner[2].exchange.availableToBack[0].size;
                                        }
                                    }
                                    if (GetRunner[2].exchange.availableToLay != null)
                                    {
                                        if (GetRunner[2].exchange.availableToLay.Count > 0)
                                        {
                                            pricel13 = GetRunner[2].exchange.availableToLay[0].price;
                                            sizel13 = "$" + (int)GetRunner[2].exchange.availableToLay[0].size;
                                        }
                                    }
                                }

                                messages.Add(item: new OddsGetOne
                                {
                                    EventCode = event_code1,
                                    back1 = priceb11,
                                    lay1 = pricel11,
                                    back11 = priceb12,
                                    lay11 = pricel12,
                                    back22 = priceb13,
                                    lay22 = pricel13,
                                    Runnername = runteama,
                                    RunnernameB = runteamb,
                                    BetfairId = EvnetMarketId,
                                    MatchTime = MatchTTime,
                                    back1size = sizeb11,
                                    back2size = sizeb12,
                                    back3size = sizeb13,
                                    lay1size = sizel11,
                                    lay2size = sizel12,
                                    lay3size = sizel13,
                                    MatchedAmt = totalMatched,
                                    status = MatchStatus
                                });
                            }
                        }
                        else
                        {
                            messages.Add(item: new OddsGetOne
                            {
                                EventCode = "",
                                back1 = "",
                                lay1 = "",
                                back11 = "",
                                lay11 = "",
                                back22 = "",
                                lay22 = "",
                                Runnername = "",
                                RunnernameB = "",
                                BetfairId = "",
                                MatchTime = "",
                                back1size = "",
                                back2size = "",
                                back3size = "",
                                lay1size = "",
                                lay2size = "",
                                lay3size = "",
                                MatchedAmt = "",
                                status = "SUSPENDED"
                            });
                        }
                    }
                    catch (Exception f)
                    {

                    }

                }
                catch (TaskCanceledException ex)
                {

                }

            }


            return messages;
        }

        public JsonResult GetMessages1MView1()
        {

            List<OddsGetOne> messages = new List<OddsGetOne>();
            messages = GetAllMessagesNewMMVIewbf1();
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<OddsGetOne> GetAllMessagesNewMMVIewbf1()
        {
            var messages = new List<OddsGetOne>();
            string dl_block_sports_string = FunctionDataController.getblocksport();
            string dl_block_leagues_string = FunctionDataController.getblockleague();
            string dl_block_events_string = FunctionDataController.getblockevent();
            DateTime time = DateTime.Now;
            string format = "yyyy-MM-dd HH:mm:ss";
            try
            {
                using (var connection = new SqlConnection(_connString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT match_title, match_time,teama,teamb,betfair_id,event_code from matches where sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND league_id = '88' AND betfair_id != '0' AND teama != 'nikhil' AND status='OPEN' order by match_time ", connection))
                    {
                        var reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string EvnetMarketId = (string)reader["betfair_id"];
                                string event_code1 = (string)reader["event_code"];
                                string runteama = (string)reader["teama"];
                                string runteamb = (string)reader["teamb"];
                                string match_title = (string)reader["match_title"];
                                DateTime cric_match_time = (DateTime)reader["match_time"];
                                string play = "false";
                                if (time > cric_match_time)
                                {
                                    play = "true";
                                }
                                string MatchTTime = cric_match_time.ToString("HH:mm");
                                string MatchDate = cric_match_time.ToString("dd/MM");
                                messages.Add(item: new OddsGetOne
                                {
                                    EventCode = event_code1,
                                    Runnername = match_title,
                                    RunnernameB = runteamb,
                                    BetfairId = EvnetMarketId,
                                    MatchTime = MatchTTime,
                                    MatchDate = MatchDate,
                                    MatchedAmt = "0",
                                    play = play
                                });

                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {

            }

            return messages;
        }

        public JsonResult getmatchlistspo(string sp_id)
        {
            List<OddsGetOne> messages = new List<OddsGetOne>();
            messages = getmatchlistspo1(sp_id);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<OddsGetOne> getmatchlistspo1(string sp_id)
        {
            var messages = new List<OddsGetOne>();
            DateTime time = DateTime.Now;
            string format = "yyyy-MM-dd HH:mm:ss";
            try
            {
                using (var connection = new SqlConnection(_connString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT matches.e_sport, matches.is_fancy, matches.book_id, matches.tv_ch, matches.match_title, matches.match_time, matches.teama, matches.teamb, matches.betfair_id, matches.event_code, matches.league_id FROM matches WHERE matches.sport_id = '" + sp_id + "'AND matches.betfair_id != '0'AND matches.teama != 'nikhil'AND matches.status = 'OPEN'ORDER BY matches.match_time", connection))
                    {
                        var reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string EvnetMarketId = (string)reader["betfair_id"];
                                string event_code1 = (string)reader["event_code"];
                                string runteama = (string)reader["teama"];
                                string runteamb = (string)reader["teamb"];
                                string match_title = (string)reader["match_title"];
                                string lg_id = (string)reader["league_id"];
                                string book_id = (string)reader["book_id"];
                                string tv_ch = (string)reader["tv_ch"];
                                int e_sport = (int)reader["e_sport"];
                                int is_fancy = (int)reader["is_fancy"];
                                DateTime cric_match_time = (DateTime)reader["match_time"];
                                string play = "false";
                                if (time > cric_match_time)
                                {
                                    play = "true";
                                }
                                string MatchTTime = cric_match_time.ToString("HH:mm");
                                string MatchDate = cric_match_time.ToString("dd/MM");
                                messages.Add(item: new OddsGetOne
                                {
                                    EventCode = event_code1,
                                    Runnername = match_title,
                                    RunnernameB = runteamb,
                                    BetfairId = EvnetMarketId,
                                    MatchTime = MatchTTime,
                                    MatchDate = MatchDate,
                                    MatchedAmt = "0",
                                    play = play,
                                    lg_id = lg_id,
                                    book_id = book_id,
                                    e_sport = e_sport,
                                    is_fancy = is_fancy,
                                    tv_ch = tv_ch
                                });

                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return messages;
        }
        public JsonResult getmatchlistspolive(string sp_id)
        {
            List<OddsGetOne> messages = new List<OddsGetOne>();
            messages = getmatchlistspolive1(sp_id);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }
        public List<OddsGetOne> getmatchlistspolive1(string sp_id)
        {
            var messages = new List<OddsGetOne>();
            string dl_block_sports_string = FunctionDataController.getblocksport();
            string dl_block_leagues_string = FunctionDataController.getblockleague();
            string dl_block_events_string = FunctionDataController.getblockevent();
            DateTime time = DateTime.Now;
            string format = "yyyy-MM-dd HH:mm:ss";
            try
            {
                using (var connection = new SqlConnection(_connString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT e_sport,is_fancy,book_id,tv_ch,match_title, match_time,teama,teamb,betfair_id,event_code,league_id from matches where sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND sport_id = '" + sp_id + "' AND betfair_id != '0' AND teama != 'nikhil' AND status='OPEN' AND match_time<'" + time.ToString(format) + "' order by match_time ", connection))
                    {
                        var reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string EvnetMarketId = (string)reader["betfair_id"];
                                string event_code1 = (string)reader["event_code"];
                                string runteama = (string)reader["teama"];
                                string runteamb = (string)reader["teamb"];
                                string match_title = (string)reader["match_title"];
                                DateTime cric_match_time = (DateTime)reader["match_time"];
                                string lg_id = (string)reader["league_id"];
                                string book_id = (string)reader["book_id"];
                                int e_sport = (int)reader["e_sport"];
                                int is_fancy = (int)reader["is_fancy"];
                                string tv_ch = (string)reader["tv_ch"];
                                string play = "false";
                                if (time > cric_match_time)
                                {
                                    play = "true";
                                }
                                string MatchTTime = cric_match_time.ToString("HH:mm");
                                string MatchDate = cric_match_time.ToString("dd/MM");
                                messages.Add(item: new OddsGetOne
                                {
                                    EventCode = event_code1,
                                    Runnername = match_title,
                                    RunnernameB = runteamb,
                                    BetfairId = EvnetMarketId,
                                    MatchTime = MatchTTime,
                                    MatchDate = MatchDate,
                                    MatchedAmt = "0",
                                    play = play,
                                    lg_id = lg_id,
                                    book_id = book_id,
                                    e_sport = e_sport,
                                    is_fancy = is_fancy,
                                    tv_ch = tv_ch
                                });

                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {

            }

            return messages;
        }

        public ActionResult MMatchList(string sportid, string leagueid)
        {
            try
            {
                string dl_block_sports_string = FunctionDataController.getblocksport();
                string dl_block_leagues_string = FunctionDataController.getblockleague();
                string dl_block_events_string = FunctionDataController.getblockevent();
                DateTime time = DateTime.Now;
                string format = "yyyy-MM-dd HH:mm:ss";
                if (sportid == null)
                {
                    sportid = "4";
                }
                string LeagueIdGetHere = leagueid;
                string GetLeagueNameDBH = GetSportsNameForClick(sportid);
                League_Name_SendTop = GetLeagueNameDBH;
                ViewBag.LeagueName = League_Name_SendTop;
                ViewBag.SportsId = sportid;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    string stmts = "SELECT COUNT(id) FROM matches where sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND sport_id!=7888 AND betfair_id != '0' AND teama != 'nikhil' AND status='OPEN' AND match_time < '" + time.ToString(format) + "' ";
                    int countinplay = 0;
                    using (SqlCommand cmdCounts = new SqlCommand(stmts, con))
                    {
                        countinplay = (int)cmdCounts.ExecuteScalar();
                    }
                    con.Close();
                    ViewBag.countinplay = countinplay;
                }
            }
            catch (Exception f)
            {
                System.Diagnostics.Debug.WriteLine("sfsfsfsf" + f.ToString());
            }
            return View();
        }

        public ActionResult MGullyMatch(string sportid)
        {
            try
            {
                string dl_block_sports_string = FunctionDataController.getblocksport();
                string dl_block_leagues_string = FunctionDataController.getblockleague();
                string dl_block_events_string = FunctionDataController.getblockevent();
                DateTime time = DateTime.Now;
                string format = "yyyy-MM-dd HH:mm:ss";

                string leagueid = "88";
                sportid = "4";
                string LeagueIdGetHere = leagueid;
                string GetLeagueNameDBH = GetSportsNameForClick(sportid);
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    string stmts = "SELECT COUNT(id) FROM matches where sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND sport_id!=7888 AND betfair_id != '0' AND teama != 'nikhil' AND status='OPEN' AND match_time < '" + time.ToString(format) + "' ";
                    int countinplay = 0;
                    using (SqlCommand cmdCounts = new SqlCommand(stmts, con))
                    {
                        countinplay = (int)cmdCounts.ExecuteScalar();
                    }
                    con.Close();
                    ViewBag.countinplay = countinplay;
                }
                League_Name_SendTop = GetLeagueNameDBH;
                ViewBag.LeagueName = League_Name_SendTop;
                ViewBag.SportsId = sportid;
            }
            catch (Exception f)
            {

            }
            return View();
        }

        public ActionResult MInplay(string sportid)
        {
            var AllsportMLiveList = new List<ListAllsportMLive>();
            try
            {
                string dl_block_sports_string = FunctionDataController.getblocksport();
                string dl_block_leagues_string = FunctionDataController.getblockleague();
                string dl_block_events_string = FunctionDataController.getblockevent();
                if (sportid != null)
                {
                    ViewBag.SportsId = sportid;
                    ViewBag.SportName = FunctionDataController.GetSportsNameById(sportid);
                    DateTime time = DateTime.Now;
                    string format = "yyyy-MM-dd HH:mm:ss";
                    int countinplay = 0;
                    string Minplay_ids = "";
                    using (var connection = new SqlConnection(_connString))
                    {
                        connection.Open();

                        using (var command = new SqlCommand("SELECT [sport_id],[sport_name] FROM sports where sport_id Not IN(" + dl_block_sports_string + ") AND status='activate' AND sport_id!='7888' ", connection))
                        {
                            var reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                string sport_id = (string)reader["sport_id"];
                                string sport_name = (string)reader["sport_name"];

                                string stmtc = "SELECT COUNT(id) FROM matches where sport_id!='7888' AND sport_id Not IN(" + dl_block_sports_string + ") AND  league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND sport_id = '" + sport_id + "'  AND sport_id!='7888' AND betfair_id != '0' AND teama != 'nikhil' AND status='OPEN' AND match_time < '" + time.ToString(format) + "' ";
                                int countinplayc = 0;
                                using (SqlCommand cmdCountc = new SqlCommand(stmtc, connection))
                                {
                                    countinplayc = (int)cmdCountc.ExecuteScalar();
                                }
                                if (countinplayc > 0)
                                {
                                    AllsportMLiveList.Add(item: new ListAllsportMLive
                                    {
                                        SportsName = sport_name,
                                        SportsId = sport_id,
                                        countinplayc = countinplayc
                                    });
                                }
                            }
                        }


                        connection.Close();
                        ViewBag.Minplay_ids = Minplay_ids;
                    }
                }
                else
                {
                    int buj_cric = 0;
                    int buj_socc = 0;
                    int buj_tenn = 0;
                    string SPID = "0";
                    DateTime time = DateTime.Now;
                    string format = "yyyy-MM-dd HH:mm:ss";
                    int countinplay = 0;
                    string Minplay_ids = "";
                    using (var connection = new SqlConnection(_connString))
                    {
                        connection.Open();

                        using (var command = new SqlCommand("SELECT [sport_id],[sport_name] FROM sports where sport_id Not IN(" + dl_block_sports_string + ") AND status='activate' AND sport_id!='7888' ", connection))
                        {
                            var reader = command.ExecuteReader();
                            while (reader.Read())
                            {

                                string sport_id = (string)reader["sport_id"];
                                string sport_name = (string)reader["sport_name"];

                                string stmtc = "SELECT COUNT(id) FROM matches where sport_id!='7888' AND sport_id Not IN(" + dl_block_sports_string + ") AND  league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND sport_id = '" + sport_id + "'  AND sport_id!='7888' AND betfair_id != '0' AND teama != 'nikhil' AND status='OPEN' AND match_time < '" + time.ToString(format) + "' ";
                                int countinplayc = 0;
                                using (SqlCommand cmdCountc = new SqlCommand(stmtc, connection))
                                {
                                    countinplayc = (int)cmdCountc.ExecuteScalar();
                                }
                                if (countinplayc > 0)
                                {
                                    if (sport_id == "1")
                                    {
                                        buj_socc = countinplayc;
                                    }
                                    else if (sport_id == "2")
                                    {
                                        buj_tenn = countinplayc;
                                    }
                                    else if (sport_id == "4")
                                    {
                                        buj_cric = countinplayc;
                                    }
                                    else
                                    {
                                        SPID = sport_id;
                                    }
                                    AllsportMLiveList.Add(item: new ListAllsportMLive
                                    {
                                        SportsName = sport_name,
                                        SportsId = sport_id,
                                        countinplayc = countinplayc
                                    });
                                }
                            }
                        }
                        sportid = SPID;
                        if (buj_tenn > 0)
                        {
                            sportid = "2";
                        }
                        if (buj_socc > 0)
                        {
                            sportid = "1";
                        }
                        if (buj_cric > 0)
                        {
                            sportid = "4";
                        }
                        using (var command = new SqlCommand("SELECT Top (39) [betfair_id] FROM matches where sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND sport_id = '" + sportid + "' AND betfair_id != '0' AND teama != 'nikhil' AND status='OPEN' AND match_time < '" + time.ToString(format) + "' order by match_time ", connection))
                        {
                            var reader = command.ExecuteReader();
                            while (reader.Read())
                            {

                                string betfair_code = (string)reader["betfair_id"];
                                if (betfair_code != "0")
                                {
                                    Minplay_ids += betfair_code + ",";
                                }
                                countinplay++;
                            }
                        }

                        connection.Close();
                        ViewBag.Minplay_ids = Minplay_ids;
                    }

                    ViewBag.SportsId = sportid;
                    ViewBag.SportName = FunctionDataController.GetSportsNameById(sportid);
                }
            }
            catch (Exception ex)
            {

            }
            return View(AllsportMLiveList);
        }

        public ActionResult MMarkets(EventIdSend eventIdSend, string sportid, string leagueid)
        {
            string Event_Code_Id = sportid;
            string Event_Type_Id = "";
            string EventMatch_timeDB = "";
            string teama = "";
            string teamb = "";
            string Event_Betfair_Id = "";
            string x_type = "";
            string x_code = "0";
            string tv_ch = "0";
            string video_l = "0";
            Double max_limit_mo_bm = 0;
            Double max_limit_mo = 0;
            Double max_limit_sess = 0;
            string book_id = "0";
            int is_fancy = 0;
            try
            {
                using (var connection = new SqlConnection(_connString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT [is_fancy],[book_id],[max_limit_mo_bm],[max_limit_mo],[max_limit_sess],[tv_ch] ,[video_l] ,[x_code] ,[x_type] , [teama] ,[teamb] , [match_time] , [betfair_id], [sport_id] FROM matches where event_code='" + Event_Code_Id + "' ", connection))
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            teama = (string)reader["teama"];
                            teamb = (string)reader["teamb"];
                            x_code = (string)reader["x_code"];
                            x_type = (string)reader["x_type"];
                            tv_ch = (string)reader["tv_ch"];
                            video_l = (string)reader["video_l"];
                            book_id = (string)reader["book_id"];
                            max_limit_mo_bm = (Double)reader["max_limit_mo_bm"];
                            max_limit_mo = (Double)reader["max_limit_mo"];
                            max_limit_sess = (Double)reader["max_limit_sess"];
                            DateTime match_time = (DateTime)reader["match_time"];
                            EventMatch_timeDB = match_time.ToString("dd/MM/yyyy HH:mm");
                            long unixTime = ((DateTimeOffset)match_time).ToUnixTimeSeconds();
                            long Time_Stamp = ((DateTimeOffset)CurrentTime).ToUnixTimeSeconds();
                            long diff = unixTime - Time_Stamp;
                            ViewBag.Time_Stamp = diff;
                            Event_Betfair_Id = (string)reader["betfair_id"];
                            Event_Type_Id = (string)reader["sport_id"];
                            is_fancy = (int)reader["is_fancy"];
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {

            }
            ViewBag.tv_ch = tv_ch;
            ViewBag.x_code = x_code;
            ViewBag.video_l = video_l;
            ViewBag.x_type = x_type;
            ViewBag.EventCode = Event_Code_Id;
            ViewBag.BetfairId = Event_Betfair_Id;
            ViewBag.teama = teama;
            ViewBag.teamb = teamb;
            ViewBag.Event_TimeDB = EventMatch_timeDB;
            ViewBag.Event_Type_Id = Event_Type_Id;
            ViewBag.max_limit_mo_bm = max_limit_mo_bm;
            ViewBag.max_limit_mo = max_limit_mo;
            ViewBag.max_limit_sess = max_limit_sess;
            ViewBag.book_id = book_id;
            ViewBag.is_fancy = is_fancy;
            return View();
        }

        public ActionResult MGully(EventIdSend eventIdSend, string sportid, string leagueid)
        {
            string dl_block_sports_string = FunctionDataController.getblocksport();
            string dl_block_leagues_string = FunctionDataController.getblockleague();
            string dl_block_events_string = FunctionDataController.getblockevent();
            string Event_Code_Id = sportid;
            string Event_Type_Id = "";
            string EventMatch_timeDB = "";
            string teama = "";
            string teamb = "";
            string Event_Betfair_Id = "";
            string x_type = "";
            string x_code = "0";
            string tv_ch = "0";
            string video_l = "0";
            Double max_limit_mo = 0;
            Double max_limit_sess = 0;
            int is_fancy = 0;
            try
            {
                using (var connection = new SqlConnection(_connString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT [is_fancy],[max_limit_mo],[max_limit_sess],[video_l] ,[tv_ch] ,[x_code] ,[x_type] , [teama] ,[teamb] , [match_time] , [betfair_id], [sport_id] FROM matches where sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND event_code='" + Event_Code_Id + "' ", connection))
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            teama = (string)reader["teama"];
                            teamb = (string)reader["teamb"];
                            x_code = (string)reader["x_code"];
                            x_type = (string)reader["x_type"];
                            tv_ch = (string)reader["tv_ch"];
                            video_l = (string)reader["video_l"];
                            max_limit_mo = (Double)reader["max_limit_mo"];
                            max_limit_sess = (Double)reader["max_limit_sess"];
                            DateTime match_time = (DateTime)reader["match_time"];
                            EventMatch_timeDB = match_time.ToString("dd/MM/yyyy HH:mm");
                            long unixTime = ((DateTimeOffset)match_time).ToUnixTimeSeconds();
                            long Time_Stamp = ((DateTimeOffset)CurrentTime).ToUnixTimeSeconds();
                            long diff = unixTime - Time_Stamp;
                            ViewBag.Time_Stamp = diff;
                            Event_Betfair_Id = (string)reader["betfair_id"];
                            Event_Type_Id = (string)reader["sport_id"];
                            is_fancy = (int)reader["is_fancy"];
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {

            }
            ViewBag.tv_ch = tv_ch;
            ViewBag.x_code = x_code;
            ViewBag.x_type = x_type;
            ViewBag.video_l = video_l;
            ViewBag.EventCode = Event_Code_Id;
            ViewBag.BetfairId = Event_Betfair_Id;
            ViewBag.teama = teama;
            ViewBag.teamb = teamb;
            ViewBag.Event_TimeDB = EventMatch_timeDB;
            ViewBag.Event_Type_Id = Event_Type_Id;
            ViewBag.max_limit_mo = max_limit_mo;
            ViewBag.max_limit_sess = max_limit_sess;
            ViewBag.is_fancy = is_fancy;
            return View();
        }

        public ActionResult MMarketx(EventIdSend eventIdSend, string sportid, string leagueid)
        {
            string dl_block_sports_string = FunctionDataController.getblocksport();
            string dl_block_leagues_string = FunctionDataController.getblockleague();
            string dl_block_events_string = FunctionDataController.getblockevent();
            string Event_Code_Id = sportid;
            string Event_Type_Id = "";
            string EventMatch_timeDB = "";
            string teama = "";
            string teamb = "";
            string Event_Betfair_Id = "";
            string x_type = "";
            string x_code = "0";
            string tv_ch = "0";
            string video_l = "0";
            Double max_limit_mo = 0;
            Double max_limit_sess = 0;
            int is_fancy = 0;
            try
            {
                using (var connection = new SqlConnection(_connString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT [is_fancy],[max_limit_mo],[max_limit_sess],[tv_ch] ,[video_l] ,[x_code] ,[x_type] , [teama] ,[teamb] , [match_time] , [betfair_id], [sport_id] FROM matches where sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND event_code='" + Event_Code_Id + "' ", connection))
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            teama = (string)reader["teama"];
                            teamb = (string)reader["teamb"];
                            x_code = (string)reader["x_code"];
                            x_type = (string)reader["x_type"];
                            tv_ch = (string)reader["tv_ch"];
                            video_l = (string)reader["video_l"];
                            max_limit_mo = (Double)reader["max_limit_mo"];
                            max_limit_sess = (Double)reader["max_limit_sess"];
                            DateTime match_time = (DateTime)reader["match_time"];
                            EventMatch_timeDB = match_time.ToString("dd/MM/yyyy HH:mm");
                            long unixTime = ((DateTimeOffset)match_time).ToUnixTimeSeconds();
                            long Time_Stamp = ((DateTimeOffset)CurrentTime).ToUnixTimeSeconds();
                            long diff = unixTime - Time_Stamp;
                            ViewBag.Time_Stamp = diff;
                            Event_Betfair_Id = (string)reader["betfair_id"];
                            Event_Type_Id = (string)reader["sport_id"];
                            is_fancy = (int)reader["is_fancy"];
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {

            }
            ViewBag.tv_ch = tv_ch;
            ViewBag.video_l = video_l;
            ViewBag.x_code = x_code;
            ViewBag.x_type = x_type;
            ViewBag.EventCode = Event_Code_Id;
            ViewBag.BetfairId = Event_Betfair_Id;
            ViewBag.teama = teama;
            ViewBag.teamb = teamb;
            ViewBag.Event_TimeDB = EventMatch_timeDB;
            ViewBag.Event_Type_Id = Event_Type_Id;
            ViewBag.max_limit_mo = max_limit_mo;
            ViewBag.max_limit_sess = max_limit_sess;
            ViewBag.is_fancy = is_fancy;
            return View();
        }

        public ActionResult Binary(EventIdSend eventIdSend, string sportid, string leagueid)
        {
            CULL();
            string dl_block_sports_string = FunctionDataController.getblocksport();
            string dl_block_leagues_string = FunctionDataController.getblockleague();
            string dl_block_events_string = FunctionDataController.getblockevent();
            string Event_Code_Id = sportid;
            string Event_Type_Id = "";
            string EventMatch_timeDB = "";
            string teama = "";
            string teamb = "";
            string Event_Betfair_Id = "";
            string x_type = "";
            string x_code = "0";
            string tv_ch = "0";
            string video_l = "0";
            Double max_limit_mo = 0;
            Double max_limit_sess = 0;
            try
            {
                using (var connection = new SqlConnection(_connString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT [max_limit_mo],[max_limit_sess],[video_l] ,[tv_ch] ,[x_code] ,[x_type] , [teama] ,[teamb] , [match_time] , [betfair_id], [sport_id] FROM matches where sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND event_code='" + Event_Code_Id + "' ", connection))
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            teama = (string)reader["teama"];
                            teamb = (string)reader["teamb"];
                            x_code = (string)reader["x_code"];
                            x_type = (string)reader["x_type"];
                            tv_ch = (string)reader["tv_ch"];
                            video_l = (string)reader["video_l"];
                            max_limit_mo = (Double)reader["max_limit_mo"];
                            max_limit_sess = (Double)reader["max_limit_sess"];
                            DateTime match_time = (DateTime)reader["match_time"];
                            EventMatch_timeDB = match_time.ToString("dd/MM/yyyy HH:mm");
                            long unixTime = ((DateTimeOffset)match_time).ToUnixTimeSeconds();
                            long Time_Stamp = ((DateTimeOffset)CurrentTime).ToUnixTimeSeconds();
                            long diff = unixTime - Time_Stamp;
                            ViewBag.Time_Stamp = diff;
                            Event_Betfair_Id = (string)reader["betfair_id"];
                            Event_Type_Id = (string)reader["sport_id"];
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {

            }
            ViewBag.tv_ch = tv_ch;
            ViewBag.x_code = x_code;
            ViewBag.x_type = x_type;
            ViewBag.video_l = video_l;
            ViewBag.EventCode = Event_Code_Id;
            ViewBag.BetfairId = Event_Betfair_Id;
            ViewBag.teama = teama;
            ViewBag.teamb = teamb;
            ViewBag.Event_TimeDB = EventMatch_timeDB;
            ViewBag.Event_Type_Id = Event_Type_Id;
            ViewBag.max_limit_mo = max_limit_mo;
            ViewBag.max_limit_sess = max_limit_sess;
            return View();
        }

        public ActionResult Market(string sportid, string leagueid)
        {
            if (check_hash_key != null && check_hash_key != "")
            {
                CULL();
            }
            string dl_block_sports_string = FunctionDataController.getblocksport();
            string dl_block_leagues_string = FunctionDataController.getblockleague();
            string dl_block_events_string = FunctionDataController.getblockevent();
            string Event_Code_Id = sportid;
            string Event_Type_Id = "";
            string EventMatch_timeDB = "";
            string teama = "";
            string teamb = "";
            string Event_Betfair_Id = "";
            string x_type = "";
            string x_code = "0";
            Double max_limit_mo = 0;
            Double max_limit_sess = 0;

            var AllSportsAddMe = new List<EventIdSend>();
            var AllSportsAddM = new List<NavbarListAllSports>();
            try
            {
                using (var connection = new SqlConnection(_connString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT [max_limit_mo],[max_limit_sess],[x_code] ,[x_type] , [teama] ,[teamb] , [match_time] , [betfair_id], [sport_id] FROM matches where sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND event_code='" + Event_Code_Id + "' ", connection))
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            teama = (string)reader["teama"];
                            teamb = (string)reader["teamb"];
                            x_code = (string)reader["x_code"];
                            x_type = (string)reader["x_type"];
                            max_limit_mo = (Double)reader["max_limit_mo"];
                            max_limit_sess = (Double)reader["max_limit_sess"];
                            DateTime match_time = (DateTime)reader["match_time"];
                            EventMatch_timeDB = match_time.ToString("dd/MM/yyyy HH:mm");
                            long unixTime = ((DateTimeOffset)match_time).ToUnixTimeSeconds();
                            long Time_Stamp = ((DateTimeOffset)CurrentTime).ToUnixTimeSeconds();
                            long diff = unixTime - Time_Stamp;
                            ViewBag.Time_Stamp = diff;
                            Event_Betfair_Id = (string)reader["betfair_id"];
                            Event_Type_Id = (string)reader["sport_id"];
                        }
                    }
                    connection.Close();
                }
                ViewBag.x_code = x_code;
                ViewBag.x_type = x_type;
                ViewBag.EventCode = Event_Code_Id;
                ViewBag.BetfairId = Event_Betfair_Id;
                ViewBag.teama = teama;
                ViewBag.teamb = teamb;
                ViewBag.Event_TimeDB = EventMatch_timeDB;
                ViewBag.Event_Type_Id = Event_Type_Id;
                ViewBag.max_limit_mo = max_limit_mo;
                ViewBag.max_limit_sess = max_limit_sess;

                AllSportsAddMe.Add(item: new EventIdSend
                {
                    mk_code = Event_Betfair_Id,
                    event_code = Event_Code_Id

                });
                string sport_id = "";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT market_name,sport_id,league_id,betfair_id,event_code FROM markets where status='activate' AND result_value='' AND event_code = '" + Event_Code_Id + "' "))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            SportsNameForShowAllSports = (string)reader["market_name"];
                            SportsidForShowAllSports = (string)reader["sport_id"];
                            string league_id = (string)reader["league_id"];
                            string betfair_id = (string)reader["betfair_id"];
                            string event_code = (string)reader["event_code"];
                            sport_id = (string)reader["sport_id"];
                            string Getdata = GetSportsNameForClick(sport_id);
                            string GetLeagueNameS = GetLeagueName(league_id);
                            string matchTt = GetMatchTitleN(event_code);
                            AllSportsAddM.Add(item: new NavbarListAllSports
                            {
                                SportsName = SportsNameForShowAllSports,
                                SportsId = SportsidForShowAllSports,
                                LaegueId = league_id,
                                EventId = event_code,
                                MarketId = betfair_id,
                                GetPreviousName = Getdata,
                                GetPreviousLName = GetLeagueNameS,
                                GetPreviousMTitle = matchTt

                            });
                        }
                        con.Close();
                    }


                }
            }
            catch (SqlException ex)
            {

            }
            catch (Exception ex)
            {

            }
            return View(AllSportsAddM);
        }

        public JsonResult GetOtherMarketsNameInerView()
        {
            List<OtherMarketNameModel> messages = new List<OtherMarketNameModel>();
            messages = GetOtherMarketsName();
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJavanScriptString(string data, string type)
        {
            List<InerPageOtherMarketsModel> messages = new List<InerPageOtherMarketsModel>();
            messages = InerPageOtherMarkets(data, type);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }
        public List<InerPageOtherMarketsModel> InerPageOtherMarkets(string jjg, string type)
        {
            var messages = new List<InerPageOtherMarketsModel>();
            string Event_Sport_Id = "";
            string EV_Code = type;
            var RunnerNameMAtchOddsList = new List<String>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://api_link.com");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("market/2/29278528/" + jjg).Result;
                    if (response != null)
                    {
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);

                        string totalMatched = "" + (int)responseJson[0].totalMatched;
                        var GetRunner = responseJson[0].runners;
                        string EvnetMarketId = responseJson[0].marketId;
                        string match_status = responseJson[0].status;

                        for (int i = 0; i < GetRunner.Count; i++)
                        {
                            string priceb11 = "";
                            string priceb12 = "";
                            string priceb13 = "";

                            string pricel11 = "";
                            string pricel12 = "";
                            string pricel13 = "";

                            string sizeb11 = "";
                            string sizeb12 = "";
                            string sizeb13 = "";

                            string sizel11 = "";
                            string sizel12 = "";
                            string sizel13 = "";
                            string status = GetRunner[i].status;
                            var availableToBack = GetRunner[i].ex.availableToBack;

                            if (availableToBack.Count > 0)
                            {
                                priceb11 = availableToBack[0].price;
                                sizeb11 = "" + (int)availableToBack[0].size;
                            }

                            if (availableToBack.Count > 1)
                            {
                                priceb12 = availableToBack[1].price;
                                sizeb12 = "" + (int)availableToBack[1].size;
                            }
                            if (availableToBack.Count > 2)
                            {
                                priceb13 = availableToBack[2].price;
                                sizeb13 = "" + (int)availableToBack[2].size;
                            }

                            var availableToLay = GetRunner[i].ex.availableToLay;
                            if (availableToLay.Count > 0)
                            {
                                pricel11 = availableToLay[0].price;
                                sizel11 = "" + (int)availableToLay[0].size;
                            }

                            if (availableToLay.Count > 1)
                            {
                                pricel12 = availableToLay[1].price;
                                sizel12 = "" + (int)availableToLay[1].size;
                            }
                            if (availableToLay.Count > 2)
                            {
                                pricel13 = availableToLay[2].price;
                                sizel13 = "" + (int)availableToLay[2].size;
                            }
                            messages.Add(item: new InerPageOtherMarketsModel
                            {
                                EventCode = EV_Code,
                                back1 = priceb11,
                                lay1 = pricel11,
                                back11 = priceb12,
                                lay11 = pricel12,
                                back22 = priceb13,
                                lay22 = pricel13,
                                Runnername = "",
                                BetfairId = EvnetMarketId,
                                back1size = sizeb11,
                                back2size = sizeb12,
                                back3size = sizeb13,
                                lay1size = sizel11,
                                lay2size = sizel12,
                                lay3size = sizel13,
                                totalMatched = totalMatched,
                                Event_Type_Id = Event_Sport_Id,
                                status = status,
                                match_status = match_status
                            }); ;
                        }
                    }
                    else
                    {
                        messages.Add(item: new InerPageOtherMarketsModel
                        {

                        });
                    }

                }
                catch (Exception fjk)
                {

                }
            }
            catch (TaskCanceledException ex)
            {

            }

            return messages;
            // return messages;
        }

        public List<InerPageOtherMarketsModel> DelayDataofmmarket(string jjg)
        {
            var messages = new List<InerPageOtherMarketsModel>();
            string Event_Sport_Id = "";
            string EV_Code = "";
            var RunnerNameMAtchOddsList = new List<String>();
            try
            {
                HttpClient client1 = new HttpClient();
                client1.BaseAddress = new Uri("https://ero.betfair.com/www/sports/exchange/readonly/v1/bymarket");
                client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client1.GetAsync("?_ak=nzIFcwyWhrlwYMrh&alt=json&marketIds=" + jjg + "&types=MARKET_STATE,RUNNER_EXCHANGE_PRICES_BEST,RUNNER_DESCRIPTION,RUNNER_STATE").Result;
                    if (response != null)
                    {
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        Event_Sport_Id = responseJson.eventTypes[0].eventTypeId;
                        EV_Code = responseJson.eventTypes[0].eventNodes[0].eventId;
                        string EvnetMarketId = responseJson.eventTypes[0].eventNodes[0].marketNodes[0].marketId;
                        string match_status = responseJson.eventTypes[0].eventNodes[0].marketNodes[0].state.status;
                        string totalMatched = "" + (int)responseJson.eventTypes[0].eventNodes[0].marketNodes[0].state.totalMatched;
                        var GetRunner = responseJson.eventTypes[0].eventNodes[0].marketNodes[0].runners;
                        for (int i = 0; i < GetRunner.Count; i++)
                        {
                            string priceb11 = "";
                            string priceb12 = "";
                            string priceb13 = "";

                            string pricel11 = "";
                            string pricel12 = "";
                            string pricel13 = "";

                            string sizeb11 = "";
                            string sizeb12 = "";
                            string sizeb13 = "";

                            string sizel11 = "";
                            string sizel12 = "";
                            string sizel13 = "";
                            string status = GetRunner[i].state.status;
                            var availableToBack = GetRunner[i].exchange.availableToBack;

                            if (availableToBack != null)
                            {
                                if (availableToBack.Count > 0)
                                {
                                    priceb11 = availableToBack[0].price;
                                    sizeb11 = "" + (int)availableToBack[0].size;
                                }
                                if (availableToBack.Count > 1)
                                {
                                    priceb12 = availableToBack[1].price;
                                    sizeb12 = "" + (int)availableToBack[1].size;
                                }
                                if (availableToBack.Count > 2)
                                {
                                    priceb13 = availableToBack[2].price;
                                    sizeb13 = "" + (int)availableToBack[2].size;
                                }
                            }

                            var availableToLay = GetRunner[i].exchange.availableToLay;
                            if (availableToLay != null)
                            {
                                if (availableToLay.Count > 0)
                                {
                                    pricel11 = availableToLay[0].price;
                                    sizel11 = "" + (int)availableToLay[0].size;
                                }
                                if (availableToLay.Count > 1)
                                {
                                    pricel12 = availableToLay[1].price;
                                    sizel12 = "" + (int)availableToLay[1].size;
                                }
                                if (availableToLay.Count > 2)
                                {
                                    pricel13 = availableToLay[2].price;
                                    sizel13 = "" + (int)availableToLay[2].size;
                                }
                            }

                            string runteamaa = GetRunner[i].description.runnerName;

                            messages.Add(item: new InerPageOtherMarketsModel
                            {
                                EventCode = EV_Code,
                                back1 = priceb11,
                                lay1 = pricel11,
                                back11 = priceb12,
                                lay11 = pricel12,
                                back22 = priceb13,
                                lay22 = pricel13,
                                Runnername = runteamaa,
                                BetfairId = EvnetMarketId,
                                back1size = sizeb11,
                                back2size = sizeb12,
                                back3size = sizeb13,
                                lay1size = sizel11,
                                lay2size = sizel12,
                                lay3size = sizel13,
                                totalMatched = totalMatched,
                                Event_Type_Id = Event_Sport_Id,
                                status = status,
                                match_status = match_status
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
            // return messages;
        }
        public ActionResult GetJavanScriptString66(string data, string type)
        {
            List<InerPageOtherMarketsModel> messages = new List<InerPageOtherMarketsModel>();
            messages = xmaryh(data, type);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<InerPageOtherMarketsModel> xmaryh(string xid, string type)
        {
            var messages = new List<InerPageOtherMarketsModel>();
            string Event_Sport_Id = "";
            string EV_Code = "";
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://1xbet.mobi/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync(type + "/GetGameZip?id=" + xid + "&lng=en&cfview=0&isSubGames=true&GroupEvents=true&countevents=600&grMode=2").Result;  // Blocking call! 
                    if (response != null)
                    {
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        var isresult = responseJson["Value"]["GE"];
                        if (responseJson["Value"]["I"] != null)
                        {
                            Event_Sport_Id = responseJson["Value"]["SI"];
                            EV_Code = responseJson["Value"]["I"];
                            string EvnetMarketId = responseJson["Value"]["I"];
                            string totalMatched = "100000";
                            string T1 = responseJson["Value"]["O1"];
                            string T2 = responseJson["Value"]["O2"];
                            foreach (var item in isresult)
                            {
                                string runteamaa = "";
                                int GE = item["G"];
                                if (GE == 1 || GE == 38)
                                {
                                    string priceb11 = "";
                                    string priceb12 = "";
                                    string priceb13 = "";

                                    string pricel11 = "";
                                    string pricel12 = "";
                                    string pricel13 = "";

                                    string sizeb11 = "";
                                    string sizeb12 = "";
                                    string sizeb13 = "";

                                    string sizel11 = "";
                                    string sizel12 = "";
                                    string sizel13 = "";
                                    var E1 = item["E"];
                                    int ELength1 = E1.Count;
                                    if (Event_Sport_Id == "99")
                                    {
                                        if (ELength1 == 3)
                                        {
                                            for (int iii = 0; iii < ELength1; iii++)
                                            {
                                                if (iii == 0 || iii == 2)
                                                {
                                                    var SC1 = responseJson["Value"]["SC"];
                                                    var SC = responseJson["Value"]["SC"]["S"];
                                                    String RunnerScore = "";
                                                    String RunnerScoreA = "";
                                                    String RunnerScoreB = "";
                                                    string gi = SC1["I"];
                                                    if (gi != null)
                                                    {
                                                    }
                                                    else
                                                    {
                                                        String Key1 = SC[0]["Key"];
                                                        String Key2 = SC[1]["Key"];
                                                        String Value1 = SC[0]["Value"];
                                                        String Value2 = SC[1]["Value"];
                                                        if (Key1 == "Team1Scores")
                                                        {
                                                            RunnerScoreA = Value1;
                                                        }
                                                        if (Key2 == "Team2Scores")
                                                        {
                                                            RunnerScoreB = Value2;
                                                        }
                                                    }
                                                    var E = E1[iii];
                                                    int ELength = E.Count;

                                                    if (iii == 0)
                                                    {
                                                        runteamaa = T1;
                                                    }
                                                    else if (iii == 2)
                                                    {
                                                        runteamaa = T2;
                                                    }
                                                    Boolean filedALock = false;
                                                    Boolean filedBLock = false;
                                                    if (E1[0][0]["B"] != null)
                                                    {
                                                        filedALock = E1[0][0]["B"];
                                                    }

                                                    if (E1[2][0]["B"] != null)
                                                    {
                                                        filedBLock = E1[2][0]["B"];
                                                    }
                                                    string status = "SUSPENDED";
                                                    if (filedBLock == true && filedALock == true)
                                                    {
                                                        priceb11 = "";
                                                        pricel11 = "";
                                                        status = "Suspended";
                                                    }
                                                    else
                                                    {
                                                        string checkAB = "";
                                                        Double filedArate = E1[0][0]["C"];
                                                        Double filedBrate = E1[2][0]["C"];

                                                        String Back1 = "";
                                                        String Back2 = "";
                                                        String LayOdds = "";
                                                        String LayOdds1 = "";
                                                        if (filedALock == true && filedBLock == false)
                                                        {
                                                            Back2 = filedBrate.ToString();
                                                            Double LayOdds11111 = System.Math.Round((Double.Parse(Back2) + 0.10), 2);
                                                            LayOdds1 = LayOdds11111.ToString();
                                                        }
                                                        else if (filedALock == false && filedBLock == true)
                                                        {
                                                            Back1 = filedArate.ToString();
                                                            Double LayOOODS = System.Math.Round((Double.Parse(Back1) + 0.10), 2);
                                                            LayOdds = LayOOODS.ToString();
                                                        }
                                                        else
                                                        {
                                                            if (filedArate < 2 && filedBrate < 2)
                                                            {
                                                                Back1 = filedArate.ToString();
                                                                Back2 = filedBrate.ToString();
                                                            }
                                                            else
                                                            {
                                                                if (filedArate < filedBrate)
                                                                {
                                                                    Back1 = filedArate.ToString();
                                                                    Double LayOOODS = System.Math.Round((Double.Parse(Back1) + 0.05), 2);
                                                                    LayOdds = LayOOODS.ToString();
                                                                    Double Back22222 = System.Math.Round(((1 / (LayOOODS - 1)) + 1), 2);
                                                                    Back2 = Back22222.ToString();
                                                                    Double LayOdds11111 = System.Math.Round((Double.Parse(Back2) + 0.30), 2);
                                                                    LayOdds1 = LayOdds11111.ToString();
                                                                }
                                                                else if (filedArate > filedBrate)
                                                                {
                                                                    Back2 = filedBrate.ToString();
                                                                    Double LayOdds11111 = System.Math.Round((Double.Parse(Back2) + 0.05), 2);
                                                                    LayOdds1 = LayOdds11111.ToString();
                                                                    Double Back11111 = System.Math.Round(((1 / (LayOdds11111 - 1)) + 1), 2);
                                                                    Back1 = Back11111.ToString();
                                                                    Double LayOOODS = System.Math.Round((Double.Parse(Back1) + 0.30), 2);
                                                                    LayOdds = LayOOODS.ToString();
                                                                }
                                                                else
                                                                {
                                                                    Back1 = filedArate.ToString();
                                                                    Back2 = filedBrate.ToString();
                                                                }
                                                            }
                                                        }

                                                        if (iii == 0)
                                                        {
                                                            status = "active";
                                                            priceb11 = Back1;
                                                            pricel11 = LayOdds;
                                                            RunnerScore = RunnerScoreA;
                                                        }
                                                        else if (iii == 2)
                                                        {
                                                            status = "active";
                                                            priceb11 = Back2;
                                                            pricel11 = LayOdds1;
                                                            RunnerScore = RunnerScoreB;
                                                        }
                                                    }
                                                    messages.Add(item: new InerPageOtherMarketsModel
                                                    {
                                                        EventCode = EV_Code,
                                                        back1 = priceb11,
                                                        lay1 = pricel11,
                                                        back11 = priceb12,
                                                        lay11 = pricel12,
                                                        back22 = priceb13,
                                                        lay22 = pricel13,
                                                        Runnername = runteamaa,
                                                        BetfairId = EvnetMarketId,
                                                        back1size = sizeb11,
                                                        back2size = sizeb12,
                                                        back3size = sizeb13,
                                                        lay1size = sizel11,
                                                        lay2size = sizel12,
                                                        lay3size = sizel13,
                                                        totalMatched = totalMatched,
                                                        Event_Type_Id = Event_Sport_Id,
                                                        RunnerScore = RunnerScore,
                                                        status = status
                                                    });
                                                }
                                            }
                                        }
                                    }
                                    else if (Event_Sport_Id == "10" || Event_Sport_Id == "21" || Event_Sport_Id == "3")
                                    {
                                        if (ELength1 == 2)
                                        {
                                            for (int iii = 0; iii < ELength1; iii++)
                                            {
                                                String RunnerScore = "";
                                                String RunnerScoreA = "";
                                                String RunnerScoreB = "";
                                                var E = E1[iii];
                                                int ELength = E.Count;

                                                if (iii == 0)
                                                {
                                                    runteamaa = T1;
                                                }
                                                else if (iii == 1)
                                                {
                                                    runteamaa = T2;
                                                }
                                                Boolean filedALock = false;
                                                Boolean filedBLock = false;
                                                if (E1[0][0]["B"] != null)
                                                {
                                                    filedALock = E1[0][0]["B"];
                                                }

                                                if (E1[1][0]["B"] != null)
                                                {
                                                    filedBLock = E1[1][0]["B"];
                                                }
                                                string status = "SUSPENDED";
                                                if (filedBLock == true && filedALock == true)
                                                {
                                                    priceb11 = "";
                                                    pricel11 = "";
                                                    status = "Suspended";
                                                }
                                                else
                                                {
                                                    string checkAB = "";
                                                    Double filedArate = E1[0][0]["C"];
                                                    Double filedBrate = E1[1][0]["C"];

                                                    String Back1 = "";
                                                    String Back2 = "";
                                                    String LayOdds = "";
                                                    String LayOdds1 = "";
                                                    if (filedALock == true && filedBLock == false)
                                                    {
                                                        Back1 = "";
                                                        LayOdds = "";
                                                        Back2 = filedBrate.ToString();
                                                        Double LayOdds11111 = System.Math.Round((Double.Parse(Back2) + 0.10), 2);
                                                        LayOdds1 = LayOdds11111.ToString();
                                                    }
                                                    else if (filedALock == false && filedBLock == true)
                                                    {
                                                        Back1 = filedArate.ToString();
                                                        Double LayOOODS = System.Math.Round((Double.Parse(Back1) + 0.10), 2);
                                                        LayOdds = LayOOODS.ToString();
                                                        Back2 = "";
                                                        LayOdds1 = "";
                                                    }
                                                    else
                                                    {
                                                        if (filedArate < 2 && filedBrate < 2)
                                                        {
                                                            Back1 = filedArate.ToString();
                                                            Back2 = filedBrate.ToString();
                                                        }
                                                        else
                                                        {
                                                            if (filedArate < filedBrate)
                                                            {
                                                                if (filedArate > 1.04 && filedArate < 1.10)
                                                                {
                                                                    filedArate = filedArate - 0.01;
                                                                }
                                                                else if (filedArate > 1.09 && filedArate < 1.15)
                                                                {
                                                                    filedArate = filedArate + 0.01;
                                                                }
                                                                else if (filedArate > 1.14 && filedArate < 1.20)
                                                                {
                                                                    filedArate = filedArate - 0.01;
                                                                }
                                                                else if (filedArate > 1.19 && filedArate < 1.30)
                                                                {
                                                                    filedArate = filedArate + 0.02;
                                                                }
                                                                else if (filedArate > 1.29 && filedArate < 1.50)
                                                                {
                                                                    filedArate = filedArate - 0.02;
                                                                }
                                                                else if (filedArate > 1.49 && filedArate < 1.60)
                                                                {
                                                                    filedArate = filedArate + 0.03;
                                                                }
                                                                else if (filedArate > 1.59 && filedArate < 1.80)
                                                                {
                                                                    filedArate = filedArate - 0.02;
                                                                }
                                                                else if (filedArate > 1.79 && filedArate < 1.90)
                                                                {
                                                                    filedArate = filedArate - 0.04;
                                                                }
                                                                else if (filedArate > 1.89 && filedArate < 1.97)
                                                                {
                                                                    filedArate = filedArate + 0.02;
                                                                }
                                                                Back1 = filedArate.ToString();
                                                                Double LayOOODS = System.Math.Round((Double.Parse(Back1) + 0.10), 2);
                                                                LayOdds = LayOOODS.ToString();
                                                            }
                                                            else if (filedArate > filedBrate)
                                                            {
                                                                if (filedBrate > 1.04 && filedBrate < 1.10)
                                                                {
                                                                    filedBrate = filedBrate - 0.01;
                                                                }
                                                                else if (filedBrate > 1.09 && filedBrate < 1.15)
                                                                {
                                                                    filedBrate = filedBrate + 0.01;
                                                                }
                                                                else if (filedBrate > 1.14 && filedBrate < 1.20)
                                                                {
                                                                    filedBrate = filedBrate - 0.01;
                                                                }
                                                                else if (filedBrate > 1.19 && filedBrate < 1.30)
                                                                {
                                                                    filedBrate = filedBrate + 0.02;
                                                                }
                                                                else if (filedBrate > 1.29 && filedBrate < 1.50)
                                                                {
                                                                    filedBrate = filedBrate - 0.02;
                                                                }
                                                                else if (filedBrate > 1.49 && filedBrate < 1.60)
                                                                {
                                                                    filedBrate = filedBrate + 0.03;
                                                                }
                                                                else if (filedBrate > 1.59 && filedBrate < 1.80)
                                                                {
                                                                    filedBrate = filedBrate - 0.02;
                                                                }
                                                                else if (filedBrate > 1.79 && filedBrate < 1.90)
                                                                {
                                                                    filedBrate = filedBrate - 0.04;
                                                                }
                                                                else if (filedBrate > 1.89 && filedBrate < 1.97)
                                                                {
                                                                    filedBrate = filedBrate + 0.02;
                                                                }
                                                                Back2 = filedBrate.ToString();
                                                                Double LayOdds11111 = System.Math.Round((Double.Parse(Back2) + 0.10), 2);
                                                                LayOdds1 = LayOdds11111.ToString();
                                                            }
                                                            else
                                                            {
                                                                Back1 = filedArate.ToString();
                                                                Back2 = filedBrate.ToString();
                                                            }
                                                        }
                                                    }

                                                    if (iii == 0)
                                                    {
                                                        status = "active";
                                                        priceb11 = Back1;
                                                        pricel11 = LayOdds;
                                                        RunnerScore = RunnerScoreA;
                                                    }
                                                    else if (iii == 1)
                                                    {
                                                        status = "active";
                                                        priceb11 = Back2;
                                                        pricel11 = LayOdds1;
                                                        RunnerScore = RunnerScoreB;
                                                    }
                                                }
                                                messages.Add(item: new InerPageOtherMarketsModel
                                                {
                                                    EventCode = EV_Code,
                                                    back1 = priceb11,
                                                    lay1 = pricel11,
                                                    back11 = priceb12,
                                                    lay11 = pricel12,
                                                    back22 = priceb13,
                                                    lay22 = pricel13,
                                                    Runnername = runteamaa,
                                                    BetfairId = EvnetMarketId,
                                                    back1size = sizeb11,
                                                    back2size = sizeb12,
                                                    back3size = sizeb13,
                                                    lay1size = sizel11,
                                                    lay2size = sizel12,
                                                    lay3size = sizel13,
                                                    totalMatched = totalMatched,
                                                    Event_Type_Id = Event_Sport_Id,
                                                    RunnerScore = RunnerScore,
                                                    status = status
                                                });
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (ELength1 == 2)
                                        {
                                            for (int iii = 0; iii < ELength1; iii++)
                                            {
                                                String RunnerScore = "";
                                                String RunnerScoreA = "";
                                                String RunnerScoreB = "";
                                                var E = E1[iii];
                                                int ELength = E.Count;

                                                if (iii == 0)
                                                {
                                                    runteamaa = T1;
                                                }
                                                else if (iii == 1)
                                                {
                                                    runteamaa = T2;
                                                }
                                                Boolean filedALock = false;
                                                Boolean filedBLock = false;
                                                if (E1[0][0]["B"] != null)
                                                {
                                                    filedALock = E1[0][0]["B"];
                                                }

                                                if (E1[1][0]["B"] != null)
                                                {
                                                    filedBLock = E1[1][0]["B"];
                                                }
                                                string status = "SUSPENDED";
                                                if (filedBLock == true && filedALock == true)
                                                {
                                                    priceb11 = "";
                                                    pricel11 = "";
                                                    status = "Suspended";
                                                }
                                                else
                                                {
                                                    string checkAB = "";
                                                    Double filedArate = E1[0][0]["C"];
                                                    Double filedBrate = E1[1][0]["C"];

                                                    String Back1 = "";
                                                    String Back2 = "";
                                                    String LayOdds = "";
                                                    String LayOdds1 = "";
                                                    if (filedALock == true && filedBLock == false)
                                                    {
                                                        Back1 = "";
                                                        LayOdds = "";
                                                        Back2 = filedBrate.ToString();
                                                        Double LayOdds11111 = System.Math.Round((Double.Parse(Back2) + 0.10), 2);
                                                        LayOdds1 = LayOdds11111.ToString();
                                                    }
                                                    else if (filedALock == false && filedBLock == true)
                                                    {
                                                        Back1 = filedArate.ToString();
                                                        Double LayOOODS = System.Math.Round((Double.Parse(Back1) + 0.10), 2);
                                                        LayOdds = LayOOODS.ToString();
                                                        Back2 = "";
                                                        LayOdds1 = "";
                                                    }
                                                    else
                                                    {
                                                        if (filedArate < 2 && filedBrate < 2)
                                                        {
                                                            Back1 = filedArate.ToString();
                                                            Back2 = filedBrate.ToString();
                                                        }
                                                        else
                                                        {
                                                            if (filedArate < filedBrate)
                                                            {
                                                                Back1 = filedArate.ToString();
                                                                Double LayOOODS = System.Math.Round((Double.Parse(Back1) + 0.05), 2);
                                                                LayOdds = LayOOODS.ToString();
                                                                Double Back22222 = System.Math.Round(((1 / (LayOOODS - 1)) + 1), 2);
                                                                Back2 = Back22222.ToString();
                                                                Double LayOdds11111 = System.Math.Round((Double.Parse(Back2) + 0.30), 2);
                                                                LayOdds1 = LayOdds11111.ToString();
                                                            }
                                                            else if (filedArate > filedBrate)
                                                            {
                                                                Back2 = filedBrate.ToString();
                                                                Double LayOdds11111 = System.Math.Round((Double.Parse(Back2) + 0.05), 2);
                                                                LayOdds1 = LayOdds11111.ToString();
                                                                Double Back11111 = System.Math.Round(((1 / (LayOdds11111 - 1)) + 1), 2);
                                                                Back1 = Back11111.ToString();
                                                                Double LayOOODS = System.Math.Round((Double.Parse(Back1) + 0.30), 2);
                                                                LayOdds = LayOOODS.ToString();
                                                            }
                                                            else
                                                            {
                                                                Back1 = filedArate.ToString();
                                                                Back2 = filedBrate.ToString();
                                                            }
                                                        }
                                                    }

                                                    if (iii == 0)
                                                    {
                                                        status = "active";
                                                        priceb11 = Back1;
                                                        pricel11 = LayOdds;
                                                        RunnerScore = RunnerScoreA;
                                                    }
                                                    else if (iii == 1)
                                                    {
                                                        status = "active";
                                                        priceb11 = Back2;
                                                        pricel11 = LayOdds1;
                                                        RunnerScore = RunnerScoreB;
                                                    }
                                                }
                                                messages.Add(item: new InerPageOtherMarketsModel
                                                {
                                                    EventCode = EV_Code,
                                                    back1 = priceb11,
                                                    lay1 = pricel11,
                                                    back11 = priceb12,
                                                    lay11 = pricel12,
                                                    back22 = priceb13,
                                                    lay22 = pricel13,
                                                    Runnername = runteamaa,
                                                    BetfairId = EvnetMarketId,
                                                    back1size = sizeb11,
                                                    back2size = sizeb12,
                                                    back3size = sizeb13,
                                                    lay1size = sizel11,
                                                    lay2size = sizel12,
                                                    lay3size = sizel13,
                                                    totalMatched = totalMatched,
                                                    Event_Type_Id = Event_Sport_Id,
                                                    RunnerScore = RunnerScore,
                                                    status = status
                                                });
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

            return messages;
            // return messages;
        }
        public ActionResult GetGullyData(string data)
        {
            List<InerPageOtherMarketsModel> messages = new List<InerPageOtherMarketsModel>();
            messages = GetGullyDatammarket(data);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<InerPageOtherMarketsModel> GetGullyDatammarket(string jjg)
        {
            var messages = new List<InerPageOtherMarketsModel>();
            string Event_Sport_Id = "";
            string EV_Code = "";
            try
            {
                HttpClient client1 = new HttpClient();
                client1.BaseAddress = new Uri("http://api_link.com/");
                client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client1.GetAsync("sky_mo.php?event_id=" + jjg).Result;
                    if (response != null)
                    {
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        Event_Sport_Id = responseJson.eventTypeId;
                        EV_Code = responseJson.eventId;
                        string EvnetMarketId = responseJson.marketId;
                        string status = responseJson.status;
                        string totalMatched = "1000000";
                        var GetRunner = responseJson.runner;
                        for (int i = 0; i < GetRunner.Count; i++)
                        {
                            string priceb11 = "";
                            string priceb12 = "";
                            string priceb13 = "";

                            string pricel11 = "";
                            string pricel12 = "";
                            string pricel13 = "";

                            string sizeb11 = "";
                            string sizeb12 = "";
                            string sizeb13 = "";

                            string sizel11 = "";
                            string sizel12 = "";
                            string sizel13 = "";
                            if (status == "OPEN")
                            {
                                priceb11 = GetRunner[i].rate1;
                                sizeb11 = "";
                                pricel11 = GetRunner[i].rate2;
                                sizel11 = "";
                            }

                            string runteamaa = GetRunner[i].runnerName;

                            messages.Add(item: new InerPageOtherMarketsModel
                            {
                                EventCode = EV_Code,
                                back1 = priceb11,
                                lay1 = pricel11,
                                back11 = priceb12,
                                lay11 = pricel12,
                                back22 = priceb13,
                                lay22 = pricel13,
                                Runnername = runteamaa,
                                BetfairId = EvnetMarketId,
                                back1size = sizeb11,
                                back2size = sizeb12,
                                back3size = sizeb13,
                                lay1size = sizel11,
                                lay2size = sizel12,
                                lay3size = sizel13,
                                totalMatched = totalMatched,
                                Event_Type_Id = Event_Sport_Id
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("dddd" + ex);
                }
            }
            catch (TaskCanceledException ex)
            {

            }

            return messages;
            // return messages;
        }

        public List<OtherMarketNameModel> GetOtherMarketsName()
        {
            var messages = new List<OtherMarketNameModel>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                string query = "SELECT market_name,betfair_id from markets WHERE event_code='" + GetEventCodeForInnerPage + "' AND betfair_id !='" + GetBetfairidForInnerPage + "'";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string GetOtherMarketsName = ds.Tables[0].Rows[i][0].ToString();
                        string GetOtherMarketsBetfair_Id = ds.Tables[0].Rows[i][1].ToString();

                        messages.Add(item: new OtherMarketNameModel
                        {
                            GetOtherMarketName = GetOtherMarketsName,
                            GetOtherMarketBFId = GetOtherMarketsBetfair_Id
                        });
                    }
                    con.Close();
                }
            }

            return messages;
        }

        public ActionResult Mindex(string sportid)
        {
            string dl_block_sports_string = FunctionDataController.getblocksport();
            string dl_block_leagues_string = FunctionDataController.getblockleague();
            string dl_block_events_string = FunctionDataController.getblockevent();
            string sidget = "";
            string CheckQuickLinksShow = "";
            if (sportid == null)
            {
                sportid = "4";
                return RedirectToAction("MMatchList/4", "exchange");
            }
            if (sportid != null)
            {
                sidget = sportid;
                CheckQuickLinksShow = "Yes";
                string SportNameAcordingto = FunctionDataController.GetSportsNameById(sportid);
                ViewBag.SportNameAcordingto = SportNameAcordingto;
                ViewBag.SportIId = sportid;
            }
            else
            {
                ViewBag.SportNameAcordingto = "";
                ViewBag.SportIId = "";
            }
            DateTime time = DateTime.Now;
            string format = "yyyy-MM-dd HH:mm:ss";
            var rec = new List<MLeagueName>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    string query = "SELECT league_name,league_id,sport_id FROM league WHERE sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND sport_id='" + sidget + "' AND EXISTS (SELECT event_code FROM matches  WHERE event_code Not IN(" + dl_block_events_string + ") AND sport_id='" + sidget + "' AND league.league_id = matches.league_id AND betfair_id!='0' AND teama!='nikhil' AND status = 'OPEN') ";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string SportsLeagueName = (string)reader["league_name"];
                            string SportsLeagueId = (string)reader["league_id"];
                            string SportsId = (string)reader["sport_id"];
                            AllSportsSideNavbar.Add(SportsLeagueName);

                            rec.Add(item: new MLeagueName
                            {
                                league_id = SportsLeagueId,
                                league_name = SportsLeagueName,
                                league_sports_id = SportsId,
                                checkshow = CheckQuickLinksShow

                            });
                            ViewBag.Message = rec;
                        }

                        string stmts = "SELECT COUNT(id) FROM matches where sport_id Not IN(" + dl_block_sports_string + ") AND league_id Not IN(" + dl_block_leagues_string + ") AND event_code Not IN(" + dl_block_events_string + ") AND sport_id!=7888 AND betfair_id != '0' AND teama != 'nikhil' AND status='OPEN' AND match_time < '" + time.ToString(format) + "' ";
                        int countinplay = 0;
                        using (SqlCommand cmdCounts = new SqlCommand(stmts, con))
                        {
                            countinplay = (int)cmdCounts.ExecuteScalar();
                        }
                        con.Close();
                        ViewBag.countinplay = countinplay;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View(rec);
        }

        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            con = new SqlConnection(constr);

        }

        private static HttpClient apiClient = new HttpClient();

        /*  [System.Web.Http.HttpPost]
          public async Task<IHttpActionResult> GetWebApiData(string Event_Code)
          {
              var response = await apiClient.GetAsync("http://api_link.com/sess_ap.php?event_code="+Event_Code);
              var resultContent = response.Content;
              var model = await resultContent.ReadAsAsync<dynamic>();
              return Json(model, JsonRequestBehavior.AllowGet);
          }*/
        public JsonResult SessionDataAView(string Event_Code)
        {
            var products = "";
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://Api_link.com");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("/market?Event_Code=" + Event_Code ).Result;
                products = response.Content.ReadAsStringAsync().Result;

            }
            catch (TaskCanceledException)
            {

            }
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        public string GullyScore(string Event_Code)
        {
            string products = "";
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("sky_score.php?event_id=" + Event_Code).Result;  // Blocking call! 

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
        public JsonResult GullySession(string Event_Code)
        {
            var products = "";
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("sky_sess.php?event_id=" + Event_Code).Result;  // Blocking call! 

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
        public JsonResult BinarySession(string event_code)
        {
            var products = "";
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("binary_sess.php?id=" + event_code).Result;  // Blocking call! 

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
        public JsonResult SessionDataAView1(string Event_Code)
        {
            var products = "";
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://api_link.com");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("market/2/29278528/" + Event_Code).Result;  // Blocking call! 

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

        public JsonResult OtherMarketData(string Event_Code_other, string Event_Type_other)
        {

            List<OtherMarketList> messages = new List<OtherMarketList>();
            messages = GetAllMessagesNew11(Event_Code_other, Event_Type_other);
            return Json(messages, JsonRequestBehavior.AllowGet);

        }

        public List<OtherMarketList> GetAllMessagesNew11(string Event_Code_other, string Event_Type_other)
        {
            var messages = new List<OtherMarketList>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("allmarkets.php?event_type_id=" + Event_Type_other + "&event_code=" + Event_Code_other).Result;

                    var products = response.Content.ReadAsStringAsync().Result;
                    dynamic responseJson = JsonConvert.DeserializeObject(products);
                    for (int i = 0; i < responseJson.Count; i++)
                    {

                        string OtherName = responseJson[i].marketName;
                        string OtherBf_id = responseJson[i].marketId;
                        if (OtherName == "Over/Under 1.5 Goals" || OtherName == "Over/Under 2.5 Goals" || OtherName == "Over/Under 3.5 Goals" || OtherName == "To Win the Toss")
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

        public ActionResult GetWinMAtchDa(string data)
        {
            List<SendWMEData> messages = new List<SendWMEData>();
            messages = SendWMEDB(data);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<SendWMEData> SendWMEDB(string jjg)
        {
            var messages = new List<SendWMEData>();
            try
            {
                string user_ids = (string)System.Web.HttpContext.Current.Session["UIDS"];
                int user_id = 0;
                if (user_ids != "" && user_ids != null)
                {
                    user_id = Int32.Parse(user_ids);
                }

                using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con1.Open();
                    string stmt1 = "select amount from runner_cal where user_id='" + user_id + "' AND market_id='" + jjg + "' ";

                    using (SqlCommand cmdCount1 = new SqlCommand(stmt1, con1))
                    {
                        cmdCount1.ExecuteScalar();
                        var reader1 = cmdCount1.ExecuteReader();
                        while (reader1.Read())
                        {
                            Double getamt = (Double)reader1["amount"];
                            messages.Add(item: new SendWMEData
                            {
                                MainD = getamt
                            });
                        }
                    }
                    con1.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return messages;
        }
        public ActionResult GetWmebook(string data)
        {
            List<SendWMEData> messages = new List<SendWMEData>();
            messages = GetWmebook1(data);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<SendWMEData> GetWmebook1(string jjg)
        {
            var messages = new List<SendWMEData>();
            try
            {
                string user_ids = (string)System.Web.HttpContext.Current.Session["UIDS"];
                int user_id = 0;
                if (user_ids != "" && user_ids != null)
                {
                    user_id = Int32.Parse(user_ids);
                }

                using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con1.Open();

                    Double TotalamountA = 0;
                    Double TotalamountB = 0;
                    Double TotalamountC = 0;
                    SqlCommand sqlLost = new SqlCommand("SELECT stakes,total_value,runner_posi,event_id FROM live_bet WHERE betfair_id='" + jjg + "' AND user_id='" + user_id + "' ", con1);
                    var dataLost = sqlLost.ExecuteReader();
                    if (dataLost.HasRows)
                    {
                        while (dataLost.Read())
                        {
                            Double stakes = (Double)dataLost["stakes"];
                            string event_id = (string)dataLost["event_id"];
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
                            else if (runner_pos == 3 || runner_pos == 41)
                            {
                                TotalamountA = TotalamountA - stakes;
                                TotalamountB = TotalamountB - stakes;
                                TotalamountC = TotalamountC + total_value;
                            }
                        }
                    }
                    messages.Add(item: new SendWMEData
                    {
                        team_a_balance = TotalamountA,
                        team_b_balance = TotalamountB,
                        team_c_balance = TotalamountC,
                        MainD = 0
                    });
                    con1.Close();
                }
            }
            catch (Exception ex)
            {

            }

            return messages;
        }

        public ActionResult Login()
        {
            return View();
        }


        public ActionResult LoginRanjya()
        {
            return View();
        }


        public ActionResult ApkFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data/";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + "Probet247.apk");
            string fileName = "Probet247.apk";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult ApkFileA()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data/";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + "Probet247A.apk");
            string fileName = "Probet247A.apk";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }


        public string GetSportsNameForClick(string sportid)
        {
            string SportsNameForShowAllSportsf = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {

                using (var cmd = new SqlCommand("SELECT sport_name FROM sports Where sport_id='" + sportid + "'"))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SportsNameForShowAllSportsf = (string)reader["sport_name"];

                    }

                    con.Close();
                }
            }

            return SportsNameForShowAllSportsf;
        }

        public string GetLeagueName(string LeagueI)
        {
            string LeagueNamestr = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT league_name FROM league WHERE league_id = '" + LeagueI + "'"))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        LeagueNamestr = (string)reader["league_name"];

                    }
                    con.Close();
                }
            }
            return LeagueNamestr;
        }

        public string GetMatchTitleN(string EveCodeM)
        {
            string MAtchTG = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT match_title FROM matches where event_code= '" + EveCodeM + "' "))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        MAtchTG = (string)reader["match_title"];

                    }

                    con.Close();
                }
            }
            return MAtchTG;
        }

        public string GetBetfairIdDB(string Eve_Code)
        {

            string MAtchTG = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT betfair_id FROM matches where event_code= '" + Eve_Code + "' "))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        MAtchTG = (string)reader["betfair_id"];

                    }

                    con.Close();
                }
            }

            return MAtchTG;
        }

        public string GetMatchTimeDB(string Eve_Code)
        {

            string MAtchTG = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT match_time FROM matches where event_code= '" + Eve_Code + "' "))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DateTime MAtchTG1 = (DateTime)reader["match_time"];
                        MAtchTG = MAtchTG1.ToString("yyyy/MM/dd HH:mm:ss");

                    }

                    con.Close();
                }
            }

            return MAtchTG;
        }

        public string GetSportIdDB(string Eve_Code)
        {

            string MAtchTG = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT sport_id FROM matches where event_code= '" + Eve_Code + "' "))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        MAtchTG = (string)reader["sport_id"];

                    }

                    con.Close();
                }
            }

            return MAtchTG;
        }

        public string GetLeagueNameDB(string Eve_Code)
        {
            string MAtchTG = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT league_name from league where league_id='" + Eve_Code + "'"))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        MAtchTG = (string)reader["league_name"];
                    }
                    con.Close();
                }
            }
            return MAtchTG;
        }

        public ActionResult myAccount()
        {
            if (check_hash_key != null && check_hash_key != "")
            {
                CULL();
            }
            return View();
        }
        public ActionResult BalanceOverview()
        {

            return View();
        }
        public ActionResult accountCashStatement()
        {
            var DL_UserBetList = new List<ClientaccountCashStatement>();
            try
            {
                string login_user_idn = (string)System.Web.HttpContext.Current.Session["UIDS"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT created,debit,credit,balance,remark,description FROM user_account_statements WHERE user_id='" + login_user_idn + "' AND acc_stat_type='dw_coins' order by created desc "))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            DateTime Event_time = (DateTime)dr["created"];
                            Double debit = (Double)dr["debit"];
                            Double credit = (Double)dr["credit"];
                            Double balance = (Double)dr["balance"];
                            string remark = (string)dr["remark"];
                            string desc = (string)dr["description"];
                            DL_UserBetList.Add(item: new ClientaccountCashStatement
                            {
                                DTime = Event_time,
                                Remark = desc,
                                Balance = balance,
                                Deposit = credit,
                                Withdraw = debit
                            });
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View(DL_UserBetList);
        }

        public ActionResult LogOut()
        {
            //  HttpContext.Session.Remove();//This will remove all keys from session variable. For example, if your session contains id, name, phone number, email etc.
            //  HttpContext.Session.RemoveAll();//This will remove all session from application
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "exchange");
        }

        public ActionResult Ucurrent_bets()
        {
            return View();
        }
        public ActionResult BetHistory()
        {
            string pageNumber = "1";
            int max_page = 20;
            int min_page = 0;
            var DL_UserBetList = new List<MatchedClientBetList>();
            try
            {
                if (Request.QueryString["betStatus"] != null && Request.QueryString["startDate"] != null && Request.QueryString["endDate"] != null)
                {
                    string betStatus = Request.QueryString["betStatus"];
                    string startDate = Request.QueryString["startDate"];
                    string endDate = Request.QueryString["endDate"];
                    if (Request.QueryString["pageNumber"] != null)
                    {
                        pageNumber = Request.QueryString["pageNumber"];
                    }
                    if (startDate != "" && endDate != "" && pageNumber != "" && betStatus != "")
                    {
                        int pageno = Int32.Parse(pageNumber);
                        //DateTime startdate = DateTime.Parse(startDate);
                        //DateTime enddate = DateTime.Parse(endDate);
                        max_page = 20 * pageno;
                        min_page = max_page - 20;
                        string login_user_id = (string)System.Web.HttpContext.Current.Session["UIDS"];
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            using (var cmd = new SqlCommand("SELECT place_time,event_id,id,betfair_id,field,field_pos,status,rate,stakes,total_value,odds_type FROM(SELECT place_time,event_id,id,betfair_id,field,field_pos,status,rate,stakes,total_value,odds_type , ROW_NUMBER() over(order by id desc) as row FROM live_bet WHERE (place_time BETWEEN '" + startDate + "' AND '" + endDate + "') AND  status!='' AND user_id='" + login_user_id + "') a where a.row > '" + min_page + "'  and a.row <= '" + max_page + "' ", con))
                            {
                                con.Open();
                                var dr = cmd.ExecuteReader();
                                while (dr.Read())
                                {
                                    DateTime time = (DateTime)dr["place_time"];
                                    string format = "yyyy-MM-dd HH:mm:ss";
                                    DateTime Event_time = (DateTime)dr["place_time"];
                                    string Event_code_Get = (string)dr["event_id"];
                                    int betid = (int)dr["id"];
                                    string betfair_id_Get = (string)dr["betfair_id"];
                                    string field = (string)dr["field"];
                                    string field_pos = (string)dr["field_pos"];
                                    string status = (string)dr["status"];
                                    Double rate = (Double)dr["rate"];
                                    Double stakes = (Double)dr["stakes"];
                                    stakes = System.Math.Round(stakes, 2);
                                    Double profit_loss = (Double)dr["total_value"];
                                    profit_loss = System.Math.Round(profit_loss, 2);
                                    string GetEventTName = FunctionDataController.GetEventTitleName(Event_code_Get);
                                    string GetMarketName = FunctionDataController.GetMArketName(Event_code_Get, betfair_id_Get);
                                    string GetSportsNameS = FunctionDataController.GetSportsName(Event_code_Get);
                                    string BetType1 = (string)dr["field_pos"];
                                    string BetName = (string)dr["odds_type"];
                                    string BetType = "";
                                    if (BetName == "sess")
                                    {
                                        if (BetType1 == "a")
                                        {
                                            BetType = "lay";
                                        }
                                        else if (BetType1 == "b")
                                        {
                                            BetType = "back";
                                        }
                                    }
                                    else
                                    {
                                        BetType = BetType1;
                                    }
                                    DL_UserBetList.Add(item: new MatchedClientBetList
                                    {
                                        EventTime = time.ToString(format),
                                        Description = GetSportsNameS,
                                        Field = field,
                                        Rate = rate,
                                        OddsReq = rate,
                                        Stakes = stakes,
                                        PL = profit_loss,
                                        Type = BetType,
                                        Status = status,
                                        GetEventTName = GetEventTName,
                                        GetMarketName = GetMarketName,
                                        betid = betid,
                                        Field_pos = field_pos
                                    });
                                }
                                con.Close();
                            }
                            string stmts = "SELECT COUNT(id) FROM live_bet WHERE (place_time BETWEEN '" + startDate + "' AND '" + endDate + "') AND  status!='' AND user_id='" + login_user_id + "'";
                            int countbeth = 0;
                            using (SqlCommand cmdCounts = new SqlCommand(stmts, con))
                            {
                                con.Open();
                                countbeth = (int)cmdCounts.ExecuteScalar();
                                con.Close();
                            }
                            ViewBag.countbeth = countbeth;
                            ViewBag.startDate = startDate;
                            ViewBag.endDate = endDate;
                        }
                    }
                }
                else
                {
                    string login_user_id = (string)System.Web.HttpContext.Current.Session["UIDS"];
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        using (var cmd = new SqlCommand("SELECT place_time,event_id,id,betfair_id,field,field_pos,status,rate,stakes,total_value,odds_type FROM(SELECT place_time,event_id,id,betfair_id,field,field_pos,status,rate,stakes,total_value,odds_type , ROW_NUMBER() over(order by id desc) as row FROM live_bet WHERE place_time >'" + todaydate.ToString(today) + "'  AND  status!='' AND user_id='" + login_user_id + "') a where a.row > '" + min_page + "'  and a.row <= '" + max_page + "' ", con))
                        {
                            con.Open();
                            var dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                DateTime time = (DateTime)dr["place_time"];
                                string format = "yyyy-MM-dd HH:mm:ss";
                                DateTime Event_time = (DateTime)dr["place_time"];
                                string Event_code_Get = (string)dr["event_id"];
                                int betid = (int)dr["id"];
                                string betfair_id_Get = (string)dr["betfair_id"];
                                string field = (string)dr["field"];
                                string field_pos = (string)dr["field_pos"];
                                string status = (string)dr["status"];
                                Double rate = (Double)dr["rate"];
                                Double stakes = (Double)dr["stakes"];
                                stakes = System.Math.Round(stakes, 2);
                                Double profit_loss = (Double)dr["total_value"];
                                profit_loss = System.Math.Round(profit_loss, 2);
                                string GetEventTName = FunctionDataController.GetEventTitleName(Event_code_Get);
                                string GetMarketName = FunctionDataController.GetMArketName(Event_code_Get, betfair_id_Get);
                                string GetSportsNameS = FunctionDataController.GetSportsName(Event_code_Get);
                                string BetType1 = (string)dr["field_pos"];
                                string BetName = (string)dr["odds_type"];
                                string BetType = "";
                                if (BetName == "sess")
                                {
                                    if (BetType1 == "a")
                                    {
                                        BetType = "lay";
                                    }
                                    else if (BetType1 == "b")
                                    {
                                        BetType = "back";
                                    }
                                }
                                else
                                {
                                    BetType = BetType1;
                                }
                                DL_UserBetList.Add(item: new MatchedClientBetList
                                {
                                    EventTime = time.ToString(format),
                                    Description = GetSportsNameS,
                                    Field = field,
                                    Rate = rate,
                                    OddsReq = rate,
                                    Stakes = stakes,
                                    PL = profit_loss,
                                    Type = BetType,
                                    Status = status,
                                    GetEventTName = GetEventTName,
                                    GetMarketName = GetMarketName,
                                    betid = betid,
                                    Field_pos = field_pos
                                });
                            }
                            con.Close();
                        }
                        string stmts = "SELECT COUNT(id) FROM live_bet WHERE place_time >'" + todaydate.ToString(today) + "' AND  status!='' AND user_id='" + login_user_id + "'";
                        int countbeth = 0;
                        using (SqlCommand cmdCounts = new SqlCommand(stmts, con))
                        {
                            con.Open();
                            countbeth = (int)cmdCounts.ExecuteScalar();
                            con.Close();
                        }
                        ViewBag.countbeth = countbeth;
                        ViewBag.startDate = "0";
                        ViewBag.endDate = "0";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            ViewBag.pageNumber = pageNumber;
            return View(DL_UserBetList);
        }
        public ActionResult ProfitLoss()
        {
            if (check_hash_key != null && check_hash_key != "")
            {
                CULL();
            }

            string pageNumber = "1";
            int max_page = 20;
            int min_page = 0;
            var clientprofitlossstat = new List<clientprofitlossstat>();
            try
            {
                if (Request.QueryString["startDate"] != null && Request.QueryString["endDate"] != null)
                {
                    string startDate = Request.QueryString["startDate"];
                    string endDate = Request.QueryString["endDate"];
                    if (Request.QueryString["pageNumber"] != null)
                    {
                        pageNumber = Request.QueryString["pageNumber"];
                    }
                    if (startDate != "" && endDate != "" && pageNumber != "")
                    {
                        int pageno = Int32.Parse(pageNumber);
                        max_page = 20 * pageno;
                        min_page = max_page - 20;
                        string login_user_id = (string)System.Web.HttpContext.Current.Session["UIDS"];
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            using (var cmd = new SqlCommand("SELECT created,event_id,market_id,debit,credit,description FROM(SELECT created,event_id,market_id,debit,credit,description , ROW_NUMBER() over(order by id desc) as row FROM user_account_statements WHERE acc_stat_type='pl_coins' AND (created BETWEEN '" + startDate + "' AND '" + endDate + "') AND user_id='" + login_user_id + "') a where a.row > '" + min_page + "'  and a.row <= '" + max_page + "' ", con))
                            {
                                con.Open();
                                var dr = cmd.ExecuteReader();
                                while (dr.Read())
                                {
                                    DateTime created1 = (DateTime)dr["created"];
                                    string created = created1.ToString("yyyy-MM-dd HH:mm:ss");
                                    string event_id = (string)dr["event_id"];
                                    string market_id = (string)dr["market_id"];
                                    Double debit = (Double)dr["debit"];
                                    Double credit = (Double)dr["credit"];
                                    string sport_n = "";
                                    string match_n = "";
                                    string market_n = "";
                                    string description = (string)dr["description"];
                                    string[] desc = Regex.Split(description, "/");
                                    sport_n = desc[0];
                                    match_n = desc[1];
                                    market_n = desc[2];
                                    Double debitcomm = 0;
                                    Double creditcomm = 0;
                                    /*using (var cmdcomm = new SqlCommand("SELECT [debit] , [credit] FROM user_account_statements WHERE event_id='"+event_id+"' AND market_id='"+market_id+"' AND acc_stat_type='comm' AND user_id='" + login_user_id + "' ", con))
                                    {
                                        var drcomm = cmdcomm.ExecuteReader();
                                        drcomm.Read();
                                        debitcomm = (Double)dr["debit"];
                                        creditcomm = (Double)dr["credit"];

                                    }*/
                                    string total_pl = "0";
                                    Double profit_loss = credit - debit - creditcomm - debitcomm;
                                    string total_pl_color = "black";
                                    if (profit_loss < 0)
                                    {
                                        total_pl_color = "#D0021B";
                                        profit_loss = System.Math.Abs(profit_loss);
                                        total_pl = "(" + profit_loss + ")";
                                    }
                                    else
                                    {
                                        total_pl = profit_loss.ToString();
                                    }
                                    clientprofitlossstat.Add(item: new clientprofitlossstat
                                    {
                                        user_id = login_user_id,
                                        sport = sport_n,
                                        match = match_n,
                                        market = market_n,
                                        event_id = event_id,
                                        market_id = market_id,
                                        created = created,
                                        total_pl = total_pl,
                                        total_pl_color = total_pl_color
                                    });
                                }
                                con.Close();
                            }
                            string stmts = "SELECT COUNT(id) FROM user_account_statements WHERE acc_stat_type='pl_coins' AND (created BETWEEN '" + startDate + "' AND '" + endDate + "') AND user_id='" + login_user_id + "'";
                            int countbeth = 0;
                            using (SqlCommand cmdCounts = new SqlCommand(stmts, con))
                            {
                                con.Open();
                                countbeth = (int)cmdCounts.ExecuteScalar();
                                con.Close();
                            }
                            ViewBag.countbeth = countbeth;
                            ViewBag.startDate = startDate;
                            ViewBag.endDate = endDate;
                        }
                    }
                }
                else
                {
                    string login_user_id = (string)System.Web.HttpContext.Current.Session["UIDS"];
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        using (var cmd = new SqlCommand("SELECT created,event_id,market_id,debit,credit,description FROM(SELECT created,event_id,market_id,debit,credit,description , ROW_NUMBER() over(order by id desc) as row FROM user_account_statements WHERE acc_stat_type='pl_coins' AND created >'" + todaydate.ToString(today) + "' AND user_id='" + login_user_id + "') a where a.row > '" + min_page + "'  and a.row <= '" + max_page + "' ", con))
                        {
                            con.Open();
                            var dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                DateTime created1 = (DateTime)dr["created"];
                                string created = created1.ToString("yyyy-MM-dd HH:mm:ss");
                                string event_id = (string)dr["event_id"];
                                string market_id = (string)dr["market_id"];
                                Double debit = (Double)dr["debit"];
                                Double credit = (Double)dr["credit"];
                                string sport_n = "";
                                string match_n = "";
                                string market_n = "";
                                string description = (string)dr["description"];
                                string[] desc = Regex.Split(description, "/");
                                sport_n = desc[0];
                                match_n = desc[1];
                                market_n = desc[2];
                                Double debitcomm = 0;
                                Double creditcomm = 0;
                                /*using (var cmdcomm = new SqlCommand("SELECT [debit] , [credit] FROM user_account_statements WHERE event_id='" + event_id + "' AND market_id='" + market_id + "' AND acc_stat_type='comm' AND user_id='" + login_user_id + "' ", con))
                                {
                                    var drcomm = cmdcomm.ExecuteReader();
                                    drcomm.Read();
                                    debitcomm = (Double)dr["debit"];
                                    creditcomm = (Double)dr["credit"];

                                }*/
                                string total_pl = "0";
                                Double profit_loss = credit - debit - creditcomm - debitcomm;
                                string total_pl_color = "black";
                                if (profit_loss < 0)
                                {
                                    total_pl_color = "#D0021B";
                                    profit_loss = System.Math.Abs(profit_loss);
                                    total_pl = "(" + profit_loss + ")";
                                }
                                else
                                {
                                    total_pl = profit_loss.ToString();
                                }
                                clientprofitlossstat.Add(item: new clientprofitlossstat
                                {
                                    sport = sport_n,
                                    match = match_n,
                                    market = market_n,
                                    event_id = event_id,
                                    market_id = market_id,
                                    created = created,
                                    total_pl = total_pl,
                                    total_pl_color = total_pl_color
                                });
                            }
                            con.Close();
                        }
                        string stmts = "SELECT COUNT(id) FROM user_account_statements WHERE acc_stat_type='pl_coins' AND created >'" + todaydate.ToString(today) + "' AND user_id='" + login_user_id + "'";
                        int countbeth = 0;
                        using (SqlCommand cmdCounts = new SqlCommand(stmts, con))
                        {
                            con.Open();
                            countbeth = (int)cmdCounts.ExecuteScalar();
                            con.Close();
                        }
                        ViewBag.countbeth = countbeth;
                        ViewBag.startDate = "0";
                        ViewBag.endDate = "0";
                    }
                }
                ViewBag.pageNumber = pageNumber;
            }
            catch (Exception ex)
            {

            }
            return View(clientprofitlossstat);
        }
        public ActionResult AutoLiveIne()
        {
            var AllSportsAddM = new List<NavbarListAllSports>();
            try
            {
                String id = Request.QueryString["id"];
                ViewBag.id = id;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT market_name,betfair_id,status,created FROM markets where event_code='" + id + "' AND (status='activate' OR status='inactive') AND type='TP' ", con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string market_name = (string)reader["market_name"];
                            string betfair_id = (string)reader["betfair_id"];
                            string status = (string)reader["status"];
                            DateTime created = (DateTime)reader["created"];
                            string created1 = created.ToString("yyyy/MM/dd HH:mm:ss");

                            AllSportsAddM.Add(item: new NavbarListAllSports
                            {
                                SportsName = market_name,
                                SportsId = betfair_id,
                                IsBlock = status,
                                ForTest = created1
                            });
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View(AllSportsAddM);
        }

        public ActionResult MTeenpatti(EventIdSend eventIdSend, string sportid, string leagueid)
        {
            if (check_hash_key != null && check_hash_key != "")
            {
                CULL();
            }
            string Event_Code_Id = sportid;
            ViewBag.EventCode = Event_Code_Id;
            ViewBag.BetfairId = Event_Code_Id;
            return View();
        }

        public ActionResult GetCasinoT(string data)
        {
            List<InerPageOtherMarketsModel> messages = new List<InerPageOtherMarketsModel>();
            if ((check_sess_id != null && check_sess_id != "") || (is_panel_log != null && is_panel_log != ""))
            {
                messages = InerPageCasinoTe(data);
            }
            return Json(messages, JsonRequestBehavior.AllowGet);
        }
        public List<InerPageOtherMarketsModel> InerPageCasinoTe(string Eve_Code)
        {
            var messages = new List<InerPageOtherMarketsModel>();
            string evv_name = "";
            if (Eve_Code == "20202020")
            {
                evv_name = "TeenPatiT20";
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://diamond8exch.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("CasinoData/" + evv_name).Result;

                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        if (responseJson != null)
                        {
                            var t1 = responseJson.t1;
                            var t2 = responseJson.t2;
                            string betfair_id = t1[0]["mid"];
                            string autotime = t1[0]["autotime"];
                            string c1 = t1[0]["C1"];
                            string c2 = t1[0]["C2"];
                            string c3 = t1[0]["C3"];
                            string c4 = t1[0]["C4"];
                            string c5 = t1[0]["C5"];
                            string c6 = t1[0]["C6"];
                            string market_name = "Round : " + betfair_id;
                            for (int i = 0; i < t2.Count; i++)
                            {
                                string runner_name = t2[i]["nation"];
                                string status = t2[i]["gstatus"];
                                string sid = t2[i]["sid"];
                                string rate = t2[i]["rate"];
                                if (status == "1")
                                {
                                    status = "OPEN";
                                }
                                else
                                {
                                    status = "SUSPENDED";
                                }
                                if (sid == "1" || sid == "3")
                                {
                                    messages.Add(item: new InerPageOtherMarketsModel
                                    {
                                        EventCode = Eve_Code,
                                        MarketName = market_name,
                                        back1 = rate,
                                        lay1 = "",
                                        back11 = "",
                                        lay11 = "",
                                        back22 = "",
                                        lay22 = "",
                                        Runnername = runner_name,
                                        BetfairId = betfair_id,
                                        back1size = c1,
                                        back2size = c2,
                                        back3size = c3,
                                        lay1size = c4,
                                        lay2size = c5,
                                        lay3size = c6,
                                        totalMatched = autotime,
                                        status = status,
                                        Event_Type_Id = "7888"
                                    });
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
            }
            else if (Eve_Code == "30303030")
            {
                evv_name = "TeenPatiT20";
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://skyinplay99.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("CasinoData/TeenPatiT20B").Result;

                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        if (responseJson != null)
                        {
                            var t1 = responseJson.t1;
                            var t2 = responseJson.t2;
                            string betfair_id = t1[0]["mid"];
                            string autotime = t1[0]["autotime"];
                            string c1 = t1[0]["C1"];
                            string c2 = t1[0]["C2"];
                            string c3 = t1[0]["C3"];
                            string c4 = t1[0]["C4"];
                            string c5 = t1[0]["C5"];
                            string c6 = t1[0]["C6"];
                            string market_name = "Round : " + betfair_id;
                            for (int i = 0; i < t2.Count; i++)
                            {
                                string runner_name = t2[i]["nation"];
                                string status = t2[i]["gstatus"];
                                string sid = t2[i]["sid"];
                                string rate = t2[i]["rate"];
                                if (status == "1")
                                {
                                    status = "OPEN";
                                }
                                else
                                {
                                    status = "SUSPENDED";
                                }
                                if (sid == "1" || sid == "3")
                                {
                                    messages.Add(item: new InerPageOtherMarketsModel
                                    {
                                        EventCode = Eve_Code,
                                        MarketName = market_name,
                                        back1 = rate,
                                        lay1 = "",
                                        back11 = "",
                                        lay11 = "",
                                        back22 = "",
                                        lay22 = "",
                                        Runnername = runner_name,
                                        BetfairId = betfair_id,
                                        back1size = c1,
                                        back2size = c2,
                                        back3size = c3,
                                        lay1size = c4,
                                        lay2size = c5,
                                        lay3size = c6,
                                        totalMatched = autotime,
                                        status = status,
                                        Event_Type_Id = "7888"
                                    });
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
            }
            else if (Eve_Code == "50505050")
            {
                evv_name = "dt20";
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://api_link.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("t20api.php?type=" + evv_name).Result;

                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        if (responseJson != null)
                        {
                            var t1 = responseJson.data.t1;
                            var t2 = responseJson.data.t2;
                            string betfair_id = t1[0]["mid"];
                            string autotime = t1[0]["autotime"];
                            string c1 = t1[0]["C1"];
                            string c2 = t1[0]["C2"];
                            string c3 = "";
                            string c4 = "";
                            string c5 = "";
                            string c6 = "";
                            string market_name = "Round : " + betfair_id;
                            for (int i = 0; i < 2; i++)
                            {
                                string runner_name = t2[i]["nat"];
                                string status = t2[i]["gstatus"];
                                string sid = t2[i]["sid"];
                                string rate = t2[i]["rate"];
                                if (status == "1")
                                {
                                    status = "OPEN";
                                }
                                else
                                {
                                    status = "SUSPENDED";
                                }
                                if (sid == "1" || sid == "2")
                                {
                                    messages.Add(item: new InerPageOtherMarketsModel
                                    {
                                        EventCode = Eve_Code,
                                        MarketName = market_name,
                                        back1 = rate,
                                        lay1 = "",
                                        back11 = "",
                                        lay11 = "",
                                        back22 = "",
                                        lay22 = "",
                                        Runnername = runner_name,
                                        BetfairId = betfair_id,
                                        back1size = c1,
                                        back2size = c2,
                                        back3size = c3,
                                        lay1size = c4,
                                        lay2size = c5,
                                        lay3size = c6,
                                        totalMatched = autotime,
                                        status = status,
                                        Event_Type_Id = "7888"
                                    });
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
            }
            else if (Eve_Code == "51515151")
            {
                evv_name = "dtl20";
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://api_link.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("t20api.php?type=" + evv_name).Result;

                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        if (responseJson != null)
                        {
                            var t1 = responseJson.data.t1;
                            var t2 = responseJson.data.t2;
                            string betfair_id = t1[0]["mid"];
                            string autotime = t1[0]["autotime"];
                            string c1 = t1[0]["C1"];
                            string c2 = t1[0]["C2"];
                            string c3 = t1[0]["C3"];
                            string c4 = "";
                            string c5 = "";
                            string c6 = "";
                            string market_name = "Round : " + betfair_id;
                            for (int i = 0; i < t2.Count; i++)
                            {
                                string runner_name = t2[i]["nat"];
                                string status = t2[i]["gstatus"];
                                string sid = t2[i]["sid"];
                                string rate = t2[i]["b1"];
                                if (status == "1")
                                {
                                    status = "OPEN";
                                }
                                else
                                {
                                    status = "SUSPENDED";
                                }
                                if (sid == "1" || sid == "21" || sid == "41")
                                {
                                    messages.Add(item: new InerPageOtherMarketsModel
                                    {
                                        EventCode = Eve_Code,
                                        MarketName = market_name,
                                        back1 = rate,
                                        lay1 = "",
                                        back11 = "",
                                        lay11 = "",
                                        back22 = "",
                                        lay22 = "",
                                        Runnername = runner_name,
                                        BetfairId = betfair_id,
                                        back1size = c1,
                                        back2size = c2,
                                        back3size = c3,
                                        lay1size = c4,
                                        lay2size = c5,
                                        lay3size = c6,
                                        totalMatched = autotime,
                                        status = status,
                                        Event_Type_Id = "7888"
                                    });
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
            }
            else if (Eve_Code == "60606060")
            {
                evv_name = "lucky7";
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://skyinplay99.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("CasinoData/Lucky7").Result;

                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        if (responseJson != null)
                        {
                            var t1 = responseJson.data.t1;
                            var t2 = responseJson.data.t2;
                            string betfair_id = t1[0]["mid"];
                            string autotime = t1[0]["autotime"];
                            string c1 = t1[0]["C1"];
                            string c2 = "";
                            string c3 = "";
                            string c4 = "";
                            string c5 = "";
                            string c6 = "";
                            string market_name = "Round : " + betfair_id;
                            for (int i = 0; i < 2; i++)
                            {
                                string runner_name = t2[i]["nat"];
                                string status = t2[i]["gstatus"];
                                string sid = t2[i]["sid"];
                                string rate = t2[i]["rate"];
                                if (status == "1")
                                {
                                    status = "OPEN";
                                }
                                else
                                {
                                    status = "SUSPENDED";
                                }
                                messages.Add(item: new InerPageOtherMarketsModel
                                {
                                    EventCode = Eve_Code,
                                    MarketName = market_name,
                                    back1 = rate,
                                    lay1 = "",
                                    back11 = "",
                                    lay11 = "",
                                    back22 = "",
                                    lay22 = "",
                                    Runnername = runner_name,
                                    BetfairId = betfair_id,
                                    back1size = c1,
                                    back2size = c2,
                                    back3size = c3,
                                    lay1size = c4,
                                    lay2size = c5,
                                    lay3size = c6,
                                    totalMatched = autotime,
                                    status = status,
                                    Event_Type_Id = "7888"
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
            }
            else if (Eve_Code == "61616161")
            {
                evv_name = "lucky7B";
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://api_link.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("t20api.php?type=" + evv_name).Result;

                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        if (responseJson != null)
                        {
                            var t1 = responseJson.data.t1;
                            var t2 = responseJson.data.t2;
                            string betfair_id = t1[0]["mid"];
                            string autotime = t1[0]["autotime"];
                            string c1 = t1[0]["C1"];
                            string c2 = "";
                            string c3 = "";
                            string c4 = "";
                            string c5 = "";
                            string c6 = "";
                            string market_name = "Round : " + betfair_id;
                            for (int i = 0; i < 2; i++)
                            {
                                string runner_name = t2[i]["nat"];
                                string status = t2[i]["gstatus"];
                                string sid = t2[i]["sid"];
                                string rate = t2[i]["rate"];
                                if (status == "1")
                                {
                                    status = "OPEN";
                                }
                                else
                                {
                                    status = "SUSPENDED";
                                }
                                messages.Add(item: new InerPageOtherMarketsModel
                                {
                                    EventCode = Eve_Code,
                                    MarketName = market_name,
                                    back1 = rate,
                                    lay1 = "",
                                    back11 = "",
                                    lay11 = "",
                                    back22 = "",
                                    lay22 = "",
                                    Runnername = runner_name,
                                    BetfairId = betfair_id,
                                    back1size = c1,
                                    back2size = c2,
                                    back3size = c3,
                                    lay1size = c4,
                                    lay2size = c5,
                                    lay3size = c6,
                                    totalMatched = autotime,
                                    status = status,
                                    Event_Type_Id = "7888"
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
            }
            else if (Eve_Code == "40404040")
            {
                evv_name = "TeenPatiT20Poker";
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://api_link.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("t20api.php?type=" + evv_name).Result;

                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        if (responseJson != null)
                        {
                            var t1 = responseJson.data.t1;
                            var t2 = responseJson.data.t2;
                            string betfair_id = t1[0]["mid"];
                            string autotime = t1[0]["autotime"];
                            string c1 = t1[0]["C1"];
                            string c2 = t1[0]["C2"];
                            string c3 = t1[0]["C3"];
                            string c4 = t1[0]["C4"];
                            string c5 = t1[0]["C5"];
                            string c6 = t1[0]["C6"];
                            string c7 = t1[0]["C7"];
                            string c8 = t1[0]["C8"];
                            string c9 = t1[0]["C9"];
                            string market_name = "Round : " + betfair_id;
                            for (int i = 0; i < 2; i++)
                            {
                                string runner_name = t2[i]["nation"];
                                string vbfb = "";
                                if (i == 0)
                                {
                                    vbfb = runner_name + " (Player A)";
                                }
                                else if (i == 1)
                                {
                                    vbfb = runner_name + " (Player B)";
                                }
                                else
                                {

                                }
                                string status = t2[i]["gstatus"];
                                string sid = t2[i]["sid"];
                                string rate = t2[i]["rate"];
                                if (status == "1")
                                {
                                    status = "OPEN";
                                }
                                else
                                {
                                    status = "SUSPENDED";
                                }
                                messages.Add(item: new InerPageOtherMarketsModel
                                {
                                    EventCode = Eve_Code,
                                    MarketName = market_name,
                                    back1 = rate,
                                    lay1 = "",
                                    back11 = "",
                                    lay11 = "",
                                    back22 = "",
                                    lay22 = "",
                                    Runnername = vbfb,
                                    BetfairId = betfair_id,
                                    back1size = c1,
                                    back2size = c2,
                                    back3size = c3,
                                    lay1size = c4,
                                    lay2size = c5,
                                    lay3size = c6,
                                    c7 = c7,
                                    c8 = c8,
                                    c9 = c9,
                                    totalMatched = autotime,
                                    status = status,
                                    Event_Type_Id = "7888"
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
            }
            else if (Eve_Code == "31313131")
            {
                evv_name = "TeenPatiMuflis";
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://skyinplay99.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("CasinoData/TeenPatiMuflis").Result;

                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        if (responseJson != null)
                        {
                            var t1 = responseJson.t1;
                            var t2 = responseJson.t2;
                            string betfair_id = t1[0]["mid"];
                            string autotime = t1[0]["autotime"];
                            string c1 = t1[0]["C1"];
                            string c2 = t1[0]["C2"];
                            string c3 = t1[0]["C3"];
                            string c4 = t1[0]["C4"];
                            string c5 = t1[0]["C5"];
                            string c6 = t1[0]["C6"];
                            string market_name = "Round : " + betfair_id;
                            for (int i = 0; i < t2.Count; i++)
                            {
                                string runner_name = t2[i]["nation"];
                                string status = t2[i]["gstatus"];
                                string sid = t2[i]["sid"];
                                string rate = t2[i]["rate"];
                                if (status == "1")
                                {
                                    status = "OPEN";
                                }
                                else
                                {
                                    status = "SUSPENDED";
                                }
                                if (sid == "1" || sid == "3")
                                {
                                    messages.Add(item: new InerPageOtherMarketsModel
                                    {
                                        EventCode = Eve_Code,
                                        MarketName = market_name,
                                        back1 = rate,
                                        lay1 = "",
                                        back11 = "",
                                        lay11 = "",
                                        back22 = "",
                                        lay22 = "",
                                        Runnername = runner_name,
                                        BetfairId = betfair_id,
                                        back1size = c1,
                                        back2size = c2,
                                        back3size = c3,
                                        lay1size = c4,
                                        lay2size = c5,
                                        lay3size = c6,
                                        totalMatched = autotime,
                                        status = status,
                                        Event_Type_Id = "7888"
                                    });
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
            }
            else if (Eve_Code == "52525252")
            {
                evv_name = "dt6";
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://api_link.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("t20api.php?type=" + evv_name).Result;

                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        if (responseJson != null)
                        {
                            var t1 = responseJson.data.t1;
                            var t2 = responseJson.data.t2;
                            string betfair_id = t1[0]["mid"];
                            string autotime = t1[0]["autotime"];
                            string c1 = t1[0]["C1"];
                            string c2 = t1[0]["C2"];
                            string market_name = "Round : " + betfair_id;
                            for (int i = 0; i < 2; i++)
                            {
                                string runner_name = t2[i]["nat"];
                                string status = t2[i]["gstatus"];
                                string sid = t2[i]["sid"];
                                string RateSenad1 = "";
                                string RateSenad2 = "";
                                string rate = t2[i]["b1"];
                                string rate2 = t2[i]["l1"];
                                if (rate == "0.00" || rate == "0")
                                {
                                    RateSenad1 = "";
                                }
                                else
                                {
                                    RateSenad1 = rate;
                                }
                                if (rate2 == "0.00" || rate2 == "0")
                                {
                                    RateSenad2 = "";
                                }
                                else
                                {
                                    RateSenad2 = rate2;
                                }
                                if (status == "ACTIVE")
                                {
                                    status = "OPEN";
                                }
                                else
                                {
                                    status = "SUSPENDED";
                                }
                                messages.Add(item: new InerPageOtherMarketsModel
                                {
                                    EventCode = Eve_Code,
                                    MarketName = market_name,
                                    back1 = RateSenad1,
                                    lay1 = RateSenad2,
                                    back11 = "",
                                    lay11 = "",
                                    back22 = "",
                                    lay22 = "",
                                    Runnername = runner_name,
                                    BetfairId = betfair_id,
                                    back1size = c1,
                                    back2size = c2,
                                    totalMatched = autotime,
                                    status = status,
                                    Event_Type_Id = "7888"
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
            }

            else if(Eve_Code == "64646464")
            {
                evv_name = "dt6";
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://newpark.bet/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("fancy.php").Result;

                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        if (responseJson != null)
                        {
                            //string data = responseJson.Content;

                           
                            string distance = responseJson.SelectToken("data.markets[0].runners[0].ex.availableToBack[0].price").ToString();
                            string distance2 = responseJson.SelectToken("data.markets[0].runners[0].ex.availableToBack[0].price").ToString();
                           
                           
                            // var GetRunner = responseJson.data["markets"].id;
                            var GetRunnerLo = responseJson.data.id;
                           // var availableToBack = GetRunner[i].ex.availableToBack;
                            var t333 = responseJson.data.markets[0]["id"];
                            var t333d = responseJson.data.markets[0].runners[0].status;
                            var t1 = responseJson.data.markets[0].runners[0].ex.availableToBack[0].price;
                            var t2 = responseJson.data.t2;
                            string betfair_id = t1[0]["mid"];
                            string autotime = t1[0]["autotime"];
                            string c1 = t1[0]["C1"];
                            string c2 = t1[0]["C2"];
                            string market_name = "Round : " + betfair_id;
                            for (int i = 0; i < 2; i++)
                            {
                                string runner_name = t2[i]["nat"];
                                string status = t2[i]["gstatus"];
                                string sid = t2[i]["sid"];
                                string RateSenad1 = "";
                                string RateSenad2 = "";
                                string rate = t2[i]["b1"];
                                string rate2 = t2[i]["l1"];
                                if (rate == "0.00" || rate == "0")
                                {
                                    RateSenad1 = "";
                                }
                                else
                                {
                                    RateSenad1 = rate;
                                }
                                if (rate2 == "0.00" || rate2 == "0")
                                {
                                    RateSenad2 = "";
                                }
                                else
                                {
                                    RateSenad2 = rate2;
                                }
                                if (status == "ACTIVE")
                                {
                                    status = "OPEN";
                                }
                                else
                                {
                                    status = "SUSPENDED";
                                }
                                messages.Add(item: new InerPageOtherMarketsModel
                                {
                                    EventCode = Eve_Code,
                                    MarketName = market_name,
                                    back1 = RateSenad1,
                                    lay1 = RateSenad2,
                                    back11 = "",
                                    lay11 = "",
                                    back22 = "",
                                    lay22 = "",
                                    Runnername = runner_name,
                                    BetfairId = betfair_id,
                                    back1size = c1,
                                    back2size = c2,
                                    totalMatched = autotime,
                                    status = status,
                                    Event_Type_Id = "7888"
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
            }

            else if(Eve_Code == "5676756767")
            {
                var client = new RestClient("https://betexch.uk/api/v1/matchDetails");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("sec-ch-ua", "\"Google Chrome\";v=\"87\", \" Not;A Brand\";v=\"99\", \"Chromium\";v=\"87\"");
                request.AddHeader("Accept", "application/json, text/plain, */*");
                request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOnsiaWQiOjE5MjcsInJvbGUiOiJBZG1pblRlc3QiLCJwYXJlbnRfaWQiOiIxODU5LDE4NTgsMTg1NywxLDAiLCJ1c2VyX3R5cGVfaWQiOjV9LCJpYXQiOjE2MDkyNDA5NDcsImV4cCI6MTYxMDEwNDk0N30.BetHC7zqmpfnSOMj-kf_Je8b1viIUPpECer4WC5nt6Y");
                request.AddHeader("sec-ch-ua-mobile", "?0");
                client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", "{\"match_id\":\"56767\",\"market_id\":\"-100.56767\",\"user_id\":1864,\"user_type_id\":5}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                string data = response.Content;
                JObject jObject = JObject.Parse(data);
                string betfair_id = jObject.SelectToken("data.match[0].roundId").ToString();
                string market_name = "Round : " + betfair_id;
                string autotime = jObject.SelectToken("data.match[0].autotime").ToString();
                string status = jObject.SelectToken("data.match[0].status").ToString();
                for (int i = 0; i < 2; i++)
                {
                    string runner_name = jObject.SelectToken("data.match[0].runners[" + i + "].name").ToString();                  
                    string sid = "";
                    string RateSenad1 = "";
                    string RateSenad2 = "";
                    string c1 = jObject.SelectToken("data.match[0].runners[0].cards").ToString();
                    string c2 = jObject.SelectToken("data.match[0].runners[1].cards").ToString();

                    string fvhbgfh = jObject.SelectToken("data.match[0].runners[" + i + "].lay").ToString();
                    string backConts = jObject.SelectToken("data.match[0].runners[" + i + "].back").ToString();
                    string rate = "";
                    string rate2 = "";
                    if (backConts.Contains("price"))
                    {
                         rate = jObject.SelectToken("data.match[0].runners[" + i + "].back[0].price").ToString();
                    }
                    if (fvhbgfh.Contains("price"))
                    {
                        rate2 =  jObject.SelectToken("data.match[0].runners[" + i + "].lay[0].price").ToString();
                    }
                    if (rate == "0.00" || rate == "0")
                    {
                        RateSenad1 = "";
                    }
                    else
                    {
                        RateSenad1 = rate;
                    }
                    if (rate2 == "0.00" || rate2 == "0")
                    {
                        RateSenad2 = "";
                    }
                    else
                    {
                        RateSenad2 = rate2;
                    }
                    if (status == "OPEN")
                    {
                        status = "OPEN";
                    }
                    else
                    {
                        status = "SUSPENDED";
                    }

                    messages.Add(item: new InerPageOtherMarketsModel
                    {
                        EventCode = Eve_Code,
                        MarketName = market_name,
                        back1 = RateSenad1,
                        lay1 = RateSenad2,
                        back11 = "",
                        lay11 = "",
                        back22 = "",
                        lay22 = "",
                        Runnername = runner_name,
                        BetfairId = betfair_id,
                        back1size = c1,
                        back2size = c2,
                        totalMatched = autotime,
                        status = status,
                        Event_Type_Id = "4466"
                    });
                }
                    //string distance = jObject.SelectToken("data.match[0].runners[0].back[0].price").ToString();
               
                // return Json(response.Content, JsonRequestBehavior.AllowGet);
            }
            return messages;
        }

        public String GetCasinoTResult(string id)
        {
            string evv_nameR = "";
            var products = "";
            if (check_sess_id != null && check_sess_id != "")
            {
                if (id == "20202020")
                {
                    evv_nameR = "TeenPatiT20Result";
                    try
                    {
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri("http://diamond8exch.com/");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        try
                        {
                            HttpResponseMessage response = client.GetAsync("CasinoData/" + evv_nameR).Result;  // Blocking call! 
                            products = response.Content.ReadAsStringAsync().Result;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    catch (TaskCanceledException ex)
                    {

                    }
                }
                else if (id == "30303030")
                {
                    evv_nameR = "TeenPatiT20Result";
                    try
                    {
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri("http://api_link.com/");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        try
                        {
                            HttpResponseMessage response = client.GetAsync("tpresult.php?type=" + evv_nameR).Result;  // Blocking call! 
                            products = response.Content.ReadAsStringAsync().Result;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    catch (TaskCanceledException ex)
                    {

                    }
                }
                else if (id == "50505050")
                {
                    evv_nameR = "dt20Result";
                    try
                    {
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri("http://api_link.com/");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        try
                        {
                            HttpResponseMessage response = client.GetAsync("tpresult.php?type=" + evv_nameR).Result;  // Blocking call! 
                            products = response.Content.ReadAsStringAsync().Result;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    catch (TaskCanceledException ex)
                    {

                    }
                }
                else if (id == "60606060")
                {
                    evv_nameR = "lucky7Result";
                    try
                    {
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri("http://api_link.com/");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        try
                        {
                            HttpResponseMessage response = client.GetAsync("tpresult.php?type=" + evv_nameR).Result;  // Blocking call! 
                            products = response.Content.ReadAsStringAsync().Result;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    catch (TaskCanceledException ex)
                    {

                    }
                }
                else if (id == "61616161")
                {
                    evv_nameR = "lucky7BResult";
                    try
                    {
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri("http://api_link.com/");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        try
                        {
                            HttpResponseMessage response = client.GetAsync("tpresult.php?type=" + evv_nameR).Result;  // Blocking call! 
                            products = response.Content.ReadAsStringAsync().Result;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    catch (TaskCanceledException ex)
                    {

                    }
                }
                else if (id == "51515151")
                {
                    evv_nameR = "dtl20Result";
                    try
                    {
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri("http://api_link.com/");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        try
                        {
                            HttpResponseMessage response = client.GetAsync("tpresult.php?type=" + evv_nameR).Result;  // Blocking call! 
                            products = response.Content.ReadAsStringAsync().Result;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    catch (TaskCanceledException ex)
                    {

                    }
                }
                else if (id == "40404040")
                {
                    evv_nameR = "TeenPatiT20PokerResult";
                    try
                    {
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri("http://api_link.com/");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        try
                        {
                            HttpResponseMessage response = client.GetAsync("tpresult.php?type=" + evv_nameR).Result;  // Blocking call! 
                            products = response.Content.ReadAsStringAsync().Result;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    catch (TaskCanceledException ex)
                    {

                    }
                }
                else if (id == "31313131")
                {
                    evv_nameR = "TeenPatiOneDayResult";
                    try
                    {
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri("http://api_link.com/");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        try
                        {
                            HttpResponseMessage response = client.GetAsync("tpresult.php?type=" + evv_nameR).Result;  // Blocking call! 
                            products = response.Content.ReadAsStringAsync().Result;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    catch (TaskCanceledException ex)
                    {

                    }
                }
                else if (id == "41414141")
                {
                    evv_nameR = "PokerResult";
                    try
                    {
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri("http://api_link.com/");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        try
                        {
                            HttpResponseMessage response = client.GetAsync("tpresult.php?type=" + evv_nameR).Result;  // Blocking call! 
                            products = response.Content.ReadAsStringAsync().Result;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    catch (TaskCanceledException ex)
                    {

                    }
                }
                else if (id == "52525252")
                {
                    evv_nameR = "dt6Result";
                    try
                    {
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri("http://api_link.com/");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        try
                        {
                            HttpResponseMessage response = client.GetAsync("tpresult.php?type=" + evv_nameR).Result;  // Blocking call! 
                            products = response.Content.ReadAsStringAsync().Result;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    catch (TaskCanceledException ex)
                    {

                    }
                }
                else if(id == "5676756767")
                {
                    var client = new RestClient("https://betexch.uk/api/v1/getLastWinnersResult");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("sec-ch-ua", "\"Google Chrome\";v=\"87\", \" Not;A Brand\";v=\"99\", \"Chromium\";v=\"87\"");
                    request.AddHeader("Accept", "application/json, text/plain, */*");
                    request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOnsiaWQiOjE5MjcsInJvbGUiOiJBZG1pblRlc3QiLCJwYXJlbnRfaWQiOiIxODU5LDE4NTgsMTg1NywxLDAiLCJ1c2VyX3R5cGVfaWQiOjV9LCJpYXQiOjE2MDkyNDA5NDcsImV4cCI6MTYxMDEwNDk0N30.BetHC7zqmpfnSOMj-kf_Je8b1viIUPpECer4WC5nt6Y");
                    request.AddHeader("sec-ch-ua-mobile", "?0");
                    client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", "{\"market_id\":\"-100.56767\"}", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    products = response.Content;
                }
            }
            return products;
        }
        public String GetCasinoTResultD(string id, string mid)
        {
            string evv_nameR = "";
            var products = "";
            if (id == "20202020")
            {
                evv_nameR = "TeenPatiT20ResultDetail";
            }
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://lotusbook247.com/");
                var values = new Dictionary<string, string>
                {
                { "mid", mid }
                };

                var content = new FormUrlEncodedContent(values);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.PostAsync("CasinoData/" + evv_nameR, content).Result;  // Blocking call! 
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

        public void CULL()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT id from users_client where hash_key='" + check_hash_key + "' AND id='" + check_sess_id + "' ", con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        LogOut();
                    }
                    con.Close();
                }
            }
        }
        public ActionResult TeenShow()
        {
            return View();
        }

        public ActionResult tplive()
        {
            String type = Request.QueryString["type"];
            ViewBag.type = type;
            return View();
        }
        public ActionResult tplive1()
        {
            String type = Request.QueryString["type"];
            ViewBag.type = type;
            return View();
        }

        public ActionResult tpbetfair()
        {
            String type = "frji";
            if (check_sess_id != null && check_sess_id != "")
            {
                type = Request.QueryString["type"];
            }
            ViewBag.type = type;
            return View();
        }

        public ActionResult MTP(string sportid, string leagueid)
        {
            if (check_hash_key != null && check_hash_key != "")
            {
                CULL();
            }
            ViewBag.EventCode = sportid;
            ViewBag.BetfairId = leagueid;
            return View();
        }

        public ActionResult GetCasinoTP(string tplid, string tpevid)
        {
            List<InerPageOtherMarketsModel> messages = new List<InerPageOtherMarketsModel>();
            messages = InerPageCasinoTP(tplid, tpevid);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }
        public List<InerPageOtherMarketsModel> InerPageCasinoTP(string tplid, string tpevid)
        {
            var messages = new List<InerPageOtherMarketsModel>();
            string betfair_id = "";
            if (tpevid == "808")
            {
                betfair_id = "56768";
            }
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://lotusbook247.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("api/member/exchange/event/1000/808").Result;

                    var products = response.Content.ReadAsStringAsync().Result;
                    dynamic responseJson = JsonConvert.DeserializeObject(products);
                    if (responseJson.success == true && responseJson.result != null)
                    {
                        var t2 = responseJson.result;
                        for (int i = 0; i < 1; i++)
                        {
                            string id = t2[i]["id"];
                            string id1 = id.Replace("1.808.", "");
                            string id2 = id1.Replace(".808winner", "");
                            string market_name = "Round : " + id2;
                            string url = t2[i]["event"]["url"];
                            string runner_name = t2[i]["nation"];
                            string status = t2[i]["status"];
                            var runner = t2[i]["runners"];
                            for (int i1 = 0; i1 < t2.Count; i++)
                            {

                                string sid = runner[i1]["sort"];
                                string rate = "";
                                if (runner[i1]["back"].Count > 0)
                                {
                                    rate = runner[i1]["back"][0]["price"];
                                }
                                messages.Add(item: new InerPageOtherMarketsModel
                                {
                                    EventCode = tpevid,
                                    MarketName = market_name,
                                    back1 = rate,
                                    lay1 = "",
                                    back11 = "",
                                    lay11 = "",
                                    back22 = "",
                                    lay22 = "",
                                    Runnername = runner_name,
                                    BetfairId = betfair_id,
                                    status = status,
                                    Event_Type_Id = "567"
                                });
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
            return messages;
        }

        public JsonResult GetNewsData(string xid, string type)
        {
            List<GetNewsData> messages = new List<GetNewsData>();
            messages = GetNewsData1(xid, type);
            return Json(messages, JsonRequestBehavior.AllowGet);

        }

        public List<GetNewsData> GetNewsData1(string xid, string type)
        {
            var messages = new List<GetNewsData>();
            Double BackSize1 = 0;
            Double LaySize1 = 0;
            string RunnerName1x = "";
            int GE = 9999;
            int GES = 9999;
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://1xbet.mobi/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync(type + "/GetGameZip?id=" + xid + "&lng=en&cfview=0&isSubGames=true&GroupEvents=true&countevents=600&grMode=2").Result;  // Blocking call! 
                    if (response != null)
                    {
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        var isresult = responseJson["Value"]["GE"];
                        if (responseJson["Value"]["I"] != null)
                        {
                            string SI = responseJson["Value"]["SI"];
                            string T1 = responseJson["Value"]["O1"];
                            string T2 = responseJson["Value"]["O2"];
                            if (SI != "10" && SI != "21" && SI != "4" && SI != "1" && SI != "3")
                            {
                                foreach (var item in isresult)
                                {
                                    string session_name = "";
                                    string SelectionId = "";
                                    string GameStatus = "";
                                    GE = item["G"];
                                    if (GE == 525 || GE == 526 || GE == 537 || GE == 539 || GE == 540 || GE == 6 || GE == 5 || GE == 4 || GE == 805 || GE == 806 || GE == 578 || GE == 579)
                                    {
                                        var E = item["E"];
                                        int ELength = (E[0]).Count;
                                        for (int ii = 0; ii < ELength; ii++)
                                        {
                                            if (GE == 537)
                                            {
                                                RunnerName1x = "(3) Over,(2) Ball [" + T1 + "]";
                                            }
                                            else if (GE == 539)
                                            {
                                                RunnerName1x = "(3) Over,Total [" + T1 + "]";
                                            }
                                            else if (GE == 540)
                                            {
                                                RunnerName1x = "(3) Over,Total [" + T2 + "]";
                                            }
                                            else if (GE == 525)
                                            {
                                                RunnerName1x = "First (3) Overs [" + T1 + "]";
                                            }
                                            else if (GE == 526)
                                            {
                                                RunnerName1x = "First (3) Overs [" + T2 + "]";
                                            }
                                            else if (GE == 6)
                                            {
                                                RunnerName1x = "Total Runs [" + T2 + "]";
                                            }
                                            else if (GE == 5)
                                            {
                                                RunnerName1x = "Total Runs [" + T1 + "]";
                                            }
                                            else if (GE == 4)
                                            {
                                                RunnerName1x = "Both Team Total Runs";
                                            }
                                            else if (GE == 578)
                                            {
                                                RunnerName1x = "Total Six [" + T1 + "]";
                                            }
                                            else if (GE == 579)
                                            {
                                                RunnerName1x = "Total Six [" + T2 + "]";
                                            }
                                            else if (GE == 805)
                                            {
                                                RunnerName1x = "Total Four [" + T1 + "]";
                                            }
                                            else if (GE == 806)
                                            {
                                                RunnerName1x = "Total Four [" + T2 + "]";
                                            }

                                            Boolean filedALock = false;
                                            Boolean filedBLock = false;

                                            Double YesSize1 = E[0][ii]["C"];
                                            BackSize1 = Math.Floor((100 * YesSize1) - 100);
                                            if (E[0][ii]["B"] != null)
                                            {
                                                filedBLock = E[0][ii]["B"];
                                            }
                                            string First = "";
                                            string Second = "";
                                            string Third = "";
                                            string rdataname = E[0][ii]["P"];
                                            String startdata = rdataname.Split('.')[0];
                                            String firstbox = rdataname.Substring(rdataname.IndexOf(".") + 1);
                                            float FirstBox = float.Parse(firstbox) / 10;
                                            First = FirstBox.ToString();
                                            String thirdbox = "";
                                            int ThirdBox = 0;
                                            if (startdata.Length == 5)
                                            {
                                                thirdbox = startdata.Substring(0, 4);
                                                ThirdBox = (Int32.Parse(thirdbox)) / 10;
                                                Third = ThirdBox.ToString();
                                                Second = startdata.Substring(startdata.Length - 2); //startdata.Replace(Third, "");
                                            }
                                            else if (startdata.Length == 4)
                                            {
                                                thirdbox = startdata.Substring(0, 3);
                                                ThirdBox = (Int32.Parse(thirdbox)) / 10;
                                                Third = ThirdBox.ToString();
                                                Second = startdata.Substring(startdata.Length - 2); //startdata.Replace(Third, "");
                                            }
                                            else if (startdata.Length == 3)
                                            {
                                                thirdbox = startdata.Substring(0, 2);
                                                ThirdBox = (Int32.Parse(thirdbox)) / 10;
                                                Third = ThirdBox.ToString();
                                                Second = startdata.Substring(startdata.Length - 2); //startdata.Replace(Third, "");
                                            }
                                            else
                                            {
                                                Third = startdata;
                                                Second = "0";
                                            }

                                            session_name = RunnerName1x.Replace("(3)", Third);
                                            session_name = session_name.Replace("(2)", Second);

                                            /* for decimal before and after
                                            String startdata3 = rdataname.Split('.')[0];
                                            String startdata4 = rdataname.Split('.')[1];*/
                                            Double gyg = 0;
                                            if (GE == 5 || GE == 6 || GE == 4 || GE == 805 || GE == 806 || GE == 578 || GE == 579)
                                            {
                                                gyg = Double.Parse(rdataname) + 0.5;
                                            }
                                            else
                                            {
                                                gyg = Double.Parse(First) + 0.5;
                                            }
                                            int xrate = Convert.ToInt32(gyg);

                                            if (E[1][ii]["C"] != null)
                                            {
                                                Double NotSize1 = E[1][ii]["C"];
                                                LaySize1 = Math.Round(100 / (NotSize1 - 1));
                                                if (E[1][ii]["B"] != null)
                                                {
                                                    filedALock = E[1][ii]["B"];
                                                }
                                            }
                                            if (filedBLock == true || filedALock == true)
                                            {
                                                GameStatus = "Ball Running";
                                            }
                                            else
                                            {
                                                GameStatus = "";
                                            }
                                            SelectionId = GE + "_" + Third + "_" + Second;
                                            messages.Add(item: new GetNewsData
                                            {
                                                SelectionId = SelectionId,
                                                TempId = SelectionId + xrate.ToString(),
                                                RunnerName = session_name,
                                                BackSize1 = BackSize1.ToString(),
                                                LaySize1 = LaySize1.ToString(),
                                                BackPrice1 = xrate.ToString(),
                                                LayPrice1 = xrate.ToString(),
                                                GameStatus = GameStatus
                                            });
                                        }
                                    }
                                    else if (GE == 538)
                                    {
                                        var E = item["E"];
                                        int ELength = (E[0]).Count;
                                        for (int ii = 0; ii < ELength; ii++)
                                        {
                                            if (GE == 538)
                                            {
                                                RunnerName1x = "(3) Over,(2) Ball [" + T2 + "]";
                                            }

                                            Boolean filedALock = false;
                                            Boolean filedBLock = false;

                                            Double YesSize1 = E[0][ii]["C"];
                                            BackSize1 = Math.Floor((100 * YesSize1) - 100);
                                            if (E[0][ii]["B"] != null)
                                            {
                                                filedBLock = E[0][ii]["B"];
                                            }
                                            string First = "";
                                            string Second = "";
                                            string Third = "";
                                            string rdataname = E[0][ii]["P"];
                                            String startdata = rdataname.Split('.')[0];
                                            String firstbox = rdataname.Substring(rdataname.IndexOf(".") + 1);
                                            float FirstBox = float.Parse(firstbox) / 10;
                                            First = FirstBox.ToString();
                                            String thirdbox = "";
                                            int ThirdBox = 0;
                                            if (startdata.Length == 5)
                                            {
                                                thirdbox = startdata.Substring(0, 4);
                                                ThirdBox = (Int32.Parse(thirdbox)) / 10;
                                                Third = ThirdBox.ToString();
                                                Second = startdata.Substring(startdata.Length - 2); //startdata.Replace(Third, "");
                                            }
                                            else if (startdata.Length == 4)
                                            {
                                                thirdbox = startdata.Substring(0, 3);
                                                ThirdBox = (Int32.Parse(thirdbox)) / 10;
                                                Third = ThirdBox.ToString();
                                                Second = startdata.Substring(startdata.Length - 2); //startdata.Replace(Third, "");
                                            }
                                            else if (startdata.Length == 3)
                                            {
                                                thirdbox = startdata.Substring(0, 2);
                                                ThirdBox = (Int32.Parse(thirdbox)) / 10;
                                                Third = ThirdBox.ToString();
                                                Second = startdata.Substring(startdata.Length - 2); //startdata.Replace(Third, "");
                                            }
                                            else
                                            {
                                                Third = startdata;
                                                Second = "0";
                                            }

                                            session_name = RunnerName1x.Replace("(3)", Third);
                                            session_name = session_name.Replace("(2)", Second);

                                            /* for decimal before and after
                                            String startdata3 = rdataname.Split('.')[0];
                                            String startdata4 = rdataname.Split('.')[1];*/

                                            Double gyg = Double.Parse(First) + 0.5;
                                            int xrate = Convert.ToInt32(gyg);
                                            ii++;
                                            if (E[0][ii]["C"] != null)
                                            {
                                                Double NotSize1 = E[0][ii]["C"];
                                                LaySize1 = Math.Round(100 / (NotSize1 - 1));
                                                if (E[0][ii]["B"] != null)
                                                {
                                                    filedALock = E[0][ii]["B"];
                                                }
                                            }
                                            if (filedBLock == true || filedALock == true)
                                            {
                                                GameStatus = "Ball Running";
                                            }
                                            else
                                            {
                                                GameStatus = "";
                                            }
                                            SelectionId = GE + "_" + Third + "_" + Second;
                                            messages.Add(item: new GetNewsData
                                            {
                                                SelectionId = SelectionId,
                                                TempId = SelectionId + xrate.ToString(),
                                                RunnerName = session_name,
                                                BackSize1 = BackSize1.ToString(),
                                                LaySize1 = LaySize1.ToString(),
                                                BackPrice1 = xrate.ToString(),
                                                LayPrice1 = xrate.ToString(),
                                                GameStatus = GameStatus
                                            });
                                        }
                                    }
                                }
                                try
                                {
                                    if (responseJson["Value"]["SG"][0]["I"] != null)
                                    {

                                        var SGisresult = responseJson["Value"]["SG"];
                                        foreach (var item in SGisresult)
                                        {

                                            if (item["EGC"] != null)
                                            {
                                                var CPS = "";
                                                var TGCPS = "";
                                                if (item["PN"] != null)
                                                {
                                                    CPS = item["PN"];
                                                    CPS = "(" + CPS + ")";
                                                    CPS = CPS.Replace("innings", "Inn");
                                                }
                                                if (item["TG"] != null)
                                                {
                                                    TGCPS = item["TG"];
                                                }
                                                else
                                                {
                                                    TGCPS = "";
                                                }
                                                if (TGCPS != "")
                                                {

                                                }

                                                else
                                                {
                                                    var EE1 = item["GE"];
                                                    foreach (var item1 in EE1)
                                                    {
                                                        string session_name = "";
                                                        string SelectionId = "";
                                                        string GameStatus = "";
                                                        GES = item1["G"];
                                                        if (GES == 537 || GES == 539 || GES == 540)
                                                        {
                                                            var E = item1["E"];
                                                            int ELength = (E[0]).Count;
                                                            for (int ii = 0; ii < ELength; ii++)
                                                            {
                                                                if (GES == 537)
                                                                {
                                                                    RunnerName1x = "(3) Over,(2) Ball [" + T1 + "]";
                                                                }
                                                                else if (GES == 539)
                                                                {
                                                                    RunnerName1x = "(3) Over,Total [" + T1 + "]";
                                                                }
                                                                else if (GES == 540)
                                                                {
                                                                    RunnerName1x = "(3) Over,Total [" + T2 + "]";
                                                                }
                                                                Boolean filedALock = false;
                                                                Boolean filedBLock = false;

                                                                Double YesSize1 = E[0][ii]["C"];
                                                                BackSize1 = Math.Floor((100 * YesSize1) - 100);
                                                                if (E[0][ii]["B"] != null)
                                                                {
                                                                    filedBLock = E[0][ii]["B"];
                                                                }
                                                                string First = "";
                                                                string Second = "";
                                                                string Third = "";
                                                                string rdataname = E[0][ii]["P"];
                                                                String startdata = rdataname.Split('.')[0];
                                                                String firstbox = rdataname.Substring(rdataname.IndexOf(".") + 1);
                                                                float FirstBox = float.Parse(firstbox) / 10;
                                                                First = FirstBox.ToString();
                                                                String thirdbox = "";
                                                                int ThirdBox = 0;
                                                                if (startdata.Length == 5)
                                                                {
                                                                    thirdbox = startdata.Substring(0, 4);
                                                                    ThirdBox = (Int32.Parse(thirdbox)) / 10;
                                                                    Third = ThirdBox.ToString();
                                                                    Second = startdata.Substring(startdata.Length - 2); //startdata.Replace(Third, "");
                                                                }
                                                                else if (startdata.Length == 4)
                                                                {
                                                                    thirdbox = startdata.Substring(0, 3);
                                                                    ThirdBox = (Int32.Parse(thirdbox)) / 10;
                                                                    Third = ThirdBox.ToString();
                                                                    Second = startdata.Substring(startdata.Length - 2); //startdata.Replace(Third, "");
                                                                }
                                                                else if (startdata.Length == 3)
                                                                {
                                                                    thirdbox = startdata.Substring(0, 2);
                                                                    ThirdBox = (Int32.Parse(thirdbox)) / 10;
                                                                    Third = ThirdBox.ToString();
                                                                    Second = startdata.Substring(startdata.Length - 2); //startdata.Replace(Third, "");
                                                                }
                                                                else
                                                                {
                                                                    Third = startdata;
                                                                    Second = "0";
                                                                }

                                                                session_name = RunnerName1x.Replace("(3)", Third);
                                                                session_name = session_name.Replace("(2)", Second);

                                                                session_name = session_name + " " + CPS;
                                                                Double gyg = Double.Parse(First) + 0.5;
                                                                int xrate = Convert.ToInt32(gyg);

                                                                if (E[1][ii]["C"] != null)
                                                                {
                                                                    Double NotSize1 = E[1][ii]["C"];
                                                                    LaySize1 = Math.Round(100 / (NotSize1 - 1));
                                                                    if (E[1][ii]["B"] != null)
                                                                    {
                                                                        filedALock = E[1][ii]["B"];
                                                                    }
                                                                }
                                                                if (filedBLock == true || filedALock == true)
                                                                {
                                                                    GameStatus = "Ball Running";
                                                                }
                                                                else
                                                                {
                                                                    GameStatus = "";
                                                                }
                                                                SelectionId = GES + "_" + Third + "_" + Second;
                                                                messages.Add(item: new GetNewsData
                                                                {
                                                                    SelectionId = SelectionId,
                                                                    TempId = SelectionId + xrate.ToString(),
                                                                    RunnerName = session_name,
                                                                    BackSize1 = BackSize1.ToString(),
                                                                    LaySize1 = LaySize1.ToString(),
                                                                    BackPrice1 = xrate.ToString(),
                                                                    LayPrice1 = xrate.ToString(),
                                                                    GameStatus = GameStatus
                                                                });
                                                            }
                                                        }
                                                        else if (GES == 538)
                                                        {
                                                            var E = item1["E"];
                                                            int ELength = (E[0]).Count;
                                                            for (int ii = 0; ii < ELength; ii++)
                                                            {
                                                                RunnerName1x = "(3) Over,(2) Ball [" + T2 + "]";
                                                                Boolean filedALock = false;
                                                                Boolean filedBLock = false;

                                                                Double YesSize1 = E[0][ii]["C"];
                                                                BackSize1 = Math.Floor((100 * YesSize1) - 100);
                                                                if (E[0][ii]["B"] != null)
                                                                {
                                                                    filedBLock = E[0][ii]["B"];
                                                                }
                                                                string First = "";
                                                                string Second = "";
                                                                string Third = "";
                                                                string rdataname = E[0][ii]["P"];
                                                                String startdata = rdataname.Split('.')[0];
                                                                String firstbox = rdataname.Substring(rdataname.IndexOf(".") + 1);
                                                                float FirstBox = float.Parse(firstbox) / 10;
                                                                First = FirstBox.ToString();
                                                                String thirdbox = "";
                                                                int ThirdBox = 0;
                                                                if (startdata.Length == 5)
                                                                {
                                                                    thirdbox = startdata.Substring(0, 4);
                                                                    ThirdBox = (Int32.Parse(thirdbox)) / 10;
                                                                    Third = ThirdBox.ToString();
                                                                    Second = startdata.Substring(startdata.Length - 2); //startdata.Replace(Third, "");
                                                                }
                                                                else if (startdata.Length == 4)
                                                                {
                                                                    thirdbox = startdata.Substring(0, 3);
                                                                    ThirdBox = (Int32.Parse(thirdbox)) / 10;
                                                                    Third = ThirdBox.ToString();
                                                                    Second = startdata.Substring(startdata.Length - 2); //startdata.Replace(Third, "");
                                                                }
                                                                else if (startdata.Length == 3)
                                                                {
                                                                    thirdbox = startdata.Substring(0, 2);
                                                                    ThirdBox = (Int32.Parse(thirdbox)) / 10;
                                                                    Third = ThirdBox.ToString();
                                                                    Second = startdata.Substring(startdata.Length - 2); //startdata.Replace(Third, "");
                                                                }
                                                                else
                                                                {
                                                                    Third = startdata;
                                                                    Second = "0";
                                                                }

                                                                session_name = RunnerName1x.Replace("(3)", Third);
                                                                session_name = session_name.Replace("(2)", Second);

                                                                session_name = session_name + " " + CPS;
                                                                Double gyg = Double.Parse(First) + 0.5;
                                                                int xrate = Convert.ToInt32(gyg);
                                                                ii++;
                                                                if (E[0][ii]["C"] != null)
                                                                {
                                                                    Double NotSize1 = E[0][ii]["C"];
                                                                    LaySize1 = Math.Round(100 / (NotSize1 - 1));
                                                                    if (E[0][ii]["B"] != null)
                                                                    {
                                                                        filedALock = E[0][ii]["B"];
                                                                    }
                                                                }
                                                                if (filedBLock == true || filedALock == true)
                                                                {
                                                                    GameStatus = "Ball Running";
                                                                }
                                                                else
                                                                {
                                                                    GameStatus = "";
                                                                }
                                                                SelectionId = GES + "_" + Third + "_" + Second;
                                                                messages.Add(item: new GetNewsData
                                                                {
                                                                    SelectionId = SelectionId,
                                                                    TempId = SelectionId + xrate.ToString(),
                                                                    RunnerName = session_name,
                                                                    BackSize1 = BackSize1.ToString(),
                                                                    LaySize1 = LaySize1.ToString(),
                                                                    BackPrice1 = xrate.ToString(),
                                                                    LayPrice1 = xrate.ToString(),
                                                                    GameStatus = GameStatus
                                                                });
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception e)
                                {

                                }
                            }
                            else
                            {
                                foreach (var item in isresult)
                                {
                                    string session_name = "";
                                    string SelectionId = "";
                                    string GameStatus = "";
                                    GE = item["G"];
                                    if (GE == 6 || GE == 5 || GE == 4)
                                    {
                                        var E = item["E"];
                                        int ELength = (E[0]).Count;
                                        for (int ii = 0; ii < ELength; ii++)
                                        {
                                            if (SI == "1")
                                            {
                                                if (GE == 6)
                                                {
                                                    session_name = "Total Goals [" + T2 + "]";
                                                }
                                                else if (GE == 5)
                                                {
                                                    session_name = "Total Goals [" + T1 + "]";
                                                }
                                                else if (GE == 4)
                                                {
                                                    session_name = "Total Goals";
                                                }
                                            }
                                            else
                                            {
                                                if (GE == 6)
                                                {
                                                    session_name = "Total Point [" + T2 + "]";
                                                }
                                                else if (GE == 5)
                                                {
                                                    session_name = "Total Point [" + T1 + "]";
                                                }
                                                else if (GE == 4)
                                                {
                                                    session_name = "Total Point";
                                                }
                                            }

                                            Boolean filedALock = false;
                                            Boolean filedBLock = false;

                                            Double YesSize1 = E[0][ii]["C"];
                                            BackSize1 = Math.Floor((100 * YesSize1) - 100);
                                            if (E[0][ii]["B"] != null)
                                            {
                                                filedBLock = E[0][ii]["B"];
                                            }
                                            string rdataname = E[0][ii]["P"];
                                            if (rdataname.Contains(".5"))
                                            {
                                                Double gyg = Double.Parse(rdataname) + 0.5;
                                                int xrate = Convert.ToInt32(gyg);

                                                if (E[1][ii]["C"] != null)
                                                {
                                                    Double NotSize1 = E[1][ii]["C"];
                                                    LaySize1 = Math.Round(100 / (NotSize1 - 1));
                                                    if (E[1][ii]["B"] != null)
                                                    {
                                                        filedALock = E[1][ii]["B"];
                                                    }
                                                }
                                                if (filedBLock == true || filedALock == true)
                                                {
                                                    GameStatus = "SUSPENDED";
                                                }
                                                else
                                                {
                                                    GameStatus = "";
                                                }
                                                SelectionId = GE + "_" + xrate.ToString();
                                                messages.Add(item: new GetNewsData
                                                {
                                                    SelectionId = SelectionId,
                                                    TempId = SelectionId,
                                                    RunnerName = session_name,
                                                    BackSize1 = BackSize1.ToString(),
                                                    LaySize1 = LaySize1.ToString(),
                                                    BackPrice1 = xrate.ToString(),
                                                    LayPrice1 = xrate.ToString(),
                                                    GameStatus = GameStatus
                                                });
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

            return messages;
        }

        public ActionResult criclive()
        {
            String eventcode = Request.QueryString["eventcode"];
            string tv_hdmi = GetTV_hdmi(eventcode);
            ViewBag.tv_hdmi = tv_hdmi;
            return View();
        }

        public ActionResult add_tv()
        {
            if (Request["tv_id"] != null && Request["cric_id"] != null && Request["ev_id"] != null)
            {
                string ev_id = Request["ev_id"];
                string tv_id = Request["tv_id"];
                string cric_id = Request["cric_id"];
                if (tv_id != null && cric_id != null && ev_id != null && tv_id != "" && cric_id != "" && ev_id != "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        SqlCommand update_hash = new SqlCommand("UPDATE matches set tv_ch='" + tv_id + "' , cric_id='" + cric_id + "' where event_code='" + ev_id + "'", con);
                        int vtvt = update_hash.ExecuteNonQuery();
                        if (vtvt == 1)
                        {
                            con.Close();
                            ViewBag.resda = "success";
                        }
                        else
                        {
                            con.Close();
                            ViewBag.resda = "error";
                        }
                    }
                }
                else
                {
                    ViewBag.resda = "error";
                }
            }
            else
            {
                ViewBag.resda = "";
            }
            return View();
        }

        public ActionResult check_tv()
        {
            String eventcode = Request.QueryString["eventcode"];
            //string tv_hdmi = GetTV_hdmi(eventcode);
            ViewBag.tv_hdmi = eventcode;
            return View();
        }
        public String GetTV_hdmi(string eventcode)
        {

            string tv_ch = "HDMI21";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT tv_ch FROM matches where event_code= '" + eventcode + "' OR cric_id= '" + eventcode + "' "))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        tv_ch = (string)reader["tv_ch"];
                        con.Close();
                        return tv_ch;
                    }
                }
            }
            return tv_ch;
        }

        public ActionResult DragonT20(EventIdSend eventIdSend, string sportid, string leagueid)
        {
            if (check_hash_key != null && check_hash_key != "")
            {
                CULL();
            }
            string Event_Code_Id = sportid;
            ViewBag.EventCode = Event_Code_Id;
            ViewBag.BetfairId = Event_Code_Id;
            return View();
        }

        public ActionResult DragonT20Lion(EventIdSend eventIdSend, string sportid, string leagueid)
        {
            if (check_hash_key != null && check_hash_key != "")
            {
                CULL();
            }
            string Event_Code_Id = sportid;
            ViewBag.EventCode = Event_Code_Id;
            ViewBag.BetfairId = Event_Code_Id;
            return View();
        }

        public ActionResult Lucky7(EventIdSend eventIdSend, string sportid, string leagueid)
        {
            if (check_hash_key != null && check_hash_key != "")
            {
                CULL();
            }
            string Event_Code_Id = sportid;
            ViewBag.EventCode = Event_Code_Id;
            ViewBag.BetfairId = Event_Code_Id;
            return View();
        }

        public ActionResult FancyLucky7(EventIdSend eventIdSend, string sportid, string leagueid)
        {
            if (check_hash_key != null && check_hash_key != "")
            {
                CULL();
            }
            string Event_Code_Id = sportid;
            ViewBag.EventCode = Event_Code_Id;
            ViewBag.BetfairId = Event_Code_Id;
            return View();
        }

        public ActionResult Lucky7B(EventIdSend eventIdSend, string sportid, string leagueid)
        {
            if (check_hash_key != null && check_hash_key != "")
            {
                CULL();
            }
            string Event_Code_Id = sportid;
            ViewBag.EventCode = Event_Code_Id;
            ViewBag.BetfairId = Event_Code_Id;
            return View();
        }

        public ActionResult PokerT20(EventIdSend eventIdSend, string sportid, string leagueid)
        {
            if (check_hash_key != null && check_hash_key != "")
            {
                CULL();
            }
            string Event_Code_Id = sportid;
            ViewBag.EventCode = Event_Code_Id;
            ViewBag.BetfairId = Event_Code_Id;
            return View();
        }
        public ActionResult LTTeen(EventIdSend eventIdSend, string sportid, string leagueid)
        {
            if (check_hash_key != null && check_hash_key != "")
            {
                CULL();
            }
            string Event_Code_Id = sportid;
            ViewBag.EventCode = Event_Code_Id;
            ViewBag.BetfairId = Event_Code_Id;
            return View();
        }

        public ActionResult OneDayTP(EventIdSend eventIdSend, string sportid, string leagueid)
        {
            if (check_hash_key != null && check_hash_key != "")
            {
                CULL();
            }
            string Event_Code_Id = sportid;
            ViewBag.EventCode = Event_Code_Id;
            ViewBag.BetfairId = Event_Code_Id;
            return View();
        }

        public ActionResult OneDayDragonTiger(EventIdSend eventIdSend, string sportid, string leagueid)
        {
            if (check_hash_key != null && check_hash_key != "")
            {
                CULL();
            }
            string Event_Code_Id = sportid;
            ViewBag.EventCode = Event_Code_Id;
            ViewBag.BetfairId = Event_Code_Id;
            return View();
        }

        public JsonResult GetXScore(string xid, string type)
        {
            List<XScore> messages = new List<XScore>();
            messages = GetXScore1(xid, type);
            return Json(messages, JsonRequestBehavior.AllowGet);

        }

        public List<XScore> GetXScore1(string xid, string type)
        {
            var messages = new List<XScore>();
            String T1_name = "";
            String T2_name = "";
            String rb = "";
            String status = "false";
            string T1 = "";
            string T2 = "";
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://1xbet.mobi/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync(type + "/GetGameZip?id=" + xid + "&lng=en&cfview=0&isSubGames=true&GroupEvents=true&countevents=600&grMode=2").Result;  // Blocking call! 
                    if (response != null)
                    {
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        var isresult = responseJson["Value"]["GE"];
                        if (responseJson["Value"]["I"] != null)
                        {
                            T1_name = responseJson["Value"]["O1"];
                            T2_name = responseJson["Value"]["O2"];
                            var SC1 = responseJson["Value"]["SC"];
                            var SC = responseJson["Value"]["SC"]["S"];
                            string gi = SC1["I"];
                            if (gi != null)
                            {
                            }
                            else
                            {
                                String Key1 = SC[0]["Key"];
                                String Key2 = SC[1]["Key"];
                                String Value1 = SC[0]["Value"];
                                String Value2 = SC[1]["Value"];
                                status = "true";
                                if (Key1 == "Team1Scores")
                                {
                                    T1 = Value1;
                                }
                                if (Key2 == "Team2Scores")
                                {
                                    T2 = Value2;
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
            messages.Add(item: new XScore
            {
                status = status,
                T1 = T1,
                T2 = T2,
                T1_name = T1_name,
                T2_name = T2_name,
                rb = rb
            });

            return messages;
        }

        public ActionResult LiveLive(string sportid)
        {
            string event_code = sportid;
            string clientt_id = (string)System.Web.HttpContext.Current.Session["UIDS"];
            string hashh_id = (string)System.Web.HttpContext.Current.Session["hash_keyS"];
            string hi = "";
            string video_l = "";
            if (hashh_id != null && hashh_id != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT id from users_client where hash_key='" + hashh_id + "' AND id='" + clientt_id + "' ", con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            using (var cmd2 = new SqlCommand("SELECT video_l from matches where event_code='" + event_code + "' AND status='OPEN' ", con))
                            {
                                var reader2 = cmd2.ExecuteReader();
                                if (reader2.HasRows)
                                {
                                    reader2.Read();
                                    video_l = (string)reader2["video_l"];
                                }
                            }
                        }
                        con.Close();
                    }
                }
            }
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
            {
                hi = "mobile";
            }
            else
            {
                hi = "desktop";
            }

            if (hi == "mobile")
            {
                ViewBag.video_l = video_l;
            }
            else
            {
                ViewBag.video_l = "";
            }
            return View();
        }

        public string NewsLine()
        {
            string news = "";
            /* using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
             {
                 using (var cmd = new SqlCommand("select news from news_line where site='probet' ", con))
                 {
                     cmd.Connection = con;
                     con.Open();
                     var data = cmd.ExecuteReader();
                     if (data.HasRows)
                     {
                         data.Read();
                         news = (string)data["news"];
                     }
                     else
                     {
                     }
                     con.Close();
                 }
             }*/
            return news;
        }

        public ActionResult ttt20()
        {
            return View();
        }
        public ActionResult ttt10()
        {
            return View();
        }
        public ActionResult luvk7()
        {
            return View();
        }

        public string LOdas()
        {
            var Data = new List<Data>();
            var client = new RestClient("http://109.74.206.131:3333/api/v1/match/match-details");
            var request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "87293ced-1a21-dd20-2281-6881ed9b8531");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOjU5MywiZGF0YSI6eyJpZCI6NTkzLCJvcmdfaWQiOjU5Mywicm9sZV9pZCI6MCwicm9sZXMiOltdLCJsb2dpbl9uYW1lIjoibW1leGNoIiwibmFtZSI6ImRlbW8iLCJ1c2VyX3R5cGVfaWQiOjUsIm9yZ191c2VyX3R5cGVfaWQiOjUsInBhcmVudF9pZCI6NTkyLCJwYXJlbnRfaWRzIjoiMSwyLDI0NSwsNTkyIiwiYWdlbnRfaWQiOjU5MiwiYWdlbnRfbmFtZSI6Im1vbmV5OCIsInN1cGVyX2FnZW50X2lkIjpudWxsLCJzdXBlcl9hZ2VudF9uYW1lIjpudWxsLCJtYXN0ZXJfaWQiOjI0NSwibWFzdGVyX25hbWUiOiJzYWxhc2FyIiwiYWRtaW5faWQiOjIsImFkbWluX25hbWUiOiJhZG1pbiIsInN1cGVyX2FkbWluX2lkIjoxLCJzdXBlcl9hZG1pbl9uYW1lIjoic3VwZXIgQWRtaW4iLCJsYXN0X2xvZ2luX2RhdGUiOiIyMDIwLTEwLTE1VDA2OjMzOjQzLjg0MFoifSwiaWF0IjoxNjAyNzQzNjIzLCJleHAiOjE2MDI3NjUyMjN9.WDKwC-fUYG3of20ZNKLIM1ezwZ9BBw3UZaX_cjUD9-w");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"match_id\": \"-6\"}", ParameterType.RequestBody);
            var response = client.Execute(request);
            string data = response.Content;

            JObject jObject = JObject.Parse(data);
            string distance = jObject.SelectToken("data.markets[0].runners[0].ex.availableToBack[0].price").ToString();
           /* string data = response.Content;
            Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            object test;
            string tt1 = "lodaaa";
            if (values.TryGetValue("data", out test)) // Returns true.
            {
                tt1 = (string)test;
                Console.WriteLine(test); // This is the value at key1.
            }*/



            return "LOdda__ "+distance;
        }

        public JsonResult SessionDatasAView(string Event_Code)
        {

            var client = new RestClient("http://lotusbook247");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "a7e1f24b-55be-1939-5163-f3118978a660");
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);

            /*  var products = "";
              try
              {
                  HttpClient client = new HttpClient();
                  client.BaseAddress = new Uri("http://bet-fair.co/exchange/");
                  client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                  try
                  {
                      HttpResponseMessage response = client.GetAsync("GetJavanScriptString?data=1.176086802&type=LiveFeed").Result;  // Blocking call! 

                      products = response.Content.ReadAsStringAsync().Result;
                  }
                  catch (Exception ex)
                  {

                  }
              }
              catch (TaskCanceledException ex)
              {

              }*/
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult hgdfjhf()
        {
            var client = new RestClient("https://bxawscf.skyexchange.com/exchange/member/playerService/queryFullMarkets");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("sec-ch-ua", "\"Google Chrome\";v=\"87\", \" Not;A Brand\";v=\"99\", \"Chromium\";v=\"87\"");
            request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
            request.AddHeader("Cookie", "_ga=GA1.2.208298303.1607411342; intercom-id-sdicqbx3=c185d2f2-1ed0-4457-8d09-82c717b96e5d; load_balancer=a0ce3cad-809f-468a-ab8c-74c2f427fc4d; visid_incap_1554243=kuhPWdW1TKWnHp6PcnCkVEZ32F8AAAAAQUIPAAAAAADrQc6pb4LuMFMZdPL7XLth; nlbi_1554243=IQFQI3QQAXDA+a2eq7bknwAAAACyOl02JzqtmdCRxZeWfEE5; website=SKYEXCHANGE; lang=en; _gid=GA1.2.982887612.1612530461; intercom-session-sdicqbx3=; incap_ses_746_1554243=ZuGPCFzaywL93sEJS1NaCgtcHmAAAAAAgOw2MdF7MxJrya2qV5lzTw==; JSESSIONID=54EAB3B4B79AFDD76D6C912ECBD094EA.player09; AWSELB=F32D73BD1ABA44844673992FB2E7A68D3F75DB07B5D39E4CFD043731A24288B5BA4FB4A0D02532F1206E7AFE24255BE3E190D3DBB34B7A9089F83E41B9138290E542F7AB9852A23AD07C31635ED82C47268760290B; incap_ses_737_1554243=oe59H9RXfEI7QjOpr1k6CmQAIGAAAAAA+xKrSp4pU4zIY1+rDWb4Tw==");
            request.AddParameter("eventId", "30194778");
            request.AddParameter("isGetRunnerMetadata", "true");
            request.AddParameter("marketId", "1.177132011");
            request.AddParameter("selectionTs", "-1");
            IRestResponse response = client.Execute(request);
            string hjdfg = response.Content;
            string hjdfgh = "dfhjhgf";


            return Json(response.Content, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SkyLelo()
        {
            var client = new RestClient("https://bxawscf.skyexchange.com/exchange/member/playerService/queryFullMarkets");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("sec-ch-ua", "\"Chromium\";v=\"88\", \"Google Chrome\";v=\"88\", \";Not A Brand\";v=\"99\"");
            request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.104 Safari/537.36";
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
            request.AddHeader("Cookie", "AWSELB=F32D73BD1ABA44844673992FB2E7A68D3F75DB07B5F657990C61C95F43D64876F52C020B62CB2D3E895136495DAE12ABC7C4A142144B7A9089F83E41B9138290E542F7AB986FCC8843517C343E1CFE22F5EE50F70F; JSESSIONID=8BE3E03062659E7E8A7B91AF5A1C4721.player12; betSlips_johndemo01=%5B%5D; filterSports_=%5B0%2C1%2C2%2C4%2C5%2C6%2C7%2C8%2C11%2C1477%2C3503%2C3988%2C4339%2C6231%2C6422%2C6423%2C7511%2C7522%2C7524%2C61420%2C136332%2C315220%2C606611%2C2152880%2C2378961%2C26420387%2C27454571%2C2378962%2C9999999%5D; filterSports_johndemo01=%5B0%2C1%2C2%2C4%2C5%2C6%2C7%2C8%2C11%2C1477%2C3503%2C3988%2C4339%2C6231%2C6422%2C6423%2C7511%2C7522%2C7524%2C61420%2C136332%2C315220%2C606611%2C2152880%2C2378961%2C26420387%2C27454571%2C2378962%2C9999999%5D; uiSelect_=%7B%22act%22%3A%22selectMarket%22%2C%22eventType%22%3A%224%22%2C%22eventId%22%3A%2230265733%22%2C%22marketId%22%3A%221.178784589%22%7D; uiSelect_johndemo01=%7B%22act%22%3A%22selectMarket%22%2C%22eventType%22%3A%224%22%2C%22eventId%22%3A%2230265733%22%2C%22marketId%22%3A%221.178784589%22%7D");
            request.AddParameter("eventId", "30233775");
            request.AddParameter("isGetRunnerMetadata", "true");
            request.AddParameter("marketId", "1.178000173");
            request.AddParameter("queryPass", "B768090246722E1C4F67F700C8C2B835.player09");
            request.AddParameter("selectionTs", "-1");
            IRestResponse response = client.Execute(request);
            string hjdfg = response.Content;
            string hjdfgh = "dfhjhgf";


            return Json(response.Content, JsonRequestBehavior.AllowGet);

            //Console.WriteLine(response.Content);
        }

        public JsonResult SkyFancyS()
        {
            var client = new RestClient("http://64.227.40.177:3333/api/v1/match/match-details");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json, text/plain, */*");
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOjQwMTQsImRhdGEiOnsiaWQiOjQwMTQsIm9yZ19pZCI6NDAxNCwicm9sZV9pZCI6MCwicm9sZXMiOltdLCJsb2dpbl9uYW1lIjoiTUFIIiwibmFtZSI6Ik1BSCIsInVzZXJfdHlwZV9pZCI6NSwib3JnX3VzZXJfdHlwZV9pZCI6NSwicGFyZW50X2lkIjozNjE1LCJwYXJlbnRfaWRzIjoiMSwyLDM2MDgsMzYxMSwzNjE1IiwiYWdlbnRfaWQiOjM2MTUsImFnZW50X25hbWUiOiJLcmlzaG5hIiwic3VwZXJfYWdlbnRfaWQiOjM2MTEsInN1cGVyX2FnZW50X25hbWUiOiJTaHJlZXNoeWFtMDEiLCJtYXN0ZXJfaWQiOjM2MDgsIm1hc3Rlcl9uYW1lIjoiU2hyZWVzaHlhbSIsImFkbWluX2lkIjoyLCJhZG1pbl9uYW1lIjoiYWRtaW4iLCJzdXBlcl9hZG1pbl9pZCI6MSwic3VwZXJfYWRtaW5fbmFtZSI6InN1cGVyIEFkbWluIiwibGFzdF9sb2dpbl9kYXRlIjoiMjAyMS0wNC0xMVQxNjowMToxNy41MDJaIn0sImlhdCI6MTYxODE1Njg3NywiZXhwIjoxNjE4MTc4NDc3fQ.BmQP7Nwb-7gWR9FD3CWytlTNUMAzI6_Fhjlqsmkjlgk");
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.114 Safari/537.36";
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "XSRF-TOKEN=790057e318a933e1e77752965c63e3a6SMQHR3O8dXbR6vchRj7AzmfhjAZxq1McQD4NLwe92NwlA1YWOagISLMMb0W3lKAO34e1sRVUFIFRGeaEmu%2BrUuD5jcCpqBHmeHxYddDcUDTYXoZ0Bbg0zX6kwpqd9c3r; adonis-session=dfae7be0cc20dcabb4ae4c7369192235NKOv3bnFbw0UtOAIJUQL8hpqZEvBa4D6vpYCX3nfvI6vUe63j6jnlqasds5IchOLF9VllO5p6FI9JxdeA6javVfqh7BoEY%2BpW2qlLDc2kgd3GGuTk1xM4OQNCMdX8Pm2; adonis-session-values=b33ae279cca89ad480de1277fb132682LKKhUoDEN4nS5abAQKmSg2lYkVhhxHdZ6u7vAfqvS0MBvLezhVvgsUlsE4dsomqYYdbNnhR2Vc30IfOCrbabB4qnFRo4QAbf7Akv8mc3pPvj0DXDYgD69Fzljekb4en%2FDgy5YqQx6nA6a2xYA5Pt4o4O%2F%2By%2BHZDAMO%2B10tVqfNM%3D");
            request.AddParameter("application/json", "{\"match_id\":\"30337488\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return Json(response.Content, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SkyTVVV()
        {
            var client = new RestClient("https://innohls.s.llnwi.net/live/m1028/playlist.m3u8?e=1612713589629&ip=106.207.146.226&h=a754c0c2e4c86e2daa5c83235be0ab84&uid=johndemo01");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("sec-ch-ua", "\"Chromium\";v=\"88\", \"Google Chrome\";v=\"88\", \";Not A Brand\";v=\"99\"");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.104 Safari/537.36";
            request.AddHeader("Accept", "*/*");
            IRestResponse response = client.Execute(request);
            return Json(response.Content, JsonRequestBehavior.AllowGet);
            Console.WriteLine(response.Content);
        }


        public JsonResult Love()
        {
            /* var accessToken = GetAccessToken();  // Does what ever is required to get the acces token
             Authenticator = new JwtAuthenticator(accessToken);

 */
/*
            var tokenString = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Ik1uQ19WWmNBVGZNNXBPWWlKSE1iYTlnb0VLWSJ9.eyJhdWQiOiIwMDAwMDAwMC0wMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDAiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8wMDAwMDAwMC0wMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDAiLCJpYXQiOiIxNDI4MDM2NTM5IiwibmJmIjoiMTQyODAzNjUzOSIsImV4cCI6IjE0MjgwNDA0MzkiLCJ2ZXIiOiIxLjAiLCJ0aWQiOiIwMDAwMDAwMC0wMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDAiLCJhbXIiOiJwd2QiLCJvaWQiOiIwMDAwMDAwMC0wMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDAiLCJlbWFpbCI6Impkb2VAbGl2ZS5jb20iLCJwdWlkIjoiSm9obiBEb2UiLCJpZHAiOiJsaXZlLmNvbSIsImFsdHNlY2lkIjoiMTpsaXZlLmNvbTowMDAwMDAwMDAwMDAwMDAwIiwic3ViIjoieHh4eHh4eHh4eHh4eHh4eC15eXl5eSIsImdpdmVuX25hbWUiOiJKb2huIiwiZmFtaWx5X25hbWUiOiJEb2UiLCJuYW1lIjoiSm9obiBEb2UiLCJncm91cHMiOiIwMDAwMDAwMC0wMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDAiLCJ1bmlxdWVfbmFtZSI6ImxpdmUuY29tI2pkb2VAbGl2ZS5jb20iLCJhcHBpZCI6IjAwMDAwMDAwLTAwMDAtMDAwMC0wMDAwLTAwMDAwMDAwMDAwMCIsImFwcGlkYWNyIjoiMCIsInNjcCI6InVzZXJfaW1wZXJzb25hdGlvbiIsImFjciI6IjEifQ.K7BCa0NO-A5f9exFiWcIXFMGnLmmt3V2HVP0itMT-GsAxnQROWzJFDIQNFo4QhiW0NCCqJykVELeVBCy_7Dex2-szUPZ69rmmDVJhy_qkmAiHhS1mNZDvJ1sB-whb5wOJ_QPIlByVzubhTcNnuliTVjnTeuOurVJJcn0Vugx9UDkGgky0etHXzmKukWYp4nzA68Wf1xnzlMZBz7PfoPGhjgzQfceOkZJVXIBRMB_7tsyW7gYNbHB_aTiT47cEjkh-UdrZEdp2UaAKugC-es3m076kRHMJqx31x-zDLDBttKinRJVPctiqwb1jMOMV6cUAp2E6aMfEbNk_iqX_OKFJg";

            var jwtEncodedString = tokenString.Substring(7); // trim 'Bearer ' from the start since its just a prefix for the token string

            var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);
            Console.WriteLine("email => " + token.Claims.First(c => c.Type == "email").Value);*/

            /*new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMinutes(90),
                ....);*/

            var client = new RestClient("https://lotusbook247.games/api/exchange/odds/event/1444001/56767");
            client.Timeout = -1;
          /*  string s = Convert.ToBase64String(Encoding.ASCII.GetBytes(creds.appID + ":" + creds.sharedSecret));
            DateTime refreshTokenLifeTime = context. OwinContext.Get<DateTime>("as:clientRefreshTokenLifeTime");*/
            var request = new RestRequest(Method.GET);
            request.AddHeader("X-Client-Info", "c1878c78bd8eb5cf16b20037e582478e");
            request.AddHeader("X-App-Version", "4.0.19.2");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("Authorization", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJtZW1iZXJDb2RlIjoiQkwwMTAxMDFNMDMiLCJ0b2tlbklkIjoiNmMyZGFiOTkxNDNkY2IyM2E4OGI0OThhZDk4NjFhYmIzOWRlNzdiYzdhYmZiYTNkZTkwMGY0M2ExMWJkNzdjNCIsImxvZ2luQ291bnRyeSI6IklOIiwic2Vzc2lvbklkIjoiYzlhNDY5ZmE1YWFjNjJmNjM2NzcyMTM3NmVjYThmYTQ2MTU5YTBjOWFhYmQ5ZGIxZjBjMjk4MWYwM2JhNjUzMSIsImFsbG93U2hha3RpUHJvIjpmYWxzZSwibGFzdExvZ2luVGltZSI6MTYwODg5NzQ5MDA3NiwibmJmIjoxNjA4OTA0OTA5LCJsb2dpbk5hbWUiOiJ6b21pIiwibG9naW5JUCI6IjEwNi4yMDcuMTUyLjExOCIsInRoZW1lIjoibG90dXMiLCJleHAiOjE2MDg5MTAwNjgsImlhdCI6MTYwODkwNDkwOSwibWVtYmVySWQiOjExNTcxMywidXBsaW5lcyI6eyJDT1kiOnsidXNlcklkIjoxLCJ1c2VyQ29kZSI6ImFkbWluLnVzZXIifSwiU01BIjp7InVzZXJJZCI6MTE1NzA0LCJ1c2VyQ29kZSI6IkJMMDEifSwiTUEiOnsidXNlcklkIjoxMTU3MDUsInVzZXJDb2RlIjoiQkwwMTAxIn0sIkFnZW50Ijp7InVzZXJJZCI6MTE1NzA2LCJ1c2VyQ29kZSI6IkJMMDEwMTAxIn19LCJjdXJyZW5jeSI6IklOUiIsImp0aSI6IjZjNWUxYTQ2LWZiYmQtNGM0MC1hMmQxLWU0NzI2ODFiZmE3NCJ9.yvxBYavcFV6a3gJHKfDGz038FQL8rLd_8H92ELu94SE");
            request.AddHeader("X-xid", "d87d23aa-4ee9-2992-2744-f41c8fddd1ad");
            request.AddHeader("content-type", "text/plain");
            request.AddHeader("Accept", "application/json, text/plain, */*");
            request.AddHeader("X-Client-Id", "1399978428.1608715557");
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
            request.AddHeader("X-Client", "mobile");
            request.AddHeader("X-User-Id", "BL010101M03");
            request.AddHeader("sec-ch-ua", "\"Google Chrome\";v=\"87\", \" Not;A Brand\";v=\"99\", \"Chromium\";v=\"87\"");
            IRestResponse response = client.Execute(request);
            return Json(response.Content, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LODHJHG()
        {
            var client = new RestClient("http://lotusbook247./M/oddsC?id=30303030");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
            client.UserAgent = "Mozilla/5.0 (Linux; Android 5.0; SM-G900P Build/LRX21T) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Mobile Safari/537.36";
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            IRestResponse response = client.Execute(request);
            return Json(response.Content, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LOve1()
        {
            var client = new RestClient("https://betexch.uk/api/v1/matchDetails");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("sec-ch-ua", "\"Google Chrome\";v=\"87\", \" Not;A Brand\";v=\"99\", \"Chromium\";v=\"87\"");
            request.AddHeader("Accept", "application/json, text/plain, */*");
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOnsiaWQiOjE5MjcsInJvbGUiOiJBZG1pblRlc3QiLCJwYXJlbnRfaWQiOiIxODU5LDE4NTgsMTg1NywxLDAiLCJ1c2VyX3R5cGVfaWQiOjV9LCJpYXQiOjE2MDkxNDkyMjAsImV4cCI6MTYxMDAxMzIyMH0.apWlBy91OZPGvWOPGzIBUNM6IQ6lIJ47QdXDsm37iyo");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\"match_id\":\"56767\",\"market_id\":\"-100.56767\",\"user_id\":1864,\"user_type_id\":5}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string data = response.Content;
            JObject jObject = JObject.Parse(data);
            string c1 = jObject.SelectToken("data.match[0].runners[0].cards").ToString();
            string c2 = jObject.SelectToken("data.match[0].runners[1].cards").ToString();
            string fvhbgfh = jObject.SelectToken("data.match[0].runners[0].lay").ToString();
            if (fvhbgfh.Contains("price")) 
            {
                string distance = jObject.SelectToken("data.match[0].runners[0].lay[0].price").ToString();

            }
            return Json(response.Content, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Lodaskj()
        {
            var client = new RestClient("https://lotusbook247.com/api/exchange/odds/sma-event/4/30411027");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("X-Client-Info", "ceb5818bcb4bec0951a0fb4f0319fb16");
            request.AddHeader("X-App-Version", "4.0.21.0");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("Authorization", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJtZW1iZXJDb2RlIjoiOUkwMzAyMDZNMDEiLCJ0b2tlbklkIjoiZjNkZDVjZTM5YjlkZjBiNzU2ZGNmMGUzZTg3OTE3MWJkM2ZhMmZkYzRhODkxNzg5OWNmODc4NzBlNTBlYzk2MCIsImxvZ2luQ291bnRyeSI6IklOIiwic2Vzc2lvbklkIjoiYWFjOGQ1NjkxNjI5YjY1Y2JlODNhYWIyY2IzNGQzYjA1YWU2YmI5ZGNlZTY2ZjZhYzZkMzQ2YTMyNTgwZDE0NyIsImFsbG93U2hha3RpUHJvIjpmYWxzZSwibGFzdExvZ2luVGltZSI6MTYxODA1NzIyODEyMiwibmJmIjoxNjE4MDU3MjczLCJsb2dpbk5hbWUiOiJyYWF2aTk4MyIsImxvZ2luSVAiOiIxMDYuMjA3LjEzOC4xMzIiLCJ0aGVtZSI6ImxvdHVzIiwiZXhwIjoxNjE4MDYwODczLCJpYXQiOjE2MTgwNTcyNzMsIm1lbWJlcklkIjoyMTA4MjIsInVwbGluZXMiOnsiQ09ZIjp7InVzZXJJZCI6MSwidXNlckNvZGUiOiJhZG1pbi51c2VyIn0sIlNNQSI6eyJ1c2VySWQiOjEyNDI2MiwidXNlckNvZGUiOiI5STAzIn0sIk1BIjp7InVzZXJJZCI6MTYxNzU4LCJ1c2VyQ29kZSI6IjlJMDMwMiJ9LCJBZ2VudCI6eyJ1c2VySWQiOjE4NjExOCwidXNlckNvZGUiOiI5STAzMDIwNiJ9fSwiY3VycmVuY3kiOiJJTlIifQ.ncuFn2r09dvI9RQbpwLY0BUCcu2_RCRFVTUd1EyUDtE");
            request.AddHeader("X-xid", "8c993df3-f5f6-471b-b2da-f39673fccfb0");
            request.AddHeader("Accept", "application/json, text/plain, */*");
            request.AddHeader("X-Client-Id", "1676362581.1613557370");
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.114 Safari/537.36";
            request.AddHeader("X-Client", "desktop");
            request.AddHeader("X-User-Id", "9I030206M01");
            request.AddHeader("sec-ch-ua", "\"Google Chrome\";v=\"89\", \"Chromium\";v=\"89\", \";Not A Brand\";v=\"99\"");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return Json(response.Content, JsonRequestBehavior.AllowGet);
        }

    }
}