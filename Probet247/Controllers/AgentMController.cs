using Newtonsoft.Json;
using RBetfair.Models;
using sky888.Models;
using System;
using Probet247.Models;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Security;

namespace Probet247.Controllers
{
    public class AgentMController : Controller
    {

        // GET: AgentM

        private SqlConnection con2;
        string MDL_login_user_id = "";
        static string DlStaticId = "";
        DateTime todaydate = DateTime.Now;
        string today = "yyyy-MM-dd";

        private void connection2()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            con2 = new SqlConnection(constr);
        }

        public ActionResult Index()
        {
            CheckSession_MDL();
            var Dl_UserStatement = new List<DL_UserStatement>();
            try
            {
                if (MDL_login_user_id != "0" && MDL_login_user_id != "" && MDL_login_user_id != null)
                {
                    string word = Request.QueryString["word"];
                    string qwes = "";
                    if (word != null && word != "All")
                    {
                        qwes = "AND username LIKE ('" + word + "%')";
                    }
                  /*  int xdgcfh = Int32.Parse(MDL_login_user_id);
                    double mdlbal = getMDLDLBal(xdgcfh);
                    mdlbal = System.Math.Round(mdlbal, 0);
                    double mdlcbal = getMDLClientBal(xdgcfh);
                    mdlcbal = System.Math.Round(mdlcbal, 0);
                    ViewBag.TmdlcBala = mdlcbal;
                    double mdlDLlib = getMDLClientLib(xdgcfh);
                    mdlDLlib = System.Math.Round(mdlDLlib, 0);
                    ViewBag.TmdlcLib = mdlDLlib;
                    ViewBag.MDLTOTALBAL = mdlbal + mdlcbal - mdlDLlib;
                    ViewBag.MDLTotalAVLBAL = mdlbal + mdlcbal;*/
                    connection2();
                    con2.Open();
                    SqlCommand com = new SqlCommand();
                    SqlDataReader dr;
                    com.Connection = con2;
                    com.CommandText = "SELECT id,username,name,status,balance,total_balance,profit_loss,coin_rate,credit_ref FROM distributors where md_id='" + MDL_login_user_id + "' "+qwes ;
                    dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            int dist_id = (int)dr["id"];
                            string login_username = (string)dr["username"];
                            string login_user_full_name = (string)dr["name"];
                            string user_status = (string)dr["status"];
                            Double login_user_balance = (Double)dr["balance"];
                            Double login_user_total_balance = (Double)dr["total_balance"];
                            Double user_profit_loss = (Double)dr["coin_rate"];
                            Double credit_ref = (Double)dr["credit_ref"];
                            Double profit_loss = (Double)dr["profit_loss"];
                            Double user_exposure = getDLClientLib(dist_id);
                            Double user_t_bal = getTClientBal(dist_id);
                            Double DLTOTALBL = user_t_bal + user_exposure + login_user_balance;
                            Double DLAVLBL = user_t_bal + login_user_balance;
                            Double DLPROF_LOSS = -(login_user_total_balance - DLTOTALBL);
                            DLPROF_LOSS = System.Math.Round(DLPROF_LOSS, 2);

                            Dl_UserStatement.Add(item: new DL_UserStatement
                            {
                                Client_Id = dist_id,    
                                Client_Username = login_username,
                                Client_balance = DLTOTALBL,
                                Client_avl_balance = DLAVLBL,
                                Client_profit_loss = user_profit_loss,
                                Client_status = user_status,
                                Client_exposure = user_exposure,
                                Client_ttl_balance= user_t_bal,
                                DLPROF_LOSS = DLPROF_LOSS,
                                credit_ref = credit_ref
                            });
                        }
                    }
                    else
                    {

                    }
                    con2.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return View("Index",Dl_UserStatement);
        }

        public void CheckSession_MDL()
        {
            string SessionDLUName = (string)System.Web.HttpContext.Current.Session["MDL_UserName"];
            string SessionDL_hash_key = (string)System.Web.HttpContext.Current.Session["MDL_hash_key"];

            MDL_login_user_id = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];

            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT id,username,name,balance FROM masterdistributors WHERE  id='" + MDL_login_user_id + "' AND username='" + SessionDLUName + "' AND hash_key='" + SessionDL_hash_key + "' AND status='activate'";
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

        public ActionResult cashBanking()
        {
            CheckSession_MDL();
            var DL_UserStatement = new List<DL_UserStatement>();
            try
            {
                string MDL_login_user_id = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];

                string word = Request.QueryString["word"];
                string qwes = "";
                if (word != null && word != "All")
                {
                    qwes = "AND username LIKE ('" + word + "%')";
                }
                connection2();
                con2.Open();
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con2;
                com.CommandText = "SELECT credit_ref,id,username,name,status,balance,total_balance,profit_loss FROM distributors WHERE md_id='" + MDL_login_user_id + "' "+qwes;
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
                        Double credit_ref = (Double)dr["credit_ref"];
                        Double login_user_total_balance = (Double)dr["total_balance"];
                        Double user_profit_loss = (Double)dr["profit_loss"];
                        Double user_exposure = getDLClientLib(login_user_id);
                        Double DLCLIENTBAL = getTClientBal(login_user_id);
                        Double profit_loss = DLCLIENTBAL+ user_exposure+ login_user_balance-credit_ref;
                        DL_UserStatement.Add(item: new DL_UserStatement
                        {
                            Client_Id = login_user_id,
                            Client_Username = login_username,
                            Client_balance = login_user_balance + DLCLIENTBAL + user_exposure,
                            Client_avl_balance = login_user_balance,
                            Client_profit_loss = profit_loss,
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

        public JsonResult DlClientList(string dl_id)
        {
            List<DLClientList> messages = new List<DLClientList>();
            messages = DlClientList1(dl_id);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<DLClientList> DlClientList1(string dl1_id)
        {
            var messages = new List<DLClientList>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    string stmt = "select username,id,dl_id from users_client where dl_id='" + dl1_id + "' ";

                    using (SqlCommand cmdCount = new SqlCommand(stmt, con))
                    {
                        cmdCount.ExecuteScalar();
                        var reader = cmdCount.ExecuteReader();
                        while (reader.Read())
                        {
                            string username = (string)reader["username"];
                            int id = (int)reader["id"];
                            int dl_id = (int)reader["dl_id"];

                            messages.Add(item: new DLClientList
                            {
                                username = username,
                                id = id,
                                dl_id = dl_id
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

        public JsonResult MDlClientList()
        {
            List<DLClientList> messages = new List<DLClientList>();
            messages = MDlClientList1();
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<DLClientList> MDlClientList1()
        {
            var messages = new List<DLClientList>();
            try
            {
                string MDL_login_user_id = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    string stmt = "select username, id from distributors where md_id='" + MDL_login_user_id + "' ";

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

        public ActionResult profitAndLoss()
        {
            CheckSession_MDL();
            var clientprofitlossstat = new List<clientprofitlossstat>();
            try
            {
                string GetDLIdIn11 = Request.QueryString["dist_id"];
                if (GetDLIdIn11 != null)
                {

                    string vhbj = "0";
                    vhbj = GetDLIdIn11;
                    string sportid = "0";
                    sportid = Request.QueryString["uid"];
                    int dlid = Int32.Parse(GetDLIdIn11);
                    ViewBag.DLUname = GetDLUserName(dlid);
                    ViewBag.DLID = vhbj;


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
                                                user_id = login_user_id,
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
                                            user_id = login_user_id,
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
            }
            catch (Exception ex)
            {

            }
            return View(clientprofitlossstat);
        }

        public ActionResult myAccountSummary()
        {
            CheckSession_MDL();
            return View();
        }
        public ActionResult accountCashStatement()
        {
            string page = Request.QueryString["page"];
            CheckSession_MDL();
            int countbeth = 0;
            string pageNumber = "1";
            var DL_UserBetList = new List<AccountStatement_DL>();
            try
            {
                string MDL_login_user_idn = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];
                int max_page = 20;
                int min_page = 0;
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
                com.CommandText = "SELECT created,debit,credit,balance,remark,description FROM(SELECT created,debit,credit,balance,remark,description , ROW_NUMBER() over(order by created desc) as row FROM md_account_statements WHERE acc_stat_type='dw_coins'  AND md_id='" + MDL_login_user_idn + "') a where a.row > '" + min_page + "'  and a.row <= '" + max_page + "' ";
                dr = com.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        DateTime Event_time = (DateTime)dr["created"];
                        Double debit = (Double)dr["debit"];
                        Double credit = (Double)dr["credit"];
                        Double balance = (Double)dr["balance"];
                        string remark = (string)dr["remark"];
                        string description = (string)dr["description"];
                        DL_UserBetList.Add(item: new AccountStatement_DL
                        {
                            Desc = description,
                            DTime = Event_time,
                            Remark = remark,
                            Balance = balance,
                            Deposit = debit,
                            Withdraw = credit
                        });
                    }
                    string stmts = "SELECT COUNT(id) FROM md_account_statements WHERE md_id='" + MDL_login_user_idn + "' AND acc_stat_type='dw_coins' ";
                    using (SqlCommand cmdCounts = new SqlCommand(stmts, con2))
                    {
                        countbeth = (int)cmdCounts.ExecuteScalar();
                    }
                }
                con2.Close();
            }
            catch (Exception ex)
            {

            }
            ViewBag.countbeth = countbeth;
            ViewBag.pageNumber = pageNumber;
            return View(DL_UserBetList);
        }

        public ActionResult profile()
        {
            CheckSession_MDL();
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "exchange");
        }

        public ActionResult home_dl()
        {
            CheckSession_MDL();
            var Dl_UserStatement = new List<DL_UserStatement>();
            try
            {
                string sportid = Request.QueryString["dist_id"];
                string MA_Name = "";
                if(sportid!="" && sportid != null)
                {
                    int MA_id = int.Parse(sportid);
                    MA_Name = GetDLUserName(MA_id);
                }
                ViewBag.MA_Name = MA_Name;


                string word = Request.QueryString["word"];
                string qwes = "";
                if (word != null && word != "All")
                {
                    qwes = "AND username LIKE ('" + word + "%')";
                }
                int getdlin = Int32.Parse(sportid);
                Double ToBala = 0;
                Double ToExp = 0;
                Double ToAvlBa = 0;
                Double Balan = 0;
                ToBala = getTClientBal(getdlin);
                ViewBag.ToBala = ToBala;
                ToExp = getDLClientLib(getdlin);
                ViewBag.ToExp = ToExp;
                ToAvlBa = ToBala - ToExp;
                ViewBag.ToAvBal = ToAvlBa;
                Balan = GetDLBalance(getdlin);
                ViewBag.Balan = Balan;
                Double AVBAH = ToBala + Balan;
                ViewBag.AVBAH = AVBAH;

                ViewBag.DLID = "0";
                ViewBag.ClientId = "0";

                if (sportid != "0" && sportid != "" && sportid != null)
                {
                    ViewBag.DLID = sportid;
                    connection2();
                    con2.Open();
                    SqlCommand com = new SqlCommand();
                    SqlDataReader dr;
                    com.Connection = con2;
                    com.CommandText = "SELECT profit_loss,credit_ref,exposure_limit,id,username,name,status,balance,total_balance,exposure FROM users_client WHERE dl_id='" + sportid + "'" + qwes;
                    dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            int login_user_id = (int)dr["id"];
                            string login_username = (string)dr["username"];
                            string login_user_full_name = (string)dr["name"];
                            string user_status = (string)dr["status"];
                            Double credit_ref = (Double)dr["credit_ref"];
                            Double exposure_limit = (Double)dr["exposure_limit"];
                            Double login_user_balance = (Double)dr["balance"];
                            Double profit_loss = (Double)dr["profit_loss"];
                            login_user_balance = System.Math.Round(login_user_balance, 2);
                            Double user_exposure = (Double)dr["exposure"];
                            user_exposure = System.Math.Round(user_exposure, 2);
                            Double user_avail_bal = login_user_balance - user_exposure;
                            Double user_profit_loss = login_user_balance + user_exposure-credit_ref;
                            user_profit_loss = System.Math.Round(user_profit_loss, 2);
                            Dl_UserStatement.Add(item: new DL_UserStatement
                            {
                                Client_Id = login_user_id,
                                dl_Id = getdlin,
                                Client_Username = login_username,
                                Client_balance = login_user_balance,
                                Client_avl_balance = user_avail_bal,
                                Client_profit_loss = profit_loss,
                                Client_status = user_status,
                                Client_exposure = user_exposure,
                                credit_ref = credit_ref,
                                exposure_limit = exposure_limit
                            });
                        }
                    }
                    else
                    {

                    }
                    con2.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return View(Dl_UserStatement);
        }

        public ActionResult ClientLiability(string sportid, string leagueid)
        {
            var DL_UserBetList = new List<DL_UserBetList>();
            try
            {
                string dist_id = sportid;
                string user_id = leagueid;
                string lib_query = "SELECT place_time,event_id,id,betfair_id,field,rate,stakes,total_value FROM live_bet WHERE status='' AND user_id='" + user_id + "'";
                if (user_id == "0")
                {
                    lib_query = "SELECT place_time,event_id,id,betfair_id,field,rate,stakes,total_value FROM live_bet WHERE status='' AND dist_id='" + dist_id + "'";
                }
                connection2();
                con2.Open();
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con2;
                com.CommandText = lib_query;
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

        public string GetEvTitleEV(string event_code)
        {
            string match_title = "";
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT match_title from [matches] where event_code='" + event_code + "' ";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                match_title = (string)dr["match_title"];
            }
            con2.Close();
            return match_title;
        }

        public string GetMarketNameEVBF(string betfair_id, string event_code)
        {
            string market_name = "";
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT market_name from [markets] where betfair_id='" + betfair_id + "' AND event_code='" + event_code + "' ";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    market_name = (string)dr["market_name"];
                }
            }
            con2.Close();
            return market_name;
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

        public string GetDLUserName(int user_id)
        {
            string username = "";
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT username from [distributors] where id='" + user_id + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                username = (string)dr["username"];
            }
            con2.Close();
            return username;
        }

        public Double GetDLRate(int user_id)
        {
            Double coin_rate = 0;
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT coin_rate from [distributors] where id='" + user_id + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                coin_rate = (Double)dr["coin_rate"];
            }
            con2.Close();
            return coin_rate;
        }

        public Double GetMDLRate(int user_id)
        {
            Double coin_rate = 0;
            connection2();
            con2.Open();
            using (SqlCommand cmd1 = new SqlCommand("SELECT md_id from [distributors] where id='" + user_id + "'", con2))
            {
                var dr1 = cmd1.ExecuteReader();
                if (dr1.HasRows)
                {
                    dr1.Read();
                    int md_id = (Int32)dr1["md_id"];
                    using (SqlCommand cmd = new SqlCommand("SELECT coin_rate from [masterdistributors] where id='" + md_id + "'", con2))
                    {
                        var dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            coin_rate = (Double)dr["coin_rate"];
                        }
                    }
                }
            }
            con2.Close();
            return coin_rate;
        }

        public ActionResult betList()
        {
            CheckSession_MDL();
            var DL_UserBetList = new List<MatchedClientBetList>();
            try
            {
                string GetDLIdIn = Request.QueryString["dist_id"];
                ViewBag.DLID = "0";
                ViewBag.ClientId = "0";

                if (GetDLIdIn != null)
                {
                    string user_id = Request.QueryString["uid"];
                    int gfh = Int32.Parse(user_id);
                    int dlid = Int32.Parse(GetDLIdIn);
                    ViewBag.DLUnameM = GetDLUserName(dlid);
                    ViewBag.DLID = GetDLIdIn;

                    ViewBag.CUname = GetCUserName(gfh);
                    ViewBag.ClientId = user_id;

                    string login_user_id = user_id;
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
                                        Double stakes1 = stakes;
                                        if (BetType1 == "lay")
                                        {
                                            stakes1 = profit_loss;
                                        }
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
                                            Stakes1 = stakes1,
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
                                    Double stakes1 = stakes;
                                    if (BetType1 == "lay")
                                    {
                                        stakes1 = profit_loss;
                                    }
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
                                        Stakes1 = stakes1,
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
                    ViewBag.pageNumber = pageNumber;
                }
            }
            catch (Exception ex)
            {

            }
            return View(DL_UserBetList);
        }

        public ActionResult accountSummary()
        {
            CheckSession_MDL();
            try
            {
                string GetDLIdIn = Request.QueryString["dist_id"];
                string ClientId = Request.QueryString["uid"];
                ViewBag.DLID = GetDLIdIn;
                ViewBag.ClientId = ClientId;
                if (ClientId != "0" && GetDLIdIn != "0")
                {
                    int dlid = Int32.Parse(GetDLIdIn);
                    int user_id = Int32.Parse(ClientId);
                    ViewBag.DLUnameM = GetDLUserName(dlid);
                    ViewBag.CUname = GetCUserName(user_id);
                    ViewBag.UserNameC = GetCUserName(user_id);
                    ViewBag.FUserNameC = GetCFUserName(user_id);
                    ViewBag.UserBalC = GetCbalance(user_id);
                    ViewBag.UserExpC = GetCexposure(user_id);
                }

                else
                {
                    int dlid = Int32.Parse(GetDLIdIn);
                    connection2();
                    con2.Open();
                    SqlCommand com = new SqlCommand();
                    SqlDataReader dr;
                    com.Connection = con2;
                    com.CommandText = "SELECT balance , username , name from [distributors] where id='" + dlid + "'";
                    dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        string DLUnameM = (String)dr["username"];
                        string FUserNameC = (String)dr["name"];
                        double UserBalC = (Double)dr["balance"];
                        ViewBag.DLUnameM = DLUnameM;
                        ViewBag.UserNameC = DLUnameM;
                        ViewBag.CUname = "";
                        ViewBag.FUserNameC = FUserNameC;
                        ViewBag.UserBalC = UserBalC;
                        ViewBag.UserExpC = getDLClientLib(dlid);
                    }
                    con2.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult PlayerAccount()
        {
            CheckSession_MDL();
            try
            {
                string GetDLIdIn = Request.QueryString["dist_id"];
                string ClientId = Request.QueryString["uid"];
                ViewBag.DLID = GetDLIdIn;
                ViewBag.ClientId = ClientId;
                if (ClientId != "0" && GetDLIdIn != "0")
                {
                    int dlid = Int32.Parse(GetDLIdIn);
                    int user_id = Int32.Parse(ClientId);
                    ViewBag.DLUnameM = GetDLUserName(dlid);
                    ViewBag.CUname = GetCUserName(user_id);
                    ViewBag.UserNameC = GetCUserName(user_id);
                    ViewBag.FUserNameC = GetCFUserName(user_id);
                    ViewBag.UserBalC = GetCbalance(user_id);
                    ViewBag.UserExpC = GetCexposure(user_id);
                    ViewBag.Exp_limit = GetExpLimit(user_id);
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public ActionResult transactionHistory()
        {
            CheckSession_MDL();
            var DL_UserBetList = new List<ClientaccountCashStatement>();
            try
            {
                string GetDLIdIn = Request.QueryString["dist_id"];
                ViewBag.DLID = "0";
                ViewBag.ClientId = "0";
                if (GetDLIdIn != null)
                {
                    int dlid = Int32.Parse(GetDLIdIn);
                    ViewBag.DLUnameM = GetDLUserName(dlid);
                    ViewBag.DLID = GetDLIdIn;

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        using (var cmd = new SqlCommand("SELECT created,debit,credit,balance,remark,description FROM dist_account_statements WHERE dist_id='" + GetDLIdIn + "'  AND acc_stat_type = 'dw_coins' order by created desc "))
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
        public ActionResult PlayerTransaction()
        {
            CheckSession_MDL();
            var DL_UserBetList = new List<ClientaccountCashStatement>();
            try
            {
                string GetDLIdIn = Request.QueryString["dist_id"];
                string Player_id = Request.QueryString["uid"];
                ViewBag.DLID = "0";
                ViewBag.ClientId = "0";
                if (Player_id != null)
                {
                    int dlid = Int32.Parse(GetDLIdIn);
                    int ClientId = Int32.Parse(Player_id);
                    ViewBag.DLUnameM = GetDLUserName(dlid);
                    ViewBag.PlayerName = GetCUserName(ClientId);
                    ViewBag.DLID = GetDLIdIn;
                    ViewBag.ClientId = ClientId;

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        using (var cmd = new SqlCommand("SELECT created,debit,credit,balance,remark,description FROM user_account_statements WHERE user_id='" + Player_id + "'  AND acc_stat_type = 'dw_coins' order by created desc "))
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
            Double username = 0;
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
                username = (Double)dr["balance"];
            }
            con2.Close();
            return username;
        }

        public Double GetCexposure(int user_id)
        {
            Double username = 0;
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
                username = (Double)dr["exposure"];
            }
            con2.Close();
            return username;
        }
        public Double GetExpLimit(int user_id)
        {
            Double Exp_limit = 0;
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
                Exp_limit = (Double)dr["exposure_limit"];
            }
            con2.Close();
            return Exp_limit;
        }

        public ActionResult RiskManagement()
        {
            return View();
        }

        public ActionResult mdlanalysis()
        {
            mdlanalysisRepositary _messageRepository = new mdlanalysisRepositary();
            return PartialView("mdlanalysis", _messageRepository.GetAllMessages());
        }
        public ActionResult Msdlanuhytalysis()
        {
            MDLSessionRepository _sessionRepository = new MDLSessionRepository();
            return PartialView("mdlsanalysisdata", _sessionRepository.GetAllMessagesS());
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
                    string dist_id = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];

                    Double TotalA = 0;
                    Double TotalB = 0;
                    Double TotalC = 0;
                    Double TotalD = 0;
                    Double TotalE = 0;
                    Double TotalF = 0;

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        SqlCommand sqlmodel = new SqlCommand("SELECT [user_id],[stakes],[total_value],[field],[rate] FROM live_bet_new WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_id + "' AND md_id = '" + dist_id + "' ", con);
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
                    string md_id = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];

                    var user_id = new List<int>();
                    var stakes = new List<Double>();
                    var total_value = new List<Double>();
                    var field = new List<string>();
                    var rate = new List<Double>();

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        SqlCommand sqlmodel = new SqlCommand("SELECT [dist_id],[stakes],[total_value],[field],[rate] FROM live_bet_new WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_id + "' AND md_id = '" + md_id + "' ", con);
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
                                int user_id1 = (int)datamodel["dist_id"];
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
                        SqlCommand sqlm = new SqlCommand("SELECT DISTINCT [dist_id] FROM live_bet_new WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_id + "' AND md_id = '" + md_id + "' ", con);
                        var datam = sqlm.ExecuteReader();
                        if (datam.HasRows)
                        {
                            while (datam.Read())
                            {
                                int user_id01 = (Int32)datam["dist_id"];
                                string client_name1 = GetDLUserName(user_id01);
                                Double dl_rate = GetDLRate(user_id01);
                                Double ag_rate = 100 - dl_rate;
                                Double mdl_rate = GetMDLRate(user_id01);
                                Double agm_rate = dl_rate - mdl_rate;
                                Double admin_rate = mdl_rate;

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
                                    ag_rate = ag_rate,
                                    agm_rate = agm_rate,
                                    ad_rate = admin_rate,
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

        public JsonResult SessionDldatabkdl(string betfair_id, string event_id, string dist_id)
        {
            List<Sessiondldata> messages = new List<Sessiondldata>();
            messages = SessionDldata2dl(betfair_id, event_id, dist_id);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<Sessiondldata> SessionDldata2dl(string betfair_id, string event_id, string dist_id)
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
            string EventNameGet = GetEventTitleName(event_id);
            ViewBag.betfair_id = betfair_id;
            ViewBag.EventNameB = EventNameGet;
            ViewBag.event_id = event_id;
            return View();
        }
        public ActionResult sbookviewdl()
        {
            string betfair_id = Request.QueryString["betfair_id"];
            string event_id = Request.QueryString["event_code"];
            string dist_id = Request.QueryString["uid"];
            string EventNameGet = GetEventTitleName(event_id);
            ViewBag.betfair_id = betfair_id;
            ViewBag.EventNameB = EventNameGet;
            ViewBag.event_id = event_id;
            ViewBag.dist_id = dist_id;
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

        public ActionResult bookviewdl()
        {
            string event_id = Request.QueryString["event_code"];
            string betfair_id = Request.QueryString["betfair_id"];
            string dist_id = Request.QueryString["uid"];
            string EventNameGet = GetEventTitleName(event_id);
            ViewBag.EventNameB = EventNameGet;
            ViewBag.BetfairId = betfair_id;
            ViewBag.event_id = event_id;
            ViewBag.dist_id = dist_id;
            return View();
        }

        public ActionResult allbets()
        {
            var DL_UserBetList = new List<MatchedClientBetList>();
            try
            {
                string betfair_id = Request.QueryString["betfair_id"];
                string event_code = Request.QueryString["event_code"];
                string EventNameGet = GetEventTitleNameBF(betfair_id);
                ViewBag.EventNameBU = EventNameGet;
                string user_ids = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT place_time,user_id,event_id,field,rate,stakes,total_value FROM live_bet WHERE md_id='" + user_ids + "' AND betfair_id='" + betfair_id + "' AND event_id='" + event_code + "'"))
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

        public JsonResult BookviewSV(string bfid)
        {
            List<BookViewAg> messages = new List<BookViewAg>();
            messages = BookviewS(bfid);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<BookViewAg> BookviewS(string bfid)
        {
            var AllSportsAddM = new List<BookViewAg>();
            try
            {
                string user_ids = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT distinct(dist_id) from live_bet where betfair_id='" + bfid + "' AND md_id='" + user_ids + "' AND status='' AND odds_type='MO'", con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Double teamAWD = 0;
                            Double teamBWD = 0;
                            Double teamCWD = 0;
                            int dist_idN = (int)reader["dist_id"];
                            string UCname = GetDLUserName(dist_idN);
                            Double dl_rate = GetDLRate(dist_idN);
                            Double ag_rate = 100 - dl_rate;
                            Double mdl_rate = GetMDLRate(dist_idN);
                            Double agm_rate = dl_rate - mdl_rate;
                            Double admin_rate = mdl_rate;
                            using (var cmd2 = new SqlCommand("select runner_pos,amount from runner_cal where dist_id='" + dist_idN + "' AND market_id='" + bfid + "'", con))
                            {
                                var reader2 = cmd2.ExecuteReader();
                                while (reader2.Read())
                                {
                                    int runner_posrd = (Int32)reader2["runner_pos"];
                                    Double amountt = (Double)reader2["amount"];
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

                            AllSportsAddM.Add(item: new BookViewAg
                            {
                                user_id = dist_idN,
                                cname = UCname,
                                teamAWD = teamAWD,
                                teamBWD = teamBWD,
                                teamCWD = teamCWD,
                                ag_rate = ag_rate,
                                agm_rate = agm_rate,
                                ad_rate = admin_rate,
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

        public JsonResult BookviewSVdl(string bfid, string uid)
        {
            List<BookViewAg> messages = new List<BookViewAg>();
            messages = BookviewSdl(bfid, uid);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }
        public List<BookViewAg> BookviewSdl(string bfid, string uid)
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
                string user_ids = uid;
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

        public ActionResult dlallbet()
        {
            var DL_UserBetList = new List<MatchedClientBetList>();
            try
            {
                string uid = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];
                string betfair_id = Request.QueryString["betfair_id"];
                string event_code = Request.QueryString["event_code"];
                string EventNameGet = GetEvTitleEV(event_code);
                string Ev_Market_Name = GetMarketNameEVBF(betfair_id, event_code);
                ViewBag.EventNameBU = EventNameGet;
                ViewBag.Ev_Market_Name = Ev_Market_Name;
                ViewBag.event_code = event_code;
                ViewBag.betfair_id = betfair_id;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT Distinct(dist_id) FROM live_bet WHERE md_id='" + uid + "' AND event_id='" + event_code + "' AND betfair_id='" + betfair_id + "' "))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            int dist_id = (Int32)dr["dist_id"];
                            string UsernameC = GetDLUserName(dist_id);
                            DL_UserBetList.Add(item: new MatchedClientBetList
                            {
                                EventTime = "-",
                                uid = dist_id,
                                Description = UsernameC,
                                Field = "-",
                                Rate = 0,
                                Stakes = 0,
                                PL = 0,
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
                string event_code = Request.QueryString["event_code"];
                string EventNameGet = GetEvTitleEV(event_code);
                string Ev_Market_Name = GetMarketNameEVBF(betfair_id, event_code);
                ViewBag.EventNameBU = EventNameGet;
                ViewBag.Ev_Market_Name = Ev_Market_Name;
                ViewBag.event_code = event_code;
                ViewBag.betfair_id = betfair_id;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT user_id,place_time,field,rate,stakes,total_value FROM live_bet WHERE dist_id='" + uid + "' AND event_id='" + event_code + "' AND betfair_id='" + betfair_id + "' order by place_time desc"))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            int user_id = (Int32)dr["user_id"];
                            string UsernameC = GetCUserName(user_id);
                            DateTime time = (DateTime)dr["place_time"];
                            string format = "yyyy-MM-dd HH:mm:ss";

                            string field = (string)dr["field"];
                            Double rate = (Double)dr["rate"];
                            Double stakes = (Double)dr["stakes"];
                            Double profit_loss = (Double)dr["total_value"];
                            DL_UserBetList.Add(item: new MatchedClientBetList
                            {
                                EventTime = time.ToString(format),
                                uid = user_id,
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
        public ActionResult userallbetper()
        {
            var DL_UserBetList = new List<MatchedClientBetList>();
            try
            {
                string uid = Request.QueryString["uid"];
                string betfair_id = Request.QueryString["betfair_id"];
                string event_code = Request.QueryString["event_code"];
                string EventNameGet = GetEvTitleEV(event_code);
                string Ev_Market_Name = GetMarketNameEVBF(betfair_id, event_code);
                ViewBag.EventNameBU = EventNameGet;
                ViewBag.Ev_Market_Name = Ev_Market_Name;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT user_id,place_time,field,rate,stakes,total_value FROM live_bet WHERE user_id='" + uid + "' AND event_id='" + event_code + "' AND betfair_id='" + betfair_id + "' order by place_time desc"))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            int user_id = (Int32)dr["user_id"];
                            string UsernameC = GetCUserName(user_id);
                            DateTime time = (DateTime)dr["place_time"];
                            string format = "yyyy-MM-dd HH:mm:ss";

                            string field = (string)dr["field"];
                            Double rate = (Double)dr["rate"];
                            Double stakes = (Double)dr["stakes"];
                            Double profit_loss = (Double)dr["total_value"];
                            DL_UserBetList.Add(item: new MatchedClientBetList
                            {
                                EventTime = time.ToString(format),
                                uid = user_id,
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

        public ActionResult marketProfitLoss()
        {
            CheckSession_MDL();
            var DL_UserBetList = new List<ClientPLModel>();
            try
            {
                string MD_ID = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];

                string pageNumber = "1";
                string sport_id = "0";
                int max_page = 100;
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
                    max_page = 100 * pageno;
                    min_page = max_page - 100;
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        using (var cmd = new SqlCommand("SELECT distinct(event_id)  FROM(SELECT distinct(event_id) , ROW_NUMBER() over(order by id desc) as row FROM pl_statement WHERE (created BETWEEN '" + startDate + "' AND '" + endDate + "') and sport_id='" + sport_id + "' AND md_id='" + MD_ID + "' ) a where a.row > '" + min_page + "'  and a.row <= '" + max_page + "' "))
                        {
                            cmd.Connection = con;
                            con.Open();
                            var dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                string event_id_g = (string)dr["event_id"];
                                using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                                {
                                    using (var cmd1 = new SqlCommand("select created,total_pl from pl_statement where md_id='" + MD_ID + "' AND event_id='" + event_id_g + "'"))
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
                                            total_pl = -total_pl,
                                            event_id = event_id_g
                                        });
                                        con1.Close();
                                    }
                                }
                            }
                            con.Close();
                        }

                        string stmts = "SELECT COUNT(id) FROM pl_statement WHERE (created BETWEEN '" + startDate + "' AND '" + endDate + "') AND  sport_id='" + sport_id + "' AND md_id='" + MD_ID + "' ";
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
            CheckSession_MDL();
            var DL_UserBetList = new List<ClientPLModel>();
            try
            {
                string event_id_g = Request.QueryString["event_id"];
                string login_user_idn = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("select distinct(market_id) from pl_statement where md_id='" + login_user_idn + "' AND event_id='" + event_id_g + "'"))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            string market_id_g = (string)dr["market_id"];
                            using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                            {
                                using (var cmd1 = new SqlCommand("select created,total_pl,description from pl_statement where md_id='" + login_user_idn + "' AND market_id='" + market_id_g + "'"))
                                {
                                    cmd1.Connection = con1;
                                    con1.Open();
                                    var dr1 = cmd1.ExecuteReader();
                                    DateTime created = new DateTime();
                                    string description = "";
                                    Double total_pl = 0;
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
            CheckSession_MDL();
            var DL_UserBetList = new List<ClientPLModel>();
            try
            {
                string event_id_g = Request.QueryString["event_id"];
                string market_id_g = Request.QueryString["market_id"];
                string login_user_idn = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];

                using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd1 = new SqlCommand("select created,debit,credit,user_id,dist_id,description from user_account_statements where md_id='" + login_user_idn + "' AND market_id='" + market_id_g + "' AND event_id='" + event_id_g + "'"))
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
                            int did = (int)dr1["dist_id"];
                            CUname = GetCUserName(ucid);
                            String DLname = GetDLUserName(did);
                            description = (string)dr1["description"];

                            DL_UserBetList.Add(item: new ClientPLModel
                            {
                                description = description,
                                created = created,
                                debit = debit,
                                credit = credit,
                                uname = CUname + " [" + DLname + "] "
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

        public Double getMDLDLBal(int mdlid)
        {
            Double sport_id = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT COALESCE(sum(balance),0) AS t_bal FROM distributors WHERE md_id='" + mdlid + "'", con))
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    sport_id = (Double)reader["t_bal"];
                    sport_id = System.Math.Round(sport_id, 0);
                }
                con.Close();
            }
            return sport_id;
        }

        public Double getMDLClientBal(int mdlid)
        {
            Double sport_id = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT COALESCE(sum(balance),0) AS t_bal FROM users_client WHERE mdl_id='" + mdlid + "'", con))
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    sport_id = (Double)reader["t_bal"];
                    sport_id = System.Math.Round(sport_id, 0);
                }
                con.Close();
            }
            return sport_id;
        }

        public Double getMDLClientLib(int mdlid)
        {
            Double sport_id = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT COALESCE(sum(exposure),0) AS t_bal FROM users_client WHERE mdl_id='" + mdlid + "'", con))
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    sport_id = (Double)reader["t_bal"];
                    sport_id = System.Math.Round(sport_id, 0);
                }
                con.Close();
            }
            return sport_id;
        }

        public Double getMDLPl(int mdlid)
        {
            Double sport_id = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT profit_loss FROM masterdistributors WHERE id='" + mdlid + "'", con))
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    sport_id = (Double)reader["profit_loss"];
                    sport_id = System.Math.Round(sport_id, 0);
                }
                con.Close();
            }
            return sport_id;
        }
        public Double getMDLComm(int mdlid)
        {
            Double sport_id = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT commission FROM masterdistributors WHERE id='" + mdlid + "'", con))
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    sport_id = (Double)reader["commission"];
                    sport_id = System.Math.Round(sport_id, 0);
                }
                con.Close();
            }
            return sport_id;
        }

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
                    sport_id = System.Math.Round(sport_id, 0);
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
                    sport_id = System.Math.Round(sport_id, 0);
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

        public ActionResult TPbookviewdl()
        {
            string event_id = Request.QueryString["event_code"];
            string betfair_id = Request.QueryString["betfair_id"];
            string dist_id = Request.QueryString["uid"];
            string EventNameGet = GetEventTitleName(event_id);
            ViewBag.EventNameB = EventNameGet;
            ViewBag.BetfairId = betfair_id;
            ViewBag.event_id = event_id;
            ViewBag.dist_id = dist_id;
            return View();
        }
       /* public ActionResult teenmdlrisk()
        {
            Teenmdlrepos _teenmdlRepository = new Teenmdlrepos();
            return PartialView("teenmdlrisk", _teenmdlRepository.GetAllMessages());
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
                string user_ids = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT distinct(dist_id) from live_bet where betfair_id='" + bfid + "' AND md_id='" + user_ids + "' AND status='' AND odds_type='TP'", con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            int dist_idN = (int)reader["dist_id"];
                            string UCname = GetDLUserName(dist_idN);
                            Double TotalamountA = 0;
                            Double TotalamountB = 0;
                            Double TotalamountC = 0;
                            SqlCommand sqlLost = new SqlCommand("SELECT stakes,total_value,runner_posi,event_id FROM live_bet WHERE betfair_id='" + bfid + "' AND dist_id='" + dist_idN + "' ", con);
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
                                user_id = dist_idN,
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

        public JsonResult TPBookviewSVdl(string bfid, string uid)
        {
            List<BookViewAg> messages = new List<BookViewAg>();
            messages = TPBookviewSdl(bfid, uid);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }
        public List<BookViewAg> TPBookviewSdl(string bfid, string uid)
        {
            var AllSportsAddM = new List<BookViewAg>();
            try
            {
                string user_ids = uid;
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
            string user_ids = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];
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
                    crkt.CommandText = "select distinct(dist_id) from pl_statement where md_id='" + user_ids + "' AND created BETWEEN '" + startDate + "' AND '" + endDate + "'";
                    dr = crkt.ExecuteReader();
                    if (dr.HasRows)
                    {
                        string nm = "";
                        Double cr = 0;
                        Double soc = 0;
                        Double ten = 0;
                        while (dr.Read())
                        {
                            int i = (int)dr["dist_id"];
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
                                tot = cr + soc + ten,
                                id_ = i
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
                crkt.CommandText = "sp_dlSprtPL";
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
                crkt.CommandText = "sp_dlSprtPL";
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
                crkt.CommandText = "sp_dlSprtPL";
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
                    using (var cmd = new SqlCommand("SELECT username FROM distributors WHERE id='" + id + "' ", con))
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

        public ActionResult SpoplU(string sportid)
        {
            var spStat = new List<sportsStat>();
            if (sportid != null)
            {
                string startDate = Request.QueryString["startDate"];
                string endDate = Request.QueryString["endDate"];
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
                        crkt.CommandText = "select distinct(user_id) from pl_statement where dist_id='" + sportid + "' AND created BETWEEN '" + startDate + "' AND '" + endDate + "' AND user_id!='0'";
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
                                nm = GetMDLNameU(i);
                                cr = getMDLCrktPLU(i, 4, startDate, endDate);
                                soc = getMDLSoccPLU(i, 1, startDate, endDate);
                                ten = getMDLTennPLU(i, 2, startDate, endDate);
                                spStat.Add(item: new sportsStat
                                {
                                    naam = nm,
                                    crkt = cr,
                                    socc = soc,
                                    tenn = ten,
                                    tot = cr + soc + ten,
                                    id_ = i
                                });
                            }
                            con2.Close();
                        }
                    }
                    catch (SqlException ex)
                    {

                    }
                }
            }

            return View("SPoPlU", spStat);
        }

        public double getMDLCrktPLU(int mid, int spt, string startDate, string endDate)
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
        public double getMDLSoccPLU(int mid, int spt, string startDate, string endDate)
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
        public double getMDLTennPLU(int mid, int spt, string startDate, string endDate)
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
        public string GetMDLNameU(int id)
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
                string id = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("select mdl.username from admin mdl inner join masterdistributors dl on dl.admin_id = mdl.id where dl.id = '" + id + "' ", con))
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
            Double Tota2 = 0;
            Double Tota8 = 0;
            Double Tota9 = 0;
            Double Tota3 = 0;
            Double Tota4 = 0;
            try
            {
                string DL_id = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];
                if (DL_id != null && DL_id != "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        using (var cmd = new SqlCommand("SELECT id,admin_id,username,mdl_profit_loss FROM distributors where md_id='" + DL_id + "' ", con))
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
                                    profit_loss = Math.Round((Double)dr["mdl_profit_loss"], 2);
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
                                        profitlosP.Add(Math.Abs(profit_loss));
                                        UserIdP.Add(userID);
                                        fhbgh = "Plus";
                                    }

                                }
                                if (CashRec < 0)
                                {
                                    ViewBag.DlCashP = Math.Abs(CashRec);
                                    ViewBag.DlCashPL = CashRec;
                                }
                                else
                                {
                                    ViewBag.DlCash = Math.Abs(CashRec);
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
                    CheckSession_MDL();
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
            string DL_id = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT profit_loss FROM masterdistributors WHERE id='" + DL_id + "' ", con))
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
            string DL_id = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT COALESCE(sum(profit_loss),0) AS profit_loss FROM distributors WHERE md_id='" + DL_id + "' ", con))
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
            string md_id = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT cash from masterdistributors where id='" + md_id + "'"))
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
            string DL_id = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT admin_profit_loss FROM masterdistributors WHERE id='" + DL_id + "' ", con))
                    {
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            profit_loss = Math.Round((Double)dr["admin_profit_loss"], 2);
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
            string ReturnMSG = "Failed";
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
                    string DL_login_user_idin = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];
                    string DL_login_username = (string)System.Web.HttpContext.Current.Session["MDL_UserName"];

                    Double GetDlBal = GetDLChips(DL_login_user_idin);
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        string stmt = "SELECT md_id,admin_id,mdl_profit_loss,username FROM distributors WHERE id='" + UserID + "' ";

                        using (SqlCommand cmdCount = new SqlCommand(stmt, con))
                        {
                            cmdCount.ExecuteScalar();
                            var reader = cmdCount.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    int md_id = (int)reader["md_id"];
                                    int admin_id = (int)reader["admin_id"];

                                    Double profit_loss = (Double)reader["mdl_profit_loss"];
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
                                    SqlCommand cmd2 = new SqlCommand("INSERT INTO md_account_statements(event_id,market_id,admin_id,md_id,dist_id,user_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver) VALUES " +
                                            "('','','" + admin_id + "','" + DL_login_user_idin + "','" + UserID + "','0','cs_coins','" + descUstDL + "','','" + debit + "','" + credit + "','" + dist_net_balance + "','" + time.ToString(format) + "' ,'','') ", con);
                                    int CheckDataInsert = cmd2.ExecuteNonQuery();

                                    /*      SqlCommand cmd2 = new SqlCommand("INSERT INTO md_account_statements(market_id,admin_id,md_id,dist_id,user_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver,cash) VALUES " +
                                              "('','" + admin_id + "','" + md_id + "','" + DL_login_user_idin + "','" + UserID + "','cs_coins','" + descUstDL + "','','" + debit + "','" + credit + "','','" + time.ToString(format) + "' ,'','','" + dist_net_balance + "') ", con);
                                          int CheckDataInsert = cmd2.ExecuteNonQuery();*/
                                    if (CheckDataInsert > 0)
                                    {
                                        SqlCommand user_account_statement = new SqlCommand("INSERT INTO dist_account_statements(admin_id,md_id,dist_id,user_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver,market_id) VALUES " +
                                            "('" + admin_id + "','" + DL_login_user_idin + "','" + UserID + "','0','cs_coins','" + descUst + "','','" + debit + "','" + credit + "','','" + time.ToString(format) + "' ,'','','')", con);
                                        user_account_statement.ExecuteNonQuery();


                                        SqlCommand user_update = new SqlCommand("UPDATE distributors SET mdl_profit_loss='" + user_net_balance + "' WHERE id='" + UserID + "'", con);
                                        user_update.ExecuteNonQuery();

                                        SqlCommand dist_update = new SqlCommand("UPDATE masterdistributors SET cash='" + dist_net_balance + "' WHERE id='" + DL_login_user_idin + "'", con);
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
                string stmt1 = "SELECT cash FROM masterdistributors WHERE id='" + DLId + "' ";

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
        public ActionResult OverAllDL()
        {
            CheckSession_MDL();
            string startDate = Request.QueryString["startDate"];
            string endDate = Request.QueryString["endDate"];
            var Dl_UserStatement = new List<DL_UserStatement>();
            try
            {
                if (startDate != null && startDate != "" && endDate != null && endDate != "")
                {
                    if (MDL_login_user_id != "" && MDL_login_user_id != null)
                    {
                        using (SqlConnection con = ConnectionHandler.Connection())
                        {
                            SqlCommand com = new SqlCommand("SELECT id,username FROM distributors where md_id='" + MDL_login_user_id + "' ", con);
                            con.Open();
                            SqlDataReader dr = com.ExecuteReader();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    int dist_id = (int)dr["id"];
                                    string username = (string)dr["username"];
                                    SqlCommand cmd = new SqlCommand("dlTotDepositeWithdrawl", con);
                                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@dlID", System.Data.SqlDbType.Int).Value = dist_id;
                                    cmd.Parameters.Add("@startDate", System.Data.SqlDbType.DateTime).Value = startDate;
                                    cmd.Parameters.Add("@enddate", System.Data.SqlDbType.DateTime).Value = endDate;
                                    SqlDataReader dr1 = cmd.ExecuteReader();
                                    if (dr1.HasRows)
                                    {
                                        while (dr1.Read())
                                        {
                                            Double totCredit = System.Math.Round((Double)dr1["totDLCredit"], 2);
                                            Double totDebit = System.Math.Round((Double)dr1["totDLDebit"], 2);
                                            SqlCommand cmd1 = new SqlCommand("dlTotProLoss", con);
                                            cmd1.CommandType = System.Data.CommandType.StoredProcedure;
                                            cmd1.Parameters.Add("@dlID", System.Data.SqlDbType.Int).Value = dist_id;
                                            cmd1.Parameters.Add("@startDate", System.Data.SqlDbType.DateTime).Value = startDate;
                                            cmd1.Parameters.Add("@enddate", System.Data.SqlDbType.DateTime).Value = endDate;
                                            SqlDataReader dr2 = cmd1.ExecuteReader();
                                            if (dr2.HasRows)
                                            {
                                                while (dr2.Read())
                                                {
                                                    Double totProf = System.Math.Round((Double)dr2["totDLProfit"], 2);
                                                    Double totLoss = System.Math.Round((Double)dr2["totDLLoss"], 2);
                                                    Dl_UserStatement.Add(item: new DL_UserStatement
                                                    {
                                                        Client_Id = dist_id,
                                                        Client_Username = username,
                                                        credit_ref = totCredit,
                                                        Client_exposure = totProf,
                                                        Client_lib = totLoss,
                                                        Client_profit_loss = totProf - totLoss,
                                                        exposure_limit = totDebit,
                                                        Client_avl_balance = (totCredit - totDebit) + (totProf - totLoss)
                                                    });
                                                }

                                            }

                                        }

                                    }

                                }
                            }
                            else
                            {
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
            }
            return View("OverAllDL", Dl_UserStatement);
        }
        public ActionResult DlWiseUsrAcct()
        {
            CheckSession_MDL();
            string startDate = Request.QueryString["startDate"];
            string endDate = Request.QueryString["endDate"];
            string dlID = Request.QueryString["dist_id"];
            ViewBag.DLID = dlID;
            ViewBag.MdName = Request.QueryString["MdName"];
            ViewBag.dl_Name = GetDLUserName(int.Parse(dlID));
            var Dl_UserStatement = new List<DL_UserStatement>();
            try
            {
                if (startDate != null && startDate != "" && endDate != null && endDate != "")
                {

                    if (dlID != "" && dlID != null)
                    {
                        ViewBag.DLID = dlID;
                        using (SqlConnection con = ConnectionHandler.Connection())
                        {
                            SqlCommand comdbtCrdt = new SqlCommand("clTotDepositeWithdrawl", con);
                            comdbtCrdt.CommandType = System.Data.CommandType.StoredProcedure;
                            comdbtCrdt.Parameters.Add("@dlID", System.Data.SqlDbType.Int).Value = int.Parse(dlID);
                            comdbtCrdt.Parameters.Add("@startDate", System.Data.SqlDbType.DateTime).Value = startDate;
                            comdbtCrdt.Parameters.Add("@enddate", System.Data.SqlDbType.DateTime).Value = endDate;
                            con.Open();
                            SqlDataReader drdbtCrdt = comdbtCrdt.ExecuteReader();
                            if (drdbtCrdt.HasRows)
                            {
                                while (drdbtCrdt.Read())
                                {
                                    int usr_id = (int)drdbtCrdt["id"];
                                    string username = (string)drdbtCrdt["username"];
                                    Double totCredit = (Double)drdbtCrdt["totCredit"];
                                    Double totDebit = (Double)drdbtCrdt["totDebit"];

                                    SqlCommand comproloss = new SqlCommand("clTotProLoss", con);
                                    comproloss.CommandType = System.Data.CommandType.StoredProcedure;
                                    comproloss.Parameters.Add("@clID", System.Data.SqlDbType.Int).Value = usr_id;
                                    comproloss.Parameters.Add("@startDate", System.Data.SqlDbType.DateTime).Value = startDate;
                                    comproloss.Parameters.Add("@enddate", System.Data.SqlDbType.DateTime).Value = endDate;
                                    SqlDataReader drproloss = comproloss.ExecuteReader();
                                    if (drproloss.HasRows)
                                    {
                                        while (drproloss.Read())
                                        {
                                            Double totProf = (Double)drproloss["totProf"];
                                            totProf = System.Math.Round(totProf, 2);
                                            Double totLoss = (Double)drproloss["totLoss"];
                                            totLoss = System.Math.Round(totLoss, 2);
                                            Dl_UserStatement.Add(item: new DL_UserStatement
                                            {
                                                Client_Username = username,
                                                credit_ref = System.Math.Round(totCredit, 2),
                                                Client_exposure = System.Math.Round(totProf, 2),
                                                Client_lib = System.Math.Round(totLoss, 2),
                                                Client_profit_loss = System.Math.Round(totProf - totLoss, 2),
                                                exposure_limit = System.Math.Round(totDebit, 2),
                                                Client_avl_balance = System.Math.Round((totCredit - totDebit) + (totProf - totLoss), 2)
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
                        }

                    }
                }
            }

            catch (Exception)
            {

            }
            return View(Dl_UserStatement);
        }
    }
}