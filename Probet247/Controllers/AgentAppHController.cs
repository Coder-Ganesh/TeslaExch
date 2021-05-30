using BetBarter.Models;
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
    public class AgentAppHController : Controller
    {

        private SqlConnection con2;
        private void connection2()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            con2 = new SqlConnection(constr);
        }

        DateTime time = DateTime.Now;
        string format1 = "yyyy-MM-dd HH:mm:ss";

        // GET: AgentAppH
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetSessionDataS1()
        {
            List<UserRagister> messages = new List<UserRagister>();
            if (Request["inputUserid"] != null && Request["inputUserpass"] != null)
            {
                string inputUserid = Request["inputUserid"];
                string inputUserpass = Request["inputUserpass"];
                messages = GetAllMessagesNewS1(inputUserid, inputUserpass);
            }
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<UserRagister> GetAllMessagesNewS1(string user, string pass)
        {
            var messages = new List<UserRagister>();
            string Statusstr = "Failed";
            string username = "";
            string hash_key = "";
            int uid = 0;
            string OddsUrl = "";
            string SessUrl = "";
            if (user != null && pass != null && user != "" && pass != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    string query = "SELECT id,status,hash_key,username from distributors where username='" + user + "' and password='" + pass + "' AND status='activate' ";
                    using (var cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            reader.Read();
                            Statusstr = (string)reader["status"];
                            if (Statusstr == "deactivate")
                            {
                                Statusstr = "Your Account is deactivate";
                                hash_key = "";
                                username = "";
                                uid = 0;
                            }
                            else if (Statusstr == "activate")
                            {
                                Statusstr = "Success";
                                hash_key = (string)reader["hash_key"];
                                username = (string)reader["username"];
                                uid = (int)reader["id"];
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            Statusstr = "Failed";
                        }
                        con.Close();
                    }
                }
            }
            messages.Add(item: new UserRagister
            {
                Status = Statusstr,
                hash_key = hash_key,
                uid = uid,
                UserName = username,
                OddsUrl = OddsUrl,
                SessUrl = SessUrl
            });
            return messages;

        }

        public Double DL_Balance(string uid)
        {
            Double Balance = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("select balance from distributors where id='" + uid + "'", con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        Balance = Math.Round((Double)reader["balance"]);
                    }
                    con.Close();
                }
            }
            return Balance;
        }


        public string AddDLClient(string username, string name, string password, string exposure_limit, 
            string StrEt_Mobile,string dl_id)
        {
            string SendStatus = "Error in Adding Player";
            string commission_mo = "0";
            string commission_sess = "0";
            try
            {
                int CheckDataInsert = 0;
                connection2();
                con2.Open();
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con2;
                com.CommandText = "SELECT username FROM users_client WHERE username='" + username + "'";
                dr = com.ExecuteReader();
                if (dr.HasRows)
                {
                    SendStatus = "This Player is already registered.";
                }
                else
                {
                    int MDL_login_user_id = MDL_ID(dl_id);
                    int Admin_login_user_id = Admin_ID(dl_id);
                    DateTime GetCurrentDT = System.DateTime.Now;
                    connection2();
                    long Time_Stamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    string query = "INSERT INTO  users_client (admin_id,mdl_id,dl_id,name,username,password,status,ragister_time,last_login,hash_key,balance,exposure,total_balance,profit_loss,ocomm_rate,scomm_rate,exposure_limit,mobile) VALUES " +
                        "('" + Admin_login_user_id + "','" + MDL_login_user_id + "','" + dl_id + "','" + name + "','" + username + "','" + password + "','activate','" + time.ToString(format1) + "','" + time.ToString(format1) + "','" + Time_Stamp + "','0','0','0','0','" + commission_mo + "','" + commission_sess + "','" + exposure_limit + "','"+ StrEt_Mobile + "')";
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



        public JsonResult UserList_DL()
        {
            List<UserRagister> messages = new List<UserRagister>();
            if (Request["dl_id"] != null)
            {
                string inputUserid = Request["dl_id"];
                messages = UserList_DL1(inputUserid);
            }
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<UserRagister> UserList_DL1(string dl_id)
        {
            var messages = new List<UserRagister>();
            string Statusstr = "Failed";
            string username = "";
            string hash_key = "";
            int uid = 0;
            string OddsUrl = "";
            string SessUrl = "";
            Double balance = 0;
            Double exposure = 0;
            if (dl_id != null && dl_id != null)
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    string query = "SELECT id,status,username,balance,exposure from users_client where dl_id='"+ dl_id + "' ";
                    using (var cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Statusstr = (string)reader["status"];
                                balance = System.Math.Round((Double)reader["balance"], 2);
                                exposure = System.Math.Round((Double)reader["exposure"], 2);
                                username = (string)reader["username"];
                                uid = (int)reader["id"];

                                messages.Add(item: new UserRagister
                                {
                                    Status = Statusstr,
                                    hash_key = hash_key,
                                    uid = uid,
                                    UserName = username,
                                    OddsUrl = OddsUrl,
                                    SessUrl = SessUrl,
                                    balance = balance,
                                    exposure = exposure
                                });
                            }
                           
                        }
                      
                        con.Close();
                    }
                }
            }
           
            return messages;

        }


        public int MDL_ID(string dl_id)
        {
            int nfdjh = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("select md_id from distributors where id='" + dl_id + "'", con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        nfdjh = (int)reader["md_id"];
                    }
                    con.Close();
                }
            }

            return nfdjh;
        }

        public int Admin_ID(string dl_id)
        {
            int nfdjh = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("select admin_id from distributors where id='" + dl_id + "'", con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        nfdjh = (int)reader["admin_id"];
                    }
                    con.Close();
                }
            }

            return nfdjh;
        }

        public string UserStatus(string uid,string status)
        {
            string nfdjh = "Failed";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                SqlCommand sqlMarketUpdate = new SqlCommand("UPDATE users_client SET status='"+status+"' WHERE id='"+uid+"' ", con);
                int sqlMarketUpdatedone = sqlMarketUpdate.ExecuteNonQuery();
                if (sqlMarketUpdatedone > 0)
                {
                    nfdjh = "Success";
                }
                con.Close();
            }
            return nfdjh;
        }


        public string ClientDepositCoins(string user_id, float coins, string DL_login_user_idin)
        {
            string ReturnMSG = "Failed";
            try
            {
                if (user_id != "" && coins != 0 && user_id != "0")
                {
                    DateTime time = DateTime.Now;
                    string format = "yyyy-MM-dd HH:mm:ss";
                  
                    Double GetDlBal = GetDLBalance(DL_login_user_idin);
                    string DL_login_username = GetDLUsername(DL_login_user_idin);
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
                                        ReturnMSG = "Insufficient Amount";

                                    }
                                    else
                                    {
                                        dist_net_balance = GetDlBal - coins;

                                        SqlCommand cmd2 = new SqlCommand("INSERT INTO dist_account_statements(market_id,admin_id,md_id,dist_id,user_id,acc_stat_type, description, remark, debit, credit, balance, created,sender,receiver) VALUES " +
                                            "('','" + admin_id + "','" + md_id + "','" + DL_login_user_idin + "','" + user_id + "','dw_coins','Deposit " + coins + " Coins to " + user_detail + "','','" + coins + "','','" + dist_net_balance + "','" + time.ToString(format) + "' ,'','') ", con);
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

                                            ReturnMSG = "Amount Deposit Successfully!";
                                        }
                                        else
                                        {
                                            ReturnMSG = "Failed";
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

        public string ClientWithdrawalCoins(string user_id, float coins, string DL_login_user_idin)
        {
            string ReturnMSG = "";
            try
            {
                if (user_id != "" && coins != 0 && user_id != "0")
                {
                    DateTime time = DateTime.Now;
                    string format = "yyyy-MM-dd HH:mm:ss";
                   
                    Double GetDlBal = GetDLBalance(DL_login_user_idin);
                    string DL_login_username = GetDLUsername(DL_login_user_idin);
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
                                            "('','" + admin_id + "','" + md_id + "','" + DL_login_user_idin + "','" + user_id + "','dw_coins','Withdraw " + coins + " Coins From " + user_detail + "','','0','" + coins + "','" + dist_net_balance + "','" + time.ToString(format) + "' ,'','') ", con);
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

                                            ReturnMSG = "Amount Withdrawal Successfully!";
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

        public string GetDLUsername(string DLId)
        {
            string balance = "";
            using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con1.Open();
                string stmt1 = "SELECT username FROM distributors WHERE id='" + DLId + "' ";

                using (SqlCommand cmdCount1 = new SqlCommand(stmt1, con1))
                {
                    cmdCount1.ExecuteScalar();
                    var reader1 = cmdCount1.ExecuteReader();
                    while (reader1.Read())
                    {
                        balance = (string)reader1["username"];
                    }
                }
                con1.Close();
            }
            return balance;
        }

        public JsonResult accountCashStatement()
        {
            List<AccountStatement_DL> messages = new List<AccountStatement_DL>();
            if (Request["DL_login_user_idn"] != null)
            {
                string DL_login_user_idn = Request["DL_login_user_idn"];
                messages = accountCashStatement1(DL_login_user_idn);
            }
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<AccountStatement_DL> accountCashStatement1(string DL_login_user_idn)
        {
            var DL_UserBetList = new List<AccountStatement_DL>();
            try
            {
                connection2();
                con2.Open();
                SqlCommand com = new SqlCommand();
                SqlDataReader dr;
                com.Connection = con2;
                com.CommandText = "SELECT created,debit,credit,balance,description FROM dist_account_statements WHERE acc_stat_type='dw_coins'  AND dist_id='"+ DL_login_user_idn + "' order by id desc";
                dr = com.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        DateTime created1 = (DateTime)dr["created"];
                        string Event_time = created1.ToString("yyyy-MM-dd HH:mm:ss");
                        Double debit = (Double)dr["debit"];
                        string desc = (string)dr["description"];
                        Double credit = (Double)dr["credit"];
                        Double balance = (Double)dr["balance"];
                        DL_UserBetList.Add(item: new AccountStatement_DL
                        {
                            time = Event_time,
                            Desc = desc,
                            Balance = balance,
                            Remark = "",
                            Deposit = debit,
                            Withdraw = credit
                        });
                    }
                  
                }
                con2.Close();
            }
            catch (Exception ex)
            {

            }
            return DL_UserBetList;
        }


    }
}