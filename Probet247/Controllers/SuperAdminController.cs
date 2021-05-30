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
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Probet247.Controllers
{
    public class SuperAdminController : Controller
    {
        // GET: SuperAdmin

        private SqlConnection con2;
        string SA_login_user_id = "";
        static string DlStaticId = "";

        private void connection2()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            con2 = new SqlConnection(constr);
        }
        public ActionResult Index()
        {
            CheckSession_SA();
            var Dl_UserStatement = new List<DL_UserStatement>();
            try
            {

                if (SA_login_user_id != "0" && SA_login_user_id != "" && SA_login_user_id != null)
                {
                    int SS_id = Int32.Parse(SA_login_user_id);
                    Double MBalance = GetMDLTBalance(SS_id);
                    Double TotalDLCoins = getTotalDLCoins(SS_id);
                    Double TotalClientCoins = getTotalClientCoins(SS_id);
                    Double TotalClientLib = getTotalClientLib(SS_id);
                    ViewBag.getTotalClientLib = TotalClientLib;
                    Double Self_bal = GetAdminself(SS_id);
                    ViewBag.DownLineTotal = MBalance + TotalDLCoins + TotalClientCoins + TotalClientLib;
                    ViewBag.DownLineAvlTotal = MBalance + TotalDLCoins + TotalClientCoins;
                    ViewBag.AvlTotal = MBalance + TotalDLCoins + TotalClientCoins+Self_bal;
                    connection2();
                    con2.Open();
                    SqlCommand com = new SqlCommand();
                    SqlDataReader dr;
                    com.Connection = con2;
                    com.CommandText = "SELECT id,username,password,name,status,balance,credit_ref FROM masterdistributors where admin_id='" + SA_login_user_id + "'";
                    dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            int login_user_id = (int)dr["id"];
                            string login_username = (string)dr["username"];
                            string login_password = (string)dr["password"];
                            string login_user_full_name = (string)dr["name"];
                            string user_status = (string)dr["status"];
                            Double login_user_balance = (Double)dr["balance"];
                            login_user_balance = System.Math.Round(login_user_balance, 2);
                            Double credit_ref = (Double)dr["credit_ref"];
                            Double user_exposure = 0;
                            Double MA_balance = getMDLDLBal(login_user_id);
                            Double PL_balance = getMDLClientBal(login_user_id);
                            Double Client_lib = getMDLClientLib(login_user_id);
                            Double TOTALBAL = login_user_balance + MA_balance + PL_balance + Client_lib;
                            Double TOTALAVLBAL = login_user_balance + MA_balance + PL_balance;
                            Double MDLPROF_LOSS = TOTALBAL-credit_ref;
                            MDLPROF_LOSS = System.Math.Round(MDLPROF_LOSS, 2);
                            Dl_UserStatement.Add(item: new DL_UserStatement
                            {
                                Client_Id = login_user_id,
                                Client_Username = login_username,
                                Client_balance = TOTALAVLBAL,
                                Client_avl_balance = 0,
                                Client_profit_loss = MDLPROF_LOSS,
                                Client_status = user_status,
                                Client_exposure = user_exposure,
                                Coin_Rate = 0,
                                MA_balance = TOTALBAL,
                                PL_balance = PL_balance,
                                Client_lib = Client_lib,
                                Client_pas = login_password,
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
            return View(Dl_UserStatement);
        }

        public void CheckSession_SA()
        {
            string SessionDLUName = (string)System.Web.HttpContext.Current.Session["Admin_UserName"];
            string SessionDL_hash_key = (string)System.Web.HttpContext.Current.Session["Admin_hash_key"];

            SA_login_user_id = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];

            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT id,username,name,balance,total_balance FROM admin WHERE id='" + SA_login_user_id + "' AND username='" + SessionDLUName + "' AND hash_key='" + SessionDL_hash_key + "' AND status='activate'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                int login_user_id = (int)dr["id"];
                string login_username = (string)dr["username"];
                string login_user_full_name = (string)dr["name"];
                Double login_user_balance = (Double)dr["balance"];
                Double login_user_tbalance = (Double)dr["total_balance"];

                ViewBag.DLUserID = login_user_id;
                ViewBag.login_username = login_username;
                ViewBag.login_user_full_name = login_user_full_name;
                ViewBag.login_user_balance = login_user_balance;
                ViewBag.login_user_tbalance = login_user_tbalance;
            }
            else
            {
                Logout();
            }
            con2.Close();
        }

        public ActionResult profitAndLoss()
        {
            CheckSession_SA();
            var DL_UserBetList = new List<ClientPLModel>();
            try
            {
                string GetDLIdIn11 = Request.QueryString["md_id"];
                ViewBag.mdidsendpl = GetDLIdIn11;
                if (GetDLIdIn11 != null)
                {
                    DlStaticId = GetDLIdIn11;
                    string vhbj = "0";
                    vhbj = GetDLIdIn11;
                    string sportid = "0";
                    string sportid1 = "0";
                    sportid = Request.QueryString["uid"];
                    sportid1 = Request.QueryString["dist_id"];
                    int Mdlid = Int32.Parse(GetDLIdIn11);
                    int dlid = Int32.Parse(sportid1);
                    ViewBag.DLUname = GetMDLUserName(Mdlid);
                    ViewBag.DLUna = GetDLUserName(dlid);
                    ViewBag.DLID = sportid1;
                    ViewBag.ClientId = sportid;
                    ViewBag.MDLID = GetDLIdIn11;
                    ViewBag.DLIDN = dlid;
                    if (sportid != null)
                    {
                        int gfh = Int32.Parse(sportid);
                        ViewBag.CUname = GetCUserName(gfh);
                        ViewBag.ClientId = sportid;
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            using (var cmd = new SqlCommand("SELECT [description],[created],[debit],[credit] FROM (select [description],[created],[debit],[credit], ROW_NUMBER() over(order by user_id) as row from  user_account_statements WHERE acc_stat_type='pl_coins' AND user_id ='" + sportid + "') a where a.row>0 and a.row<='1000'"))
                            {
                                cmd.Connection = con;
                                con.Open();
                                var dr = cmd.ExecuteReader();
                                while (dr.Read())
                                {
                                    DateTime created = (DateTime)dr["created"];
                                    Double debit = (Double)dr["debit"];
                                    Double credit = (Double)dr["credit"];
                                    string description = (string)dr["description"];
                                    DL_UserBetList.Add(item: new ClientPLModel
                                    {
                                        description = description,
                                        created = created,
                                        debit = debit,
                                        credit = credit
                                    });
                                }
                                con.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View(DL_UserBetList);
        }

        public ActionResult accountSummary()
        {
            CheckSession_SA();
            try
            {
                string GetmDLIdIn = Request.QueryString["md_id"];
                string GetDLIdIn = Request.QueryString["dist_id"];
                string Getuid = Request.QueryString["uid"];
                ViewBag.mdidsendpl = GetmDLIdIn;
                if (GetmDLIdIn != "0" && GetDLIdIn == "0" && Getuid == "0")
                {
                    int mdlid = Int32.Parse(GetmDLIdIn);
                    ViewBag.DLUname = GetMDLUserName(mdlid);
                    connection2();
                    con2.Open();
                    SqlCommand com = new SqlCommand();
                    SqlDataReader dr;
                    com.Connection = con2;
                    com.CommandText = "SELECT balance , username , name from [masterdistributors] where id='" + mdlid + "'";
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
                        ViewBag.UserExpC = getMDLClientLib(mdlid);
                        ViewBag.DLID = GetDLIdIn;
                        ViewBag.DLIDN = GetDLIdIn;
                        ViewBag.MDLID = GetmDLIdIn;
                        ViewBag.UserIdC = Getuid;
                        ViewBag.ClientId = Getuid;
                    }
                }
                else if (GetmDLIdIn != "0" && GetDLIdIn != "0" && Getuid == "0")
                {
                    int dlid = Int32.Parse(GetDLIdIn);
                    int mdlid = Int32.Parse(GetmDLIdIn);
                    ViewBag.DLUname = GetMDLUserName(mdlid);
                    ViewBag.DLUna = GetDLUserName(dlid);
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
                        ViewBag.DLID = GetDLIdIn;
                        ViewBag.DLIDN = GetDLIdIn;
                        ViewBag.MDLID = GetmDLIdIn;
                        ViewBag.UserIdC = Getuid;
                        ViewBag.ClientId = Getuid;
                    }
                }
                else if (GetmDLIdIn != "0" && GetDLIdIn != "0" && Getuid != "0")
                {
                    int dlid = Int32.Parse(GetDLIdIn);
                    int mdlid = Int32.Parse(GetmDLIdIn);
                    ViewBag.DLUnameM = GetDLUserName(dlid);
                    ViewBag.DLUname = GetMDLUserName(mdlid);
                    ViewBag.DLUna = GetDLUserName(dlid);
                    ViewBag.DLID = GetDLIdIn;
                    ViewBag.DLIDN = GetDLIdIn;
                    ViewBag.MDLID = GetmDLIdIn;
                    ViewBag.UserIdC = Getuid;
                    int gfh = Int32.Parse(Getuid);
                    ViewBag.CUname = GetCUserName(gfh);
                    ViewBag.ClientId = Getuid;
                    int UidInt = Int32.Parse(Getuid);
                    ViewBag.UserNameC = GetCUserName(UidInt);
                    ViewBag.FUserNameC = GetCFUserName(UidInt);
                    ViewBag.UserBalC = GetCbalance(UidInt);
                    ViewBag.UserExpC = GetCexposure(UidInt);
                }
                else
                {
                    ViewBag.DLID = 0;
                    ViewBag.MDLID = 0;
                    ViewBag.ClientId = 0;
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult betList()
        {
            CheckSession_SA();
            var DL_UserBetList = new List<MatchedClientBetList>();
            try
            {
                string GetDLIdIn = Request.QueryString["dist_id"];
                string GetmDLIdIn = Request.QueryString["md_id"];
                ViewBag.mdidsendpl = GetmDLIdIn;
                ViewBag.DLID = "0";
                ViewBag.ClientId = "0";

                if (GetmDLIdIn != null)
                {
                    int mdlidn = Int32.Parse(GetmDLIdIn);
                    ViewBag.DLUname = GetMDLUserName(mdlidn);
                    DlStaticId = GetDLIdIn;
                    string hghg = "0";
                    string ytfytfy = "0";
                    hghg = DlStaticId;
                    string sportid = Request.QueryString["uid"];
                    ytfytfy = sportid;
                    int dlid = Int32.Parse(GetDLIdIn);
                    ViewBag.DLUnameM = GetDLUserName(dlid);
                    ViewBag.DLUna = GetMDLUserName(dlid);
                    ViewBag.DLID = hghg;
                    ViewBag.DLIDN = GetDLIdIn;
                    ViewBag.MDLID = GetmDLIdIn;
                    if (sportid != null)
                    {
                        int gfh = Int32.Parse(sportid);
                        ViewBag.CUname = GetCUserName(gfh);
                        ViewBag.ClientId = ytfytfy;
                        string SA_login_user_id = (string)System.Web.HttpContext.Current.Session["SA_login_user_id"];
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            using (var cmd = new SqlCommand("SELECT [user_id],[id],[place_time],[event_id],[betfair_id],[field],[field_pos],[rate],[stakes],[total_value] FROM (select [user_id],[id],[place_time],[event_id],[betfair_id],[field],[field_pos],[rate],[stakes],[total_value], ROW_NUMBER() over(order by id) as row from  live_bet WHERE user_id='" + sportid + "' AND dist_id='" + GetDLIdIn + "') a where a.row>0 and a.row<='10'"))
                            {
                                cmd.Connection = con;
                                con.Open();
                                var dr = cmd.ExecuteReader();
                                while (dr.Read())
                                {
                                    DateTime time = (DateTime)dr["place_time"];
                                    string format = "yyyy-MM-dd HH:mm:ss";

                                    string Event_code_Get = (string)dr["event_id"];
                                    int betid = (int)dr["id"];
                                    int user_idC = (int)dr["user_id"];
                                    string GEtCName = GetCUserName(user_idC);
                                    string betfair_id_Get = (string)dr["betfair_id"];
                                    string field = (string)dr["field"];
                                    string field_pos = (string)dr["field_pos"];
                                    Double rate = (Double)dr["rate"];
                                    Double stakes = (Double)dr["stakes"];
                                    Double profit_loss = (Double)dr["total_value"];
                                    string GetEventTName = GetEventTitleName(Event_code_Get);
                                    string GetMarketName = FunctionDataController.GetMArketName(Event_code_Get, betfair_id_Get);
                                    string GetSportsNameS = GetSportsName(Event_code_Get);
                                    DL_UserBetList.Add(item: new MatchedClientBetList
                                    {
                                        EventTime = time.ToString(format),
                                        Description = GetSportsNameS,
                                        Field = field,
                                        Rate = rate,
                                        Stakes = stakes,
                                        PL = profit_loss,
                                        GetEventTName = GetEventTName,
                                        GetMarketName = GetMarketName,
                                        betid = betid,
                                        Field_pos = field_pos,
                                        ucname = GEtCName
                                    });
                                }
                                con.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View(DL_UserBetList);
        }

        public ActionResult transactionHistory()
        {
            CheckSession_SA();
            var DL_UserBetList = new List<ClientaccountCashStatement>();
            try
            {
                string GetmDLIdIn = Request.QueryString["md_id"];
                string GetDLIdIn = Request.QueryString["dist_id"];
                string Getuid = Request.QueryString["uid"];
                ViewBag.mdidsendpl = GetmDLIdIn;
                ViewBag.DLID = "0";
                ViewBag.ClientId = "0";
                int mdlidn = Int32.Parse(GetmDLIdIn);
                ViewBag.DLUname = GetMDLUserName(mdlidn);
                if (GetmDLIdIn != "0" && GetDLIdIn == "0" && Getuid == "0")
                {
                    int mdlid = Int32.Parse(GetmDLIdIn);
                    ViewBag.DLUname = GetMDLUserName(mdlid);
                    connection2();
                    con2.Open();
                    SqlCommand com = new SqlCommand();
                    SqlDataReader dr1;
                    com.Connection = con2;
                    com.CommandText = "SELECT username  from [masterdistributors] where id='" + mdlid + "'";
                    dr1 = com.ExecuteReader();
                    if (dr1.HasRows)
                    {
                        dr1.Read();
                        string DLUnameM = (String)dr1["username"];
                        ViewBag.DLUnameM = DLUnameM;
                        ViewBag.UserNameC = DLUnameM;
                        ViewBag.CUname = "";
                        ViewBag.DLID = GetDLIdIn;
                        ViewBag.DLIDN = GetDLIdIn;
                        ViewBag.MDLID = GetmDLIdIn;
                        ViewBag.UserIdC = Getuid;
                        ViewBag.ClientId = Getuid;
                    }
                    con2.Close();
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        using (var cmd = new SqlCommand("SELECT created,debit,credit,balance,remark,description FROM md_account_statements WHERE md_id='" + mdlid + "'  AND acc_stat_type = 'dw_coins' order by created desc "))
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
                else if (GetmDLIdIn != "0" && GetDLIdIn != "0" && Getuid == "0")
                {
                    int dlid = Int32.Parse(GetDLIdIn);
                    int mdlid = Int32.Parse(GetmDLIdIn);
                    ViewBag.DLUname = GetMDLUserName(mdlid);
                    ViewBag.DLUna = GetDLUserName(dlid);
                    connection2();
                    con2.Open();
                    SqlCommand com = new SqlCommand();
                    SqlDataReader dr1;
                    com.Connection = con2;
                    com.CommandText = "SELECT username  from [distributors] where id='" + dlid + "'";
                    dr1 = com.ExecuteReader();
                    if (dr1.HasRows)
                    {
                        dr1.Read();
                        string DLUnameM = (String)dr1["username"];
                        ViewBag.DLUnameM = DLUnameM;
                        ViewBag.UserNameC = DLUnameM;
                        ViewBag.CUname = "";
                        ViewBag.DLID = GetDLIdIn;
                        ViewBag.DLIDN = GetDLIdIn;
                        ViewBag.MDLID = GetmDLIdIn;
                        ViewBag.UserIdC = Getuid;
                        ViewBag.ClientId = Getuid;
                    }
                    con2.Close();
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        using (var cmd = new SqlCommand("SELECT created,debit,credit,balance,remark,description FROM dist_account_statements WHERE dist__id='" + dlid + "'  AND acc_stat_type = 'dw_coins' order by created desc "))
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
                else if (GetmDLIdIn != "0" && GetDLIdIn != "0" && Getuid != "0")
                {
                    int dlid = Int32.Parse(GetDLIdIn);
                    int mdlid = Int32.Parse(GetmDLIdIn);
                    ViewBag.DLUnameM = GetDLUserName(dlid);
                    ViewBag.DLUname = GetMDLUserName(mdlid);
                    ViewBag.DLUna = GetDLUserName(dlid);
                    ViewBag.DLID = GetDLIdIn;
                    ViewBag.DLIDN = GetDLIdIn;
                    ViewBag.MDLID = GetmDLIdIn;
                    ViewBag.UserIdC = Getuid;
                    int gfh = Int32.Parse(Getuid);
                    ViewBag.CUname = GetCUserName(gfh);
                    ViewBag.ClientId = Getuid;
                    int UidInt = Int32.Parse(Getuid);
                    ViewBag.UserNameC = GetCUserName(UidInt);

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        using (var cmd = new SqlCommand("SELECT created,debit,credit,balance,remark,description FROM user_account_statements WHERE user_id='" + Getuid + "'  AND acc_stat_type = 'dw_coins' order by created desc "))
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
                else
                {
                    ViewBag.DLID = 0;
                    ViewBag.MDLID = 0;
                    ViewBag.ClientId = 0;
                }

            }
            catch (Exception ex)
            {

            }
            return View(DL_UserBetList);
        }

        public ActionResult cashBanking()
        {
            CheckSession_SA();
            var DL_UserStatement = new List<DL_UserStatement>();
            try
            {
                string SA_login_user_id = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];
                connection2();
                con2.Open();
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con2;
                com.CommandText = "SELECT id,username,name,status,balance,credit_ref FROM masterdistributors WHERE admin_id='" + SA_login_user_id + "'";
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
                        Double MA_balance = getMDLDLBal(login_user_id);
                        Double PL_balance = getMDLClientBal(login_user_id);
                        Double Client_lib = getMDLClientLib(login_user_id);
                        Double TOTALBAL = login_user_balance + MA_balance + PL_balance + Client_lib;
                        Double Ref_pl = TOTALBAL - credit_ref;
                        DL_UserStatement.Add(item: new DL_UserStatement
                        {
                            Client_Id = login_user_id,
                            Client_Username = login_username + "( " + login_user_full_name + " )",
                            Client_balance = TOTALBAL,
                            Client_avl_balance = login_user_balance,
                            Client_profit_loss = Ref_pl,
                            Client_status = user_status,
                            Client_exposure = Client_lib
                        });
                    }
                }
                else { }
                con2.Close();
            }
            catch (Exception ex)
            {

            }
            return View(DL_UserStatement);
        }
        public ActionResult power_panel()
        {
            //CheckSession_SA();
            var AllSportsAddM = new List<NavbarListAllSports>();
            try
            {
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

                            AllSportsAddM.Add(item: new NavbarListAllSports
                            {
                                SportsName = SportName,
                                SportsId = Sportsid
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
        public Double GetMDRate(int user_id)
        {
            Double coin_rate = 0;
            connection2();
            con2.Open();
            using (SqlCommand cmd1 = new SqlCommand("SELECT coin_rate from [masterdistributors] where id='" + user_id + "'", con2))
            {
                var dr1 = cmd1.ExecuteReader();
                if (dr1.HasRows)
                {
                    dr1.Read();
                    coin_rate = (Double)dr1["coin_rate"];
                }
            }
            con2.Close();
            return coin_rate;
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
                username = System.Math.Round(username, 2);
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
                username = System.Math.Round(username, 2);
            }
            con2.Close();
            return username;
        }

        public string GetMDLUserName(int user_id)
        {
            string username = "";
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT username from [masterdistributors] where id='" + user_id + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                username = (string)dr["username"];
            }
            con2.Close();
            return username;
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

        public Double GetAdminself(int user_id)
        {
            Double username = 0;
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT balance from [admin] where id='" + user_id + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                username = (Double)dr["balance"];
                username = System.Math.Round(username, 2);
            }
            con2.Close();
            return username;
        }

        public Double GetAdminTPL(int user_id)
        {
            Double username = 0;
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT profit_loss from [admin] where id='" + user_id + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                username = (Double)dr["profit_loss"];
                username = System.Math.Round(username, 2);
            }
            con2.Close();
            return username;
        }

        public Double GetAdminComm(int user_id)
        {
            Double username = 0;
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT commission from [admin] where id='" + user_id + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                username = (Double)dr["commission"];
                username = System.Math.Round(username, 2);
            }
            con2.Close();
            return username;
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
                    sport_id = System.Math.Round(sport_id, 2);
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
                    sport_id = System.Math.Round(sport_id, 2);
                }
                con.Close();
            }
            return sport_id;
        }

        public Double getTotalDLCoins(int mdlid)
        {
            Double sport_id = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT COALESCE(sum(balance),0) AS t_bal FROM distributors WHERE admin_id='" + mdlid + "'", con))
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    sport_id = (Double)reader["t_bal"];
                    sport_id = System.Math.Round(sport_id, 2);
                }
                con.Close();
            }
            return sport_id;
        }
        public Double getTotalClientCoins(int mdlid)
        {
            Double sport_id = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT COALESCE(sum(balance),0) AS t_bal FROM users_client WHERE admin_id='" + mdlid + "'", con))
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    sport_id = (Double)reader["t_bal"];
                    sport_id = System.Math.Round(sport_id, 2);
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
                    sport_id = System.Math.Round(sport_id, 2);
                }
                con.Close();
            }
            return sport_id;
        }

        public Double getTotalClientLib(int mdlid)
        {
            Double sport_id = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT COALESCE(sum(exposure),0) AS t_bal FROM users_client WHERE admin_id='" + mdlid + "'", con))
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    sport_id = (Double)reader["t_bal"];
                    sport_id = System.Math.Round(sport_id, 2);
                }
                con.Close();
            }
            return sport_id;
        }

        public List<DLClientList> MDlClientList1()
        {
            var messages = new List<DLClientList>();
            try
            {
                string SA_login_user_id = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    string stmt = "select username,id from masterdistributors where admin_id='" + SA_login_user_id + "' ";

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

        public JsonResult MDlClientList()
        {

            List<DLClientList> messages = new List<DLClientList>();
            messages = MDlClientList1();
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<DLClientList> DlClientList1(int uid)
        {
            var messages = new List<DLClientList>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    string stmt = "select username,id from distributors where md_id='" + uid + "' ";

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
                                id = id,
                                md_id = uid
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

        public JsonResult DlClientList(int mdlid)
        {
            List<DLClientList> messages = new List<DLClientList>();
            messages = DlClientList1(mdlid);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<DLClientList> DlClientList11(int uid)
        {
            var messages = new List<DLClientList>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    string stmt = "select username,id,mdl_id from users_client where dl_id='" + uid + "' ";

                    using (SqlCommand cmdCount = new SqlCommand(stmt, con))
                    {
                        cmdCount.ExecuteScalar();
                        var reader = cmdCount.ExecuteReader();
                        while (reader.Read())
                        {
                            string username = (string)reader["username"];
                            int id = (int)reader["id"];
                            int mdl_id = (int)reader["mdl_id"];

                            messages.Add(item: new DLClientList
                            {
                                username = username,
                                id = id,
                                md_id = mdl_id,
                                dl_id = uid
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

        public JsonResult UClientList(int mdlid)
        {
            List<DLClientList> messages = new List<DLClientList>();
            messages = DlClientList11(mdlid);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public ActionResult home_dl()
        {
            CheckSession_SA();
            var Dl_UserStatement = new List<DL_UserStatement>();
            try
            {
                string sportid = Request.QueryString["md_id"];
                int dfgvhb = Int32.Parse(sportid);
                ViewBag.MBalance = GetMDLBalance(dfgvhb);
                ViewBag.DLBalance = getMDLDLBal(dfgvhb); ;
                ViewBag.MDLBalance = getMDLClientBal(dfgvhb); ;
                ViewBag.MDLLib = getMDLClientLib(dfgvhb); ;
                ViewBag.MDLComm = GetMDLComm(dfgvhb); ;
                if (sportid != "0" && sportid != "" && sportid != null)
                {
                    connection2();
                    con2.Open();
                    SqlCommand com = new SqlCommand();
                    SqlDataReader dr;
                    com.Connection = con2;
                    com.CommandText = "SELECT id,username,name,status,balance,total_balance,profit_loss,coin_rate FROM distributors where md_id='" + sportid + "'";
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
                            Double login_user_total_balance = (Double)dr["total_balance"];
                            Double user_profit_loss = (Double)dr["coin_rate"];
                            Double user_exposure = getDLClientLib(login_user_id);
                            Double user_t_bal = getTClientBal(login_user_id);
                            Double DLTOTALBL = user_t_bal + user_exposure + login_user_balance;
                            Double DLAVLBL = user_t_bal + login_user_balance;
                            Double DLPROF_LOSS = -(login_user_total_balance - DLTOTALBL);
                            DLPROF_LOSS = System.Math.Round(DLPROF_LOSS, 2);
                            Dl_UserStatement.Add(item: new DL_UserStatement
                            {
                                Client_Id = login_user_id,
                                Client_Username = login_username,
                                Client_balance = DLTOTALBL,
                                Client_avl_balance = DLAVLBL,
                                Client_profit_loss = user_profit_loss,
                                Client_status = user_status,
                                Client_exposure = user_exposure,
                                Client_ttl_balance = user_t_bal,
                                DLPROF_LOSS = DLPROF_LOSS,
                                md_Id = dfgvhb
                            });
                        }
                    }
                    else { }
                    con2.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return View(Dl_UserStatement);
        }

        public ActionResult home_client()
        {
            CheckSession_SA();
            var Dl_UserStatement = new List<DL_UserStatement>();
            try
            {
                string sportid = Request.QueryString["md_id"];
                string dist_id = Request.QueryString["dist_id"];
                string word = Request.QueryString["word"];
                string qwes = "";
                if (word != null && word != "All")
                {
                    qwes = "AND username LIKE ('" + word + "%')";
                }
                ViewBag.MDDlID = sportid;
                ViewBag.DDlID = dist_id;
                int dfgvhb = Int32.Parse(sportid);
                int dfgvh5b = Int32.Parse(dist_id);
                Double Dlbalance = 0;
                Double Clientbalance = 0;
                Double ClientLib = 0;
                Double total_balll = 0;
                Double Net_total_balll = 0;
                Dlbalance = GetDLBalance(dfgvh5b);
                ViewBag.Dlbalance = Dlbalance;
                Clientbalance = getDLClientBal(dfgvh5b);
                ViewBag.Clientbalance = Clientbalance;
                ClientLib = getDLClientLib(dfgvh5b);
                ViewBag.ClientLib = ClientLib;
                total_balll = Clientbalance + ClientLib;
                ViewBag.total_balll = total_balll;
                Net_total_balll = Dlbalance + Clientbalance + ClientLib;
                ViewBag.Net_total_balll = Net_total_balll;
                ViewBag.DlPl = GetDLPL(dfgvh5b);
                ViewBag.Dlcomm = GetDLComm(dfgvh5b);
                if (dist_id != "0" && dist_id != "" && dist_id != null)
                {
                    connection2();
                    con2.Open();
                    SqlCommand com = new SqlCommand();
                    SqlDataReader dr;
                    com.Connection = con2;
                    com.CommandText = "SELECT id,username,name,status,balance,total_balance,profit_loss,exposure FROM users_client WHERE dl_id='" + dist_id + "' " + qwes;
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
                            Double login_user_total_balance = (Double)dr["total_balance"];
                            login_user_total_balance = System.Math.Round(login_user_total_balance, 2);
                            Double user_profit_loss = (Double)dr["profit_loss"];
                            user_profit_loss = System.Math.Round(user_profit_loss, 2);
                            Double user_exposure = (Double)dr["exposure"];
                            user_exposure = System.Math.Round(user_exposure, 2);
                            Dl_UserStatement.Add(item: new DL_UserStatement
                            {
                                Client_Id = login_user_id,
                                Client_Username = login_username,
                                Client_balance = login_user_balance,
                                Client_avl_balance = login_user_total_balance,
                                Client_profit_loss = user_profit_loss,
                                Client_status = user_status,
                                Client_exposure = user_exposure,
                                md_Id = dfgvhb,
                                dl_Id = dfgvh5b
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
                username = System.Math.Round(username, 0);
            }
            con2.Close();
            return username;
        }

        public Double GetMDLBalance(int user_id)
        {
            Double username = 0;
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT balance from [masterdistributors] where id='" + user_id + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                username = (Double)dr["balance"];
                username = System.Math.Round(username, 0);
            }
            con2.Close();
            return username;
        }

        public Double GetMDLTBalance(int user_id)
        {
            Double username = 0;
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT COALESCE(sum(balance),0) AS t_bal FROM [masterdistributors] where admin_id='" + user_id + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                username = (Double)dr["t_bal"];
                username = System.Math.Round(username, 0);
            }
            con2.Close();
            return username;
        }

        public Double GetDLPL(int user_id)
        {
            Double username = 0;
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT profit_loss from [distributors] where id='" + user_id + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                username = (Double)dr["profit_loss"];
                username = System.Math.Round(username, 0);
            }
            con2.Close();
            return username;
        }

        public Double GetDLComm(int user_id)
        {
            Double username = 0;
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT commission from [distributors] where id='" + user_id + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                username = (Double)dr["commission"];
                username = System.Math.Round(username, 0);
            }
            con2.Close();
            return username;
        }

        public Double GetMDLComm(int user_id)
        {
            Double username = 0;
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT commission from [masterdistributors] where id='" + user_id + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                username = (Double)dr["commission"];
                username = System.Math.Round(username, 0);
            }
            con2.Close();
            return username;
        }

        public Double getDLClientBal(int mdlid)
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
                    sport_id = System.Math.Round(sport_id, 2);
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
                    sport_id = System.Math.Round(sport_id, 2);
                }
                con.Close();
            }
            return sport_id;
        }

        public ActionResult myAccountSummary()
        {
            CheckSession_SA();
            return View();
        }
        public ActionResult accountCashStatement()
        {
            CheckSession_SA();
            var DL_UserBetList = new List<AccountStatement_DL>();
            try
            {
                string SA_login_user_idn = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];
                connection2();
                con2.Open();
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con2;
                com.CommandText = "SELECT created,debit,credit,balance,remark,description FROM admin_account_statements WHERE admin_id='" + SA_login_user_idn + "' AND acc_stat_type = 'dw_coins' order by created desc ";
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
                }
            }
            catch (Exception ex)
            {

            }
            return View(DL_UserBetList);
        }
        public ActionResult profile()
        {
            CheckSession_SA();
            return View();
        }

        public ActionResult marketProfitLoss()
        {
            CheckSession_SA();
            var DL_UserBetList = new List<ClientPLModel>();
            try
            {
                string Admin_ID = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];

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
                        using (var cmd = new SqlCommand("SELECT distinct(event_id)  FROM(SELECT distinct(event_id) , ROW_NUMBER() over(order by id desc) as row FROM pl_statement WHERE (created BETWEEN '" + startDate + "' AND '" + endDate + "') and sport_id='" + sport_id + "' AND admin_id='" + Admin_ID + "' ) a where a.row > '" + min_page + "'  and a.row <= '" + max_page + "' "))
                        {
                            cmd.Connection = con;
                            con.Open();
                            var dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                string event_id_g = (string)dr["event_id"];
                                using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                                {
                                    using (var cmd1 = new SqlCommand("select created,total_pl from pl_statement where admin_id='" + Admin_ID + "' AND event_id='" + event_id_g + "'"))
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

                        string stmts = "SELECT COUNT(id) FROM pl_statement WHERE (created BETWEEN '" + startDate + "' AND '" + endDate + "') AND  sport_id='" + sport_id + "' AND admin_id='" + Admin_ID + "' ";
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
            CheckSession_SA();
            var DL_UserBetList = new List<ClientPLModel>();
            try
            {
                string event_id_g = Request.QueryString["event_id"];
                string login_user_idn = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("select distinct(market_id) from pl_statement where admin_id='" + login_user_idn + "' AND event_id='" + event_id_g + "'"))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            string market_id_g = (string)dr["market_id"];
                            using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                            {
                                using (var cmd1 = new SqlCommand("select created,total_pl,description from pl_statement where admin_id='" + login_user_idn + "' AND market_id='" + market_id_g + "'"))
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
            CheckSession_SA();
            var DL_UserBetList = new List<ClientPLModel>();
            try
            {
                string event_id_g = Request.QueryString["event_id"];
                string market_id_g = Request.QueryString["market_id"];
                string login_user_idn = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];

                using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd1 = new SqlCommand("select created,debit,credit,user_id,md_id,dist_id,description from user_account_statements where admin_id='" + login_user_idn + "' AND market_id='" + market_id_g + "' AND event_id='" + event_id_g + "'"))
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
                        while (dr1.Read())
                        {
                            created = (DateTime)dr1["created"];
                            debit = (Double)dr1["debit"];
                            credit = (Double)dr1["credit"];
                            ucid = (int)dr1["user_id"];
                            int did = (int)dr1["dist_id"];
                            int mdid = (int)dr1["md_id"];
                            CUname = GetCUserName(ucid);
                            String DLname = GetDLUserName(did);
                            String MDLname = GetMDLUserName(mdid);
                            description = (string)dr1["description"];

                            DL_UserBetList.Add(item: new ClientPLModel
                            {
                                description = description,
                                created = created,
                                debit = debit,
                                credit = credit,
                                uname = CUname+" ["+DLname+"] "+" ["+MDLname+"]"
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

        public ActionResult RiskManagement()
        {
            CheckSession_SA();
            return View();
        }

        public ActionResult adminanalysis()
        {
            adminanalysisRepositary _messageRepository = new adminanalysisRepositary();
            return PartialView("adminanalysis", _messageRepository.GetAllMessages());
        }
        public ActionResult Msdlanuhytalysis()
        {
            AdminSessionRepository _sessionRepository = new AdminSessionRepository();
            return PartialView("adminsanalysisdata", _sessionRepository.GetAllMessagesS());
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
                    string admin_id = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];

                    Double TotalA = 0;
                    Double TotalB = 0;
                    Double TotalC = 0;
                    Double TotalD = 0;
                    Double TotalE = 0;
                    Double TotalF = 0;

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        SqlCommand sqlmodel = new SqlCommand("SELECT [user_id],[stakes],[total_value],[field],[rate] FROM live_bet_new WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_id + "' AND admin_id = '" + admin_id + "' ", con);
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
                            DlRatioA = System.Math.Round(TotalF, 2),
                            DlRatioB = System.Math.Round(TotalE, 2),
                            DlRatioC = System.Math.Round(TotalD, 2),
                            DlRatioD = System.Math.Round(TotalC, 2),
                            DlRatioE = System.Math.Round(TotalB, 2),
                            DlRatioF = System.Math.Round(TotalA, 2),
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
                    string admin_id = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];

                    var user_id = new List<int>();
                    var stakes = new List<Double>();
                    var total_value = new List<Double>();
                    var field = new List<string>();
                    var rate = new List<Double>();

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        SqlCommand sqlmodel = new SqlCommand("SELECT [md_id],[stakes],[total_value],[field],[rate] FROM live_bet_new WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_id + "' AND admin_id = '" + admin_id + "' ", con);
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
                                int user_id1 = (int)datamodel["md_id"];
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
                        SqlCommand sqlm = new SqlCommand("SELECT DISTINCT [md_id] FROM live_bet_new WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_id + "' AND admin_id = '" + admin_id + "' ", con);
                        var datam = sqlm.ExecuteReader();
                        if (datam.HasRows)
                        {
                            while (datam.Read())
                            {
                                int user_id01 = (Int32)datam["md_id"];
                                string client_name1 = GetMDLUserName(user_id01);
                                Double dl_rate = 100;
                                Double ag_rate = 100 - dl_rate;
                                Double mdl_rate = GetMDRate(user_id01);
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
                                    DlRatioA = System.Math.Round(TotalF, 2),
                                    DlRatioB = System.Math.Round(TotalE, 2),
                                    DlRatioC = System.Math.Round(TotalD, 2),
                                    DlRatioD = System.Math.Round(TotalC, 2),
                                    DlRatioE = System.Math.Round(TotalB, 2),
                                    DlRatioF = System.Math.Round(TotalA, 2),
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
                System.Diagnostics.Debug.WriteLine("ErrorC" + ex.ToString());
            }

            return messages;
        }

        public JsonResult SessionDldatabkmdl(string betfair_id, string event_id, string cid)
        {
            List<Sessiondldata> messages = new List<Sessiondldata>();
            messages = SessionDldata2mdl(betfair_id, event_id, cid);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<Sessiondldata> SessionDldata2mdl(string betfair_id, string event_id, string cid)
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
                        SqlCommand sqlmodel = new SqlCommand("SELECT [dist_id],[stakes],[total_value],[field],[rate] FROM live_bet_new WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_id + "' AND md_id = '" + cid + "' ", con);
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
                        SqlCommand sqlm = new SqlCommand("SELECT DISTINCT [dist_id] FROM live_bet_new WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_id + "' AND md_id = '" + cid + "' ", con);
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
                                    DlRatioA = System.Math.Round(TotalF, 2),
                                    DlRatioB = System.Math.Round(TotalE, 2),
                                    DlRatioC = System.Math.Round(TotalD, 2),
                                    DlRatioD = System.Math.Round(TotalC, 2),
                                    DlRatioE = System.Math.Round(TotalB, 2),
                                    DlRatioF = System.Math.Round(TotalA, 2),
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
                System.Diagnostics.Debug.WriteLine("ErrorC" + ex.ToString());
            }

            return messages;
        }

        public JsonResult SessionDldatabkdl(string betfair_id, string event_id, string cid)
        {
            List<Sessiondldata> messages = new List<Sessiondldata>();
            messages = SessionDldata2dl(betfair_id, event_id, cid);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<Sessiondldata> SessionDldata2dl(string betfair_id, string event_id, string cid)
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
                        SqlCommand sqlmodel = new SqlCommand("SELECT [user_id],[stakes],[total_value],[field],[rate] FROM live_bet_new WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_id + "' AND dist_id = '" + cid + "' ", con);
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
                        SqlCommand sqlm = new SqlCommand("SELECT DISTINCT [user_id] FROM live_bet_new WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_id + "' AND dist_id = '" + cid + "' ", con);
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
                                    DlRatioA = System.Math.Round(TotalF, 2),
                                    DlRatioB = System.Math.Round(TotalE, 2),
                                    DlRatioC = System.Math.Round(TotalD, 2),
                                    DlRatioD = System.Math.Round(TotalC, 2),
                                    DlRatioE = System.Math.Round(TotalB, 2),
                                    DlRatioF = System.Math.Round(TotalA, 2),
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
                System.Diagnostics.Debug.WriteLine("ErrorC" + ex.ToString());
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
        public ActionResult sbookviewmdl()
        {
            string betfair_id = Request.QueryString["betfair_id"];
            string event_id = Request.QueryString["event_code"];
            string cid = Request.QueryString["uid"];
            string EventNameGet = GetEventTitleName(event_id);
            ViewBag.betfair_id = betfair_id;
            ViewBag.EventNameB = EventNameGet;
            ViewBag.event_id = event_id;
            ViewBag.cid = cid;
            return View();
        }
        public ActionResult sbookviewdl()
        {
            string betfair_id = Request.QueryString["betfair_id"];
            string event_id = Request.QueryString["event_code"];
            string cid = Request.QueryString["uid"];
            string EventNameGet = GetEventTitleName(event_id);
            ViewBag.betfair_id = betfair_id;
            ViewBag.EventNameB = EventNameGet;
            ViewBag.event_id = event_id;
            ViewBag.cid = cid;
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

        public ActionResult bookviewmdl()
        {
            string event_id = Request.QueryString["event_code"];
            string betfair_id = Request.QueryString["betfair_id"];
            string md_id = Request.QueryString["uid"];
            string EventNameGet = GetEventTitleName(event_id);
            ViewBag.EventNameB = EventNameGet;
            ViewBag.BetfairId = betfair_id;
            ViewBag.event_id = event_id;
            ViewBag.cid = md_id;
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
            ViewBag.cid = dist_id;
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
                string user_ids = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT place_time,field,user_id,rate,stakes,total_value,event_id FROM live_bet WHERE admin_id='" + user_ids + "' AND betfair_id='" + betfair_id + "' AND event_id='" + event_code + "'"))
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
                string user_ids = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT distinct(md_id) from live_bet where betfair_id='" + bfid + "' AND admin_id='" + user_ids + "' AND status='' AND odds_type='MO'", con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Double teamAWD = 0;
                            Double teamBWD = 0;
                            Double teamCWD = 0;
                            int md_idN = (int)reader["md_id"];
                            string UCname = GetMDLUserName(md_idN);
                            Double dl_rate = 100;
                            Double ag_rate = 0;
                            Double mdl_rate = GetMDRate(md_idN);
                            Double agm_rate = dl_rate - mdl_rate;
                            Double admin_rate = mdl_rate;
                            using (var cmd2 = new SqlCommand("select runner_pos,amount from runner_cal where md_id='" + md_idN + "' AND market_id='" + bfid + "'", con))
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
                                user_id = md_idN,
                                cname = UCname,
                                teamAWD = System.Math.Round(teamAWD, 2),
                                teamBWD = System.Math.Round(teamBWD, 2),
                                teamCWD = System.Math.Round(teamCWD, 2),
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

        public JsonResult BookviewSVmdl(string bfid, string uid)
        {
            List<BookViewAg> messages = new List<BookViewAg>();
            messages = BookviewSmdl(bfid, uid);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }
        public List<BookViewAg> BookviewSmdl(string bfid, string uid)
        {
            var AllSportsAddM = new List<BookViewAg>();
            try
            {
                string user_ids = uid;
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
                            int user_idN = (int)reader["dist_id"];
                            string UCname = GetDLUserName(user_idN);
                            Double dl_rate = GetDLRate(user_idN);
                            Double ag_rate = 0;
                            Double mdl_rate = GetMDLRate(user_idN);
                            Double agm_rate = dl_rate - mdl_rate;
                            Double admin_rate = mdl_rate;
                            using (var cmd2 = new SqlCommand("select runner_pos,amount from runner_cal where dist_id='" + user_idN + "' AND market_id='" + bfid + "'", con))
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
                                user_id = user_idN,
                                cname = UCname,
                                teamAWD = System.Math.Round(teamAWD, 2),
                                teamBWD = System.Math.Round(teamBWD, 2),
                                teamCWD = System.Math.Round(teamCWD, 2),
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
                                teamAWD = System.Math.Round(teamAWD, 2),
                                teamBWD = System.Math.Round(teamBWD, 2),
                                teamCWD = System.Math.Round(teamCWD, 2),
                                teamAWDDL = System.Math.Round(teamAWDDL, 2),
                                teamBWDDL = System.Math.Round(teamBWDDL, 2),
                                teamCWDDL = System.Math.Round(teamCWDDL, 2),
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

        public ActionResult mdlallbet()
        {
            var DL_UserBetList = new List<MatchedClientBetList>();
            try
            {
                string uid = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];
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
                    using (var cmd = new SqlCommand("SELECT Distinct(md_id) FROM live_bet WHERE admin_id='" + uid + "' AND event_id='" + event_code + "' AND betfair_id='" + betfair_id + "' "))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            int md_id = (Int32)dr["md_id"];
                            string UsernameC = GetMDLUserName(md_id);
                            DL_UserBetList.Add(item: new MatchedClientBetList
                            {
                                EventTime = "-",
                                uid = md_id,
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
        public ActionResult dlallbet()
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
                    using (var cmd = new SqlCommand("SELECT place_time,field,user_id,rate,stakes,total_value,event_id FROM live_bet WHERE dist_id='" + uid + "' AND event_id='" + event_code + "' AND betfair_id='" + betfair_id + "' order by place_time desc"))
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
                                Stakes = System.Math.Round(stakes, 2),
                                PL = System.Math.Round(profit_loss, 2),
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
                    using (var cmd = new SqlCommand("SELECT place_time,field,user_id,rate,stakes,total_value,event_id FROM live_bet WHERE user_id='" + uid + "' AND event_id='" + event_code + "' AND betfair_id='" + betfair_id + "' order by place_time desc"))
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
                                Stakes = System.Math.Round(stakes, 2),
                                PL = System.Math.Round(profit_loss, 2),
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

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "exchange");
        }

        public ActionResult cashBankingMA()
        {
            CheckSession_SA();
            var DL_UserStatement = new List<DL_UserStatement>();
            try
            {
                string MDL_login_user_id = Request.QueryString["md_id"];
                int mdidint = Int32.Parse(MDL_login_user_id);
                ViewBag.MBal = GetMDLBalance(mdidint);
                connection2();
                con2.Open();
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con2;
                com.CommandText = "SELECT id,username,name,status,balance,total_balance,profit_loss FROM distributors WHERE md_id='" + MDL_login_user_id + "'";
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
                        Double login_user_total_balance = (Double)dr["total_balance"];
                        Double user_profit_loss = (Double)dr["profit_loss"];
                        user_profit_loss = System.Math.Round(user_profit_loss, 2);
                        Double DLCLIENTBAL = getDLClientBal(login_user_id);
                        Double user_exposure = getDLClientLib(login_user_id);
                        DL_UserStatement.Add(item: new DL_UserStatement
                        {
                            Client_Id = login_user_id,
                            Client_Username = login_username + "( " + login_user_full_name + " )",
                            Client_balance = login_user_balance+ DLCLIENTBAL + user_exposure,
                            Client_avl_balance = login_user_balance,
                            Client_profit_loss = user_profit_loss,
                            Client_status = user_status,
                            Client_exposure = user_exposure,
                            md_Id = mdidint
                        });
                    }
                }
                else { }
                con2.Close();
            }
            catch (Exception ex)
            {

            }
            return View(DL_UserStatement);
        }

        public ActionResult cashBankingPL()
        {
            var DL_UserStatement = new List<DL_UserStatement>();
            try
            {
                string DL_login_user_idn = Request.QueryString["dist_id"];
                int gbhfj = Int32.Parse(DL_login_user_idn);
                CheckSession_SA();
                ViewBag.Dlball = GetDLBalance(gbhfj);
                connection2();
                con2.Open();
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con2;
                com.CommandText = "SELECT id,username,name,status,balance,total_balance,profit_loss,exposure FROM users_client WHERE dl_id='" + DL_login_user_idn + "'";
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
                        Double login_user_total_balance = (Double)dr["total_balance"];
                        Double user_profit_loss = (Double)dr["profit_loss"];
                        user_profit_loss = System.Math.Round(user_profit_loss, 2);
                        Double user_exposure = (Double)dr["exposure"];
                        user_exposure = System.Math.Round(user_exposure, 2);
                        DL_UserStatement.Add(item: new DL_UserStatement
                        {
                            Client_Id = login_user_id,
                            Client_Username = login_username + "( " + login_user_full_name + " )",
                            Client_balance = login_user_balance + user_exposure,
                            Client_avl_balance = login_user_balance,
                            Client_profit_loss = user_profit_loss,
                            Client_status = user_status,
                            Client_exposure = user_exposure,
                            dl_Id = gbhfj
                        });
                    }
                }
                else { }
                con2.Close();
            }
            catch (Exception ex)
            {

            }

            return View(DL_UserStatement);
        }

        public ActionResult ClientLiability(string sportid, string leagueid, string eventcode)
        {
            var DL_UserBetList = new List<DL_UserBetList>();
            try
            {
                string md_id = sportid;
                string dist_id = leagueid;
                string user_id = eventcode;
                string lib_query = "SELECT place_time,event_id,betfair_id,field,rate,stakes,total_value FROM live_bet WHERE status='tftt' AND user_id='" + user_id + "'";
                if (user_id == "0" && dist_id == "0" && md_id != "0")
                {
                    lib_query = "SELECT place_time,event_id,betfair_id,field,rate,stakes,total_value FROM live_bet WHERE status='' AND md_id='" + md_id + "'";
                }
                else if (user_id == "0" && dist_id != "0" && md_id == "0")
                {
                    lib_query = "SELECT place_time,event_id,betfair_id,field,rate,stakes,total_value FROM live_bet WHERE status='' AND dist_id='" + dist_id + "'";
                }
                else if (user_id != "0" && dist_id == "0" && md_id == "0")
                {
                    lib_query = "SELECT place_time,event_id,betfair_id,field,rate,stakes,total_value FROM live_bet WHERE status='' AND user_id='" + user_id + "'";
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

        public ActionResult updatex()
        {
            var AllSportsAddM = new List<NavbarListAllSports>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT match_title,event_code,x_code,x_type FROM matches where status='OPEN' AND sport_id='4' AND betfair_id!='0' order by match_time asc"))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string match_title = (string)reader["match_title"];
                            string match_id = (string)reader["event_code"];
                            string x_code = (string)reader["x_code"];
                            string x_type = (string)reader["x_type"];
                            AllSportsAddM.Add(item: new NavbarListAllSports
                            {
                                SportsName = match_title,
                                EventId = match_id,
                                LaegueId = x_code,
                                SportsId = x_type
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

        public ActionResult TPbookviewmdl()
        {
            string event_id = Request.QueryString["event_code"];
            string betfair_id = Request.QueryString["betfair_id"];
            string md_id = Request.QueryString["uid"];
            string EventNameGet = GetEventTitleName(event_id);
            ViewBag.EventNameB = EventNameGet;
            ViewBag.BetfairId = betfair_id;
            ViewBag.event_id = event_id;
            ViewBag.cid = md_id;
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
            ViewBag.cid = dist_id;
            return View();
        }
       /* public ActionResult teenadminrisk()
        {
            UserBalance _teenadminRepository = new UserBalance();
            return PartialView("teenadminrisk", _teenadminRepository.GetAllMessages());
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
                string user_ids = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT distinct(md_id) from live_bet where betfair_id='" + bfid + "' AND admin_id='" + user_ids + "' AND status='' AND odds_type='TP'", con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            int md_idN = (int)reader["md_id"];
                            string UCname = GetMDLUserName(md_idN);
                            Double TotalamountA = 0;
                            Double TotalamountB = 0;
                            Double TotalamountC = 0;
                            SqlCommand sqlLost = new SqlCommand("SELECT stakes,total_value,runner_posi,event_id FROM live_bet WHERE betfair_id='" + bfid + "' AND md_id='" + md_idN + "' ", con);
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
                                user_id = md_idN,
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

        public JsonResult TPBookviewSVmdl(string bfid, string uid)
        {
            List<BookViewAg> messages = new List<BookViewAg>();
            messages = TPBookviewSmdl(bfid, uid);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }
        public List<BookViewAg> TPBookviewSmdl(string bfid, string uid)
        {
            var AllSportsAddM = new List<BookViewAg>();
            try
            {
                string user_ids = uid;
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
            string MDL_login_user_id = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];
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
                    crkt.CommandText = "select distinct(md_id) from pl_statement where admin_id='" + MDL_login_user_id + "' AND created BETWEEN '" + startDate + "' AND '" + endDate + "'";
                    dr = crkt.ExecuteReader();
                    if (dr.HasRows)
                    {
                        string nm = "";
                        Double cr = 0;
                        Double soc = 0;
                        Double ten = 0;
                        while (dr.Read())
                        {
                            int i = (int)dr["md_id"];
                            nm = GetMDLName(i);
                            cr = getMDLCrktPL(MDL_login_user_id, i, 4, startDate,endDate);
                            soc = getMDLSoccPL(MDL_login_user_id, i, 1, startDate, endDate);
                            ten = getMDLTennPL(MDL_login_user_id, i, 2, startDate, endDate);
                            spStat.Add(item: new sportsStat
                            {
                                naam = nm,
                                crkt = cr,
                                socc = soc,
                                tenn = ten,
                                tot = cr+soc+ten
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
        public double getMDLCrktPL(string aid, int mid, int spt ,string startDate,string endDate)
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
                crkt.CommandText = "sp_mdlSprtPL";
                crkt.Parameters.Add("@adId", System.Data.SqlDbType.VarChar).Value = aid;
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
        public double getMDLSoccPL(string aid, int mid, int spt, string startDate, string endDate)
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
                crkt.CommandText = "sp_mdlSprtPL";
                crkt.Parameters.Add("@adId", System.Data.SqlDbType.VarChar).Value = aid;
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
        public double getMDLTennPL(string aid, int mid, int spt, string startDate, string endDate)
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
                crkt.CommandText = "sp_mdlSprtPL";
                crkt.Parameters.Add("@adId", System.Data.SqlDbType.VarChar).Value = aid;
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
                    using (var cmd = new SqlCommand("SELECT username FROM masterdistributors WHERE id='" + id + "' ", con))
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

        public ActionResult MdlOverallAcctStat()
        {
            CheckSession_SA();
            string startDate = Request.QueryString["startDate"];
            string endDate = Request.QueryString["endDate"];
            var Dl_UserStatement = new List<DL_UserStatement>();
            if (startDate != null && startDate != "" && endDate != null && endDate != "")
            {
                try
                {
                    if (SA_login_user_id != "0" && SA_login_user_id != "" && SA_login_user_id != null)
                    {
                        using (SqlConnection con = ConnectionHandler.Connection())
                        {
                            SqlCommand com = new SqlCommand("SELECT id,username FROM masterdistributors where admin_id='" + SA_login_user_id + "' ", con);
                            con.Open();
                            SqlDataReader dr = com.ExecuteReader();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    int mdID = (int)dr["id"];
                                    string username = (string)dr["username"];
                                    SqlCommand cmd = new SqlCommand("mdlTotDepositeWithdrawl", con);
                                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@mdID", System.Data.SqlDbType.Int).Value = mdID;
                                    cmd.Parameters.Add("@startDate", System.Data.SqlDbType.DateTime).Value = startDate;
                                    cmd.Parameters.Add("@enddate", System.Data.SqlDbType.DateTime).Value = endDate;
                                    SqlDataReader dr1 = cmd.ExecuteReader();
                                    if (dr1.HasRows)
                                    {
                                        while (dr1.Read())
                                        {
                                            Double totCredit = System.Math.Round((Double)dr1["totMDLCredit"], 2);
                                            Double totDebit = System.Math.Round((Double)dr1["totMDLDebit"], 2);
                                            SqlCommand cmd1 = new SqlCommand("mdlTotProLoss", con);
                                            cmd1.CommandType = System.Data.CommandType.StoredProcedure;
                                            cmd1.Parameters.Add("@mdID", System.Data.SqlDbType.Int).Value = mdID;
                                            cmd1.Parameters.Add("@startDate", System.Data.SqlDbType.DateTime).Value = startDate;
                                            cmd1.Parameters.Add("@enddate", System.Data.SqlDbType.DateTime).Value = endDate;
                                            SqlDataReader dr2 = cmd1.ExecuteReader();
                                            if (dr2.HasRows)
                                            {
                                                while (dr2.Read())
                                                {
                                                    Double totProf = System.Math.Round((Double)dr2["totMDLProfit"], 2);
                                                    Double totLoss = System.Math.Round((Double)dr2["totMDLLoss"], 2);
                                                    Dl_UserStatement.Add(item: new DL_UserStatement
                                                    {
                                                        Client_Id = mdID,
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
                catch (Exception)
                {
                }
            }

            return View("MdlOverallAcctStat", Dl_UserStatement);
        }
        public ActionResult DlOverallAcctStat()
        {
            CheckSession_SA();
            string mdID = Request.QueryString["mdID"];
            ViewBag.md_iD = mdID;
            string startDate = Request.QueryString["startDate"];
            string endDate = Request.QueryString["endDate"];
            ViewBag.MA_Name = GetMDLUserName(int.Parse(mdID));
            var Dl_UserStatement = new List<DL_UserStatement>();
            try
            {
                if (startDate != null && startDate != "" && endDate != null && endDate != "")
                {
                    if (mdID != "" && mdID != null)
                    {
                        using (SqlConnection con = ConnectionHandler.Connection())
                        {
                            SqlCommand com = new SqlCommand("SELECT id,username FROM distributors where md_id='" + mdID + "' ", con);
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
            return View("DlOverallAcctStat", Dl_UserStatement);
        }
        public ActionResult DlWiseUsrAcct()
        {
            CheckSession_SA();
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