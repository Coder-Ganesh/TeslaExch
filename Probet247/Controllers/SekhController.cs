using Probet247.Models;
using RBetfair.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Probet247.Controllers
{
    public class SekhController : Controller
    {
        String SAdmin_login_user_id = "";
        private SqlConnection con2;
        private void connection2()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            con2 = new SqlConnection(constr);
        }
        public Double GetAdminTBalance(int user_id)
        {
            Double username = 0;
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT COALESCE(sum(balance),0) AS t_bal FROM [admin] where id!=4 AND sup_id='" + user_id + "'";
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
        public Double GetMDLTBalance(int user_id)
        {
            Double username = 0;
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT COALESCE(sum(balance),0) AS t_bal FROM [masterdistributors] where admin_id!=4 AND sup_id='" + user_id + "'";
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
        public Double getTotalDLCoins(int mdlid)
        {
            Double sport_id = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT COALESCE(sum(balance),0) AS t_bal FROM distributors WHERE admin_id!=4 AND sup_id='" + mdlid + "'", con))
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
                using (var cmd = new SqlCommand("SELECT COALESCE(sum(balance),0) AS t_bal FROM users_client WHERE admin_id!=4 AND sup_id='" + mdlid + "'", con))
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
                using (var cmd = new SqlCommand("SELECT COALESCE(sum(exposure),0) AS t_bal FROM users_client WHERE admin_id!=4 AND sup_id='" + mdlid + "'", con))
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

        public Double GetSAdminself(int user_id)
        {
            Double username = 0;
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT balance from [super_admin] where id='" + user_id + "'";
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
        public ActionResult Index()
        {
            CheckSession_SAdmin();

            var Dl_UserStatement = new List<DL_UserStatement>();
            try
            {

                if (SAdmin_login_user_id != "0" && SAdmin_login_user_id != "" && SAdmin_login_user_id != null)
                {
                    int SS_id = Int32.Parse(SAdmin_login_user_id);
                    Double ABalance = GetAdminTBalance(SS_id);
                    Double MBalance = GetMDLTBalance(SS_id);
                    Double TotalDLCoins = getTotalDLCoins(SS_id);
                    Double TotalClientCoins = getTotalClientCoins(SS_id);
                    Double TotalClientLib = getTotalClientLib(SS_id);
                    ViewBag.getTotalClientLib = TotalClientLib;
                    Double Self_bal = GetSAdminself(SS_id);
                    ViewBag.DownLineTotal = MBalance + TotalDLCoins + TotalClientCoins + TotalClientLib;
                    ViewBag.DownLineAvlTotal = MBalance + TotalDLCoins + TotalClientCoins;
                    ViewBag.AvlTotal = MBalance + TotalDLCoins + TotalClientCoins + Self_bal;
                    connection2();
                    con2.Open();
                    SqlCommand com = new SqlCommand();
                    SqlDataReader dr;
                    com.Connection = con2;
                    com.CommandText = "SELECT id,username,password,name,status,balance,credit_ref FROM admin where id!=4 AND sup_id='" + SAdmin_login_user_id + "'";
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
                            Double SM_balance = getAdminMDLBal(login_user_id);
                            Double MA_balance = getAdminDLBal(login_user_id);
                            Double PL_balance = getAdminClientBal(login_user_id);
                            Double Client_lib = getAdminClientLib(login_user_id);
                            Double TOTALBAL = login_user_balance + SM_balance + MA_balance + PL_balance + Client_lib;
                            Double TOTALAVLBAL = login_user_balance + MA_balance + PL_balance;
                            Double MDLPROF_LOSS = TOTALBAL - credit_ref;
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

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Sekh");
        }
        public string ALoginDB(string username, string password)
        {
            string gjhgf = "";
            try
            {
                if (username != null)
                {
                    connection2();
                    con2.Open();
                    SqlCommand com = new SqlCommand();
                    SqlDataReader dr;
                    com.Connection = con2;
                    com.CommandText = "SELECT id ,hash_key FROM super_admin WHERE username='" + username + "' and password='" + password + "' AND status='activate'";
                    dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        string hash_key = "";
                        int login_user_id = 0;
                        while (dr.Read())
                        {
                            hash_key = (string)dr["hash_key"];
                            login_user_id = (int)dr["id"];
                        }
                        Session["SuperAdmin_UserName"] = username.ToString();
                        Session["SuperAdmin_hash_key"] = hash_key.ToString();
                        Session["SuperAdmin_login_user_id"] = login_user_id.ToString();
                        Session["is_panel_log"] = username.ToString();
                        gjhgf = "Success";
                    }
                    else
                    {
                        gjhgf = "Failed";
                    }
                    con2.Close();
                }
            }
            catch (Exception ex)
            {
                gjhgf = "Failed";
            }
            return gjhgf;
        }

        public string AddSuperAdmin(string email, string username, string password, Double mpl)
        {

            string SendStatus = "";
            try
            {
                DateTime time = DateTime.Now;
                string format1 = "yyyy-MM-dd HH:mm:ss";
                string SA_login_user_id = (string)System.Web.HttpContext.Current.Session["SuperAdmin_login_user_id"];
                int CheckDataInsert = 0;
                string dlname = username;
                string phone = "0123456789";
                connection2();
                con2.Open();
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con2;
                com.CommandText = "SELECT username FROM user_list WHERE username='" + username + "'";
                dr = com.ExecuteReader();
                if (dr.HasRows)
                {
                    SendStatus = "This Player is already registered.";
                }
                else
                {
                    DateTime GetCurrentDT = System.DateTime.Now;
                    connection2();
                    //long Time_Stamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                    string query = "INSERT INTO [dbo].[admin]([sup_id] ,[username],[name] ,[email],[mobile] ,[password],[super_password] ,[image],[status] ,[hash_key] ,[permissions],[last_login] ,[role_id] ,[is_live_bet] ,[register_time] ,[balance] ,[total_balance] ,[profit_loss]  ,[commission] ,[modify_time],[is_inr],[agm_limit]) " +
                        "VALUES ('" + SA_login_user_id + "' ,'" + username + "' ,'" + dlname + "' ,'" + email + "' ,'" + phone + "' ,'" + password + "','" + password + "' ,'default.png' ,'activate' ,'" + time.ToString(format1) + "' ,'0','" + time.ToString(format1) + "' ,'0','0' ,'" + time.ToString(format1) + "' ,'0','0' ,'0','0' ,'" + time.ToString(format1) + "','yes','0')";

                    SqlCommand com1 = new SqlCommand(query, con2);

                    con2.Open();
                    CheckDataInsert = com1.ExecuteNonQuery();
                    if (CheckDataInsert > 0)
                    {
                        string querylist = "INSERT INTO  user_list (username,type) VALUES ('" + username + "','mdl')";
                        SqlCommand com2 = new SqlCommand(querylist, con2);
                        com2.ExecuteNonQuery();
                        SendStatus = "Success";
                    }
                    con2.Close();
                }
                con2.Close();
            }
            catch (Exception ex)
            {
                SendStatus = ex.Message;
            }

            return SendStatus;
        }

        public void CheckSession_SAdmin()
        {
            string SessionSAdminUName = (string)System.Web.HttpContext.Current.Session["SuperAdmin_UserName"];
            string SessionSAdmin_hash_key = (string)System.Web.HttpContext.Current.Session["SuperAdmin_hash_key"];

            SAdmin_login_user_id = (string)System.Web.HttpContext.Current.Session["SuperAdmin_login_user_id"];

            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT id,username,name,balance,total_balance FROM super_admin WHERE id='" + SAdmin_login_user_id + "' AND username='" + SessionSAdminUName + "' AND hash_key='" + SessionSAdmin_hash_key + "' AND status='activate'";
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
        public ActionResult SuperAddB()
        {
            CheckSession_SAdmin();
            var DL_UserStatement = new List<DL_UserStatement>();
            try
            {
                string SA_login_user_id = (string)System.Web.HttpContext.Current.Session["SuperAdmin_login_user_id"];
                connection2();
                con2.Open();
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con2;
                com.CommandText = "SELECT id,username,name,status,balance,credit_ref FROM admin where sup_id='"+ SA_login_user_id + "' AND id !=4 ";
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
                        Double SM_balance = getAdminMDLBal(login_user_id);
                        Double MA_balance = getAdminDLBal(login_user_id);
                        Double PL_balance = getAdminClientBal(login_user_id);
                        Double Client_lib = getAdminClientLib(login_user_id);
                        Double TOTALBAL = login_user_balance + SM_balance + MA_balance + PL_balance + Client_lib;
                        Double Ref_pl = TOTALBAL - credit_ref;
                        Double user_exposure = 0;
                        DL_UserStatement.Add(item: new DL_UserStatement
                        {
                            Client_Id = login_user_id,
                            Client_Username = login_username + "( " + login_user_full_name + " )",
                            Client_balance = login_user_balance + user_exposure,
                            Client_avl_balance = login_user_balance,
                            Client_profit_loss = Ref_pl,
                            Client_status = user_status,
                            Client_exposure = user_exposure
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
        public string DepositCoinTOAdmin(string user_id, float coins, string remark1)
        {
            string ReturnMSG = "";
            try
            {
                if (user_id != "" && coins != 0 && user_id != "0")
                {
                    DateTime time = DateTime.Now;
                    string format = "yyyy-MM-dd HH:mm:ss";
                    string SuperAdmin_sess_ID = (string)System.Web.HttpContext.Current.Session["SuperAdmin_login_user_id"];
                    string SuperAdmin_Sess_Name = (string)System.Web.HttpContext.Current.Session["SuperAdmin_UserName"];

                    Double GetDlBal = GetSAdminBalance(SuperAdmin_sess_ID);
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        string stmt = "SELECT sup_id,balance,total_balance,username FROM admin WHERE id='" + user_id + "' AND sup_id='" + SuperAdmin_sess_ID + "' ";

                        using (SqlCommand cmdCount = new SqlCommand(stmt, con))
                        {
                            cmdCount.ExecuteScalar();
                            var reader = cmdCount.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    int super_id = (int)reader["sup_id"];

                                    Double user_current_balance = (Double)reader["balance"];
                                    Double total_balance = (Double)reader["total_balance"];
                                    Double total_net_balance = total_balance + coins;
                                    Double user_liability = 0;
                                    string user_detail = (string)reader["username"] + "[Senior Super]";
                                    string dist_detail = SuperAdmin_Sess_Name + "[Admin]";

                                    Double user_net_balance = user_current_balance + coins;
                                    Double user_account_balance = user_net_balance + user_liability;

                                    Double dist_net_balance = 0;
                                    if (GetDlBal < coins)
                                    {
                                        ReturnMSG = "InsufficientAmount";

                                    }
                                    else
                                    {
                                        dist_net_balance = GetDlBal - coins;

                                        SqlCommand cmd2 = new SqlCommand("INSERT INTO super_admin_account_statements(sup_id,event_id,market_id,admin_id,md_id,dist_id,user_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver) VALUES " +
                                          "('" + super_id + "','','','" + user_id + "','0','0','0','dw_coins','Deposit " + coins + " Coins to " + user_detail + "','" + remark1 + "','" + coins + "','','" + dist_net_balance + "','" + time.ToString(format) + "' ,'','') ", con);


                                        int CheckDataInsert = cmd2.ExecuteNonQuery();
                                        if (CheckDataInsert > 0)
                                        {

                                            SqlCommand user_account_statement = new SqlCommand("INSERT INTO admin_account_statements(sup_id,event_id,market_id,admin_id,md_id,dist_id,user_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver) VALUES " +
                                            "('" + super_id + "','','','" + user_id + "','0','0','0','dw_coins','Deposit " + coins + " Coins by " + dist_detail + "','" + remark1 + "','','" + coins + "','" + user_account_balance + "','" + time.ToString(format) + "' ,'','') ", con);


                                            user_account_statement.ExecuteNonQuery();

                                            SqlCommand user_update = new SqlCommand("UPDATE admin SET balance='" + user_net_balance + "' , total_balance='" + total_net_balance + "' WHERE id='" + user_id + "'", con);
                                            user_update.ExecuteNonQuery();

                                            SqlCommand dist_update = new SqlCommand("UPDATE super_admin SET balance='" + dist_net_balance + "' WHERE id='" + super_id + "'", con);
                                            dist_update.ExecuteNonQuery();

                                            string activitLogDescription = "Deposit " + coins + " Coins to " + user_detail;

                                            SqlCommand sqlActivityLog = new SqlCommand("INSERT INTO activity_logs(description, receiver_account_type, receiver_id, created) VALUES ('" + activitLogDescription + "','super_admin','" + SuperAdmin_sess_ID + "','" + time.ToString(format) + "')", con);
                                            sqlActivityLog.ExecuteNonQuery();

                                            ReturnMSG = "true";
                                        }
                                        else
                                        {
                                            ReturnMSG = "false";
                                        }

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
                ReturnMSG = "false";
            }
            return ReturnMSG;
        }

        public string WithdrawalCoinsToAdmin(string user_id, float coins, string remark1)
        {
            string ReturnMSG = "";
            try
            {
                if (user_id != "" && coins != 0 && user_id != "0")
                {
                    DateTime time = DateTime.Now;
                    string format = "yyyy-MM-dd HH:mm:ss";
                    string SuperAdmin_sess_ID = (string)System.Web.HttpContext.Current.Session["SuperAdmin_login_user_id"];
                    string Admin_Sess_Name = (string)System.Web.HttpContext.Current.Session["SuperAdmin_UserName"];

                    Double GetDlBal = GetSAdminBalance(SuperAdmin_sess_ID);
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        string stmt = "SELECT sup_id,balance,total_balance,username FROM admin WHERE id='" + user_id + "' AND sup_id='" + SuperAdmin_sess_ID + "' ";

                        using (SqlCommand cmdCount = new SqlCommand(stmt, con))
                        {
                            cmdCount.ExecuteScalar();
                            var reader = cmdCount.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    int sup_id = (int)reader["sup_id"];

                                    Double user_current_balance = (Double)reader["balance"];
                                    Double total_balance = (Double)reader["total_balance"];
                                    Double total_net_balance = total_balance - coins;
                                    Double user_liability = 0;
                                    string user_detail = (string)reader["username"] + "[Senior Super]";
                                    string dist_detail = Admin_Sess_Name + "[Admin]";

                                    Double user_net_balance = user_current_balance - coins;

                                    Double user_account_balance = user_net_balance + user_liability;

                                    Double dist_net_balance = 0;
                                    if (user_current_balance < coins)
                                    {
                                        ReturnMSG = "InsufficientAmount";

                                    }
                                    else
                                    {
                                        dist_net_balance = GetDlBal + coins;

                                        SqlCommand cmd2 = new SqlCommand("INSERT INTO super_admin_account_statements(sup_id,event_id,market_id,admin_id,md_id,dist_id,user_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver) VALUES " +
                                           "('" + sup_id + "','','','" + user_id + "','0','0','0','dw_coins','Withdraw " + coins + " Coins From " + user_detail + "','" + remark1 + "','','" + coins + "','" + dist_net_balance + "','" + time.ToString(format) + "' ,'','') ", con);
                                        int CheckDataInsert = cmd2.ExecuteNonQuery();

                                        if (CheckDataInsert > 0)
                                        {

                                            SqlCommand user_account_statement = new SqlCommand("INSERT INTO admin_account_statements(sup_id,event_id,market_id,admin_id,md_id,dist_id,user_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver) VALUES " +
                                            "('" + sup_id + "','','','" + user_id + "','0','0','0','dw_coins','Withdraw " + coins + " Coins by " + dist_detail + "','" + remark1 + "','','" + coins + "','" + user_account_balance + "','" + time.ToString(format) + "' ,'','') ", con);

                                            user_account_statement.ExecuteNonQuery();

                                            SqlCommand user_update = new SqlCommand("UPDATE admin SET balance='" + user_net_balance + "' , total_balance='" + total_net_balance + "' WHERE id='" + user_id + "'", con);
                                            user_update.ExecuteNonQuery();

                                            SqlCommand dist_update = new SqlCommand("UPDATE super_admin SET balance='" + dist_net_balance + "' WHERE id='" + SuperAdmin_sess_ID + "'", con);
                                            dist_update.ExecuteNonQuery();

                                            string activitLogDescription = "Withdraw " + coins + " Coins to " + user_detail;

                                            SqlCommand sqlActivityLog = new SqlCommand("INSERT INTO activity_logs(description, receiver_account_type, receiver_id, created) VALUES ('" + activitLogDescription + "','Superadmin','" + SuperAdmin_sess_ID + "','" + time.ToString(format) + "')", con);
                                            sqlActivityLog.ExecuteNonQuery();

                                            ReturnMSG = "true";
                                        }
                                        else
                                        {
                                            ReturnMSG = "false4545";
                                        }

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
                string fjkcvfj = ex.ToString();

            }
            return ReturnMSG;
        }

        public string ServerDepositCoins(string user_id, float coins, string remark1)
        {
            string ReturnMSG = "";
            try
            {
                if (user_id != "" && coins != 0 && user_id != "0")
                {
                    DateTime time = DateTime.Now;
                    string format = "yyyy-MM-dd HH:mm:ss";
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        string stmt = "SELECT balance,total_balance,username FROM admin WHERE id='" + user_id + "' ";

                        using (SqlCommand cmdCount = new SqlCommand(stmt, con))
                        {
                            cmdCount.ExecuteScalar();
                            var reader = cmdCount.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Double user_current_balance = (Double)reader["balance"];
                                    Double total_balance = (Double)reader["total_balance"];
                                    Double total_net_balance = total_balance + coins;

                                    Double user_net_balance = user_current_balance + coins;

                                    SqlCommand cmd3 = new SqlCommand("INSERT INTO super_admin_account_statements(event_id,market_id,user_id,admin_id,dist_id,md_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver) VALUES " +
                                        "('','','','" + user_id + "','','','dw_coins','Deposit " + coins + " Coins by Admin" + "','" + remark1 + "','','" + coins + "','" + user_net_balance + "','" + time.ToString(format) + "' ,'','') ", con);
                                    cmd3.ExecuteNonQuery();

                                    SqlCommand cmd2 = new SqlCommand("INSERT INTO admin_account_statements(event_id,market_id,user_id,admin_id,dist_id,md_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver) VALUES " +
                                        "('','','','" + user_id + "','','','dw_coins','Deposit " + coins + " Coins by Admin" + "','" + remark1 + "','','" + coins + "','" + user_net_balance + "','" + time.ToString(format) + "' ,'','') ", con);
                                    int CheckDataInsert = cmd2.ExecuteNonQuery();
                                    if (CheckDataInsert > 0)
                                    {
                                        SqlCommand user_update = new SqlCommand("UPDATE admin SET balance='" + user_net_balance + "' , total_balance='" + total_net_balance + "' WHERE id='" + user_id + "'", con);
                                        user_update.ExecuteNonQuery();
                                        string activitLogDescription = "Deposit " + coins + " Coins by Server";

                                        SqlCommand sqlActivityLog = new SqlCommand("INSERT INTO activity_logs(description, receiver_account_type, receiver_id, created) VALUES ('" + activitLogDescription + "','admin','','" + time.ToString(format) + "')", con);
                                        sqlActivityLog.ExecuteNonQuery();

                                        ReturnMSG = "true";
                                    }
                                    else
                                    {
                                        ReturnMSG = "false";
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
                ReturnMSG = "false";
            }
            return ReturnMSG;
        }

        public string ServerWithdrawalCoins(string user_id, float coins, string remark1)
        {
            string ReturnMSG = "";
            try
            {
                if (user_id != "" && coins != 0 && user_id != "0")
                {
                    DateTime time = DateTime.Now;
                    string format = "yyyy-MM-dd HH:mm:ss";

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        string stmt = "SELECT balance,total_balance,username FROM admin WHERE id='" + user_id + "' ";

                        using (SqlCommand cmdCount = new SqlCommand(stmt, con))
                        {
                            cmdCount.ExecuteScalar();
                            var reader = cmdCount.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {

                                    Double user_current_balance = (Double)reader["balance"];
                                    Double total_balance = (Double)reader["total_balance"];
                                    Double total_net_balance = total_balance - coins;
                                    Double user_net_balance = user_current_balance - coins;

                                    SqlCommand cmd2 = new SqlCommand("INSERT INTO admin_account_statements(event_id,market_id,user_id,admin_id,dist_id,md_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver) VALUES " +
                                            "('','','','" + user_id + "','','','dw_coins','Withdraw " + coins + " Coins by Admin " + "','" + remark1 + "','" + coins + "','','" + user_net_balance + "','" + time.ToString(format) + "' ,'','') ", con);
                                    int CheckDataInsert = cmd2.ExecuteNonQuery();


                                    if (CheckDataInsert > 0)
                                    {
                                        SqlCommand user_update = new SqlCommand("UPDATE admin SET balance='" + user_net_balance + "' , total_balance='" + total_net_balance + "' WHERE id='" + user_id + "'", con);
                                        user_update.ExecuteNonQuery();

                                        string activitLogDescription = "Withdraw " + coins + " Coins by Server ";

                                        SqlCommand sqlActivityLog = new SqlCommand("INSERT INTO activity_logs(description, receiver_account_type, receiver_id, created) VALUES ('" + activitLogDescription + "','admin','" + user_id + "','" + time.ToString(format) + "')", con);
                                        sqlActivityLog.ExecuteNonQuery();

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

        public Double GetMDLBalance(string MDLId)
        {
            Double balance = 0;
            using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con1.Open();
                string stmt1 = "SELECT [balance] FROM super_admin WHERE id='" + MDLId + "' ";

                using (SqlCommand cmdCount1 = new SqlCommand(stmt1, con1))
                {
                    cmdCount1.ExecuteScalar();
                    var reader1 = cmdCount1.ExecuteReader();
                    while (reader1.Read())
                    {
                        balance = (Double)reader1["balance"];
                    }
                }
                con1.Close();
            }
            return balance;
        }

        public Double GetSAdminBalance(string MDLId)
        {
            Double balance = 0;
            using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con1.Open();
                string stmt1 = "SELECT [balance] FROM super_admin WHERE id='" + MDLId + "' ";

                using (SqlCommand cmdCount1 = new SqlCommand(stmt1, con1))
                {
                    cmdCount1.ExecuteScalar();
                    var reader1 = cmdCount1.ExecuteReader();
                    while (reader1.Read())
                    {
                        balance = (Double)reader1["balance"];
                    }
                }
                con1.Close();
            }
            return balance;
        }

        public ActionResult RiskManagement()
        {
            CheckSession_SAdmin();
            return View();
        }

        public ActionResult Superadminanalysis()
        {
            SuperadminanalysisRepositary _messageRepository = new SuperadminanalysisRepositary();
            return PartialView("superadminanalysis", _messageRepository.GetAllMessages());
        }

        public ActionResult adminanuhytalysis()
        {
            SuperAdminSessionRepository _sessionRepository = new SuperAdminSessionRepository();
            return PartialView("superadminsanalysisdata", _sessionRepository.GetAllMessagesS());
        }

        public ActionResult adminallbet()
        {
            var DL_UserBetList = new List<MatchedClientBetList>();
            try
            {
                string uid = (string)System.Web.HttpContext.Current.Session["SuperAdmin_login_user_id"];
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
                    using (var cmd = new SqlCommand("SELECT Distinct(admin_id) FROM live_bet WHERE sup_id='" + uid + "' AND event_id='" + event_code + "' AND betfair_id='" + betfair_id + "' "))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            int admin_id = (Int32)dr["admin_id"];
                            string UsernameC = GetAdminUserName(admin_id);
                            DL_UserBetList.Add(item: new MatchedClientBetList
                            {
                                EventTime = "-",
                                uid = admin_id,
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

        public ActionResult mdlallbet()
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

        public ActionResult bookviewAdmin()
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
                string user_ids = (string)System.Web.HttpContext.Current.Session["SuperAdmin_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT distinct(admin_id) from live_bet where betfair_id='" + bfid + "' AND sup_id='" + user_ids + "' AND status='' AND odds_type='MO'", con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Double teamAWD = 0;
                            Double teamBWD = 0;
                            Double teamCWD = 0;
                            int admin_id = (int)reader["admin_id"];
                            string UCname = GetAdminUserName(admin_id);
                            Double dl_rate = 100;
                            Double ag_rate = 0;
                            Double mdl_rate = GetMDRate(admin_id);
                            Double agm_rate = dl_rate - mdl_rate;
                            Double admin_rate = mdl_rate;
                            using (var cmd2 = new SqlCommand("select runner_pos,amount from runner_cal where admin_id='" + admin_id + "' AND market_id='" + bfid + "'", con))
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
                                user_id = admin_id,
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

        public JsonResult BookviewSAdmin1(string bfid, string uid)
        {
            List<BookViewAg> messages = new List<BookViewAg>();
            messages = BookviewSAdmin(bfid, uid);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }
        public List<BookViewAg> BookviewSAdmin(string bfid, string uid)
        {
            var AllSportsAddM = new List<BookViewAg>();
            try
            {
                string user_ids = uid;
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
                            int md_id = (int)reader["md_id"];
                            string UCname = GetMDLUserName(md_id);
                            Double dl_rate = GetMDLRate(md_id);
                            Double ag_rate = 0;
                            Double mdl_rate = GetMDLRate(md_id);
                            Double agm_rate = dl_rate - mdl_rate;
                            Double admin_rate = mdl_rate;
                            using (var cmd2 = new SqlCommand("select runner_pos,amount from runner_cal where md_id='" + md_id + "' AND market_id='" + bfid + "'", con))
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
                                user_id = md_id,
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

        public string GetAdminUserName(int user_id)
        {
            string username = "";
            connection2();
            con2.Open();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            com.Connection = con2;
            com.CommandText = "SELECT username from [admin] where id='" + user_id + "'";
            dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                username = (string)dr["username"];
            }
            con2.Close();
            return username;
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

        public Double getAdminMDLBal(int adid)
        {
            Double total_bal = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT COALESCE(sum(balance),0) AS t_bal FROM masterdistributors WHERE admin_id='" + adid + "'", con))
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    total_bal = (Double)reader["t_bal"];
                    total_bal = System.Math.Round(total_bal, 2);
                }
                con.Close();
            }
            return total_bal;
        }
        public Double getAdminDLBal(int adid)
        {
            Double total_bal = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT COALESCE(sum(balance),0) AS t_bal FROM distributors WHERE admin_id='" + adid + "'", con))
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    total_bal = (Double)reader["t_bal"];
                    total_bal = System.Math.Round(total_bal, 2);
                }
                con.Close();
            }
            return total_bal;
        }

        public Double getAdminClientBal(int adid)
        {
            Double total_bal = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT COALESCE(sum(balance),0) AS t_bal FROM users_client WHERE admin_id='" + adid + "'", con))
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    total_bal = (Double)reader["t_bal"];
                    total_bal = System.Math.Round(total_bal, 2);
                }
                con.Close();
            }
            return total_bal;
        }

        public Double getAdminClientLib(int adid)
        {
            Double total_bal = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT COALESCE(sum(exposure),0) AS t_bal FROM users_client WHERE admin_id='" + adid + "'", con))
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    total_bal = (Double)reader["t_bal"];
                    total_bal = System.Math.Round(total_bal, 2);
                }
                con.Close();
            }
            return total_bal;
        }

        public string CreditRef(string credit_id, string new_credit_ref, string password, string old_credit_ref)
        {
            string SendMessage = "";
            try
            {
                string SA_login_user_id = (string)System.Web.HttpContext.Current.Session["SuperAdmin_login_user_id"];
                string SAdmin_login_username = (string)System.Web.HttpContext.Current.Session["SuperAdmin_UserName"];

                DateTime time = DateTime.Now;
                string format = "yyyy-MM-dd HH:mm:ss";
                connection2();
                con2.Open();
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con2;
                com.CommandText = "SELECT password FROM super_admin WHERE id='" + SA_login_user_id + "' AND password='" + password + "' ";
                dr = com.ExecuteReader();
                if (dr.HasRows)
                {
                    connection2();
                    con2.Open();
                    SqlCommand com1 = new SqlCommand();
                    com1.Connection = con2;
                    com1.CommandText = "UPDATE admin SET credit_ref='" + new_credit_ref + "' WHERE id='" + credit_id + "' AND sup_id='" + SA_login_user_id + "'";
                    int runquery2 = com1.ExecuteNonQuery();
                    if (runquery2 > 0)
                    {
                        SendMessage = "Success";
                        string activitLogDescription = credit_id + "Old Credit Ref " + old_credit_ref + "Changed to New Credit Ref" + new_credit_ref;
                        connection2();
                        string query1 = "INSERT INTO activity_logs(description, receiver_account_type, receiver_id, created) VALUES ('" + activitLogDescription + "','credit_admin','" + SA_login_user_id + "','" + time.ToString(format) + "')";
                        SqlCommand com11 = new SqlCommand(query1, con2);
                        con2.Open();
                        com11.ExecuteNonQuery();
                        con2.Close();
                    }
                    else
                    {
                        SendMessage = "Failed1"; ;
                    }
                    con2.Close();
                }
                else
                {
                    SendMessage = "IncorrectPWD";
                }
                con2.Close();
            }
            catch (Exception ex)
            {
                SendMessage = "IncorrectPWD";
            }
            return SendMessage;
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

        public ActionResult TPbookviewAdmin()
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
       /* public ActionResult teensekhrisk()
        {
            Teensekhrepos _teensekhRepository = new Teensekhrepos();
            return PartialView("teensekhrisk", _teensekhRepository.GetAllMessages());
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
                string user_ids = (string)System.Web.HttpContext.Current.Session["SuperAdmin_login_user_id"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT distinct(admin_id) from live_bet where betfair_id='" + bfid + "' AND sup_id='" + user_ids + "' AND status='' AND odds_type='TP'", con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            int admin_idN = (int)reader["admin_id"];
                            string UCname = GetAdminUserName(admin_idN);
                            Double TotalamountA = 0;
                            Double TotalamountB = 0;
                            Double TotalamountC = 0;
                            SqlCommand sqlLost = new SqlCommand("SELECT stakes,total_value,runner_posi,event_id FROM live_bet WHERE betfair_id='" + bfid + "' AND admin_id='" + admin_idN + "' ", con);
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
                                user_id = admin_idN,
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

        public JsonResult TPBookviewSAdmin1(string bfid, string uid)
        {
            List<BookViewAg> messages = new List<BookViewAg>();
            messages = TPBookviewSAdmin(bfid, uid);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }
        public List<BookViewAg> TPBookviewSAdmin(string bfid, string uid)
        {
            var AllSportsAddM = new List<BookViewAg>();
            try
            {
                string user_ids = uid;
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

        public ActionResult AddMatch()
        {
            CheckSession_SAdmin();
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
            CheckSession_SAdmin();
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

        public ActionResult SetLimit()
        {
            CheckSession_SAdmin();
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

        public ActionResult BetDelay()
        {
            CheckSession_SAdmin();
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

    }
}