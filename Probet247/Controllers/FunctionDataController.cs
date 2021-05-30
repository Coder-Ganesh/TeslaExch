using Newtonsoft.Json;
using Probet247.Models;
using RBetfair.Models;
using System;
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
    public class FunctionDataController : Controller
    {

        // GET: FunctionData
        public ActionResult Index()
        {
            return View();
        }

        static public string getSportID(string event_code)
        {
            string sport_id = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT sport_id FROM matches where event_code= '" + event_code + "' ", con))
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    sport_id = (string)reader["sport_id"];
                }
                con.Close();
            }
            return sport_id;
        }

        static public string GetSportsNameFromEvent(string event_code)
        {
            string Sport_Name = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                string sportid = getSportID(event_code);
                using (var cmd = new SqlCommand("SELECT sport_name FROM sports Where sport_id='" + sportid + "'", con))
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    Sport_Name = (string)reader["sport_name"];
                }
                con.Close();
            }
            return Sport_Name;
        }

        static public string getEventName(string EveCodeM)
        {
            string MAtchTG = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT match_title FROM matches where event_code= '" + EveCodeM + "' "))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        MAtchTG = (string)reader["match_title"];
                    }
                    con.Close();
                }
            }
            return MAtchTG;
        }

        public JsonResult UserInformationFun1()
        {

            List<UserBalance> messages = new List<UserBalance>();
            messages = UserInformationFun();
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<UserBalance> UserInformationFun()
        {
            string uid = (string)System.Web.HttpContext.Current.Session["UIDS"];
            var NewUserModel = new List<UserBalance>();
            try
            {
                using (SqlConnection con1 = ConnectionHandler.Connection())
                {
                    con1.Open();
                    SqlCommand com = new SqlCommand("userInfo", con1);
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add("@userid", System.Data.SqlDbType.NVarChar).Value = uid;
                    using (com)
                    {
                        com.ExecuteScalar();
                        var reader1 = com.ExecuteReader();
                        while (reader1.Read())
                        {
                            Double balance = Math.Round((Double)reader1["balance"], 2);
                            Double exposure = Math.Round((Double)reader1["exposure"], 2);
                            string Uusername = (string)reader1["username"];
                            string UFullusername = (string)reader1["name"];
                            NewUserModel.Add(item: new UserBalance
                            {
                                UBalance = balance,
                                UExposure = exposure,
                                UserName = Uusername,
                                FUserName = UFullusername
                            });
                        }
                    }
                    con1.Close();
                }
            }
            catch (Exception)
            {
            }
            return NewUserModel;
        }

        public string MClientChangePasswordDB(string user_id, string newPassword, string changePassword)
        {
            string ReturnMessage = "";
            try
            {
                if (user_id != "" && newPassword != "" && changePassword != "")
                {
                    string MDL_login_user_idin = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];
                    using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con1.Open();
                        string stmt1 = "SELECT id FROM masterdistributors WHERE id='" + MDL_login_user_idin + "' AND password='" + changePassword + "' ";

                        using (SqlCommand cmdCount1 = new SqlCommand(stmt1, con1))
                        {
                            cmdCount1.ExecuteScalar();
                            var reader1 = cmdCount1.ExecuteReader();
                            if (reader1.HasRows)
                            {
                                Random rand = new Random();
                                rand.Next();
                                SqlCommand dist_update = new SqlCommand("UPDATE distributors SET password='" + newPassword + "',hash_key='" + rand + "' WHERE id='" + user_id + "'", con1);
                                int CheckDataInsert = dist_update.ExecuteNonQuery();
                                if (CheckDataInsert > 0)
                                {
                                    ReturnMessage = "Success";
                                }
                            }
                            else
                            {
                                ReturnMessage = "IncorrectPWD";
                            }

                        }
                        con1.Close();
                    }
                }
                else
                {
                    ReturnMessage = "Error";
                }
            }
            catch (Exception ex)
            {
                ReturnMessage = "Error";
            }
            return ReturnMessage;
        }

        public string ClientChangePasswordDB(string user_id, string newPassword)
        {
            string ReturnMessage = "Error";
            try
            {
                if (user_id != "" && newPassword != "")
                {
                    using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con1.Open();
                        Random rand = new Random();
                        rand.Next();
                        SqlCommand dist_update = new SqlCommand("UPDATE users_client SET password='" + newPassword + "',hash_key='" + rand + "' WHERE id='" + user_id + "'", con1);
                        int CheckDataInsert = dist_update.ExecuteNonQuery();
                        if (CheckDataInsert > 0)
                        {
                            ReturnMessage = "Success";
                        }
                      
                        con1.Close();
                    }
                }
                else
                {
                    ReturnMessage = "Error";
                }
            }
            catch (Exception ex)
            {
                ReturnMessage = "Error";
            }
            return ReturnMessage;
        }

        public string UserChangePasswordDB(string user_id, string newPassword, string changePassword)
        {
            string ReturnMessage = "";
            try
            {
                if (user_id != "" && newPassword != "" && changePassword != "")
                {
                    string UIDS = (string)System.Web.HttpContext.Current.Session["UIDS"];
                    using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con1.Open();
                        string stmt1 = "SELECT id FROM users_client WHERE id='" + UIDS + "' AND password='" + changePassword + "' AND id!='6' AND id!='8' ";

                        using (SqlCommand cmdCount1 = new SqlCommand(stmt1, con1))
                        {
                            cmdCount1.ExecuteScalar();
                            var reader1 = cmdCount1.ExecuteReader();
                            if (reader1.HasRows)
                            {
                                Random rand = new Random();
                                rand.Next();
                                SqlCommand dist_update = new SqlCommand("UPDATE users_client SET password='" + newPassword + "',hash_key='" + rand + "' WHERE username!='samsung' AND id='" + user_id + "'", con1);
                                int CheckDataInsert = dist_update.ExecuteNonQuery();
                                if (CheckDataInsert > 0)
                                {
                                    ReturnMessage = "Success";
                                }
                            }
                            else
                            {
                                ReturnMessage = "IncorrectPWD";
                            }

                        }
                        con1.Close();
                    }
                }
                else
                {
                    ReturnMessage = "Error";
                }
            }
            catch (Exception ex)
            {
                ReturnMessage = "Error";
            }
            return ReturnMessage;
        }

        public JsonResult GetPlaceBets(string event_id)
        {

            List<PlaceBetsList> messages = new List<PlaceBetsList>();
            messages = GetPlaceBets1(event_id);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<PlaceBetsList> GetPlaceBets1(string event_id)
        {
            var placebetlist = new List<PlaceBetsList>();
            try
            {
                string client_uname = (string)System.Web.HttpContext.Current.Session["UserName"];
                string user_ids = (string)System.Web.HttpContext.Current.Session["UIDS"];
                int user_id = 0;
                if (user_ids != "" && user_ids != null)
                {
                    user_id = Int32.Parse(user_ids);
                }

                //int user_id = 0;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT id FROM users_client WHERE id='" + user_id + "' AND status='activate' ", con))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var data = cmd.ExecuteReader();
                        if (data.HasRows)
                        {
                            data.Read();
                            user_id = (int)data["id"];

                            using (var cmd1 = new SqlCommand("SELECT place_time,event_id,id,betfair_id,field,field_pos,status,rate,stakes,total_value,odds_type FROM live_bet  WHERE event_id='" + event_id + "' AND user_id='" + user_id + "' AND status='' order by betfair_id,place_time desc", con))
                            {
                                var rowLiveBets = cmd1.ExecuteReader();
                                if (rowLiveBets.HasRows)
                                {
                                    while (rowLiveBets.Read())
                                    {
                                        string BetType = "";
                                        string BetName = (string)rowLiveBets["odds_type"];
                                        string field = (string)rowLiveBets["field"];
                                        Double stakes = (Double)rowLiveBets["stakes"];
                                        stakes = System.Math.Round(stakes, 2);
                                        Double rate = (Double)rowLiveBets["rate"];
                                        Double total_value = (Double)rowLiveBets["total_value"];
                                        total_value = System.Math.Round(total_value, 2);
                                        string betfair_id = (string)rowLiveBets["betfair_id"];
                                        string MarketName = GetMArketName(event_id, betfair_id);
                                        DateTime time = (DateTime)rowLiveBets["place_time"];
                                        string format = "yyyy-MM-dd HH:mm:ss";
                                        string BetType1 = (string)rowLiveBets["field_pos"];
                                        //  DateTime place_time = 
                                        if (BetName == "MO" || BetName == "TP")
                                        {
                                            if (BetType1 == "back")
                                            {
                                                BetType = "B";
                                            }
                                            else
                                            {
                                                BetType = "A";
                                            }
                                        }
                                        else if (BetName == "sess")
                                        {
                                            if (BetType1 == "b")
                                            {
                                                BetType = "B";
                                            }
                                            else
                                            {
                                                BetType = "A";
                                            }
                                        }
                                        placebetlist.Add(item: new PlaceBetsList
                                        {
                                            EventName = MarketName,
                                            BetName = MarketName,
                                            BetType = BetType,
                                            RunnerName = field,
                                            event_market_field = field,
                                            Price = rate,
                                            PL = total_value,
                                            MatchOddsId = betfair_id,
                                            RunnerId = "10",
                                            created = time.ToString(format),
                                            Qty = stakes
                                        });
                                    }
                                }
                                else
                                {
                                }
                            }
                        }
                        else
                        {

                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return placebetlist;

        }

        public string GetMarketN(string BFId, string event_code)
        {
            string MarketName = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                SqlCommand sqlmatch = new SqlCommand("MarketName", con);
                sqlmatch.CommandType = System.Data.CommandType.StoredProcedure;
                sqlmatch.Parameters.Add("event_code", System.Data.SqlDbType.NVarChar).Value = event_code;
                sqlmatch.Parameters.Add("betfair_id", System.Data.SqlDbType.NVarChar).Value = BFId;
                sqlmatch.Connection = con;
                var data = sqlmatch.ExecuteReader();
                if (data.HasRows)
                {
                    data.Read();
                    MarketName = (string)data["market_name"];
                }
                con.Close();
            }
            return MarketName;
        }

        static public string AddMarkets(string BfId, string EventCode1)
        {
            string Message = "";
            DateTime time = DateTime.Now;
            string format1 = "yyyy-MM-dd HH:mm:ss";
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://bet24win.in/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("selection.php?betfair_id=" + BfId).Result;
                    var products2 = response.Content.ReadAsStringAsync().Result;
                    dynamic responseJson2 = JsonConvert.DeserializeObject(products2);
                    int CheckAL = responseJson2.Count;
                    if (CheckAL > 0)
                    {
                        var runners = responseJson2[0].runners;
                        string marketId = responseJson2[0].marketId;
                        string marketName1 = responseJson2[0].marketName;

                        string evtId = GetEventTypeId(EventCode1);
                        using (SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            con2.Open();
                            SqlCommand cmd3 = new SqlCommand("INSERT INTO  markets (market_name,back1,lay1,back2,lay2,back3,lay3,status,result_value," +
                               "market_settle,created,league_id,sport_id,betfair_id,event_code,nested_market,runner_name_done) " +
                               "VALUES ('" + marketName1 + "' ,'' ,'' ,'' ,'' ,'' ,'' ,'activate' ,'' ,'' ,'" + time.ToString(format1) + "' ,'' ,'" + evtId + "' ,'" + BfId + "' ,'" + EventCode1 + "' ,'' ,'done') ", con2);
                            cmd3.ExecuteNonQuery();
                            con2.Close();
                            int checkT = 0;
                            for (int i3 = 0; i3 < runners.Count; i3++)
                            {
                                string selectionId = runners[i3].selectionId;
                                string runnerName = runners[i3].runnerName;
                                runnerName = runnerName.Replace("'", "''");
                                string sortPriority = runners[i3].sortPriority;

                                con2.Open();
                                SqlCommand cmd5 = new SqlCommand("INSERT INTO  runner_market (runner_name,back1,back2,back3,lay1,lay2,lay3,status,selection_id,market_id,sortPriority) " +
                                       "VALUES ('" + runnerName + "' , '0' , '0' , '0' , '0' , '0' , '0' , 'activate' ,'" + selectionId + "' ,'" + marketId + "' ,'" + sortPriority + "') ", con2);
                                cmd5.ExecuteNonQuery();
                                checkT++;
                                con2.Close();
                            }
                            if (runners.Count == checkT)
                            {
                                Message = "Success";
                            }
                        }
                        return Message;
                    }
                    else
                    {
                        return "Error";
                    }
                }
                catch (Exception ex)
                {
                    return "Error";
                }
            }
            catch (TaskCanceledException ex)
            {
                return "Error";
            }
        }

        static public string GetEventTypeId(string EvCode)
        {
            string SendEVTId = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {

                using (var cmd = new SqlCommand("select sport_id from matches WHERE event_code='" + EvCode + "'"))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SendEVTId = (string)reader["sport_id"];
                    }
                    con.Close();
                }
            }
            return SendEVTId;
        }

        public JsonResult MatchedClientBetR()
        {

            List<MatchedClientBetList> messages = new List<MatchedClientBetList>();
            messages = MatchedClientBet();
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<MatchedClientBetList> MatchedClientBet()
        {
            var DL_UserBetList = new List<MatchedClientBetList>();
            try
            {
                string login_user_id = (string)System.Web.HttpContext.Current.Session["UIDS"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT place_time,event_id,id,betfair_id,field,field_pos,status,rate,stakes,total_value,odds_type FROM live_bet WHERE status='' AND user_id='" + login_user_id + "'", con))
                    {
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                DateTime time = (DateTime)dr["place_time"];
                                string format = "yyyy-MM-dd HH:mm:ss";

                                DateTime Event_time = (DateTime)dr["place_time"];
                                string Event_code_Get = (string)dr["event_id"];
                                int betid = (int)dr["id"];
                                string betfair_id_Get = (string)dr["betfair_id"];
                                string field = (string)dr["field"];
                                Double rate = (Double)dr["rate"];
                                Double stakes = (Double)dr["stakes"];
                                stakes = System.Math.Round(stakes, 2);
                                Double profit_loss = (Double)dr["total_value"];
                                profit_loss = System.Math.Round(profit_loss, 2);
                                string GetEventTName = GetEventTitleName(Event_code_Get);
                                string GetMarketName = GetMArketName(Event_code_Get, betfair_id_Get);
                                string GetSportsNameS = GetSportsName(Event_code_Get);
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
                                    Stakes = stakes,
                                    Type = BetType,
                                    PL = profit_loss,
                                    GetEventTName = GetEventTName,
                                    GetMarketName = GetMarketName,
                                    betid = betid
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
            return DL_UserBetList;
        }

        static public string GetEventTitleName(string GEtEventCode)
        {
            string match_title = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT match_title from [matches] where event_code='" + GEtEventCode + "'"))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        match_title = (string)reader["match_title"];
                    }
                    con.Close();
                }
            }
            return match_title;
        }

        static public string GetSportsName(string GEtEventCode)
        {

            string sport_name = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                string stmt = "SELECT sport_id from [matches] where event_code='" + GEtEventCode + "' ";

                using (SqlCommand cmdCount = new SqlCommand(stmt, con))
                {
                    cmdCount.ExecuteScalar();
                    var reader = cmdCount.ExecuteReader();
                    while (reader.Read())
                    {
                        string sport_id = (string)reader["sport_id"];

                        using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            con1.Open();
                            string stmt1 = "SELECT sport_name from [sports] where sport_id='" + sport_id + "'";

                            using (SqlCommand cmdCount1 = new SqlCommand(stmt1, con1))
                            {
                                cmdCount1.ExecuteScalar();
                                var reader1 = cmdCount1.ExecuteReader();
                                while (reader1.Read())
                                {
                                    sport_name = (string)reader1["sport_name"];
                                }
                            }
                            con1.Close();
                        }
                    }
                }
                con.Close();
            }
            return sport_name;
        }

        static public string GetSportsNameById(string sport_id)
        {
            string sport_name = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                string stmt = "SELECT sport_name from [sports] where sport_id='" + sport_id + "'";
                using (SqlCommand cmdCount1 = new SqlCommand(stmt, con))
                {
                    var reader1 = cmdCount1.ExecuteReader();
                    while (reader1.Read())
                    {
                        sport_name = (string)reader1["sport_name"];
                    }
                }
                con.Close();
            }
            return sport_name;
        }

        static public string GetMArketName(string Event_code_Get, string GEtbetfair_id)
        {
            string market_name = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                SqlCommand sqlmatch = new SqlCommand("MarketName", con);
                sqlmatch.CommandType = System.Data.CommandType.StoredProcedure;
                sqlmatch.Parameters.Add("event_code", System.Data.SqlDbType.NVarChar).Value = Event_code_Get;
                sqlmatch.Parameters.Add("betfair_id", System.Data.SqlDbType.NVarChar).Value = GEtbetfair_id;
                sqlmatch.Connection = con;
                var data = sqlmatch.ExecuteReader();
                if (data.HasRows)
                {
                    data.Read();
                    market_name = (string)data["market_name"];
                }
                con.Close();
            }
            return market_name;
        }

        static public string GetWinner(string Event_code_Get, string GEtbetfair_id)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT result_value from [markets] where event_code='" + Event_code_Get + "' AND betfair_id='" + GEtbetfair_id + "'"))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    string result_value = (string)reader["result_value"];
                    con.Close();
                    return result_value;
                }
            }

        }

        static public Double getMDCOIN(int md_id)
        {
            Double MDCOIN = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT coin_rate from masterdistributors where id='" + md_id + "'"))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    MDCOIN = (Double)reader["coin_rate"];
                    con.Close();
                }
            }
            return MDCOIN;
        }

        static public Double getAdminPL(int Ad_id)
        {
            Double pl_amount = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT profit_loss from admin where id='" + Ad_id + "'"))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    pl_amount = (Double)reader["profit_loss"];
                    con.Close();
                }
            }
            return pl_amount;
        }

        static public String getblocksport()
        {
            string dl_block_sports = "0";
            try
            {
                string check_ag_id = (string)System.Web.HttpContext.Current.Session["AgentID"];
                string check_agm_id = (string)System.Web.HttpContext.Current.Session["AgentMID"];
                string check_ad_id = (string)System.Web.HttpContext.Current.Session["AdminID"];
                if (check_ag_id != null && check_ag_id != "" && check_agm_id != null && check_agm_id != "" && check_ad_id != null && check_ad_id != "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {

                        using (var cmd = new SqlCommand("SELECT sport_id FROM block_sport where dl_id='" + check_ag_id + "' OR md_id='" + check_agm_id + "'  OR admin_id='" + check_ad_id + "'  ", con))
                        {
                            con.Open();
                            var reader = cmd.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    dl_block_sports = dl_block_sports + "," + (string)reader["sport_id"];
                                }
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return dl_block_sports;
        }

        static public String getblockleague()
        {
            string dl_block_leagues = "0";
            try
            {
                string check_ag_id = (string)System.Web.HttpContext.Current.Session["AgentID"];
                string check_agm_id = (string)System.Web.HttpContext.Current.Session["AgentMID"];
                string check_ad_id = (string)System.Web.HttpContext.Current.Session["AdminID"];
                if (check_ag_id != null && check_ag_id != "" && check_agm_id != null && check_agm_id != "" && check_ad_id != null && check_ad_id != "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {

                        using (var cmd = new SqlCommand("SELECT league_id FROM block_league where dl_id='" + check_ag_id + "' OR md_id='" + check_agm_id + "'  OR admin_id='" + check_ad_id + "'  ", con))
                        {
                            con.Open();
                            var reader = cmd.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    dl_block_leagues = dl_block_leagues + "," + (string)reader["league_id"];
                                }
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return dl_block_leagues;
        }

        static public String getblockevent()
        {
            string dl_block_events = "0";
            try
            {
                string check_ag_id = (string)System.Web.HttpContext.Current.Session["AgentID"];
                string check_agm_id = (string)System.Web.HttpContext.Current.Session["AgentMID"];
                string check_ad_id = (string)System.Web.HttpContext.Current.Session["AdminID"];
                if (check_ag_id != null && check_ag_id != "" && check_agm_id != null && check_agm_id != "" && check_ad_id != null && check_ad_id != "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {

                        using (var cmd = new SqlCommand("SELECT event_code FROM block_event where dl_id='" + check_ag_id + "' OR md_id='" + check_agm_id + "'  OR admin_id='" + check_ad_id + "'  ", con))
                        {
                            con.Open();
                            var reader = cmd.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    dl_block_events = dl_block_events + "," + (string)reader["event_code"];
                                }
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return dl_block_events;
        }

        static public string Getxid(string event_code)
        {
            string xid = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("select x_code from matches where event_code='" + event_code + "' ", con))
                {
                    cmd.Connection = con;
                    con.Open();
                    var data = cmd.ExecuteReader();
                    if (data.HasRows)
                    {
                        data.Read();
                        xid = (string)data["x_code"];
                    }
                    else
                    {
                    }
                    con.Close();
                }
            }
            return xid;
        }

    }
}