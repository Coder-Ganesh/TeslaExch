using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Probet247.Controllers
{
    public class AdminFunController : Controller
    {
        // GET: AdminFun
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
                    com.CommandText = "SELECT id ,hash_key FROM admin WHERE username='" + username + "' and password='" + password + "' AND status='activate'";
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
                        Session["Admin_UserName"] = username.ToString();
                        Session["Admin_hash_key"] = hash_key.ToString();
                        Session["Admin_login_user_id"] = login_user_id.ToString();
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

        public string ClientAction(string user_id, string status, string changeStatusPassword, string username)
        {
            string SendMessage = "";
            try
            {
                string MDL_login_user_id = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];
                string MDL_login_username = (string)System.Web.HttpContext.Current.Session["Admin_UserName"];

                DateTime time = DateTime.Now;
                string format = "yyyy-MM-dd HH:mm:ss";
                connection2();
                con2.Open();
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con2;
                com.CommandText = "SELECT password FROM admin WHERE id='" + MDL_login_user_id + "' AND password='" + changeStatusPassword + "' ";
                dr = com.ExecuteReader();
                if (dr.HasRows)
                {
                    if (status == "activate")
                    {
                        connection2();
                        con2.Open();
                        SqlCommand com1 = new SqlCommand();
                        com1.Connection = con2;
                        com1.CommandText = "UPDATE masterdistributors SET status='activate' WHERE status='deactivate' AND id='" + user_id + "' AND admin_id='" + MDL_login_user_id + "'";
                        int runquery2 = com1.ExecuteNonQuery();
                        if (runquery2 > 0)
                        {
                            SendMessage = "Success";
                            string activitLogDescription = username + "[SM] Account Activated by " + MDL_login_username + "[Admin].";
                            connection2();
                            string query1 = "INSERT INTO activity_logs(description, receiver_account_type, receiver_id, created) VALUES ('" + activitLogDescription + "','admin','" + MDL_login_user_id + "','" + time.ToString(format) + "')";
                            SqlCommand com11 = new SqlCommand(query1, con2);
                            com11.ExecuteNonQuery();
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
                        com2.CommandText = "UPDATE masterdistributors SET status='deactivate' WHERE status='activate' AND id='" + user_id + "' AND admin_id='" + MDL_login_user_id + "'";
                        int runquery2 = com2.ExecuteNonQuery();
                        if (runquery2 > 0)
                        {
                            SendMessage = "Success";
                            string activitLogDescription = username + "[SM] Account Deactivated by " + MDL_login_username + "[Admin].";
                            connection2();
                            string query1 = "INSERT INTO activity_logs(description, receiver_account_type, receiver_id, created) VALUES ('" + activitLogDescription + "','admin','" + MDL_login_user_id + "','" + time.ToString(format) + "')";
                            SqlCommand com11 = new SqlCommand(query1, con2);
                            com11.ExecuteNonQuery();
                        }
                        else
                        {
                            SendMessage = "Failed11"; ;
                        }
                        con2.Close();
                    }
                    else if (status == "locked")
                    {
                        connection2();
                        con2.Open();
                        SqlCommand com2 = new SqlCommand();
                        com2.Connection = con2;
                        com2.CommandText = "UPDATE masterdistributors SET status='delete' WHERE status='activate' AND id='" + user_id + "' AND admin_id='" + MDL_login_user_id + "'";
                        int runquery2 = com2.ExecuteNonQuery();
                        if (runquery2 > 0)
                        {
                            string query25 = "UPDATE distributors SET status='delete' WHERE status='activate' AND md_id='" + user_id + "'";
                            SqlCommand com3 = new SqlCommand(query25, con2);
                            com3.ExecuteNonQuery();
                            string query26 = "UPDATE users_client SET status='delete' WHERE status='activate' AND mdl_id='" + user_id + "'";
                            SqlCommand com4 = new SqlCommand(query26, con2);
                            com4.ExecuteNonQuery();
                            string activitLogDescription = username + "[SM] Account Locked by " + MDL_login_username + "[Admin].";
                            string query1 = "INSERT INTO activity_logs(description, receiver_account_type, receiver_id, created) VALUES ('" + activitLogDescription + "','admin','" + MDL_login_user_id + "','" + time.ToString(format) + "')";
                            SqlCommand com11 = new SqlCommand(query1, con2);
                            com11.ExecuteNonQuery();
                            SendMessage = "Success";
                        }
                        else
                        {
                            SendMessage = "Failed11"; ;
                        }
                        con2.Close();
                    }
                    else if (status == "unlocked")
                    {
                        connection2();
                        con2.Open();
                        SqlCommand com2 = new SqlCommand();
                        com2.Connection = con2;
                        com2.CommandText = "UPDATE masterdistributors SET status='activate' WHERE status='delete' AND id='" + user_id + "' AND admin_id='" + MDL_login_user_id + "'";
                        int runquery2 = com2.ExecuteNonQuery();
                        if (runquery2 > 0)
                        {
                            string query25 = "UPDATE distributors SET status='activate' WHERE status='delete' AND md_id='" + user_id + "'";
                            SqlCommand com3 = new SqlCommand(query25, con2);
                            com3.ExecuteNonQuery();
                            string query26 = "UPDATE users_client SET status='activate' WHERE status='delete' AND mdl_id='" + user_id + "'";
                            SqlCommand com4 = new SqlCommand(query26, con2); 
                            com4.ExecuteNonQuery();
                            string activitLogDescription = username + "[SM] Account Locked by " + MDL_login_username + "[Admin].";
                            string query1 = "INSERT INTO activity_logs(description, receiver_account_type, receiver_id, created) VALUES ('" + activitLogDescription + "','admin','" + MDL_login_user_id + "','" + time.ToString(format) + "')";
                            SqlCommand com11 = new SqlCommand(query1, con2);
                            com11.ExecuteNonQuery();
                            SendMessage = "Success";
                        }
                        else
                        {
                            SendMessage = "Failed11"; ;
                        }
                        con2.Close();
                    }
                    else { }
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
                SendMessage = ex.ToString(); //"IncorrectPWD";
            }
            return SendMessage;
        }

        public string CreditRef(string credit_id, string new_credit_ref, string password, string old_credit_ref)
        {
            string SendMessage = "";
            try
            {
                string AD_login_user_id = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];
                string MDL_login_username = (string)System.Web.HttpContext.Current.Session["Admin_UserName"];

                DateTime time = DateTime.Now;
                string format = "yyyy-MM-dd HH:mm:ss";
                connection2();
                con2.Open();
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con2;
                com.CommandText = "SELECT password FROM admin WHERE id='" + AD_login_user_id + "' AND password='" + password + "' ";
                dr = com.ExecuteReader();
                if (dr.HasRows)
                {
                    connection2();
                    con2.Open();
                    SqlCommand com1 = new SqlCommand();
                    com1.Connection = con2;
                    com1.CommandText = "UPDATE masterdistributors SET credit_ref='" + new_credit_ref + "' WHERE id='" + credit_id + "' AND admin_id='" + AD_login_user_id + "'";
                    int runquery2 = com1.ExecuteNonQuery();
                    if (runquery2 > 0)
                    {
                        SendMessage = "Success";
                        string activitLogDescription = credit_id + "Old Credit Ref " + old_credit_ref + "Changed to New Credit Ref" + new_credit_ref;
                        connection2();
                        string query1 = "INSERT INTO activity_logs(description, receiver_account_type, receiver_id, created) VALUES ('" + activitLogDescription + "','credit_mdl','" + AD_login_user_id + "','" + time.ToString(format) + "')";
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

        public string AddSuperMaster(string email, string username, string password, Double mpl)
        {

            string SendStatus = "";
            try
            {
                string SA_login_user_id = (string)System.Web.HttpContext.Current.Session["ADMIN_login_user_id"];
                int CheckDataInsert = 0;
                string dlname = username;
                string phone = "0123456789";
                Double commission = 0;
                int aglimit = 100;
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
                    long Time_Stamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                    string query = "INSERT INTO [dbo].[masterdistributors]([admin_id] ,[username],[name] ,[email],[mobile] ,[password] ,[image],[status] ,[hash_key] ,[last_login] ,[coin_rate] ,[ocomm_rate] ,[register_time] ,[role_id] ,[balance] ,[total_balance] ,[cash] ,[profit_loss] ,[admin_profit_loss] ,[total_withdrawal] ,[commission] ,[modify_time],[ag_limit]) " +
                        "VALUES ('" + SA_login_user_id + "' ,'" + username + "' ,'" + dlname + "' ,'" + email + "' ,'" + phone + "' ,'" + password + "' ,'default.png' ,'activate' ,'" + time.ToString(format1) + "' ,'" + time.ToString(format1) + "' ,'" + mpl + "' ,'" + commission + "' ,'" + time.ToString(format1) + "' ,'0' ,'0','0' ,'0','0' ,'0' ,'0' ,'0','" + time.ToString(format1) + "','" + aglimit + "')";

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
                SendStatus = "Failed";
            }

            return SendStatus;
        }

        public string ChangePasswordDB(string user_id, string newPassword, string changePassword)
        {
            string ReturnMessage = "";
            try
            {
                if (user_id != "" && newPassword != "" && changePassword != "")
                {
                    string SA_login_user_idin = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];
                    using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con1.Open();
                        string stmt1 = "SELECT id FROM admin WHERE id='" + SA_login_user_idin + "' AND password='" + changePassword + "' ";

                        using (SqlCommand cmdCount1 = new SqlCommand(stmt1, con1))
                        {
                            cmdCount1.ExecuteScalar();
                            var reader1 = cmdCount1.ExecuteReader();
                            if (reader1.HasRows)
                            {
                                Random rand = new Random();
                                rand.Next();
                                SqlCommand dist_update = new SqlCommand("UPDATE admin SET password='" + newPassword + "',hash_key='" + rand + "' WHERE id='" + SA_login_user_idin + "'", con1);
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

        public string SMChangePasswordDB(string md_id, string dist_id, string user_id, string newPassword, string changePassword)
        {
            string ReturnMessage = "";
            try
            {
                if (user_id != "" && newPassword != "" && changePassword != "")
                {
                    string SA_login_user_idin = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];
                    string table_nn = "";
                    int uid = 0;
                    if (md_id != "0" && dist_id == "0" && user_id == "0")
                    {
                        uid = Int32.Parse(md_id);
                        table_nn = "masterdistributors";
                    }
                    else if (md_id != "0" && dist_id != "0" && user_id == "0")
                    {
                        uid = Int32.Parse(dist_id);
                        table_nn = "distributors";
                    }
                    else if (md_id != "0" && dist_id != "0" && user_id != "0")
                    {
                        uid = Int32.Parse(user_id);
                        table_nn = "users_client";
                    }
                    if (table_nn != "")
                    {
                        using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            con1.Open();
                            string stmt1 = "SELECT id FROM admin WHERE id='" + SA_login_user_idin + "' AND password='" + changePassword + "' ";

                            using (SqlCommand cmdCount1 = new SqlCommand(stmt1, con1))
                            {
                                cmdCount1.ExecuteScalar();
                                var reader1 = cmdCount1.ExecuteReader();
                                if (reader1.HasRows)
                                {
                                    Random rand = new Random();
                                    rand.Next();
                                    SqlCommand dist_update = new SqlCommand("UPDATE " + table_nn + " SET password='" + newPassword + "',hash_key='" + rand + "' WHERE id='" + uid + "' AND admin_id='" + SA_login_user_idin + "' ", con1);
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

        public Double GetMDLBalance(string MDLId)
        {
            Double balance = 0;
            using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con1.Open();
                string stmt1 = "SELECT [balance] FROM admin WHERE id='" + MDLId + "' ";

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

        public Double GetMDLRBalance(string MDLId)
        {
            Double balance = 0;
            using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con1.Open();
                string stmt1 = "SELECT [balance] FROM masterdistributors WHERE id='" + MDLId + "' ";

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

        public string DepositCoinTOMDL(string user_id, float coins, string remark1)
        {
            string ReturnMSG = "";
            try
            {
                if (user_id != "" && coins != 0 && user_id != "0")
                {
                    DateTime time = DateTime.Now;
                    string format = "yyyy-MM-dd HH:mm:ss";
                    string Admin_sess_ID = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];
                    string Admin_Sess_Name = (string)System.Web.HttpContext.Current.Session["Admin_UserName"];

                    Double GetDlBal = GetMDLBalance(Admin_sess_ID);
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        string stmt = "SELECT admin_id,balance,total_balance,username FROM masterdistributors WHERE id='" + user_id + "' AND admin_id='" + Admin_sess_ID + "' ";

                        using (SqlCommand cmdCount = new SqlCommand(stmt, con))
                        {
                            cmdCount.ExecuteScalar();
                            var reader = cmdCount.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    int admin_id = (int)reader["admin_id"];

                                    Double user_current_balance = (Double)reader["balance"];
                                    Double total_balance = (Double)reader["total_balance"];
                                    Double total_net_balance = total_balance + coins;
                                    Double user_liability = 0;
                                    string user_detail = (string)reader["username"] + "[Super Master]";
                                    string dist_detail = Admin_Sess_Name + "[Senior Super]";

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

                                        SqlCommand cmd2 = new SqlCommand("INSERT INTO admin_account_statements(event_id,market_id,admin_id,md_id,dist_id,user_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver) VALUES " +
                                            "('','','" + admin_id + "','" + user_id + "','0','0','dw_coins','Deposit " + coins + " Coins to " + user_detail + "','" + remark1 + "','" + coins + "','','" + dist_net_balance + "','" + time.ToString(format) + "' ,'','') ", con);
                                        int CheckDataInsert = cmd2.ExecuteNonQuery();
                                        if (CheckDataInsert > 0)
                                        {
                                            SqlCommand user_account_statement = new SqlCommand("INSERT INTO md_account_statements(event_id,market_id,admin_id,md_id,dist_id,user_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver) VALUES " +
                                                "('','','" + admin_id + "','" + user_id + "','0','0','dw_coins','Deposit " + coins + " Coins by " + dist_detail + "','','','" + coins + "','" + user_account_balance + "','" + time.ToString(format) + "' ,'','')", con);
                                            user_account_statement.ExecuteNonQuery();

                                            SqlCommand user_update = new SqlCommand("UPDATE masterdistributors SET balance='" + user_net_balance + "' , total_balance='" + total_net_balance + "' WHERE id='" + user_id + "'", con);
                                            user_update.ExecuteNonQuery();

                                            SqlCommand dist_update = new SqlCommand("UPDATE admin SET balance='" + dist_net_balance + "' WHERE id='" + admin_id + "'", con);
                                            dist_update.ExecuteNonQuery();

                                            string activitLogDescription = "Deposit " + coins + " Coins to " + user_detail;

                                            SqlCommand sqlActivityLog = new SqlCommand("INSERT INTO activity_logs(description, receiver_account_type, receiver_id, created) VALUES ('" + activitLogDescription + "','admin','" + Admin_sess_ID + "','" + time.ToString(format) + "')", con);
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

        public string WithdrawalCoinsToMDL(string user_id, float coins, string remark1)
        {
            string ReturnMSG = "";
            try
            {
                if (user_id != "" && coins != 0 && user_id != "0")
                {
                    DateTime time = DateTime.Now;
                    string format = "yyyy-MM-dd HH:mm:ss";
                    string Admin_sess_ID = (string)System.Web.HttpContext.Current.Session["Admin_login_user_id"];
                    string Admin_Sess_Name = (string)System.Web.HttpContext.Current.Session["Admin_UserName"];

                    Double GetDlBal = GetMDLBalance(Admin_sess_ID);
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        string stmt = "SELECT admin_id,balance,total_balance,username FROM masterdistributors WHERE id='" + user_id + "' AND admin_id='" + Admin_sess_ID + "' ";

                        using (SqlCommand cmdCount = new SqlCommand(stmt, con))
                        {
                            cmdCount.ExecuteScalar();
                            var reader = cmdCount.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    int admin_id = (int)reader["admin_id"];

                                    Double user_current_balance = (Double)reader["balance"];
                                    Double total_balance = (Double)reader["total_balance"];
                                    Double total_net_balance = total_balance - coins;
                                    Double user_liability = 0;
                                    string user_detail = (string)reader["username"] + "[Super Master]";
                                    string dist_detail = Admin_Sess_Name + "[Senior Super]";

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

                                        SqlCommand cmd2 = new SqlCommand("INSERT INTO admin_account_statements(event_id,market_id,admin_id,md_id,dist_id,user_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver) VALUES " +
                                           "('','','" + admin_id + "','" + user_id + "','0','0','dw_coins','Withdraw " + coins + " Coins From " + user_detail + "','" + remark1 + "','','" + coins + "','" + dist_net_balance + "','" + time.ToString(format) + "' ,'','') ", con);
                                        int CheckDataInsert = cmd2.ExecuteNonQuery();


                                        if (CheckDataInsert > 0)
                                        {
                                            SqlCommand user_account_statement = new SqlCommand("INSERT INTO md_account_statements(event_id,market_id,admin_id,md_id,dist_id,user_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver) VALUES " +
                                               "('','','" + admin_id + "','" + user_id + "','0','0','dw_coins','Withdraw " + coins + " Coins by " + dist_detail + "','','" + coins + "','','" + user_account_balance + "','" + time.ToString(format) + "' ,'','')", con);
                                            user_account_statement.ExecuteNonQuery();

                                            SqlCommand user_update = new SqlCommand("UPDATE masterdistributors SET balance='" + user_net_balance + "' , total_balance='" + total_net_balance + "' WHERE id='" + user_id + "'", con);
                                            user_update.ExecuteNonQuery();

                                            SqlCommand dist_update = new SqlCommand("UPDATE admin SET balance='" + dist_net_balance + "' WHERE id='" + Admin_sess_ID + "'", con);
                                            dist_update.ExecuteNonQuery();

                                            string activitLogDescription = "Withdraw " + coins + " Coins to " + user_detail;

                                            SqlCommand sqlActivityLog = new SqlCommand("INSERT INTO activity_logs(description, receiver_account_type, receiver_id, created) VALUES ('" + activitLogDescription + "','admin','" + Admin_sess_ID + "','" + time.ToString(format) + "')", con);
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

        public string ClientDepositCoinsmdl(string user_id, float coins, string remark1, string mid)
        {
            string ReturnMSG = "";
            try
            {
                if (user_id != "" && coins != 0 && user_id != "0")
                {
                    DateTime time = DateTime.Now;
                    string format = "yyyy-MM-dd HH:mm:ss";
                    string MDL_login_user_id = mid;
                    string MDL_login_username = "";

                    Double GetDlBal = GetMDLRBalance(MDL_login_user_id);
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        string stmt = "SELECT admin_id,balance,total_balance,username FROM distributors WHERE id='" + user_id + "' AND md_id='" + MDL_login_user_id + "' ";

                        using (SqlCommand cmdCount = new SqlCommand(stmt, con))
                        {
                            cmdCount.ExecuteScalar();
                            var reader = cmdCount.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    int admin_id = (int)reader["admin_id"];

                                    Double user_current_balance = (Double)reader["balance"];
                                    Double total_balance = (Double)reader["total_balance"];
                                    Double total_net_balance = total_balance + coins;
                                    Double user_liability = 0;
                                    string user_detail = (string)reader["username"] + "[Client]";
                                    string dist_detail = MDL_login_username + "[MDL]";

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

                                        SqlCommand cmd2 = new SqlCommand("INSERT INTO md_account_statements(event_id,market_id,user_id,admin_id,dist_id,md_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver) VALUES " +
                                            "('','','" + user_id + "','" + admin_id + "','" + user_id + "','" + MDL_login_user_id + "','dw_coins','Deposit " + coins + " Coins to " + user_detail + "','" + remark1 + "','" + coins + "','','" + dist_net_balance + "','" + time.ToString(format) + "' ,'','') ", con);
                                        int CheckDataInsert = cmd2.ExecuteNonQuery();
                                        if (CheckDataInsert > 0)
                                        {
                                            SqlCommand user_account_statement = new SqlCommand("INSERT INTO dist_account_statements(admin_id,user_id,dist_id,md_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver,market_id) VALUES " +
                                                "('" + admin_id + "','" + user_id + "','" + user_id + "','" + MDL_login_user_id + "','dw_coins','Deposit " + coins + " Coins by " + dist_detail + "','','','" + coins + "','" + user_account_balance + "','" + time.ToString(format) + "' ,'','','')", con);
                                            user_account_statement.ExecuteNonQuery();

                                            SqlCommand user_update = new SqlCommand("UPDATE distributors SET balance='" + user_net_balance + "' , total_balance='" + total_net_balance + "' WHERE id='" + user_id + "'", con);
                                            user_update.ExecuteNonQuery();

                                            SqlCommand dist_update = new SqlCommand("UPDATE masterdistributors SET balance='" + dist_net_balance + "' WHERE id='" + MDL_login_user_id + "'", con);
                                            dist_update.ExecuteNonQuery();

                                            string activitLogDescription = "Deposit " + coins + " Coins to " + user_detail;

                                            SqlCommand sqlActivityLog = new SqlCommand("INSERT INTO activity_logs(description, receiver_account_type, receiver_id, created) VALUES ('" + activitLogDescription + "','mdl','" + MDL_login_user_id + "','" + time.ToString(format) + "')", con);
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
                ReturnMSG = "Error";
            }
            return ReturnMSG;
        }

        public string ClientWithdrawalCoinsmdl(string user_id, float coins, string remark1, string mid)
        {
            string ReturnMSG = "";
            try
            {
                if (user_id != "" && coins != 0 && user_id != "0")
                {
                    DateTime time = DateTime.Now;
                    string format = "yyyy-MM-dd HH:mm:ss";
                    string MDL_login_user_id = mid;
                    string MDL_login_username = (string)System.Web.HttpContext.Current.Session["MDL_UserName"];

                    Double GetDlBal = GetMDLRBalance(MDL_login_user_id);
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        string stmt = "SELECT admin_id,balance,total_balance,username FROM distributors WHERE id='" + user_id + "' AND md_id='" + MDL_login_user_id + "' ";

                        using (SqlCommand cmdCount = new SqlCommand(stmt, con))
                        {
                            cmdCount.ExecuteScalar();
                            var reader = cmdCount.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    int admin_id = (int)reader["admin_id"];

                                    Double user_current_balance = (Double)reader["balance"];
                                    Double total_balance = (Double)reader["total_balance"];
                                    Double total_net_balance = total_balance + coins;
                                    Double user_liability = 0;
                                    string user_detail = (string)reader["username"] + "[Client]";
                                    string dist_detail = MDL_login_username + "[MDL]";

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

                                        SqlCommand cmd2 = new SqlCommand("INSERT INTO md_account_statements(event_id,market_id,user_id,admin_id,dist_id,md_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver) VALUES " +
                                            "('','','" + user_id + "','" + admin_id + "','" + user_id + "','" + MDL_login_user_id + "','dw_coins','Withdraw " + coins + " Coins to " + user_detail + "','" + remark1 + "','" + coins + "','" + dist_net_balance + "','" + dist_net_balance + "','" + time.ToString(format) + "' ,'','') ", con);
                                        int CheckDataInsert = cmd2.ExecuteNonQuery();
                                        if (CheckDataInsert > 0)
                                        {
                                            SqlCommand user_account_statement = new SqlCommand("INSERT INTO dist_account_statements(admin_id,user_id,dist_id,md_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver,market_id) VALUES " +
                                                "('" + admin_id + "','" + user_id + "','" + user_id + "','" + MDL_login_user_id + "','dw_coins','Withdraw " + coins + " Coins by " + dist_detail + "','','" + coins + "','','" + user_account_balance + "','" + time.ToString(format) + "' ,'','','')", con);
                                            user_account_statement.ExecuteNonQuery();

                                            SqlCommand user_update = new SqlCommand("UPDATE distributors SET balance='" + user_net_balance + "' , total_balance='" + total_net_balance + "' WHERE id='" + user_id + "'", con);
                                            user_update.ExecuteNonQuery();

                                            SqlCommand dist_update = new SqlCommand("UPDATE masterdistributors SET balance='" + dist_net_balance + "' WHERE id='" + MDL_login_user_id + "'", con);
                                            dist_update.ExecuteNonQuery();

                                            string activitLogDescription = "Withdraw " + coins + " Coins to " + user_detail;

                                            SqlCommand sqlActivityLog = new SqlCommand("INSERT INTO activity_logs(description, receiver_account_type, receiver_id, created) VALUES ('" + activitLogDescription + "','mdl','" + MDL_login_user_id + "','" + time.ToString(format) + "')", con);
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
                ReturnMSG = "false";
            }
            return ReturnMSG;
        }

        public string ClientDepositCoinsdl(string user_id, float coins, string remark1, string did)
        {
            string ReturnMSG = "";
            try
            {
                if (user_id != "" && coins != 0 && user_id != "0")
                {
                    DateTime time = DateTime.Now;
                    string format = "yyyy-MM-dd HH:mm:ss";
                    string DL_login_user_idin = did;
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
                                    string user_detail = (string)reader["username"] + "[Client]";
                                    string dist_detail = DL_login_username + "[DL]";

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

                                        SqlCommand cmd2 = new SqlCommand("INSERT INTO dist_account_statements(market_id,user_id,admin_id,dist_id,md_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver) VALUES " +
                                            "('','" + user_id + "','" + admin_id + "','" + DL_login_user_idin + "','" + md_id + "','dw_coins','Deposit " + coins + " Coins to " + user_detail + "','" + remark1 + "','" + coins + "','','" + dist_net_balance + "','" + time.ToString(format) + "' ,'','') ", con);
                                        int CheckDataInsert = cmd2.ExecuteNonQuery();
                                        if (CheckDataInsert > 0)
                                        {
                                            SqlCommand user_account_statement = new SqlCommand("INSERT INTO user_account_statements(admin_id,user_id,dist_id,md_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver,market_id,cc_market_id,event_id,match_odds) VALUES " +
                                                "('" + admin_id + "','" + user_id + "','" + DL_login_user_idin + "','" + md_id + "','dw_coins','Deposit " + coins + " Coins by " + dist_detail + "','','','" + coins + "','" + user_account_balance + "','" + time.ToString(format) + "' ,'','','','','','')", con);
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
                ReturnMSG = "false";
            }
            return ReturnMSG;
        }

        public string ClientWithdrawalCoinsdl(string user_id, float coins, string remark1, string did)
        {
            string ReturnMSG = "";
            try
            {
                if (user_id != "" && coins != 0 && user_id != "0")
                {
                    DateTime time = DateTime.Now;
                    string format = "yyyy-MM-dd HH:mm:ss";
                    string DL_login_user_idin = did;
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
                                    string user_detail = (string)reader["username"] + "[Client]";
                                    string dist_detail = DL_login_username + "[DL]";

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

                                        SqlCommand cmd2 = new SqlCommand("INSERT INTO dist_account_statements(market_id,user_id,admin_id,dist_id,md_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver) VALUES " +
                                            "('','" + user_id + "','" + admin_id + "','" + DL_login_user_idin + "','" + md_id + "','dw_coins','Withdraw " + coins + " Coins to " + user_detail + "','" + remark1 + "','" + coins + "','" + dist_net_balance + "','" + dist_net_balance + "','" + time.ToString(format) + "' ,'','') ", con);
                                        int CheckDataInsert = cmd2.ExecuteNonQuery();
                                        if (CheckDataInsert > 0)
                                        {
                                            SqlCommand user_account_statement = new SqlCommand("INSERT INTO user_account_statements(admin_id,user_id,dist_id,md_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver,market_id,cc_market_id,event_id,match_odds) VALUES " +
                                                "('" + admin_id + "','" + user_id + "','" + DL_login_user_idin + "','" + md_id + "','dw_coins','Withdraw " + coins + " Coins by " + dist_detail + "','','" + coins + "','','" + user_account_balance + "','" + time.ToString(format) + "' ,'','','','','','')", con);
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
                ReturnMSG = "false";
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
                    reader1.Read();
                    balance = (Double)reader1["balance"];
                }
                con1.Close();
            }
            return balance;
        }

        public string Addxdata(string x_code, string x_type, string event_code)
        {

            string SendStatus = "";
            try
            {
                connection2();
                con2.Open();
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con2;
                com.CommandText = "SELECT event_code FROM matches WHERE event_code='" + event_code + "'";
                dr = com.ExecuteReader();
                if (dr.HasRows)
                {
                    connection2();
                    long Time_Stamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                    string query = "UPDATE matches SET x_code='" + x_code + "' , x_type='" + x_type + "' where event_code='" + event_code + "' ";
                    SqlCommand com1 = new SqlCommand(query, con2);
                    con2.Open();
                    var CheckDataInsert = com1.ExecuteNonQuery();
                    if (CheckDataInsert > 0)
                    {
                        SendStatus = "Data Update Done";
                    }
                    else
                    {
                        SendStatus = "Error in Update Done";
                    }
                    con2.Close();
                }
                else
                {
                    SendStatus = "Match Not Available";
                }
            }
            catch (Exception ex)
            {

            }
            return SendStatus;
        }
    }
}