using BertfairLive1.Models;
using Newtonsoft.Json;
using Probet247.Models;
using RBetfair.Models;
using sky888.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Probet247.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class AgentController : Controller
    {

        private SqlConnection con2;
        string DL_login_user_id = "";
        static string cric_bf_id = "";
        DateTime time = DateTime.Now;
        string format = "yyyy-MM-dd HH:mm:ss";
        DateTime todaydate = DateTime.Now;
        string today = "yyyy-MM-dd";

        private void connection2()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            con2 = new SqlConnection(constr);
        }
        // GET: Agent
        /*  [HttpGet]
          [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]*/
        public ActionResult Index(string sportid)
        {
            CheckSession_DL();
            var Dl_UserStatement = new List<DL_UserStatement>();
            try
            {
                string qwes = "";
                if (sportid != null && sportid != "All")
                {
                    qwes = "AND username LIKE ('" + sportid + "%')";
                }

                if (DL_login_user_id != "0" && DL_login_user_id != "" && DL_login_user_id != null)
                { 
                    int getdlin = Int32.Parse(DL_login_user_id);
                    Double ToBala = 0;
                    Double ToExp = 0;
                    Double ToAvlBa = 0;
                    Double Balan = 0;
                 /*   ToBala = getTClientBal(getdlin);
                    ToBala = System.Math.Round(ToBala, 0);
                    ViewBag.ToBala = ToBala;
                    ToExp = getDLClientLib(getdlin);
                    ToExp = System.Math.Round(ToExp, 0);
                    ViewBag.ToExp = ToExp;
                    ToAvlBa = ToBala - ToExp;
                    ViewBag.ToAvBal = ToAvlBa;
                    Balan = GetDLBalance(getdlin);
                    Balan = System.Math.Round(Balan, 0);
                    ViewBag.Balan = Balan;
                    Double AVBAH = ToAvlBa + Balan;
                    ViewBag.AVBAH = AVBAH;*/
                    Double prof_llo = 0;
                    connection2();
                    con2.Open();
                    SqlCommand com = new SqlCommand();
                    SqlDataReader dr;
                    com.Connection = con2;
                    com.CommandText = "SELECT is_bet,exposure_limit,id,username,name,status,balance,total_balance,profit_loss,exposure,credit_ref FROM users_client WHERE dl_id='" + DL_login_user_id + "' " + qwes;
                    dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            int login_user_id = (int)dr["id"];
                            string login_username = (string)dr["username"];
                            string login_user_full_name = (string)dr["name"];
                            string user_status = (string)dr["status"];
                            string is_bet = (string)dr["is_bet"];
                            Double login_user_avl_balance = (Double)dr["balance"];
                            login_user_avl_balance = System.Math.Round(login_user_avl_balance, 2);
                            Double login_user_total_balance = (Double)dr["total_balance"];
                            Double profit_loss = (Double)dr["profit_loss"];
                            login_user_total_balance = System.Math.Round(login_user_total_balance, 2);
                            Double user_exposure = (Double)dr["exposure"];
                            user_exposure = System.Math.Round(user_exposure, 2);
                            Double exposure_limit = (Double)dr["exposure_limit"];
                            Double credit_ref = (Double)dr["credit_ref"];
                            credit_ref = System.Math.Round(credit_ref, 2);
                            Double login_user_balance = login_user_avl_balance + user_exposure;
                            login_user_balance = System.Math.Round(login_user_balance, 2);
                            Double user_profit_loss = login_user_balance - credit_ref;
                            user_profit_loss = System.Math.Round(user_profit_loss, 2);
                            prof_llo += user_profit_loss;

                            Dl_UserStatement.Add(item: new DL_UserStatement
                            {
                                Client_Id = login_user_id,
                                Client_Username = login_username,
                                Client_balance = login_user_balance,
                                Client_avl_balance = login_user_avl_balance,
                                Client_ttl_balance = login_user_total_balance,
                                Client_profit_loss = profit_loss,
                                Client_status = user_status,
                                credit_ref = credit_ref,
                                Client_exposure = user_exposure,
                                exposure_limit = exposure_limit,
                                is_bet = is_bet
                            });
                        }
                    }
                    else
                    {

                    }
                   // ViewBag.prof_llo = prof_llo;
                    con2.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return View(Dl_UserStatement);
        }

        public ActionResult myAccountSummary()
        {
            CheckSession_DL();
            return View();
        }

        public ActionResult Blockmarket()
        {
            CheckSession_DL();
            var AllSportsAddM = new List<NavbarListAllSports>();
            try
            {
                string DL_login_user_idin = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT sport_name,sport_id FROM sports order by sport_name asc", con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string SportName = (string)reader["sport_name"];
                            string Sportsid = (string)reader["sport_id"];
                            string is_block = "no";
                            using (var cmd1 = new SqlCommand("SELECT id FROM block_sport where dl_id='" + DL_login_user_idin + "' AND  sport_id='" + Sportsid + "' AND status='yes' ", con))
                            {
                                var reader1 = cmd1.ExecuteReader();
                                if (reader1.HasRows)
                                {
                                    is_block = "yes";
                                }
                            }

                            AllSportsAddM.Add(item: new NavbarListAllSports
                            {
                                SportsName = SportName,
                                SportsId = Sportsid,
                                IsBlock = is_block
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

        public ActionResult marketProfitLoss()
        {
            CheckSession_DL();
            var DL_UserBetList = new List<ClientPLModel>();
            try
            {
                string login_user_idn = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];

                string pageNumber = "1";
                string sport_id = "0";
                int max_page = 20;
                int min_page = 0;
                if (Request.QueryString["sport_id"] != null && Request.QueryString["startDate"] != null && Request.QueryString["endDate"] != null)
                {
                    sport_id = Request.QueryString["sport_id"];
                    string startDate = Request.QueryString["startDate"];
                    string endDate = Request.QueryString["endDate"];
                    if (Request.QueryString["pageNumber"] != null)
                    {
                        pageNumber = Request.QueryString["pageNumber"];
                    }
                    int pageno = Int32.Parse(pageNumber);
                    max_page = 20 * pageno;
                    min_page = max_page - 20;
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        using (var cmd = new SqlCommand("SELECT distinct(event_id)  FROM(SELECT distinct(event_id) , ROW_NUMBER() over(order by id desc) as row FROM pl_statement WHERE (created BETWEEN '" + startDate + "' AND '" + endDate + "') and sport_id='" + sport_id + "' AND dist_id='" + login_user_idn + "' ) a where a.row > '" + min_page + "'  and a.row <= '" + max_page + "' "))
                        {
                            cmd.Connection = con;
                            con.Open();
                            var dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                string event_id_g = (string)dr["event_id"];
                                using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                                {
                                    using (var cmd1 = new SqlCommand("select created,total_pl from pl_statement where dist_id='" + login_user_idn + "' AND event_id='" + event_id_g + "'"))
                                    {
                                        cmd1.Connection = con1;
                                        con1.Open();
                                        var dr1 = cmd1.ExecuteReader();
                                        DateTime created = new DateTime();
                                        string description = "";
                                        Double total_pl = 0;
                                        description = GetEventTitleName(event_id_g);
                                        while (dr1.Read())
                                        {
                                            created = (DateTime)dr1["created"];
                                            total_pl = total_pl + (Double)dr1["total_pl"];
                                        }

                                        DL_UserBetList.Add(item: new ClientPLModel
                                        {
                                            description = description,
                                            created = created,
                                            total_pl = total_pl,
                                            event_id = event_id_g
                                        });
                                        con1.Close();
                                    }
                                }
                            }
                            con.Close();
                        }

                        string stmts = "SELECT COUNT(id) FROM pl_statement WHERE (created BETWEEN '" + startDate + "' AND '" + endDate + "') AND  sport_id='" + sport_id + "' AND dist_id='" + login_user_idn + "' ";
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

                else
                {
                    ViewBag.countbeth = 0;
                    ViewBag.startDate = "0";
                    ViewBag.endDate = "0";
                }
                ViewBag.pageNumber = pageNumber;

                ArrayList SportsNameForShowAllSportsL = new ArrayList();
                ArrayList SportsIDForShowAllSportsL = new ArrayList();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT sport_name,sport_id FROM sports where sport_id in(4,2,1,7888)"))
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
                ViewBag.sport_id = sport_id;

            }
            catch (Exception ex)
            {
                string dfv = ex.ToString();
            }
            return View(DL_UserBetList);
        }

        public ActionResult MarketPLHistory()
        {
            CheckSession_DL();
            var DL_UserBetList = new List<ClientPLModel>();
            try
            {
                string event_id_g = Request.QueryString["event_id"];
                string login_user_idn = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("select distinct(market_id) from pl_statement where dist_id='" + login_user_idn + "' AND event_id='" + event_id_g + "'"))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            string market_id_g = (string)dr["market_id"];
                            using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                            {
                                using (var cmd1 = new SqlCommand("select created,total_pl,description from pl_statement where dist_id='" + login_user_idn + "' AND market_id='" + market_id_g + "'"))
                                {
                                    cmd1.Connection = con1;
                                    con1.Open();
                                    var dr1 = cmd1.ExecuteReader();
                                    DateTime created = new DateTime();
                                    string description = "";
                                    Double total_pl = 0;
                                    description = "";
                                    while (dr1.Read())
                                    {
                                        created = (DateTime)dr1["created"];
                                        total_pl = total_pl + (Double)dr1["total_pl"];
                                        description = (string)dr1["description"];
                                    }
                                    DL_UserBetList.Add(item: new ClientPLModel
                                    {
                                        description = description,
                                        created = created,
                                        total_pl = -total_pl,
                                        market_id = market_id_g,
                                        event_id = event_id_g
                                    });

                                    con1.Close();
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
            return View(DL_UserBetList);
        }

        public ActionResult ClientProfitLossHistory()
        {
            CheckSession_DL();
            var DL_UserBetList = new List<ClientPLModel>();
            try
            {
                string event_id_g = Request.QueryString["event_id"];
                string market_id_g = Request.QueryString["market_id"];
                string login_user_idn = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];

                using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd1 = new SqlCommand("select created,debit,credit,user_id,description from user_account_statements where dist_id='" + login_user_idn + "' AND market_id='" + market_id_g + "' AND event_id='" + event_id_g + "'"))
                    {
                        cmd1.Connection = con1;
                        con1.Open();
                        var dr1 = cmd1.ExecuteReader();
                        DateTime created = new DateTime();
                        string description = "";
                        Double debit = 0;
                        Double credit = 0;
                        int ucid = 0;
                        string CUname = "";
                        description = "";
                        while (dr1.Read())
                        {
                            created = (DateTime)dr1["created"];
                            debit = (Double)dr1["debit"];
                            credit = (Double)dr1["credit"];
                            ucid = (int)dr1["user_id"];
                            CUname = GetCUserName(ucid);
                            description = (string)dr1["description"];

                            DL_UserBetList.Add(item: new ClientPLModel
                            {
                                description = description,
                                created = created,
                                debit = debit,
                                credit = credit,
                                uname = CUname
                            });
                        }
                        

                        con1.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View(DL_UserBetList);
        }

        public ActionResult betList(string sportid)
        {
            CheckSession_DL();
            string dist_id = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
            var DL_UserBetList = new List<MatchedClientBetList>();
            try
            {
                if (sportid != null)
                {
                    int gfh = Int32.Parse(sportid);
                    ViewBag.CUname = GetCUserName(gfh);
                    ViewBag.ClientId = sportid;
                    string login_user_id = sportid;
                    string pageNumber = "1";
                    int max_page = 20;
                    int min_page = 0;
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
                            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                            {
                                using (var cmd = new SqlCommand("SELECT user_id,session_rate,place_time,event_id,id,betfair_id,field,field_pos,status,rate,stakes,total_value,odds_type FROM(SELECT user_id,session_rate,place_time,event_id,id,betfair_id,field,field_pos,status,rate,stakes,total_value,odds_type , ROW_NUMBER() over(order by id desc) as row FROM live_bet WHERE (place_time BETWEEN '" + startDate + "' AND '" + endDate + "') AND  status!='' AND user_id='" + login_user_id + "') a where a.row > '" + min_page + "'  and a.row <= '" + max_page + "' ", con))
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
                                        int user_id = (int)dr["user_id"];
                                        string betfair_id_Get = (string)dr["betfair_id"];
                                        string field = (string)dr["field"];
                                        string field_pos = (string)dr["field_pos"];
                                        string status = (string)dr["status"];
                                        Double rate = (Double)dr["rate"];
                                        Double session_rate = (Double)dr["session_rate"];
                                        Double stakes = (Double)dr["stakes"];
                                        stakes = System.Math.Round(stakes, 2);
                                        Double profit_loss = (Double)dr["total_value"];
                                        profit_loss = System.Math.Round(profit_loss, 2);
                                        string GetEventTName = FunctionDataController.GetEventTitleName(Event_code_Get);
                                        string GetMarketName = FunctionDataController.GetMArketName(Event_code_Get, betfair_id_Get);
                                        string GetSportsNameS = FunctionDataController.GetSportsName(Event_code_Get);
                                        string Client_UName = GetCUserName(user_id);
                                        string BetType1 = (string)dr["field_pos"];
                                        string BetName = (string)dr["odds_type"];
                                        string BetType = "";
                                        string odds_match = rate.ToString();
                                        Double stakes1 = stakes;
                                        if(BetType1 == "lay")
                                        {
                                            stakes1 = profit_loss;
                                        }
                                        if (BetName == "sess")
                                        {
                                            odds_match = odds_match + "/" + session_rate.ToString();
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
                                            odds_match = odds_match,
                                            Stakes = stakes,
                                            Stakes1 = stakes1,
                                            PL = profit_loss,
                                            Type = BetType,
                                            Status = status,
                                            GetEventTName = GetEventTName,
                                            GetMarketName = GetMarketName,
                                            betid = betid,
                                            Field_pos = field_pos,
                                            ucname = Client_UName
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
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            using (var cmd = new SqlCommand("SELECT user_id,session_rate,place_time,event_id,id,betfair_id,field,field_pos,status,rate,stakes,total_value,odds_type FROM(SELECT user_id,session_rate,place_time,event_id,id,betfair_id,field,field_pos,status,rate,stakes,total_value,odds_type , ROW_NUMBER() over(order by id desc) as row FROM live_bet WHERE place_time >'" + todaydate.ToString(today) + "'  AND  status!='' AND user_id='" + login_user_id + "') a where a.row > '" + min_page + "'  and a.row <= '" + max_page + "' ", con))
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
                                    int user_id = (int)dr["user_id"];
                                    string betfair_id_Get = (string)dr["betfair_id"];
                                    string field = (string)dr["field"];
                                    string field_pos = (string)dr["field_pos"];
                                    string status = (string)dr["status"];
                                    Double rate = (Double)dr["rate"];
                                    Double session_rate = (Double)dr["session_rate"];
                                    Double stakes = (Double)dr["stakes"];
                                    stakes = System.Math.Round(stakes, 2);
                                    Double profit_loss = (Double)dr["total_value"];
                                    profit_loss = System.Math.Round(profit_loss, 2);
                                    string GetEventTName = FunctionDataController.GetEventTitleName(Event_code_Get);
                                    string GetMarketName = FunctionDataController.GetMArketName(Event_code_Get, betfair_id_Get);
                                    string GetSportsNameS = FunctionDataController.GetSportsName(Event_code_Get);
                                    string Client_UName = GetCUserName(user_id);
                                    string BetType1 = (string)dr["field_pos"];
                                    string BetName = (string)dr["odds_type"];
                                    string BetType = "";
                                    string odds_match = rate.ToString();
                                    Double stakes1 = stakes;
                                    if (BetType1 == "lay")
                                    {
                                        stakes1 = profit_loss;
                                    }
                                    if (BetName == "sess")
                                    {
                                        odds_match = odds_match + "/" + session_rate.ToString();
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
                                        odds_match = odds_match,
                                        Stakes = stakes,
                                        Stakes1 = stakes1,
                                        PL = profit_loss,
                                        Type = BetType,
                                        Status = status,
                                        GetEventTName = GetEventTName,
                                        GetMarketName = GetMarketName,
                                        betid = betid,
                                        Field_pos = field_pos,
                                        ucname = Client_UName
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
                    ViewBag.pageNumber = pageNumber;
                }
                else
                {
                    ViewBag.CUname = "";
                    ViewBag.ClientId = "";
                    string login_user_id = "";
                    string pageNumber = "1";
                    int max_page = 20;
                    int min_page = 0;
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
                            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                            {
                                using (var cmd = new SqlCommand("SELECT user_id,session_rate,place_time,event_id,id,betfair_id,field,field_pos,status,rate,stakes,total_value,odds_type FROM(SELECT user_id,session_rate,place_time,event_id,id,betfair_id,field,field_pos,status,rate,stakes,total_value,odds_type , ROW_NUMBER() over(order by id desc) as row FROM live_bet WHERE (place_time BETWEEN '" + startDate + "' AND '" + endDate + "') AND  status!='' AND dist_id='" + dist_id + "') a where a.row > '" + min_page + "'  and a.row <= '" + max_page + "' ", con))
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
                                        int user_id = (int)dr["user_id"];
                                        string betfair_id_Get = (string)dr["betfair_id"];
                                        string field = (string)dr["field"];
                                        string field_pos = (string)dr["field_pos"];
                                        string status = (string)dr["status"];
                                        Double rate = (Double)dr["rate"];
                                        Double session_rate = (Double)dr["session_rate"];
                                        Double stakes = (Double)dr["stakes"];
                                        stakes = System.Math.Round(stakes, 2);
                                        Double profit_loss = (Double)dr["total_value"];
                                        profit_loss = System.Math.Round(profit_loss, 2);
                                        string GetEventTName = FunctionDataController.GetEventTitleName(Event_code_Get);
                                        string GetMarketName = FunctionDataController.GetMArketName(Event_code_Get, betfair_id_Get);
                                        string GetSportsNameS = FunctionDataController.GetSportsName(Event_code_Get);
                                        string Client_UName = GetCUserName(user_id);
                                        string BetType1 = (string)dr["field_pos"];
                                        string BetName = (string)dr["odds_type"];
                                        string BetType = "";
                                        string odds_match = rate.ToString();
                                        Double stakes1 = stakes;
                                        if (BetType1 == "lay")
                                        {
                                            stakes1 = profit_loss;
                                        }
                                        if (BetName == "sess")
                                        {
                                            odds_match = odds_match + "/" + session_rate.ToString();
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
                                            odds_match = odds_match,
                                            Stakes = stakes,
                                            Stakes1 = stakes1,
                                            PL = profit_loss,
                                            Type = BetType,
                                            Status = status,
                                            GetEventTName = GetEventTName,
                                            GetMarketName = GetMarketName,
                                            betid = betid,
                                            Field_pos = field_pos,
                                            ucname = Client_UName
                                        });
                                    }
                                    con.Close();
                                }
                                string stmts = "SELECT COUNT(id) FROM live_bet WHERE (place_time BETWEEN '" + startDate + "' AND '" + endDate + "') AND  status!='' AND dist_id='" + dist_id + "'";
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
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            using (var cmd = new SqlCommand("SELECT user_id,session_rate,place_time,event_id,id,betfair_id,field,field_pos,status,rate,stakes,total_value,odds_type FROM(SELECT user_id,session_rate,place_time,event_id,id,betfair_id,field,field_pos,status,rate,stakes,total_value,odds_type , ROW_NUMBER() over(order by id desc) as row FROM live_bet WHERE place_time >'" + todaydate.ToString(today) + "'  AND  status!='' AND dist_id='" + dist_id + "') a where a.row > '" + min_page + "'  and a.row <= '" + max_page + "' ", con))
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
                                    int user_id = (int)dr["user_id"];
                                    string betfair_id_Get = (string)dr["betfair_id"];
                                    string field = (string)dr["field"];
                                    string field_pos = (string)dr["field_pos"];
                                    string status = (string)dr["status"];
                                    Double rate = (Double)dr["rate"];
                                    Double session_rate = (Double)dr["session_rate"];
                                    Double stakes = (Double)dr["stakes"];
                                    stakes = System.Math.Round(stakes, 2);
                                    Double profit_loss = (Double)dr["total_value"];
                                    profit_loss = System.Math.Round(profit_loss, 2);
                                    string GetEventTName = FunctionDataController.GetEventTitleName(Event_code_Get);
                                    string GetMarketName = FunctionDataController.GetMArketName(Event_code_Get, betfair_id_Get);
                                    string GetSportsNameS = FunctionDataController.GetSportsName(Event_code_Get);
                                    string Client_UName = GetCUserName(user_id);
                                    string BetType1 = (string)dr["field_pos"];
                                    string BetName = (string)dr["odds_type"];
                                    string BetType = "";
                                    string odds_match = rate.ToString();
                                    Double stakes1 = stakes;
                                    if (BetType1 == "lay")
                                    {
                                        stakes1 = profit_loss;
                                    }
                                    if (BetName == "sess")
                                    {
                                        odds_match = odds_match + "/" + session_rate.ToString();
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
                                        odds_match = odds_match,
                                        Stakes = stakes,
                                        Stakes1 = stakes1,
                                        PL = profit_loss,
                                        Type = BetType,
                                        Status = status,
                                        GetEventTName = GetEventTName,
                                        GetMarketName = GetMarketName,
                                        betid = betid,
                                        Field_pos = field_pos,
                                        ucname = Client_UName
                                    });
                                }
                                con.Close();
                            }
                            string stmts = "SELECT COUNT(id) FROM live_bet WHERE place_time >'" + todaydate.ToString(today) + "' AND  status!='' AND dist_id='" + dist_id + "'";
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
            }
            catch (Exception ex)
            {

            }
            return View(DL_UserBetList);
        }


        public ActionResult betListAll()
        {
            string dist_id = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
            var DL_UserBetList = new List<MatchedClientBetList>();
            try
            {
                    ViewBag.CUname = "";
                    ViewBag.ClientId = "";
                    string login_user_id = "";
                    string pageNumber = "1";
                    int max_page = 20;
                    int min_page = 0;
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
                            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                            {
                                using (var cmd = new SqlCommand("SELECT user_id,session_rate,place_time,event_id,id,betfair_id,field,field_pos,status,rate,stakes,total_value,odds_type FROM(SELECT user_id,session_rate,place_time,event_id,id,betfair_id,field,field_pos,status,rate,stakes,total_value,odds_type , ROW_NUMBER() over(order by id desc) as row FROM live_bet WHERE (place_time BETWEEN '" + startDate + "' AND '" + endDate + "') AND  status!='' AND dist_id='" + dist_id + "') a where a.row > '" + min_page + "'  and a.row <= '" + max_page + "' ", con))
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
                                        int user_id = (int)dr["user_id"];
                                        string betfair_id_Get = (string)dr["betfair_id"];
                                        string field = (string)dr["field"];
                                        string field_pos = (string)dr["field_pos"];
                                        string status = (string)dr["status"];
                                        Double rate = (Double)dr["rate"];
                                        Double session_rate = (Double)dr["session_rate"];
                                        Double stakes = (Double)dr["stakes"];
                                        stakes = System.Math.Round(stakes, 2);
                                        Double profit_loss = (Double)dr["total_value"];
                                        profit_loss = System.Math.Round(profit_loss, 2);
                                        string GetEventTName = FunctionDataController.GetEventTitleName(Event_code_Get);
                                        string GetMarketName = FunctionDataController.GetMArketName(Event_code_Get, betfair_id_Get);
                                        string GetSportsNameS = FunctionDataController.GetSportsName(Event_code_Get);
                                        string Client_UName = GetCUserName(user_id);
                                        string BetType1 = (string)dr["field_pos"];
                                        string BetName = (string)dr["odds_type"];
                                        string BetType = "";
                                        string odds_match = rate.ToString();
                                        Double stakes1 = stakes;
                                        if (BetType1 == "lay")
                                        {
                                            stakes1 = profit_loss;
                                        }
                                        if (BetName == "sess")
                                        {
                                            odds_match = odds_match + "/" + session_rate.ToString();
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
                                            odds_match = odds_match,
                                            Stakes = stakes,
                                            Stakes1 = stakes1,
                                            PL = profit_loss,
                                            Type = BetType,
                                            Status = status,
                                            GetEventTName = GetEventTName,
                                            GetMarketName = GetMarketName,
                                            betid = betid,
                                            Field_pos = field_pos,
                                            ucname = Client_UName
                                        });
                                    }
                                    con.Close();
                                }
                                string stmts = "SELECT COUNT(id) FROM live_bet WHERE (place_time BETWEEN '" + startDate + "' AND '" + endDate + "') AND  status!='' AND dist_id='" + dist_id + "'";
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
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            using (var cmd = new SqlCommand("SELECT user_id,session_rate,place_time,event_id,id,betfair_id,field,field_pos,status,rate,stakes,total_value,odds_type FROM(SELECT user_id,session_rate,place_time,event_id,id,betfair_id,field,field_pos,status,rate,stakes,total_value,odds_type , ROW_NUMBER() over(order by id desc) as row FROM live_bet WHERE place_time >'" + todaydate.ToString(today) + "'  AND  status!='' AND dist_id='" + dist_id + "') a where a.row > '" + min_page + "'  and a.row <= '" + max_page + "' ", con))
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
                                    int user_id = (int)dr["user_id"];
                                    string betfair_id_Get = (string)dr["betfair_id"];
                                    string field = (string)dr["field"];
                                    string field_pos = (string)dr["field_pos"];
                                    string status = (string)dr["status"];
                                    Double rate = (Double)dr["rate"];
                                    Double session_rate = (Double)dr["session_rate"];
                                    Double stakes = (Double)dr["stakes"];
                                    stakes = System.Math.Round(stakes, 2);
                                    Double profit_loss = (Double)dr["total_value"];
                                    profit_loss = System.Math.Round(profit_loss, 2);
                                    string GetEventTName = FunctionDataController.GetEventTitleName(Event_code_Get);
                                    string GetMarketName = FunctionDataController.GetMArketName(Event_code_Get, betfair_id_Get);
                                    string GetSportsNameS = FunctionDataController.GetSportsName(Event_code_Get);
                                    string Client_UName = GetCUserName(user_id);
                                    string BetType1 = (string)dr["field_pos"];
                                    string BetName = (string)dr["odds_type"];
                                    string BetType = "";
                                    string odds_match = rate.ToString();
                                    Double stakes1 = stakes;
                                    if (BetType1 == "lay")
                                    {
                                        stakes1 = profit_loss;
                                    }
                                    if (BetName == "sess")
                                    {
                                        odds_match = odds_match + "/" + session_rate.ToString();
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
                                        odds_match = odds_match,
                                        Stakes = stakes,
                                        Stakes1 = stakes1,
                                        PL = profit_loss,
                                        Type = BetType,
                                        Status = status,
                                        GetEventTName = GetEventTName,
                                        GetMarketName = GetMarketName,
                                        betid = betid,
                                        Field_pos = field_pos,
                                        ucname = Client_UName
                                    });
                                }
                                con.Close();
                            }
                            string stmts = "SELECT COUNT(id) FROM live_bet WHERE place_time >'" + todaydate.ToString(today) + "' AND  status!='' AND dist_id='" + dist_id + "'";
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
            return View(DL_UserBetList);
        }

        public ActionResult cashBanking(string sportid)
        {
            CheckSession_DL();
            var DL_UserStatement = new List<DL_UserStatement>();
            try
            {
                string qwes = "";
                if (sportid != null && sportid != "All")
                {
                    qwes = "AND username LIKE ('" + sportid + "%')";
                }
                string DL_login_user_idn = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                connection2();
                con2.Open();
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con2;
                com.CommandText = "SELECT id,username,name,status,balance,exposure, credit_ref FROM users_client WHERE dl_id='" + DL_login_user_idn + "'" + qwes;
                dr = com.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        int login_user_id = (int)dr["id"];
                        string login_username = (string)dr["username"];
                        string login_user_full_name = (string)dr["name"];
                        string user_status = (string)dr["status"];
                        Double login_user_balance = (Double)dr["balance"];
                        login_user_balance = System.Math.Round(login_user_balance, 2);
                        Double user_exposure = (Double)dr["exposure"];
                        user_exposure = System.Math.Round(user_exposure, 2);
                        Double total_balance = login_user_balance + user_exposure;
                        total_balance = System.Math.Round(total_balance, 2);
                        Double credit_ref = (Double)dr["credit_ref"];
                        credit_ref = System.Math.Round(credit_ref, 2);
                        Double user_profit_loss = total_balance - credit_ref;
                        user_profit_loss = System.Math.Round(user_profit_loss, 2);
                        DL_UserStatement.Add(item: new DL_UserStatement
                        {
                            Client_Id = login_user_id,
                            Client_Username = login_username,
                            Client_balance = total_balance,
                            Client_avl_balance = login_user_balance,
                            Client_profit_loss = user_profit_loss,
                            Client_status = user_status,
                            Client_exposure = user_exposure
                        });
                    }
                }
                else
                {

                }
                con2.Close();
            }
            catch (Exception ex)
            {

            }
            return View(DL_UserStatement);
        }
        public ActionResult ClientLiability(string sportid)
        {
            var DL_UserBetList = new List<DL_UserBetList>();
            CheckSession_DL();
            try
            {
                string DL_login_user_idn = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                connection2();
                con2.Open();
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con2;
                com.CommandText = "SELECT place_time,event_id,betfair_id,field,rate,stakes,total_value FROM live_bet WHERE status='' AND dist_id='" + DL_login_user_idn + "' AND user_id='" + sportid + "'";
                dr = com.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        DateTime Event_time = (DateTime)dr["place_time"];
                        string Event_code_Get = (string)dr["event_id"];
                        string betfair_id_Get = (string)dr["betfair_id"];
                        string field = (string)dr["field"];
                        Double rate = (Double)dr["rate"];
                        Double stakes = (Double)dr["stakes"];
                        Double profit_loss = (Double)dr["total_value"];
                        string GetEventTName = GetEventTitleName(Event_code_Get);
                        string GetMarketName = FunctionDataController.GetMArketName(Event_code_Get, betfair_id_Get);
                        string GetSportsNameS = GetSportsName(Event_code_Get);
                        DL_UserBetList.Add(item: new DL_UserBetList
                        {
                            EventTime = Event_time,
                            Description = GetSportsNameS + " / " + GetEventTName + " / " + GetMarketName,
                            Field = field,
                            Rate = rate,
                            Stakes = stakes,
                            PL = profit_loss
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View(DL_UserBetList);
        }

        public ActionResult profitAndLoss(string sportid)
        {
            CheckSession_DL();
            var clientprofitlossstat = new List<clientprofitlossstat>();
            try
            {
                if (sportid != null)
                {
                    int gfh = Int32.Parse(sportid);
                    ViewBag.CUname = GetCUserName(gfh);
                    ViewBag.ClientId = sportid;
                    string pageNumber = "1";
                    int max_page = 20;
                    int min_page = 0;
                    string login_user_id = sportid;
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
            }
            catch (Exception ex)
            {

            }
            return View(clientprofitlossstat);
        }

        public ActionResult myprofitAndLoss(string sportid)
        {
            CheckSession_DL();
            var clientprofitlossstat = new List<clientprofitlossstat>();
            try
            {
                if (sportid != null)
                {
                    int gfh = Int32.Parse(sportid);
                    ViewBag.CUname = GetCUserName(gfh);
                    ViewBag.ClientId = sportid;
                    string pageNumber = "1";
                    int max_page = 20;
                    int min_page = 0;
                    string login_user_id = sportid;
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
            }
            catch (Exception ex)
            {

            }
            return View(clientprofitlossstat);
        }
        public ActionResult accountSummary(string sportid)
        {
            CheckSession_DL();
            try
            {
                ViewBag.UserIdC = sportid;
                if (sportid != null)
                {
                    int gfh = Int32.Parse(sportid);
                    ViewBag.CUname = GetCUserName(gfh);
                    ViewBag.ClientId = sportid;
                    int UidInt = Int32.Parse(sportid);
                    ViewBag.UserNameC = GetCUserName(UidInt);
                    ViewBag.FUserNameC = GetCFUserName(UidInt);
                    ViewBag.UserBalC = GetCbalance(UidInt);
                    ViewBag.UserExpC = GetCexposure(UidInt);
                    ViewBag.exp_limit = GetExpLimit(UidInt);
                    ViewBag.comm_mo = GetCommMo(UidInt);
                    ViewBag.comm_sess = GetCommSess(UidInt);
                }
            }
            catch (Exception ex)
            {

            }

            return View();
        }

        public ActionResult transactionHistory(string sportid)
        {
            CheckSession_DL();
            var DL_UserBetList = new List<ClientaccountCashStatement>();
            try
            {
                if (sportid != null)
                {
                    int gfh = Int32.Parse(sportid);
                    ViewBag.CUname = GetCUserName(gfh);
                    ViewBag.ClientId = sportid;

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        using (var cmd = new SqlCommand("SELECT created,debit,credit,balance,remark,description FROM user_account_statements WHERE user_id='" + sportid + "'  AND acc_stat_type = 'dw_coins' order by created desc "))
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
                                string description = (string)dr["description"];
                                DL_UserBetList.Add(item: new ClientaccountCashStatement
                                {
                                    DTime = Event_time,
                                    Remark = remark,
                                    Balance = balance,
                                    Deposit = debit,
                                    Withdraw = credit,
                                    description = description
                                });
                            }
                            con.Close();
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return View(DL_UserBetList);
        }

        public ActionResult accountCashStatement()
        {
            string page = Request.QueryString["page"];
            var DL_UserBetList = new List<AccountStatement_DL>();
            try
            {
                string DL_login_user_idn = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                string pageNumber = "1";
                int max_page = 20;
                int min_page = 0;
                int countbeth = 0;
                if (page != null)
                {
                    pageNumber = page;
                }
                int pageno = Int32.Parse(pageNumber);
                max_page = 20 * pageno;
                min_page = max_page - 20;
                connection2();
                con2.Open();
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con2;
                com.CommandText = "SELECT created,debit,credit,balance,remark,description FROM(SELECT created,debit,credit,balance,remark,description , ROW_NUMBER() over(order by created desc) as row FROM dist_account_statements WHERE acc_stat_type='dw_coins'  AND dist_id='" + DL_login_user_idn + "') a where a.row > '" + min_page + "'  and a.row <= '" + max_page + "' ";
                dr = com.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        DateTime Event_time = (DateTime)dr["created"];
                        Double debit = (Double)dr["debit"];
                        string desc = (string)dr["description"];
                        Double credit = (Double)dr["credit"];
                        Double balance = (Double)dr["balance"];
                        string remark = (string)dr["remark"];
                        DL_UserBetList.Add(item: new AccountStatement_DL
                        {
                            DTime = Event_time,
                            Remark = remark,
                            Desc = desc,
                            Balance = balance,
                            Deposit = debit,
                            Withdraw = credit
                        });
                    }
                    string stmts = "SELECT COUNT(id) FROM dist_account_statements WHERE dist_id='" + DL_login_user_idn + "' AND acc_stat_type='dw_coins' ";
                    using (SqlCommand cmdCounts = new SqlCommand(stmts, con2))
                    {
                        countbeth = (int)cmdCounts.ExecuteScalar();
                    }
                }
                con2.Close();
                ViewBag.countbeth = countbeth;
                ViewBag.pageNumber = pageNumber;
            }
            catch (Exception ex)
            {

            }
            return View(DL_UserBetList);
        }
        public ActionResult profile()
        {
            CheckSession_DL();
            return View();
        }

        public ActionResult RiskManagement()
        {
            return View();
        }
        public ActionResult bookview()
        {
            string event_id = Request.QueryString["event_code"];
            string betfair_id = Request.QueryString["betfair_id"];
            string EventNameGet = GetEventTitleName(event_id);
            ViewBag.EventNameB = EventNameGet;
            ViewBag.BetfairId = betfair_id;
            ViewBag.event_id = event_id;
            return View();
        }
        public ActionResult allbets()
        {
            var DL_UserBetList = new List<MatchedClientBetList>();
            try
            {
                string betfair_id = Request.QueryString["betfair_id"];
                string event_code = Request.QueryString["event_code"];
                string EventNameGet = GetEventTitleName(event_code);
                string GetMarketName = FunctionDataController.GetMArketName(event_code, betfair_id);
                ViewBag.EventNameBU = EventNameGet;
                ViewBag.GetMarketName = GetMarketName;
                string user_ids = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT place_time,field,user_id,rate,stakes,total_value,event_id FROM live_bet WHERE dist_id='" + user_ids + "' AND betfair_id='" + betfair_id + "' AND event_id='" + event_code + "'"))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            DateTime time = (DateTime)dr["place_time"];
                            string format = "yyyy-MM-dd HH:mm:ss";

                            string field = (string)dr["field"];
                            int user_id = (int)dr["user_id"];
                            string UsernameC = GetCUserName(user_id);
                            Double rate = (Double)dr["rate"];
                            Double stakes = (Double)dr["stakes"];
                            Double profit_loss = (Double)dr["total_value"];
                            string event_idsend = (string)dr["event_id"];
                            DL_UserBetList.Add(item: new MatchedClientBetList
                            {
                                EventTime = time.ToString(format),
                                Description = UsernameC,
                                Field = field,
                                Rate = rate,
                                Stakes = stakes,
                                PL = profit_loss,
                                betfairid = betfair_id,
                                betid = user_id,
                                event_idsend = event_idsend
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

        public ActionResult userallbet()
        {
            var DL_UserBetList = new List<MatchedClientBetList>();
            try
            {
                string uid = Request.QueryString["uid"];
                string betfair_id = Request.QueryString["betfair_id"];
                string event_id = Request.QueryString["event_id"];
                string EventNameGet = GetEventTitleName(event_id);
                string GetMarketName = FunctionDataController.GetMArketName(event_id, betfair_id);
                int uidint = Int32.Parse(uid);
                string UsernameC = GetCUserName(uidint);
                ViewBag.EventNameBU = EventNameGet;
                ViewBag.GetMarketName = GetMarketName;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT place_time,field,rate,stakes,total_value FROM live_bet WHERE user_id='" + uid + "' AND betfair_id='" + betfair_id + "' AND event_id='" + event_id + "'"))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            DateTime time = (DateTime)dr["place_time"];
                            string format = "yyyy-MM-dd HH:mm:ss";

                            string field = (string)dr["field"];
                            Double rate = (Double)dr["rate"];
                            Double stakes = (Double)dr["stakes"];
                            Double profit_loss = (Double)dr["total_value"];
                            DL_UserBetList.Add(item: new MatchedClientBetList
                            {
                                EventTime = time.ToString(format),
                                Description = UsernameC,
                                Field = field,
                                Rate = rate,
                                Stakes = stakes,
                                PL = profit_loss,
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

        public ActionResult dlanalysis()
        {
            MessagesRepository _messageRepository = new MessagesRepository();
            return PartialView("dlanalysis", _messageRepository.GetAllMessages());
        }
        public ActionResult sdlanuhytalysis()
        {
            SessionRepository _sessionRepository = new SessionRepository();
            return PartialView("sanalysisdata", _sessionRepository.GetAllMessagesS());
        }

        public void CheckSession_DL()
        {
            string SessionDLUName = (string)System.Web.HttpContext.Current.Session["DL_UserName"];
            string SessionDL_hash_key = (string)System.Web.HttpContext.Current.Session["DL_hash_key"];

            DL_login_user_id = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];

            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT id,username,name,balance FROM distributors WHERE username='" + SessionDLUName + "' AND hash_key='" + SessionDL_hash_key + "' AND status='activate'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    int login_user_id = (int)dr["id"];
                    string login_username = (string)dr["username"];
                    string login_user_full_name = (string)dr["name"];
                    Double login_user_balance = (Double)dr["balance"];

                    ViewBag.DLUserID = login_user_id;
                    ViewBag.login_username = login_username;
                    ViewBag.login_user_full_name = login_user_full_name;
                    ViewBag.login_user_balance = login_user_balance;
                }
            }
            else
            {
                Logout();
            }
            con2.Close();
        }

        public string GetEventTitleName(string GEtEventCode)
        {
            string match_title = "";
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT match_title from [matches] where event_code='" + GEtEventCode + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                match_title = (string)dr["match_title"];
            }
            con2.Close();
            return match_title;
        }

        public string GetEventTitleNameBF(string GEtEventCode)
        {
            string match_title = "";
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT match_title from [matches] where betfair_id='" + GEtEventCode + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                match_title = (string)dr["match_title"];
            }
            con2.Close();
            return match_title;
        }

        public string GetCUserName(int user_id)
        {
            string username = "";
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT username from [users_client] where id='" + user_id + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                username = (string)dr["username"];
            }
            con2.Close();
            return username;
        }

        public string GetCFUserName(int user_id)
        {
            string username = "";
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT name from [users_client] where id='" + user_id + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                username = (string)dr["name"];
            }
            con2.Close();
            return username;
        }

        public Double GetCbalance(int user_id)
        {
            Double Cbalance = 0;
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT balance from [users_client] where id='" + user_id + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                Cbalance = (Double)dr["balance"];
                Cbalance = System.Math.Round(Cbalance, 2);
            }
            con2.Close();
            return Cbalance;
        }

        public Double GetCexposure(int user_id)
        {
            Double Cexposure = 0;
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT exposure from [users_client] where id='" + user_id + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                Cexposure = (Double)dr["exposure"];
                Cexposure = System.Math.Round(Cexposure, 2);
            }
            con2.Close();
            return Cexposure;
        }
        public Double GetExpLimit(int user_id)
        {
            Double exposure_limit = 0;
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT exposure_limit from [users_client] where id='" + user_id + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                exposure_limit = (Double)dr["exposure_limit"];
            }
            con2.Close();
            return exposure_limit;
        }
        public Double GetCommMo(int user_id)
        {
            Double ocomm_rate = 0;
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT ocomm_rate from [users_client] where id='" + user_id + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                ocomm_rate = (Double)dr["ocomm_rate"];
            }
            con2.Close();
            return ocomm_rate;
        }
        public Double GetCommSess(int user_id)
        {
            Double scomm_rate = 0;
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT scomm_rate from [users_client] where id='" + user_id + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                scomm_rate = (Double)dr["scomm_rate"];
            }
            con2.Close();
            return scomm_rate;
        }

        public string GetSportsName(string GEtEventCode)
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
                    reader.Read();
                    string sport_id = (string)reader["sport_id"];
                    sport_name = sport_id;

                    using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con1.Open();
                        string stmt1 = "SELECT sport_name from [sports] where sport_id='" + sport_id + "'";

                        using (SqlCommand cmdCount1 = new SqlCommand(stmt1, con1))
                        {
                            cmdCount1.ExecuteScalar();
                            var reader1 = cmdCount1.ExecuteReader();
                            reader1.Read();
                            sport_name = (string)reader1["sport_name"];
                        }
                        con1.Close();
                    }
                }
                con.Close();
            }

            return sport_name;
        }

        public JsonResult DlClientList()
        {
            List<DLClientList> messages = new List<DLClientList>();
            messages = DlClientList1();
            return Json(messages, JsonRequestBehavior.AllowGet);
        }
        public List<DLClientList> DlClientList1()
        {
            var messages = new List<DLClientList>();
            try
            {
                string DL_login_user_id = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    string stmt = "select username,id from users_client where dl_id='" + DL_login_user_id + "' ";

                    using (SqlCommand cmdCount = new SqlCommand(stmt, con))
                    {
                        cmdCount.ExecuteScalar();
                        var reader = cmdCount.ExecuteReader();
                        while (reader.Read())
                        {
                            string username = (string)reader["username"];
                            int id = (int)reader["id"];

                            messages.Add(item: new DLClientList
                            {
                                username = username,
                                id = id
                            });

                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return messages;
        }
        public JsonResult BookviewSV(string bfid)
        {
            List<BookViewAg> messages = new List<BookViewAg>();
            messages = BookviewS(bfid);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<BookViewAg> BookviewS(string bfid)
        {
            Double teamAWD = 0;
            Double teamBWD = 0;
            Double teamCWD = 0;
            Double teamAWDDL = 0;
            Double teamBWDDL = 0;
            Double teamCWDDL = 0;
            var AllSportsAddM = new List<BookViewAg>();
            try
            {
                string user_ids = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT distinct(user_id) from live_bet where betfair_id='" + bfid + "' AND dist_id='" + user_ids + "' AND status='' AND odds_type='MO'", con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            int user_idN = (int)reader["user_id"];
                            string UCname = GetCUserName(user_idN);
                            using (var cmd2 = new SqlCommand("select runner_pos,amount from runner_cal where user_id='" + user_idN + "' AND market_id='" + bfid + "'", con))
                            {
                                var reader2 = cmd2.ExecuteReader();
                                while (reader2.Read())
                                {
                                    int runner_posrd = (Int32)reader2["runner_pos"];
                                    if (runner_posrd == 1)
                                    {
                                        teamAWD = (Double)reader2["amount"];
                                        teamAWDDL = teamAWDDL + teamAWD;
                                    }
                                    else if (runner_posrd == 2)
                                    {
                                        teamBWD = (Double)reader2["amount"];
                                        teamBWDDL = teamBWDDL + teamBWD;
                                    }
                                    else if (runner_posrd == 3)
                                    {
                                        teamCWD = (Double)reader2["amount"];
                                        teamCWDDL = teamCWDDL + teamCWD;
                                    }
                                    else
                                    {

                                    }
                                }
                            }

                            AllSportsAddM.Add(item: new BookViewAg
                            {
                                user_id = user_idN,
                                cname = UCname,
                                teamAWD = teamAWD,
                                teamBWD = teamBWD,
                                teamCWD = teamCWD,
                                teamAWDDL = teamAWDDL,
                                teamBWDDL = teamBWDDL,
                                teamCWDDL = teamCWDDL,
                            });



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

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "exchange");
        }

        public JsonResult SessionDldata(string betfair_id, string event_id)
        {
            List<Sessiondldata> messages = new List<Sessiondldata>();
            messages = SessionDldata1(betfair_id, event_id);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<Sessiondldata> SessionDldata1(string betfair_id, string event_id)
        {
            var messages = new List<Sessiondldata>();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("sess_ap.php?event_code=" + event_id).Result;  // Blocking call! 

                    var products = response.Content.ReadAsStringAsync().Result;

                    dynamic responseJson = JsonConvert.DeserializeObject(products);
                    string RunnerName = "";
                    Double Back3 = 0;
                    Double Back2 = 0;
                    Double Back1 = 0;
                    Double Lay3 = 0;
                    Double Lay2 = 0;
                    Double Lay1 = 0;
                    int BackSize1 = 0;
                    int LaySize1 = 0;
                    for (int i = 0; i < responseJson.Count; i++)
                    {
                        string SelectionId = responseJson[i].SelectionId;
                        if (SelectionId == betfair_id)
                        {
                            RunnerName = responseJson[i].RunnerName;
                            Back1 = responseJson[i].BackPrice1;
                            BackSize1 = responseJson[i].BackSize1;
                            Lay1 = responseJson[i].LayPrice1;
                            LaySize1 = responseJson[i].LaySize1;


                            if (Back1 > 1)
                            {
                                Back2 = Back1 + 1;
                                Back3 = Back1 + 2;
                            }
                            else
                            {
                                Back2 = 0;
                                Back3 = 0;
                            }

                            if (Lay1 > 1)
                            {
                                Lay2 = Lay1 - 1;
                                Lay3 = Lay1 - 2;
                            }
                            else
                            {
                                Lay2 = 0;
                                Lay3 = 0;
                            }
                            break;
                        }
                    }
                    string dist_id = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];

                    Double TotalA = 0;
                    Double TotalB = 0;
                    Double TotalC = 0;
                    Double TotalD = 0;
                    Double TotalE = 0;
                    Double TotalF = 0;

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        SqlCommand sqlmodel = new SqlCommand("SELECT [user_id],[stakes],[total_value],[field],[rate] FROM live_bet_new WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_id + "' AND dist_id = '" + dist_id + "' ", con);
                        var datamodel = sqlmodel.ExecuteReader();
                        if (datamodel.HasRows)
                        {
                            while (datamodel.Read())
                            {
                                Double model_stakes = (Double)datamodel["stakes"];
                                Double model_total_value = (Double)datamodel["total_value"];
                                string model_field = (string)datamodel["field"];
                                Double model_rate = (Double)datamodel["rate"];
                                if ((model_field == "Yes" && model_rate <= Back3) || (model_field == "Not" && model_rate > Back3))
                                {
                                    TotalA = TotalA - model_total_value;
                                }
                                else if ((model_field == "Yes" && model_rate > Back3) || (model_field == "Not" && model_rate <= Back3))
                                {
                                    TotalA = TotalA + model_stakes;
                                }

                                if ((model_field == "Yes" && model_rate <= Back2) || (model_field == "Not" && model_rate > Back2))
                                {
                                    TotalB = TotalB - model_total_value;
                                }
                                else if ((model_field == "Yes" && model_rate > Back2) || (model_field == "Not" && model_rate <= Back2))
                                {
                                    TotalB = TotalB + model_stakes;
                                }

                                if ((model_field == "Yes" && model_rate <= Back1) || (model_field == "Not" && model_rate > Back1))
                                {
                                    TotalC = TotalC - model_total_value;
                                }
                                else if ((model_field == "Yes" && model_rate > Back1) || (model_field == "Not" && model_rate <= Back1))
                                {
                                    TotalC = TotalC + model_stakes;
                                }

                                if ((model_field == "Yes" && model_rate <= Lay1) || (model_field == "Not" && model_rate > Lay1))
                                {
                                    TotalD = TotalD - model_total_value;
                                }
                                else if ((model_field == "Yes" && model_rate > Lay1) || (model_field == "Not" && model_rate <= Lay1))
                                {
                                    TotalD = TotalD + model_stakes;
                                }

                                if ((model_field == "Yes" && model_rate <= Lay2) || (model_field == "Not" && model_rate > Lay2))
                                {
                                    TotalE = TotalE - model_total_value;
                                }
                                else if ((model_field == "Yes" && model_rate > Lay2) || (model_field == "Not" && model_rate <= Lay2))
                                {
                                    TotalE = TotalE + model_stakes;
                                }

                                if ((model_field == "Yes" && model_rate <= Lay3) || (model_field == "Not" && model_rate > Lay3))
                                {
                                    TotalF = TotalF - model_total_value;
                                }
                                else if ((model_field == "Yes" && model_rate > Lay3) || (model_field == "Not" && model_rate <= Lay3))
                                {
                                    TotalF = TotalF + model_stakes;
                                }
                            }
                        }

                        messages.Add(item: new Sessiondldata
                        {
                            marketna = RunnerName,
                            Back1 = Back1,
                            Back2 = Back2,
                            Back3 = Back3,
                            Backsize1 = BackSize1,
                            Lay1 = Lay1,
                            Lay2 = Lay2,
                            Lay3 = Lay3,
                            Laysize1 = LaySize1,
                            DlRatioA = TotalF,
                            DlRatioB = TotalE,
                            DlRatioC = TotalD,
                            DlRatioD = TotalC,
                            DlRatioE = TotalB,
                            DlRatioF = TotalA,
                        });
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

        public JsonResult SessionDldatabk(string betfair_id, string event_id)
        {
            List<Sessiondldata> messages = new List<Sessiondldata>();
            messages = SessionDldata2(betfair_id, event_id);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<Sessiondldata> SessionDldata2(string betfair_id, string event_id)
        {
            var messages = new List<Sessiondldata>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("sess_ap.php?event_code=" + event_id).Result;  // Blocking call! 

                    var products = response.Content.ReadAsStringAsync().Result;

                    dynamic responseJson = JsonConvert.DeserializeObject(products);
                    string RunnerName = "";
                    Double Back3 = 0;
                    Double Back2 = 0;
                    Double Back1 = 0;
                    Double Lay3 = 0;
                    Double Lay2 = 0;
                    Double Lay1 = 0;
                    int BackSize1 = 0;
                    int LaySize1 = 0;
                    for (int i = 0; i < responseJson.Count; i++)
                    {
                        string SelectionId = responseJson[i].SelectionId;
                        if (SelectionId == betfair_id)
                        {
                            RunnerName = responseJson[i].RunnerName;
                            Back1 = responseJson[i].BackPrice1;
                            BackSize1 = responseJson[i].BackSize1;
                            Lay1 = responseJson[i].LayPrice1;
                            LaySize1 = responseJson[i].LaySize1;

                            if (Back1 > 1)
                            {
                                Back2 = Back1 + 1;
                                Back3 = Back1 + 2;
                            }
                            else
                            {
                                Back2 = 0;
                                Back3 = 0;
                            }

                            if (Lay1 > 1)
                            {
                                Lay2 = Lay1 - 1;
                                Lay3 = Lay1 - 2;
                            }
                            else
                            {
                                Lay2 = 0;
                                Lay3 = 0;
                            }
                            break;
                        }
                    }
                    string dist_id = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];

                    var user_id = new List<int>();
                    var stakes = new List<Double>();
                    var total_value = new List<Double>();
                    var field = new List<string>();
                    var rate = new List<Double>();

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        SqlCommand sqlmodel = new SqlCommand("SELECT [user_id],[stakes],[total_value],[field],[rate] FROM live_bet_new WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_id + "' AND dist_id = '" + dist_id + "' ", con);
                        var datamodel = sqlmodel.ExecuteReader();
                        if (datamodel.HasRows)
                        {
                            user_id.Clear();
                            stakes.Clear();
                            total_value.Clear();
                            field.Clear();
                            rate.Clear();
                            while (datamodel.Read())
                            {
                                int user_id1 = (int)datamodel["user_id"];
                                Double stakes1 = (Double)datamodel["stakes"];
                                Double total_value1 = (Double)datamodel["total_value"];
                                string field1 = (string)datamodel["field"];
                                Double rate1 = (Double)datamodel["rate"];
                                user_id.Add(user_id1);
                                stakes.Add(stakes1);
                                total_value.Add(total_value1);
                                field.Add(field1);
                                rate.Add(rate1);
                            }
                        }
                        SqlCommand sqlm = new SqlCommand("SELECT DISTINCT [user_id] FROM live_bet_new WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_id + "' AND dist_id = '" + dist_id + "' ", con);
                        var datam = sqlm.ExecuteReader();
                        if (datam.HasRows)
                        {
                            while (datam.Read())
                            {
                                int user_id01 = (Int32)datam["user_id"];
                                string client_name1 = GetCUserName(user_id01);

                                Double TotalA = 0;
                                Double TotalB = 0;
                                Double TotalC = 0;
                                Double TotalD = 0;
                                Double TotalE = 0;
                                Double TotalF = 0;
                                for (int i = 0; i < user_id.Count; i++)
                                {
                                    int model_uid = user_id[i];
                                    Double model_stakes = stakes[i];
                                    Double model_total_value = total_value[i];
                                    string model_field = field[i].ToString();
                                    Double model_rate = rate[i];
                                    if (model_uid == user_id01)
                                    {
                                        if ((model_field == "Yes" && model_rate <= Back3) || (model_field == "Not" && model_rate > Back3))
                                        {
                                            TotalA = TotalA - model_total_value;
                                        }
                                        else if ((model_field == "Yes" && model_rate > Back3) || (model_field == "Not" && model_rate <= Back3))
                                        {
                                            TotalA = TotalA + model_stakes;
                                        }

                                        if ((model_field == "Yes" && model_rate <= Back2) || (model_field == "Not" && model_rate > Back2))
                                        {
                                            TotalB = TotalB - model_total_value;
                                        }
                                        else if ((model_field == "Yes" && model_rate > Back2) || (model_field == "Not" && model_rate <= Back2))
                                        {
                                            TotalB = TotalB + model_stakes;
                                        }

                                        if ((model_field == "Yes" && model_rate <= Back1) || (model_field == "Not" && model_rate > Back1))
                                        {
                                            TotalC = TotalC - model_total_value;
                                        }
                                        else if ((model_field == "Yes" && model_rate > Back1) || (model_field == "Not" && model_rate <= Back1))
                                        {
                                            TotalC = TotalC + model_stakes;
                                        }

                                        if ((model_field == "Yes" && model_rate <= Lay1) || (model_field == "Not" && model_rate > Lay1))
                                        {
                                            TotalD = TotalD - model_total_value;
                                        }
                                        else if ((model_field == "Yes" && model_rate > Lay1) || (model_field == "Not" && model_rate <= Lay1))
                                        {
                                            TotalD = TotalD + model_stakes;
                                        }

                                        if ((model_field == "Yes" && model_rate <= Lay2) || (model_field == "Not" && model_rate > Lay2))
                                        {
                                            TotalE = TotalE - model_total_value;
                                        }
                                        else if ((model_field == "Yes" && model_rate > Lay2) || (model_field == "Not" && model_rate <= Lay2))
                                        {
                                            TotalE = TotalE + model_stakes;
                                        }

                                        if ((model_field == "Yes" && model_rate <= Lay3) || (model_field == "Not" && model_rate > Lay3))
                                        {
                                            TotalF = TotalF - model_total_value;
                                        }
                                        else if ((model_field == "Yes" && model_rate > Lay3) || (model_field == "Not" && model_rate <= Lay3))
                                        {
                                            TotalF = TotalF + model_stakes;
                                        }
                                    }
                                }
                                messages.Add(item: new Sessiondldata
                                {
                                    marketna = RunnerName,
                                    client_id = user_id01,
                                    client_name = client_name1,
                                    Back1 = Back1,
                                    Back2 = Back2,
                                    Back3 = Back3,
                                    Backsize1 = BackSize1,
                                    Lay1 = Lay1,
                                    Lay2 = Lay2,
                                    Lay3 = Lay3,
                                    Laysize1 = LaySize1,
                                    DlRatioA = TotalF,
                                    DlRatioB = TotalE,
                                    DlRatioC = TotalD,
                                    DlRatioD = TotalC,
                                    DlRatioE = TotalB,
                                    DlRatioF = TotalA,
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

        public ActionResult sbookview()
        {
            string betfair_id = Request.QueryString["betfair_id"];
            string event_id = Request.QueryString["event_code"];
            ViewBag.GetEventname = GetEventTitleName(event_id);
            ViewBag.betfair_id = betfair_id;
            ViewBag.event_id = event_id;
            return View();
        }

        public string BlockSport(string sport_id)
        {
            string ReturnMessage = "Error";
            try
            {
                if (sport_id != "")
                {
                    string DL_login_user_idin = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                    using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con1.Open();
                        using (SqlCommand cmdCount1 = new SqlCommand("SELECT id FROM block_sport WHERE dl_id='" + DL_login_user_idin + "' AND sport_id='" + sport_id + "' ", con1))
                        {
                            var reader1 = cmdCount1.ExecuteReader();
                            if (reader1.HasRows)
                            {
                                SqlCommand block_sport_delete = new SqlCommand("Delete FROM block_sport where dl_id='" + DL_login_user_idin + "' AND sport_id='" + sport_id + "'", con1);
                                int CheckDataDelete = block_sport_delete.ExecuteNonQuery();
                                if (CheckDataDelete > 0)
                                {
                                    ReturnMessage = "Success";
                                }
                            }
                            else
                            {
                                SqlCommand block_sport_insert = new SqlCommand("INSERT INTO block_sport(admin_id, md_id, dl_id,user_id, sport_id, status, created) VALUES ('0','0','" + DL_login_user_idin + "','0','" + sport_id + "','yes','" + time.ToString(format) + "')", con1);
                                int CheckDataInsert = block_sport_insert.ExecuteNonQuery();
                                if (CheckDataInsert > 0)
                                {
                                    ReturnMessage = "Success";
                                }
                            }

                        }
                        con1.Close();
                    }
                    ReturnMessage = "Success";
                }
                else
                {
                    ReturnMessage = "Error";
                }
            }
            catch (Exception ex)
            {

            }
            return ReturnMessage;
        }

        public string BlockLeague(string league_id)
        {
            string ReturnMessage = "Error";
            try
            {
                if (league_id != "")
                {
                    string DL_login_user_idin = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                    using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con1.Open();
                        using (SqlCommand cmdCount1 = new SqlCommand("SELECT id FROM block_league WHERE dl_id='" + DL_login_user_idin + "' AND league_id='" + league_id + "' ", con1))
                        {
                            var reader1 = cmdCount1.ExecuteReader();
                            if (reader1.HasRows)
                            {
                                SqlCommand block_league_delete = new SqlCommand("Delete FROM block_league where dl_id='" + DL_login_user_idin + "' AND league_id='" + league_id + "'", con1);
                                int CheckDataDelete = block_league_delete.ExecuteNonQuery();
                                if (CheckDataDelete > 0)
                                {
                                    ReturnMessage = "Success";
                                }
                            }
                            else
                            {
                                SqlCommand block_league_insert = new SqlCommand("INSERT INTO block_league(admin_id, md_id, dl_id,user_id, league_id, status, created) VALUES ('0','0','" + DL_login_user_idin + "','0','" + league_id + "','yes','" + time.ToString(format) + "')", con1);
                                int CheckDataInsert = block_league_insert.ExecuteNonQuery();
                                if (CheckDataInsert > 0)
                                {
                                    ReturnMessage = "Success";
                                }
                            }

                        }
                        con1.Close();
                    }
                    ReturnMessage = "Success";
                }
                else
                {
                    ReturnMessage = "Error";
                }
            }
            catch (Exception ex)
            {

            }
            return ReturnMessage;
        }

        public JsonResult BlockLeagueList(string sport_id)
        {
            List<NavbarListAllSports> messages = new List<NavbarListAllSports>();
            messages = BlockLeagueListL(sport_id);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<NavbarListAllSports> BlockLeagueListL(string sport_id)
        {
            var AllSportsAddM = new List<NavbarListAllSports>();
            try
            {
                string DL_login_user_idin = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT league_name,league_id FROM league where sport_id='" + sport_id + "' AND EXISTS (SELECT event_code FROM matches  WHERE sport_id='" + sport_id + "' AND league.league_id = matches.league_id AND betfair_id!='0' AND teama!='nikhil' AND status = 'OPEN') ", con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string LeagueName = (string)reader["league_name"];
                            string league_id = (string)reader["league_id"];
                            string is_block = "no";
                            using (var cmd1 = new SqlCommand("SELECT id FROM block_league where dl_id='" + DL_login_user_idin + "' AND  league_id='" + league_id + "' AND status='yes' ", con))
                            {
                                var reader1 = cmd1.ExecuteReader();
                                if (reader1.HasRows)
                                {
                                    is_block = "yes";
                                }
                            }

                            AllSportsAddM.Add(item: new NavbarListAllSports
                            {
                                SportsName = LeagueName,
                                SportsId = league_id,
                                IsBlock = is_block
                            });
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

        public string BlockEvent(string event_code)
        {
            string ReturnMessage = "Error";
            try
            {
                if (event_code != "")
                {
                    string DL_login_user_idin = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                    using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con1.Open();
                        using (SqlCommand cmdCount1 = new SqlCommand("SELECT id FROM block_event WHERE dl_id='" + DL_login_user_idin + "' AND event_code='" + event_code + "' ", con1))
                        {
                            var reader1 = cmdCount1.ExecuteReader();
                            if (reader1.HasRows)
                            {
                                SqlCommand block_Event_delete = new SqlCommand("Delete FROM block_event where dl_id='" + DL_login_user_idin + "' AND event_code='" + event_code + "'", con1);
                                int CheckDataDelete = block_Event_delete.ExecuteNonQuery();
                                if (CheckDataDelete > 0)
                                {
                                    ReturnMessage = "Success";
                                }
                            }
                            else
                            {
                                SqlCommand block_Event_insert = new SqlCommand("INSERT INTO block_event(admin_id, md_id, dl_id,user_id, event_code, status, created) VALUES ('0','0','" + DL_login_user_idin + "','0','" + event_code + "','yes','" + time.ToString(format) + "')", con1);
                                int CheckDataInsert = block_Event_insert.ExecuteNonQuery();
                                if (CheckDataInsert > 0)
                                {
                                    ReturnMessage = "Success";
                                }
                            }

                        }
                        con1.Close();
                    }
                    ReturnMessage = "Success";
                }
                else
                {
                    ReturnMessage = "Error";
                }
            }
            catch (Exception ex)
            {

            }
            return ReturnMessage;
        }

        public JsonResult BlockEventList(string league_id)
        {
            List<NavbarListAllSports> messages = new List<NavbarListAllSports>();
            messages = BlockEventListL(league_id);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<NavbarListAllSports> BlockEventListL(string league_id)
        {
            var AllSportsAddM = new List<NavbarListAllSports>();
            try
            {
                string DL_login_user_idin = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT [match_title],[event_code],[sport_id] FROM matches where league_id='" + league_id + "' AND status='OPEN' ", con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string EventName = (string)reader["match_title"];
                            string event_code = (string)reader["event_code"];
                            string sport_id = (string)reader["sport_id"];
                            string is_block = "no";
                            using (var cmd1 = new SqlCommand("SELECT id FROM block_event where dl_id='" + DL_login_user_idin + "' AND  event_code='" + event_code + "' AND status='yes' ", con))
                            {
                                var reader1 = cmd1.ExecuteReader();
                                if (reader1.HasRows)
                                {
                                    is_block = "yes";
                                }
                            }

                            AllSportsAddM.Add(item: new NavbarListAllSports
                            {
                                SportsName = EventName,
                                SportsId = event_code,
                                ForTest = sport_id,
                                IsBlock = is_block
                            });
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
        public string BlockMarkets(string event_code, string type, string status)
        {
            string ReturnMessage = "Error";
            try
            {
                if (event_code != "")
                {
                    string DL_login_user_idin = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                    using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con1.Open();
                        using (SqlCommand cmdCount1 = new SqlCommand("SELECT id FROM block_market WHERE dl_id='" + DL_login_user_idin + "' AND event_code='" + event_code + "' ", con1))
                        {
                            var reader1 = cmdCount1.ExecuteReader();
                            if (reader1.HasRows)
                            {
                                SqlCommand block_Event_delete = new SqlCommand("UPDATE block_market set " + type + "='" + status + "' where dl_id='" + DL_login_user_idin + "' AND event_code='" + event_code + "'", con1);
                                int CheckDataDelete = block_Event_delete.ExecuteNonQuery();
                                if (CheckDataDelete > 0)
                                {
                                    ReturnMessage = "Success";
                                }
                            }
                            else
                            {
                                var MODDS = "no";
                                var OODDS = "no";
                                var SESS = "no";
                                var FANCY = "no";
                                if (type == "match_odds")
                                {
                                    MODDS = status;
                                }
                                else if (type == "other_odds")
                                {
                                    OODDS = status;
                                }
                                else if (type == "session")
                                {
                                    SESS = status;
                                }
                                else if (type == "fancy")
                                {
                                    FANCY = status;
                                }
                                SqlCommand block_Event_insert = new SqlCommand("INSERT INTO block_market(admin_id, md_id, dl_id,user_id, event_code, match_odds, other_odds, session, fancy, status, created) VALUES ('0','0','" + DL_login_user_idin + "','0','" + event_code + "','" + MODDS + "','" + OODDS + "','" + SESS + "','" + FANCY + "','yes','" + time.ToString(format) + "')", con1);
                                int CheckDataInsert = block_Event_insert.ExecuteNonQuery();
                                if (CheckDataInsert > 0)
                                {
                                    ReturnMessage = "Success";
                                }
                            }

                        }
                        con1.Close();
                    }
                    ReturnMessage = "Success";
                }
                else
                {
                    ReturnMessage = "Error";
                }
            }
            catch (Exception ex)
            {

            }
            return ReturnMessage;
        }

        public JsonResult BlockMarketList(string event_code, string sport_id)
        {
            List<NavbarListAllSports> messages = new List<NavbarListAllSports>();
            messages = BlockMarketListL(event_code, sport_id);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<NavbarListAllSports> BlockMarketListL(string event_code, string sport_id)
        {
            var AllSportsAddM = new List<NavbarListAllSports>();
            try
            {
                string DL_login_user_idin = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                int countmk = 2;
                if (sport_id == "4")
                {
                    countmk = 4;
                }
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {

                    using (var cmd = new SqlCommand("SELECT match_odds,other_odds,session,fancy FROM block_market where event_code='" + event_code + "' AND status='yes' ", con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        string Match_Odds = "no";
                        string Other_Odds = "no";
                        string Sess = "no";
                        string Fancy = "no";
                        string is_block = "no";
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Match_Odds = (string)reader["match_odds"];
                            Other_Odds = (string)reader["other_odds"];
                            Sess = (string)reader["session"];
                            Fancy = (string)reader["fancy"];
                            is_block = "no";
                        }
                        con.Close();
                        for (int i = 0; i < countmk; i++)
                        {
                            string MarketName = "";
                            if (i == 0)
                            {
                                MarketName = "match_odds";
                                is_block = Match_Odds;
                            }
                            else if (i == 1)
                            {
                                MarketName = "other_odds";
                                is_block = Other_Odds;
                            }
                            else if (i == 2)
                            {
                                MarketName = "session";
                                is_block = Sess;
                            }
                            else if (i == 3)
                            {
                                MarketName = "fancy";
                                is_block = Fancy;
                            }
                            AllSportsAddM.Add(item: new NavbarListAllSports
                            {
                                SportsName = MarketName,
                                IsBlock = is_block
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return AllSportsAddM;
        }

        /*FOr Calculation part start.....*/

        public Double getTClientBal(int mdlid)
        {
            Double sport_id = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT COALESCE(sum(balance),0) AS t_bal FROM users_client WHERE dl_id='" + mdlid + "'", con))
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    sport_id = (Double)reader["t_bal"];
                }
                con.Close();
            }
            return sport_id;
        }

        public Double getDLClientLib(int mdlid)
        {
            Double sport_id = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT COALESCE(sum(exposure),0) AS t_bal FROM users_client WHERE dl_id='" + mdlid + "'", con))
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    sport_id = (Double)reader["t_bal"];
                }
                con.Close();
            }
            return sport_id;
        }

        public Double GetDLBalance(int user_id)
        {
            Double username = 0;
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT balance from [distributors] where id='" + user_id + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                username = (Double)dr["balance"];
            }
            con2.Close();
            return username;
        }

        public JsonResult ShowBetsOnClick(string user_id, string event_id, string market_id)
        {
            List<ShowBetsOA> messages = new List<ShowBetsOA>();
            messages = ShowBetsOnClick1(user_id, event_id, market_id);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<ShowBetsOA> ShowBetsOnClick1(string user_id, string event_id, string market_id)
        {
            var messages = new List<ShowBetsOA>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                string query = " select id,field,rate,stakes,field_pos,place_time,total_value,status,odds_type from live_bet where user_id='" + user_id + "' and event_id='" + event_id + "' and betfair_id='" + market_id + "' ";
                using (var cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = (int)reader["id"];
                            string field = (string)reader["field"];
                            string field_pos = (string)reader["field_pos"];
                            string status = (string)reader["status"];
                            Double rate = (Double)reader["rate"];
                            Double stakes = (Double)reader["stakes"];
                            stakes = System.Math.Round(stakes, 2);
                            Double total_value = (Double)reader["total_value"];
                            total_value = System.Math.Round(total_value, 2);
                            DateTime created1 = (DateTime)reader["place_time"];
                            string place_time = created1.ToString("yyyy-MM-dd HH:mm:ss");
                            Double profit_loss = 0;
                            string BetName = (string)reader["odds_type"];
                            string BetType = "";
                            Double stakes1 = stakes;
                            if (field_pos == "lay")
                            {
                                stakes1 = total_value;
                            }
                            if (BetName == "sess")
                            {
                                if (field_pos == "a")
                                {
                                    BetType = "lay";
                                }
                                else if (field_pos == "b")
                                {
                                    BetType = "back";
                                }
                            }
                            else
                            {
                                BetType = field_pos;
                            }
                            if (status == "won")
                            {
                                profit_loss = total_value;
                            }
                            else if (status == "lost")
                            {
                                profit_loss = -stakes;
                            }
                            messages.Add(item: new ShowBetsOA
                            {
                                betid = id,
                                Type = BetType,
                                Selection = field,
                                rate = rate,
                                Stake = stakes1,
                                Placed = place_time,
                                profit_loss = profit_loss
                            });
                        }
                    }
                    con.Close();
                }


            }
            return messages;

        }

        public ActionResult TeenRisk()
        {
            return View();
        }
        public ActionResult TPbookview()
        {
            string event_id = Request.QueryString["event_code"];
            string betfair_id = Request.QueryString["betfair_id"];
            string EventNameGet = GetEventTitleName(event_id);
            ViewBag.EventNameB = EventNameGet;
            ViewBag.BetfairId = betfair_id;
            ViewBag.event_id = event_id;
            return View();
        }
       /* public ActionResult teendlrisk()
        {
            *//*Teendlrepos _teenRepository = new Teendlrepos();
            return PartialView("teendlrisk", _teenRepository.GetAllMessages());*//*
        }*/

        public JsonResult TPBookviewSV(string bfid)
        {
            List<BookViewAg> messages = new List<BookViewAg>();
            messages = TPBookviewS(bfid);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<BookViewAg> TPBookviewS(string bfid)
        {
            var AllSportsAddM = new List<BookViewAg>();
            try
            {
                string user_ids = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT distinct(user_id) from live_bet where betfair_id='" + bfid + "' AND dist_id='" + user_ids + "' AND status='' AND odds_type='TP'", con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            int user_idN = (int)reader["user_id"];
                            string UCname = GetCUserName(user_idN);
                            Double TotalamountA = 0;
                            Double TotalamountB = 0;
                            Double TotalamountC = 0;
                            SqlCommand sqlLost = new SqlCommand("SELECT stakes,total_value,runner_posi,event_id FROM live_bet WHERE betfair_id='" + bfid + "' AND user_id='" + user_idN + "' ", con);
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
                                    else if (runner_pos == 41)
                                    {
                                        TotalamountA = TotalamountA - stakes;
                                        TotalamountB = TotalamountB - stakes;
                                        TotalamountC = TotalamountC + total_value;
                                    }
                                }
                            }

                            AllSportsAddM.Add(item: new BookViewAg
                            {
                                user_id = user_idN,
                                cname = UCname,
                                teamAWD = TotalamountA,
                                teamBWD = TotalamountB,
                                teamCWD = TotalamountC,
                                teamAWDDL = 0,
                                teamBWDDL = 0,
                                teamCWDDL = 0,
                            });



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

        public ActionResult SPoPl()
        {
            string user_ids = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
            string startDate = Request.QueryString["startDate"];
            string endDate = Request.QueryString["endDate"];
            var spStat = new List<sportsStat>();
            if (startDate != null && startDate != "" && endDate != null && endDate != "")
            {
                try
                {
                    var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                    SqlConnection con2 = new SqlConnection(connectionString);
                    con2.Open();
                    SqlCommand crkt = new SqlCommand();
                    SqlDataReader dr;
                    crkt.Connection = con2;
                    crkt.CommandType = System.Data.CommandType.Text;
                    crkt.CommandText = "select distinct(user_id) from pl_statement where dist_id='" + user_ids + "' AND created BETWEEN '" + startDate + "' AND '" + endDate + "' and user_id!='0'";
                    dr = crkt.ExecuteReader();
                    if (dr.HasRows)
                    {
                        string nm = "";
                        Double cr = 0;
                        Double soc = 0;
                        Double ten = 0;
                        while (dr.Read())
                        {
                            int i = (int)dr["user_id"];
                            nm = GetMDLName(i);
                            cr = getMDLCrktPL(i, 4, startDate, endDate);
                            soc = getMDLSoccPL( i, 1, startDate, endDate);
                            ten = getMDLTennPL( i, 2, startDate, endDate);
                            spStat.Add(item: new sportsStat
                            {
                                naam = nm,
                                crkt = cr,
                                socc = soc,
                                tenn = ten,
                                tot = cr + soc + ten
                            });
                        }
                        con2.Close();
                    }
                }
                catch (SqlException ex)
                {

                }
            }
            return View("SPoPl", spStat);
        }

        public ActionResult UsePL()
        {
            string user_ids = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
            string startDate = Request.QueryString["startDate"];
            string endDate = Request.QueryString["endDate"];
            var spStat = new List<sportsStat>();
            if (startDate != null && startDate != "" && endDate != null && endDate != "")
            {
                try
                {
                    var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                    SqlConnection con2 = new SqlConnection(connectionString);
                    con2.Open();
                    SqlCommand crkt = new SqlCommand();
                    SqlDataReader dr;
                    crkt.Connection = con2;
                    crkt.CommandType = System.Data.CommandType.Text;
                    crkt.CommandText = "select distinct(user_id) from pl_statement where dist_id='" + user_ids + "' AND created BETWEEN '" + startDate + "' AND '" + endDate + "'";
                    dr = crkt.ExecuteReader();
                    if (dr.HasRows)
                    {
                        string nm = "";
                        Double cr = 0;
                        Double soc = 0;
                        Double ten = 0;
                        while (dr.Read())
                        {
                            int i = (int)dr["user_id"];
                            nm = GetMDLName(i);
                            cr = getMDLCrktPL(i, 4, startDate, endDate);
                            soc = getMDLSoccPL(i, 1, startDate, endDate);
                            ten = getMDLTennPL(i, 2, startDate, endDate);
                            spStat.Add(item: new sportsStat
                            {
                                naam = nm,
                                crkt = cr,
                                socc = soc,
                                tenn = ten,
                                tot = cr + soc + ten
                            });
                        }
                        con2.Close();
                    }
                }
                catch (SqlException ex)
                {

                }
            }
            return View("SPoPl", spStat);
        }
        public double getMDLCrktPL(int mid, int spt, string startDate, string endDate)
        {
            Double crktpl = 0;
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection con2 = new SqlConnection(connectionString);
                con2.Open();
                //cricket
                SqlCommand crkt = new SqlCommand();
                SqlDataReader dr;
                crkt.Connection = con2;
                crkt.CommandType = System.Data.CommandType.StoredProcedure;
                crkt.CommandText = "sp_userSprtPL";
                crkt.Parameters.Add("@sprt", System.Data.SqlDbType.VarChar).Value = spt;
                crkt.Parameters.Add("@mdl", System.Data.SqlDbType.VarChar).Value = mid;
                crkt.Parameters.Add("@startDate", System.Data.SqlDbType.VarChar).Value = startDate;
                crkt.Parameters.Add("@endDate", System.Data.SqlDbType.VarChar).Value = endDate;
                dr = crkt.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    crktpl = Math.Round((Double)dr["tot_pl"], 2);
                    con2.Close();
                }
            }
            catch (SqlException ex)
            {

            }
            return crktpl;
        }
        public double getMDLSoccPL(int mid, int spt, string startDate, string endDate)
        {
            Double soccpl = 0;
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection con2 = new SqlConnection(connectionString);
                con2.Open();
                //cricket
                SqlCommand crkt = new SqlCommand();
                SqlDataReader dr;
                crkt.Connection = con2;
                crkt.CommandType = System.Data.CommandType.StoredProcedure;
                crkt.CommandText = "sp_userSprtPL";
                crkt.Parameters.Add("@sprt", System.Data.SqlDbType.VarChar).Value = spt;
                crkt.Parameters.Add("@mdl", System.Data.SqlDbType.VarChar).Value = mid;
                crkt.Parameters.Add("@startDate", System.Data.SqlDbType.VarChar).Value = startDate;
                crkt.Parameters.Add("@endDate", System.Data.SqlDbType.VarChar).Value = endDate;
                dr = crkt.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    soccpl = Math.Round((Double)dr["tot_pl"], 2);
                    con2.Close();
                }
            }
            catch (SqlException ex)
            {
            }
            return soccpl;
        }
        public double getMDLTennPL(int mid, int spt, string startDate, string endDate)
        {
            Double tennpl = 0;
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection con2 = new SqlConnection(connectionString);
                con2.Open();
                //cricket
                SqlCommand crkt = new SqlCommand();
                SqlDataReader dr;
                crkt.Connection = con2;
                crkt.CommandType = System.Data.CommandType.StoredProcedure;
                crkt.CommandText = "sp_userSprtPL";
                crkt.Parameters.Add("@sprt", System.Data.SqlDbType.VarChar).Value = spt;
                crkt.Parameters.Add("@mdl", System.Data.SqlDbType.VarChar).Value = mid;
                crkt.Parameters.Add("@startDate", System.Data.SqlDbType.VarChar).Value = startDate;
                crkt.Parameters.Add("@endDate", System.Data.SqlDbType.VarChar).Value = endDate;
                dr = crkt.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    tennpl = Math.Round((Double)dr["tot_pl"], 2);
                    con2.Close();
                }
            }
            catch (SqlException ex)
            {
            }
            return tennpl;
        }
        public string GetMDLName(int id)
        {
            string dlNm = "";
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT username FROM users_client WHERE id='" + id + "' ", con))
                    {
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            dlNm = (string)dr["username"];
                        }
                        con.Close();
                    }
                }
            }
            catch (SqlException ex) { }

            return dlNm;
        }
        public string GetMDLUserName()
        {
            string profit_loss = "";
            try
            {
                string id = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("select mdl.username from masterdistributors mdl inner join distributors dl on dl.md_id = mdl.id where dl.id = '" + id + "' ", con))
                    {
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            profit_loss = (string)dr["username"];
                        }
                        con.Close();
                    }
                }
            }
            catch (SqlException ex) { }
            return profit_loss;
        }

        public ActionResult chipsummary()
        {
            ArrayList ChipSum = new ArrayList();
            ArrayList ChipSumP = new ArrayList();
            ArrayList profitlos = new ArrayList();
            ArrayList profitlosP = new ArrayList();
            ArrayList UserId = new ArrayList();
            ArrayList UserIdP = new ArrayList();
            string fhbgh = "";
            Double profit_loss = 0;
            Double md_coin = 0;
            Double dl_coin = 0;
            Double Tota2 = 0;
            Double Tota8 = 0;
            Double Tota9 = 0;
            Double Tota3 = 0;
            Double Tota4 = 0;
            try
            {
                string DL_id = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                if (DL_id != null && DL_id !="")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        using (var cmd = new SqlCommand("SELECT id,mdl_id,username,profit_loss FROM users_client WHERE dl_id='" + DL_id + "' ", con))
                        {
                            con.Open();
                            var dr = cmd.ExecuteReader();
                            Double TPRLoss = GetPLTotal();
                            Double TPRLossAP = GetPLTotalAP();
                            Double MdlTPRLoss = GetMDLPLTotal();
                            Double CashRec = getDLCash();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    profit_loss = Math.Round((Double)dr["profit_loss"], 2);
                                    string username = (string)dr["username"];
                                    // md_coin = getMDCOIN((int)dr["mdl_id"]);
                                    int userID = (int)dr["id"];
                                    // dl_coin = getDLCOIN(dlID);
                                    //ViewBag.cfrfsw = credit;
                                    if (profit_loss < 0)//Admin3 Minus
                                    {
                                        ChipSum.Add(username);
                                        profitlos.Add(Math.Abs(profit_loss));
                                        UserId.Add(userID);
                                        /*ViewBag.Dl_Tot = profit_loss * dl_coin / 100;
                                        ViewBag.MDl_Tot = profit_loss * md_coin / 100;*/
                                        fhbgh = "Minus";
                                    }
                                    else//Admin3 Plus
                                    {
                                        ChipSumP.Add(username);
                                        profitlosP.Add(profit_loss);
                                        UserIdP.Add(userID);
                                        fhbgh = "Plus";
                                    }

                                }
                                if (CashRec < 0)
                                {
                                    ViewBag.DlCashP = CashRec;
                                }
                                else
                                {
                                    ViewBag.DlCash = CashRec;
                                }
                                if (TPRLoss < TPRLossAP)
                                {
                                    ViewBag.Dl_Tot = Math.Abs(TPRLoss);
                                    ViewBag.MDl_Tot = Math.Abs(MdlTPRLoss);
                                    ViewBag.DlProYaN = "minus";
                                }
                                else
                                {
                                    ViewBag.Dl_TotP = Math.Abs(TPRLoss);
                                    ViewBag.MDl_TotP = Math.Abs(MdlTPRLoss);
                                    ViewBag.DlProYaN = "plus";
                                }
                            }
                            con.Close();
                        }

                    }
                    ViewBag.UNMin = ChipSum;
                    ViewBag.UNPlu = ChipSumP;
                    ViewBag.profitlos = profitlos;
                    ViewBag.profitlosP = profitlosP;
                    ViewBag.TPRLoss = Math.Abs(Tota2).ToString();
                    ViewBag.UserIdP = UserIdP;
                    ViewBag.UserId = UserId;
                    ViewBag.MDlUname = GetMDLUserName();
                }
                else
                {
                    CheckSession_DL();
                }
               
            }
            catch (SqlException ex)
            {

            }
            return View();
        }

        public Double GetPLTotal()
        {
            Double profit_loss = 0;
            string DL_id = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT profit_loss FROM distributors WHERE id='" + DL_id + "' ", con))
                    {
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            profit_loss = Math.Round((Double)dr["profit_loss"], 2);
                        }
                        con.Close();
                    }
                }
            }
            catch (SqlException ex)
            {

            }
            return profit_loss;
        }

        public Double GetPLTotalAP()
        {
            Double profit_loss = 0;
            string DL_id = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT COALESCE(sum(profit_loss),0) AS profit_loss FROM users_client WHERE dl_id='" + DL_id + "' ", con))
                    {
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            profit_loss = Math.Round((Double)dr["profit_loss"], 2);
                        }
                        con.Close();
                    }
                }
            }
            catch (SqlException ex)
            {

            }
            return profit_loss;
        }
        static public Double getDLCash()
        {
            Double MDCOIN = 0;
            string md_id = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT cash from distributors where id='" + md_id + "'"))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    MDCOIN = (Double)reader["cash"];
                    con.Close();
                }
            }
            return MDCOIN;
        }

        public Double GetMDLPLTotal()
        {
            Double profit_loss = 0;
            string DL_id = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT mdl_profit_loss FROM distributors WHERE id='" + DL_id + "' ", con))
                    {
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            profit_loss = Math.Round((Double)dr["mdl_profit_loss"], 2);
                        }
                        con.Close();
                    }
                }
            }
            catch (SqlException ex)
            {

            }
            return profit_loss;
        }
        public string submitClearChip(string UserID, Double Chips, string IsFree)
        {
            string ReturnMSG = "";
            Double debit = 0;
            Double credit = 0;
            try
            {
                if (IsFree == "debit")
                {
                    debit = Chips;
                }
                else if (IsFree == "credit")
                {
                    credit = Chips;
                }
                if (UserID != "" && Chips != 0 && UserID != "0")
                {
                    DateTime time = DateTime.Now;
                    string format = "yyyy-MM-dd HH:mm:ss";
                    string DL_login_user_idin = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                    string DL_login_username = (string)System.Web.HttpContext.Current.Session["DL_UserName"];

                    Double GetDlBal = GetDLChips(DL_login_user_idin);
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        string stmt = "SELECT mdl_id,admin_id,profit_loss,username FROM users_client WHERE id='" + UserID + "' AND dl_id='" + DL_login_user_idin + "' ";

                        using (SqlCommand cmdCount = new SqlCommand(stmt, con))
                        {
                            cmdCount.ExecuteScalar();
                            var reader = cmdCount.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    int md_id = (int)reader["mdl_id"];
                                    int admin_id = (int)reader["admin_id"];

                                    Double profit_loss = (Double)reader["profit_loss"];
                                    string user_detail = (string)reader["username"];
                                    string dist_detail = DL_login_username + "[Master Agent]";
                                    Double user_net_balance = 0;
                                    Double dist_net_balance = 0;
                                    string descUst = "";
                                    string descUstDL = "";
                                    if (IsFree == "debit")
                                    {
                                        user_net_balance = profit_loss + Chips;
                                        dist_net_balance = 0;
                                        dist_net_balance = GetDlBal + Chips;
                                        descUst = "Cash Paid To Parent ";
                                        descUstDL = "Cash Received from " + user_detail;
                                    }
                                    else if (IsFree == "credit")
                                    {
                                        user_net_balance = profit_loss - Chips;
                                        dist_net_balance = 0;
                                        dist_net_balance = GetDlBal - Chips;
                                        descUst = "Cash Received from Parent";
                                        descUstDL = "Cash Paid To  " + user_detail;
                                    }
                                    SqlCommand cmd2 = new SqlCommand("INSERT INTO dist_account_statements(market_id,admin_id,md_id,dist_id,user_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver,cash) VALUES " +
                                        "('','" + admin_id + "','" + md_id + "','" + DL_login_user_idin + "','" + UserID + "','cs_coins','" + descUstDL + "','','" + debit + "','" + credit + "','','" + time.ToString(format) + "' ,'','','" + dist_net_balance + "') ", con);
                                    int CheckDataInsert = cmd2.ExecuteNonQuery();
                                    if (CheckDataInsert > 0)
                                    {
                                        SqlCommand user_account_statement = new SqlCommand("INSERT INTO user_account_statements(admin_id,md_id,dist_id,user_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver,market_id,cc_market_id,event_id,match_odds) VALUES " +
                                            "('" + admin_id + "','" + md_id + "','" + DL_login_user_idin + "','" + UserID + "','cs_coins','" + descUst + " ','','" + debit + "','" + credit + "','','" + time.ToString(format) + "' ,'','','','','','')", con);
                                        user_account_statement.ExecuteNonQuery();

                                        SqlCommand user_update = new SqlCommand("UPDATE users_client SET profit_loss='" + user_net_balance + "' WHERE id='" + UserID + "'", con);
                                        user_update.ExecuteNonQuery();

                                        SqlCommand dist_update = new SqlCommand("UPDATE distributors SET cash='" + dist_net_balance + "' WHERE id='" + DL_login_user_idin + "'", con);
                                        dist_update.ExecuteNonQuery();
                                        ReturnMSG = "true";
                                    }
                                    else
                                    {
                                        ReturnMSG = "false4545";
                                    }
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
            return ReturnMSG;
        }

        public Double GetDLChips(string DLId)
        {
            Double balance = 0;
            using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con1.Open();
                string stmt1 = "SELECT cash FROM distributors WHERE id='" + DLId + "' ";

                using (SqlCommand cmdCount1 = new SqlCommand(stmt1, con1))
                {
                    cmdCount1.ExecuteScalar();
                    var reader1 = cmdCount1.ExecuteReader();
                    while (reader1.Read())
                    {
                        balance = (Double)reader1["cash"];
                        balance = System.Math.Round(balance, 2);
                    }
                }
                con1.Close();
            }
            return balance;
        }
    }
}