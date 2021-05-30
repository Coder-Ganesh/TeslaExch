using RBetfair.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Probet247.Controllers
{
    public class AgentLogin_DBController : Controller
    {
        // GET: AgentLogin_DB
        DateTime time = DateTime.Now;
        string format1 = "yyyy-MM-dd HH:mm:ss";
        public ActionResult Index()
        {
            return View();
        }

        private SqlConnection con2;
        private void connection2()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            con2 = new SqlConnection(constr);
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
                    com.CommandText = "SELECT hash_key,id,md_id,admin_id FROM distributors WHERE username='" + username + "' and password='" + password + "' AND status='activate'";
                    dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        string hash_key = "";
                        int login_user_id = 0;
                        int Mlogin_user_id = 0;
                        int Alogin_user_id = 0;
                        while (dr.Read())
                        {
                            hash_key = (string)dr["hash_key"];
                            login_user_id = (int)dr["id"];
                            Mlogin_user_id = (int)dr["md_id"];
                            Alogin_user_id = (int)dr["admin_id"];
                        }
                        Session["DL_UserName"] = username.ToString();
                        Session["DL_hash_key"] = hash_key.ToString();
                        Session["DL_login_user_id"] = login_user_id.ToString();
                        Session["MDL_login_user_id"] = Mlogin_user_id.ToString();
                        Session["Admin_login_user_id"] = Alogin_user_id.ToString();
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

            }
            return gjhgf;
        }
        public string AddDLClient(string username, string name, string password, string exposure_limit, string commission_mo, string commission_sess)
        {
            string SendStatus = "Error in Adding Player";
            try
            {
                int CheckDataInsert = 0;
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
                    string DL_login_user_id = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                    string MDL_login_user_id = (string)System.Web.HttpContext.Current.Session["MDL_login_user_id"];
                    string Admin_login_user_id = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];
                    DateTime GetCurrentDT = System.DateTime.Now;
                    connection2();
                    long Time_Stamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    string query = "INSERT INTO  users_client (admin_id,mdl_id,dl_id,name,username,password,status,ragister_time,last_login,hash_key,balance,exposure,total_balance,profit_loss,ocomm_rate,scomm_rate,exposure_limit,mobile,s_nm) VALUES " +
                        "('" + Admin_login_user_id + "','" + MDL_login_user_id + "','" + DL_login_user_id + "','" + name + "','" + username + "','" + password + "','activate','" + time.ToString(format1) + "','" + time.ToString(format1) + "','" + Time_Stamp + "','0','0','0','0','"+commission_mo+"','"+commission_sess+"','"+exposure_limit+"','"+ commission_mo + "','Probet247')";
                    SqlCommand com1 = new SqlCommand(query, con2);

                    con2.Open();
                    CheckDataInsert = com1.ExecuteNonQuery();
                    if (CheckDataInsert > 0)
                    {
                        string querylist = "INSERT INTO  user_list (username,type) VALUES ('" + username + "','client')";
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

            }

            return SendStatus;
        }
        public string ClientAction(string user_id, string status, string username)
        {
            string SendMessage = "";
            try
            {
                string DL_login_user_id = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                string DL_login_username = (string)System.Web.HttpContext.Current.Session["DL_UserName"];

                DateTime time = DateTime.Now;
                string format = "yyyy-MM-dd HH:mm:ss";
                if (status == "activate")
                {
                    connection2();
                    con2.Open();
                    SqlCommand com1 = new SqlCommand();
                    com1.Connection = con2;
                    com1.CommandText = "UPDATE users_client SET status='activate' WHERE id='" + user_id + "' AND dl_id='" + DL_login_user_id + "'";
                    int runquery2 = com1.ExecuteNonQuery();
                    if (runquery2 > 0)
                    {
                        SendMessage = "Success";
                        string activitLogDescription = username + "[Client] Account Activated by " + DL_login_username + "[DL].";
                        connection2();
                        string query1 = "INSERT INTO activity_logs(description, receiver_account_type, receiver_id, created) VALUES ('" + activitLogDescription + "','dl','" + DL_login_user_id + "','" + time.ToString(format) + "')";
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
                else if (status == "deactivate")
                {
                    connection2();
                    con2.Open();
                    SqlCommand com2 = new SqlCommand();
                    com2.Connection = con2;
                    com2.CommandText = "UPDATE users_client SET status='deactivate' WHERE id='" + user_id + "' AND dl_id='" + DL_login_user_id + "'";
                    int runquery2 = com2.ExecuteNonQuery();
                    if (runquery2 > 0)
                    {
                        SendMessage = "Success";
                        string activitLogDescription = username + "[Client] Account Activated by " + DL_login_username + "[DL].";
                        connection2();
                        string query1 = "INSERT INTO activity_logs(description, receiver_account_type, receiver_id, created) VALUES ('" + activitLogDescription + "','dl','" + DL_login_user_id + "','" + time.ToString(format) + "')";
                        SqlCommand com11 = new SqlCommand(query1, con2);
                        con2.Open();
                        com11.ExecuteNonQuery();
                        con2.Close();
                    }
                    else
                    {
                        SendMessage = "Failed11"; ;
                    }
                    con2.Close();
                }
                else { }
            }
            catch (Exception ex)
            {

            }
            con2.Close();

            return SendMessage;
        }

        public string BetLoA(string user_id, string status, string username)
        {
            string SendMessage = "Failed";
            try
            {
                if (user_id != "" && status != "" && user_id != "0")
                {                   
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        SqlCommand user_update = new SqlCommand("UPDATE users_client SET is_bet='" + status + "' WHERE id='" + user_id + "'", con);
                        int cc = user_update.ExecuteNonQuery();
                        if (cc == 1)
                        {
                            SendMessage = "Success";
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return SendMessage;
        }

        public string ClientDepositCoins(string user_id, float coins, string remark1)
        {
            string ReturnMSG = "";
            try
            {
                if (user_id != "" && coins != 0 && user_id != "0")
                {
                    DateTime time = DateTime.Now;
                    string format = "yyyy-MM-dd HH:mm:ss";
                    string DL_login_user_idin = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                    string DL_login_username = (string)System.Web.HttpContext.Current.Session["DL_UserName"];

                    Double GetDlBal = GetDLBalance(DL_login_user_idin);
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        string stmt = "SELECT mdl_id,admin_id,balance,total_balance,exposure,username FROM users_client WHERE id='" + user_id + "' AND dl_id='" + DL_login_user_idin + "' ";

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

                                    Double user_current_balance = (Double)reader["balance"];
                                    Double total_balance = (Double)reader["total_balance"];
                                    Double total_net_balance = total_balance + coins;
                                    Double user_liability = (Double)reader["exposure"];
                                    string user_detail = (string)reader["username"] + "[Player]";
                                    string dist_detail = DL_login_username + "[Master Agent]";

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

                                        SqlCommand cmd2 = new SqlCommand("INSERT INTO dist_account_statements(market_id,admin_id,md_id,dist_id,user_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver) VALUES " +
                                            "('','" + admin_id + "','" + md_id + "','" + DL_login_user_idin + "','" + user_id + "','dw_coins','Deposit " + coins + " Coins to " + user_detail + "','" + remark1 + "','" + coins + "','','" + dist_net_balance + "','" + time.ToString(format) + "' ,'','') ", con);
                                        int CheckDataInsert = cmd2.ExecuteNonQuery();
                                        if (CheckDataInsert > 0)
                                        {
                                            SqlCommand user_account_statement = new SqlCommand("INSERT INTO user_account_statements(admin_id,md_id,dist_id,user_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver,market_id,cc_market_id,event_id,match_odds) VALUES " +
                                                "('" + admin_id + "','" + md_id + "','" + DL_login_user_idin + "','" + user_id + "','dw_coins','Deposit " + coins + " Coins by " + dist_detail + "','','','" + coins + "','" + user_account_balance + "','" + time.ToString(format) + "' ,'','','','','','')", con);
                                            user_account_statement.ExecuteNonQuery();

                                            SqlCommand user_update = new SqlCommand("UPDATE users_client SET balance='" + user_net_balance + "' , total_balance='" + total_net_balance + "' WHERE id='" + user_id + "'", con);
                                            user_update.ExecuteNonQuery();

                                            SqlCommand dist_update = new SqlCommand("UPDATE distributors SET balance='" + dist_net_balance + "' WHERE id='" + DL_login_user_idin + "'", con);
                                            dist_update.ExecuteNonQuery();

                                            string activitLogDescription = "Deposit " + coins + " Coins to " + user_detail;

                                            SqlCommand sqlActivityLog = new SqlCommand("INSERT INTO activity_logs(description, receiver_account_type, receiver_id, created) VALUES ('" + activitLogDescription + "','dl','" + DL_login_user_idin + "','" + time.ToString(format) + "')", con);
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

            }
            return ReturnMSG;
        }

        public string ClientWithdrawalCoins(string user_id, float coins, string remark1)
        {
            string ReturnMSG = "";
            try
            {
                if (user_id != "" && coins != 0 && user_id != "0")
                {
                    DateTime time = DateTime.Now;
                    string format = "yyyy-MM-dd HH:mm:ss";
                    string DL_login_user_idin = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                    string DL_login_username = (string)System.Web.HttpContext.Current.Session["DL_UserName"];

                    Double GetDlBal = GetDLBalance(DL_login_user_idin);
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        string stmt = "SELECT mdl_id,admin_id,balance,total_balance,exposure,username FROM users_client WHERE id='" + user_id + "' AND dl_id='" + DL_login_user_idin + "' ";

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

                                    Double user_current_balance = (Double)reader["balance"];
                                    Double total_balance = (Double)reader["total_balance"];
                                    Double total_net_balance = total_balance - coins;
                                    Double user_liability = (Double)reader["exposure"];
                                    string user_detail = (string)reader["username"] + "[Player]";
                                    string dist_detail = DL_login_username + "[Master Agent]";

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

                                        SqlCommand cmd2 = new SqlCommand("INSERT INTO dist_account_statements(market_id,admin_id,md_id,dist_id,user_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver) VALUES " +
                                            "('','" + admin_id + "','" + md_id + "','" + DL_login_user_idin + "','" + user_id + "','dw_coins','Withdraw " + coins + " Coins From " + user_detail + "','" + remark1 + "','0','" + coins + "','" + dist_net_balance + "','" + time.ToString(format) + "' ,'','') ", con);
                                        int CheckDataInsert = cmd2.ExecuteNonQuery();
                                        if (CheckDataInsert > 0)
                                        {
                                            SqlCommand user_account_statement = new SqlCommand("INSERT INTO user_account_statements(admin_id,md_id,dist_id,user_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver,market_id,cc_market_id,event_id,match_odds) VALUES " +
                                                "('" + admin_id + "','" + md_id + "','" + DL_login_user_idin + "','" + user_id + "','dw_coins','Withdraw " + coins + " Coins by " + dist_detail + "','','" + coins + "','','" + user_account_balance + "','" + time.ToString(format) + "' ,'','','','','','')", con);
                                            user_account_statement.ExecuteNonQuery();

                                            SqlCommand user_update = new SqlCommand("UPDATE users_client SET balance='" + user_net_balance + "' , total_balance='" + total_net_balance + "' WHERE id='" + user_id + "'", con);
                                            user_update.ExecuteNonQuery();

                                            SqlCommand dist_update = new SqlCommand("UPDATE distributors SET balance='" + dist_net_balance + "' WHERE id='" + DL_login_user_idin + "'", con);
                                            dist_update.ExecuteNonQuery();

                                            string activitLogDescription = "Withdraw " + coins + " Coins to " + user_detail;

                                            SqlCommand sqlActivityLog = new SqlCommand("INSERT INTO activity_logs(description, receiver_account_type, receiver_id, created) VALUES ('" + activitLogDescription + "','dl','" + DL_login_user_idin + "','" + time.ToString(format) + "')", con);
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

            }
            return ReturnMSG;
        }

        public Double GetDLBalance(string DLId)
        {
            Double balance = 0;
            using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con1.Open();
                string stmt1 = "SELECT balance FROM distributors WHERE id='" + DLId + "' ";

                using (SqlCommand cmdCount1 = new SqlCommand(stmt1, con1))
                {
                    cmdCount1.ExecuteScalar();
                    var reader1 = cmdCount1.ExecuteReader();
                    while (reader1.Read())
                    {
                        balance = (Double)reader1["balance"];
                        balance = System.Math.Round(balance, 2);
                    }
                }
                con1.Close();
            }
            return balance;
        }
        public string ChangePasswordDB(string user_id, string newPassword, string changePassword)
        {
            string ReturnMessage = "";
            try
            {
                if (user_id != "" && newPassword != "" && changePassword != "")
                {
                    string DL_login_user_idin = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                    using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con1.Open();
                        string stmt1 = "SELECT id FROM distributors WHERE id='" + DL_login_user_idin + "' AND password='" + changePassword + "' ";

                        using (SqlCommand cmdCount1 = new SqlCommand(stmt1, con1))
                        {
                            cmdCount1.ExecuteScalar();
                            var reader1 = cmdCount1.ExecuteReader();
                            if (reader1.HasRows)
                            {
                                Random rand = new Random();
                                rand.Next();
                                SqlCommand dist_update = new SqlCommand("UPDATE distributors SET password='" + newPassword + "',hash_key='" + rand + "' WHERE id='" + DL_login_user_idin + "'", con1);
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
                string user_ids = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                int user_id = 0;
                if (user_ids != "" && user_ids != null)
                {
                    user_id = Int32.Parse(user_ids);
                }

                using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con1.Open();
                    string stmt1 = "select amount from runner_cal where dist_id='" + user_id + "' AND market_id='" + jjg + "' ";

                    using (SqlCommand cmdCount1 = new SqlCommand(stmt1, con1))
                    {
                        cmdCount1.ExecuteScalar();
                        var reader1 = cmdCount1.ExecuteReader();
                        while (reader1.Read())
                        {
                            messages.Add(item: new SendWMEData
                            {
                                MainD = (Double)reader1["amount"]
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

        public string CreditRef(string credit_id, string new_credit_ref, string password, string old_credit_ref)
        {
            string SendMessage = "";
            try
            {
                string DL_login_user_id = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                string DL_login_username = (string)System.Web.HttpContext.Current.Session["DL_UserName"];

                DateTime time = DateTime.Now;
                string format = "yyyy-MM-dd HH:mm:ss";
                connection2();
                con2.Open();
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con2;
                com.CommandText = "SELECT password FROM distributors WHERE id='" + DL_login_user_id + "' AND password='" + password + "' ";
                dr = com.ExecuteReader();
                if (dr.HasRows)
                {
                    connection2();
                    con2.Open();
                    SqlCommand com1 = new SqlCommand();
                    com1.Connection = con2;
                    com1.CommandText = "UPDATE users_client SET credit_ref='" + new_credit_ref + "' WHERE id='" + credit_id + "' AND dl_id='" + DL_login_user_id + "'";
                    int runquery2 = com1.ExecuteNonQuery();
                    if (runquery2 > 0)
                    {
                        SendMessage = "Success";
                        string activitLogDescription = credit_id + "Old Credit Ref " + old_credit_ref + "Changed to New Credit Ref" + new_credit_ref;
                        connection2();
                        string query1 = "INSERT INTO activity_logs(description, receiver_account_type, receiver_id, created) VALUES ('" + activitLogDescription + "','credit_client','" + DL_login_user_id + "','" + time.ToString(format) + "')";
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
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                SendMessage = "IncorrectPWD";
            }
            return SendMessage;
        }
        public string Expo_limit(string exp_id, string exposure_limit, string password, string old_exp_limit)
        {
            string SendMessage = "";
            try
            {
                string DL_login_user_id = (string)System.Web.HttpContext.Current.Session["DL_login_user_id"];
                string DL_login_username = (string)System.Web.HttpContext.Current.Session["DL_UserName"];

                DateTime time = DateTime.Now;
                string format = "yyyy-MM-dd HH:mm:ss";
                connection2();
                con2.Open();
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con2;
                com.CommandText = "SELECT password FROM distributors WHERE id='" + DL_login_user_id + "' AND password='" + password + "' ";
                dr = com.ExecuteReader();
                if (dr.HasRows)
                {
                    connection2();
                    con2.Open();
                    SqlCommand com1 = new SqlCommand();
                    com1.Connection = con2;
                    com1.CommandText = "UPDATE users_client SET exposure_limit='" + exposure_limit + "' WHERE id='" + exp_id + "' AND dl_id='" + DL_login_user_id + "'";
                    int runquery2 = com1.ExecuteNonQuery();
                    if (runquery2 > 0)
                    {
                        SendMessage = "Success";
                        string activitLogDescription = exp_id + " Old Exposure Limit " + old_exp_limit + "Changed to New Exposure Limit " + exposure_limit;
                        connection2();
                        string query1 = "INSERT INTO activity_logs(description, receiver_account_type, receiver_id, created) VALUES ('" + activitLogDescription + "','exp_client','" + exp_id + "','" + time.ToString(format) + "')";
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
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                SendMessage = "IncorrectPWD";
            }
            return SendMessage;
        }
    }
}