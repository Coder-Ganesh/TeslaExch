using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
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
    public class SettlingController : Controller
    {
        // GET: Settling
        public ActionResult Index()
        {
            DateTime time = DateTime.Now;              // Use current time
            string format = "yyyy-MM-dd HH:mm:ss";
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    SqlCommand sqlmatch = new SqlCommand("SELECT DISTINCT [event_id] FROM live_bet WHERE status='' AND odds_type='MO' ", con);
                    var reader_match = sqlmatch.ExecuteReader();
                    while (reader_match.Read())
                    {
                        string event_code = (string)reader_match["event_id"];
                        string match_title = event_code;
                        SqlCommand sqlmarket = new SqlCommand("SELECT [betfair_id],[market_name] FROM markets WHERE sport_id IN ('4','2','1') AND league_id NOT IN('88','99887766') AND type='MO' AND event_code='" + event_code + "' AND status='activate' AND result_value='' ", con);
                        var reader_market = sqlmarket.ExecuteReader();
                        if (reader_market.HasRows)
                        {
                            while (reader_market.Read())
                            {
                                string market_name = (string)reader_market["market_name"];
                                string betfair_id = (string)reader_market["betfair_id"];
                                try
                                {
                                    HttpClient client = new HttpClient();
                                    client.BaseAddress = new Uri("https://ero.betfair.com/www/sports/exchange/readonly/v1/bymarket");
                                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                    try
                                    {
                                        HttpResponseMessage response = client.GetAsync("?_ak=nzIFcwyWhrlwYMrh&alt=json&marketIds=" + betfair_id + "&types=MARKET_STATE,RUNNER_STATE").Result;
                                        if (response != null)
                                        {
                                            var products = response.Content.ReadAsStringAsync().Result;
                                            dynamic responseJson = JsonConvert.DeserializeObject(products);

                                            string match_status = responseJson.eventTypes[0].eventNodes[0].marketNodes[0].state.status;

                                            int SelectMatchWinner = 0;
                                            if (match_status == "CLOSED")
                                            {
                                                var runners = responseJson.eventTypes[0].eventNodes[0].marketNodes[0].runners;
                                                for (int i = 0; i < runners.Count; i++)
                                                {
                                                    if (runners[i].state.status == "WINNER")
                                                    {
                                                        SelectMatchWinner = runners[i].state.sortPriority;
                                                        string message = RunnerSettle(event_code, betfair_id, SelectMatchWinner, market_name);
                                                        ViewBag.message = message;
                                                        ViewBag.match_title = match_title;
                                                        return View();
                                                    }
                                                }
                                                if (runners[0].state.status == "REMOVED" && runners[1].state.status == "REMOVED")
                                                {
                                                    SelectMatchWinner = 9999;
                                                    string message = RunnerSettleTie(event_code, betfair_id, SelectMatchWinner, market_name);
                                                    ViewBag.message = message;
                                                    ViewBag.match_title = match_title;
                                                    return View();
                                                }
                                            }
                                            else
                                            {

                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ViewBag.message = ex.ToString();
                                        ViewBag.match_title = "";
                                    }
                                }
                                catch (TaskCanceledException ex)
                                {
                                    ViewBag.message = ex.ToString();
                                    ViewBag.match_title = "";
                                }
                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.ToString();
                ViewBag.match_title = "";
            }

            return View();
        }

        public ActionResult IndexXX()
        {
            DateTime time = DateTime.Now;              // Use current time
            string format = "yyyy-MM-dd HH:mm:ss";

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    SqlCommand sqlmatch = new SqlCommand("SELECT DISTINCT [event_id] FROM live_bet WHERE status='' AND odds_type='MO' ", con);
                    var reader_match = sqlmatch.ExecuteReader();
                    while (reader_match.Read())
                    {
                        string event_code = (string)reader_match["event_id"];
                        string match_title = event_code;
                        SqlCommand sqlmarket = new SqlCommand("SELECT [betfair_id],[market_name] FROM markets WHERE sport_id NOT IN('508','66') AND type='MO' AND event_code='" + event_code + "' AND status='activate' AND result_value='' ", con);
                        var reader_market = sqlmarket.ExecuteReader();
                        while (reader_market.Read())
                        {
                            string market_name = (string)reader_market["market_name"];
                            string betfair_id = (string)reader_market["betfair_id"];
                            try
                            {
                                HttpClient client = new HttpClient();
                                client.BaseAddress = new Uri("https://ero.betfair.com/www/sports/exchange/readonly/v1/bymarket");
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                try
                                {
                                    HttpResponseMessage response = client.GetAsync("?_ak=nzIFcwyWhrlwYMrh&alt=json&marketIds=" + betfair_id + "&types=MARKET_STATE,RUNNER_STATE").Result;
                                    if (response != null)
                                    {
                                        var products = response.Content.ReadAsStringAsync().Result;
                                        dynamic responseJson = JsonConvert.DeserializeObject(products);

                                        string match_status = responseJson.eventTypes[0].eventNodes[0].marketNodes[0].state.status;

                                        int SelectMatchWinner = 0;
                                        if (match_status == "CLOSED")
                                        {
                                            var runners = responseJson.eventTypes[0].eventNodes[0].marketNodes[0].runners;
                                            for (int i = 0; i < runners.Count; i++)
                                            {
                                                if (runners[i].state.status == "WINNER")
                                                {
                                                    SelectMatchWinner = runners[i].state.sortPriority;
                                                    string message = RunnerSettle(event_code, betfair_id, SelectMatchWinner, market_name);
                                                    ViewBag.message = message;
                                                    ViewBag.match_title = match_title;
                                                    return View();
                                                }
                                            }
                                            if (runners[0].state.status == "REMOVED" && runners[1].state.status == "REMOVED")
                                            {
                                                SelectMatchWinner = 9999;
                                                string message = RunnerSettleTie(event_code, betfair_id, SelectMatchWinner, market_name);
                                                ViewBag.message = message;
                                                ViewBag.match_title = match_title;
                                                return View();
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ViewBag.message = ex.ToString();
                                    ViewBag.match_title = "";
                                }
                            }
                            catch (TaskCanceledException ex)
                            {
                                ViewBag.message = ex.ToString();
                                ViewBag.match_title = "";
                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.ToString();
                ViewBag.match_title = "";
            }

            return View();
        }
        public string RunnerSettle(string event_code, string betfair_id, int SelectMatchWinneri, string market_name)
        {

            string resultreturn = "Equal12";
            try
            {
                DateTime time = DateTime.Now;              // Use current time
                string format = "yyyy-MM-dd HH:mm:ss";
                int runner_posi_win = SelectMatchWinneri - 1;
                string SelectMatchWinner = SelectMatchWinneri.ToString();
                string eventSportName = FunctionDataController.GetSportsNameFromEvent(event_code);
                string EventName = FunctionDataController.getEventName(event_code);

                if (event_code != "" && betfair_id != "" && SelectMatchWinneri != 0 && market_name != "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        SqlCommand sqlMarketUpdate = new SqlCommand("UPDATE markets SET status='deactivate' WHERE status='activate' AND market_settle!='1' AND betfair_id = '" + betfair_id + "' ", con);
                        int sqlMarketUpdatedone = sqlMarketUpdate.ExecuteNonQuery();
                        if (sqlMarketUpdatedone > 0)
                        {
                            SqlCommand sqlrunner_cal = new SqlCommand("SELECT id FROM runner_cal WHERE market_code='" + betfair_id + "'  AND event_code = '" + event_code + "' ", con);
                            var reader_runner_cal = sqlrunner_cal.ExecuteReader();
                            if (!reader_runner_cal.HasRows)
                            {
                                SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='1' , result_value='" + SelectMatchWinner + "' WHERE status='deactivate' AND result_value='' AND market_settle!='1' AND betfair_id = '" + betfair_id + "' ", con);
                                int sqlMarketSettleUpdatedone = sqlMarketSettleUpdate.ExecuteNonQuery();
                                if (sqlMarketSettleUpdatedone > 0)
                                {
                                    /*string stmtcli = "SELECT COUNT(id) FROM live_bet where event_id = '" + event_code + "' AND status='' ";
                                    int countinplaycli = 0;
                                    using (SqlCommand cmdCountcli = new SqlCommand(stmtcli, con))
                                    {
                                        countinplaycli = (int)cmdCountcli.ExecuteScalar();
                                        if (countinplaycli == 0)
                                        {
                                            SqlCommand sqlLivematch = new SqlCommand("UPDATE matches SET status='CLOSED' WHERE event_code='" + event_code + "' ", con);
                                            sqlLivematch.ExecuteNonQuery();
                                        }
                                    }*/
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

                                SqlCommand sqlrunner_cal_main = new SqlCommand("SELECT DISTINCT (user_id) FROM runner_cal WHERE market_code='" + betfair_id + "' AND is_result='0' AND event_code = '" + event_code + "' ", con);
                                var reader_runner_cal_main = sqlrunner_cal_main.ExecuteReader();
                                if (reader_runner_cal_main.HasRows)
                                {
                                    while (reader_runner_cal_main.Read())
                                    {
                                        int user_id = (Int32)reader_runner_cal_main["user_id"];

                                        SqlCommand sqlUser2 = new SqlCommand("SELECT [balance],[exposure],[profit_loss],[admin_id],[mdl_id],[dl_id],[ocomm_rate] FROM users_client WHERE id='" + user_id + "' ", con);
                                        var dataUser2 = sqlUser2.ExecuteReader();
                                        dataUser2.Read();
                                        Double UserBalance = (Double)dataUser2["balance"];
                                        Double UserLiability = (Double)dataUser2["exposure"];
                                        Double ClientPL = (Double)dataUser2["profit_loss"];
                                        int Admin_id = (int)dataUser2["admin_id"];
                                        int UserMDId = (int)dataUser2["mdl_id"];
                                        int UserDistId = (int)dataUser2["dl_id"];
                                        Double comm_rate = 0;//(Double)dataUser2["ocomm_rate"];
                                        Double pcomm_rate = Math.Abs(comm_rate);

                                        SqlCommand sqlwme = new SqlCommand("SELECT [amount] FROM runner_cal WHERE market_code='" + betfair_id + "' AND user_id='" + user_id + "' AND is_result='0' ", con);
                                        var rowwme = sqlwme.ExecuteReader();
                                        var calamt = new List<Double>();
                                        while (rowwme.Read())
                                        {
                                            Double new_cal_amount = (Double)rowwme["amount"];
                                            new_cal_amount = Math.Round(new_cal_amount, 2);
                                            calamt.Add(new_cal_amount);
                                        }
                                        Double mincalamt = calamt.Min();
                                        Double mincalamt1 = Math.Abs(mincalamt);
                                        Double maximum_minus_value = 0;
                                        if (mincalamt >= 0)
                                        {
                                            maximum_minus_value = 0;
                                        }
                                        else
                                        {
                                            maximum_minus_value = mincalamt;
                                        }
                                        Double runner_cal = calamt[runner_posi_win];
                                        Double debit_balance = 0;
                                        Double credit_balance = 0;
                                        if (runner_cal < 0)
                                        {
                                            debit_balance = Math.Abs(runner_cal);
                                            credit_balance = 0;
                                        }
                                        else
                                        {
                                            debit_balance = 0;
                                            credit_balance = runner_cal;
                                        }
                                        Double commission = 0;
                                        Double dpcomm = 0;
                                        Double cpcomm = 0;
                                        string commdesc = "";
                                        if (comm_rate > 0)
                                        {
                                            Double comm = (credit_balance * comm_rate) / 100;
                                            commission = Math.Round(comm, 3);
                                            dpcomm = Math.Abs(commission);
                                            cpcomm = 0;
                                            commdesc = pcomm_rate + "% Commission Charged";
                                        }
                                        else if (comm_rate < 0)
                                        {
                                            Double comm = (debit_balance * comm_rate) / 100;
                                            commission = Math.Round(comm, 3);
                                            dpcomm = 0;
                                            cpcomm = Math.Abs(commission);
                                            commdesc = pcomm_rate + "% Commission Credited";
                                        }
                                        else
                                        {
                                            commission = 0;
                                            dpcomm = 0;
                                            cpcomm = 0;
                                            commdesc = pcomm_rate + "% Commission ";
                                        }

                                        Double user_net_balance = UserBalance + runner_cal - maximum_minus_value;
                                        Double updated_user_net_balance = user_net_balance - commission;
                                        Double updated_liability = UserLiability + maximum_minus_value;
                                        Double user_updated_net_balance = user_net_balance + updated_liability;
                                        Double after_comm_user_updated_net_balance = user_updated_net_balance - commission;
                                        Double NetWinLoss = credit_balance - debit_balance - commission;
                                        Double NetClientPL = ClientPL + NetWinLoss;
                                        Double adjust_bal = 0 + runner_cal - maximum_minus_value - commission;
                                        Double adjust_exp = 0 + maximum_minus_value;
                                        Double adjust_pl = NetWinLoss;

                                        SqlCommand sqlUserBalanceUpdate = new SqlCommand("UPDATE users_client SET balance= balance + '" + adjust_bal + "' , exposure= exposure + '" + adjust_exp + "' , profit_loss= profit_loss + '" + adjust_pl + "' WHERE id = '" + user_id + "' ", con);
                                        sqlUserBalanceUpdate.ExecuteNonQuery();

                                        SqlCommand sqlWinMatchExchangeUpdate = new SqlCommand("UPDATE runner_cal SET is_result='1' WHERE market_code='" + betfair_id + "' AND user_id='" + user_id + "' AND is_result='0' ", con);
                                        sqlWinMatchExchangeUpdate.ExecuteNonQuery();

                                        SqlCommand sqlLiveBetWon = new SqlCommand("UPDATE live_bet SET status='won' , settled_time='" + time.ToString(format) + "' WHERE status='' AND user_id='" + user_id + "' AND betfair_id='" + betfair_id + "' AND ((runner_posi='" + SelectMatchWinneri + "' AND field_pos = 'back') OR (runner_posi!='" + SelectMatchWinneri + "' AND field_pos = 'lay')) ", con);
                                        sqlLiveBetWon.ExecuteNonQuery();

                                        SqlCommand sqlLiveBetLost = new SqlCommand("UPDATE live_bet SET status='lost' , settled_time='" + time.ToString(format) + "' WHERE status='' AND user_id='" + user_id + "' AND betfair_id='" + betfair_id + "' AND ((runner_posi='" + SelectMatchWinneri + "' AND field_pos = 'lay') OR (runner_posi!='" + SelectMatchWinneri + "' AND field_pos = 'back')) ", con);
                                        sqlLiveBetLost.ExecuteNonQuery();

                                        string AccStatementDescription = eventSportName + " / " + EventName + " / " + market_name;

                                        SqlCommand sqlInsertUser = new SqlCommand("INSERT INTO user_account_statements (admin_id , md_id , dist_id , user_id , acc_stat_type ,  description ,  remark ,  debit ,  credit ,  balance ,  market_id ,  event_id ,  created , match_odds ) VALUES ('" + Admin_id + "','" + UserMDId + "','" + UserDistId + "','" + user_id + "','pl_coins','" + AccStatementDescription + "','','" + debit_balance + "','" + credit_balance + "','" + user_updated_net_balance + "','" + betfair_id + "','" + event_code + "','" + time.ToString(format) + "','1')", con);
                                        sqlInsertUser.ExecuteNonQuery();

                                        if (commission != 0)
                                        {
                                            string commissionDescription = commdesc + " (" + AccStatementDescription + ")";
                                            SqlCommand sqlInsertUserC = new SqlCommand("INSERT INTO user_account_statements (admin_id , md_id , dist_id , user_id , acc_stat_type ,  description ,  remark ,  debit ,  credit ,  balance ,  market_id ,  event_id ,  created , match_odds ) VALUES ('" + Admin_id + "','" + UserMDId + "','" + UserDistId + "','" + user_id + "','comm','" + commissionDescription + "','','" + dpcomm + "','" + cpcomm + "','" + after_comm_user_updated_net_balance + "','" + betfair_id + "','" + event_code + "','" + time.ToString(format) + "','1')", con);
                                            sqlInsertUserC.ExecuteNonQuery();
                                        }
                                        else
                                        {

                                        }

                                    }
                                    SqlCommand sqlMarketSettleUpdatefinal = new SqlCommand("UPDATE markets SET market_settle='1' , result_value='" + SelectMatchWinner + "' WHERE status='deactivate' AND result_value='' AND market_settle!='1' AND betfair_id = '" + betfair_id + "' ", con);
                                    sqlMarketSettleUpdatefinal.ExecuteNonQuery();

                                    /*string stmtcli = "SELECT COUNT(id) FROM live_bet where event_id = '"+event_code+"' AND status='activate' ";
                                    int countinplaycli = 0;
                                    using (SqlCommand cmdCountcli = new SqlCommand(stmtcli, con))
                                    {
                                        countinplaycli = (int)cmdCountcli.ExecuteScalar();
                                        if(countinplaycli == 0)
                                        {
                                            SqlCommand sqlLivematch = new SqlCommand("UPDATE matches SET status='CLOSED' WHERE event_code='" + event_code + "' ", con);
                                            sqlLivematch.ExecuteNonQuery();
                                        }
                                    }*/
                                    con.Close();
                                    return "Market Settle SuccessFully";
                                }
                                else
                                {
                                    con.Close();
                                    return "Error IN jh";
                                }
                            }
                        }
                        else
                        {
                            con.Close();
                            return "Market Already Settled";
                        }
                        con.Close();
                    }
                }
                else
                {
                    return "Data Missing";
                }
            }
            catch (Exception ex)
            {
                resultreturn = ex.ToString();
            }
            return resultreturn;
        }

        public string MORevert(string event_code, string betfair_id, int SelectMatchWinneri, string market_name)
        {

            string resultreturn = "Equal12";
            try
            {
                DateTime time = DateTime.Now;              // Use current time
                string format = "yyyy-MM-dd HH:mm:ss";
                int runner_posi_win = SelectMatchWinneri - 1;
                string SelectMatchWinner = SelectMatchWinneri.ToString();
                string eventSportName = FunctionDataController.GetSportsNameFromEvent(event_code);
                string EventName = FunctionDataController.getEventName(event_code);

                if (event_code != "" && betfair_id != "" && SelectMatchWinneri != 0 && market_name != "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        SqlCommand sqlMarketUpdate = new SqlCommand("UPDATE markets SET status='activate' WHERE status='deactivate' AND market_settle='1' AND betfair_id = '" + betfair_id + "' ", con);
                        int sqlMarketUpdatedone = sqlMarketUpdate.ExecuteNonQuery();
                        if (sqlMarketUpdatedone > 0)
                        {
                            SqlCommand sqlrunner_cal = new SqlCommand("SELECT id FROM runner_cal WHERE market_code='" + betfair_id + "'  AND event_code = '" + event_code + "' ", con);
                            var reader_runner_cal = sqlrunner_cal.ExecuteReader();
                            if (!reader_runner_cal.HasRows)
                            {
                                SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='0' , result_value='' WHERE status='activate' AND result_value!='' AND market_settle='1' AND betfair_id = '" + betfair_id + "' ", con);
                                int sqlMarketSettleUpdatedone = sqlMarketSettleUpdate.ExecuteNonQuery();
                                if (sqlMarketSettleUpdatedone > 0)
                                {
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

                                SqlCommand sqlrunner_cal_main = new SqlCommand("SELECT DISTINCT (user_id) FROM runner_cal WHERE market_code='" + betfair_id + "' AND is_result='1' AND event_code = '" + event_code + "' ", con);
                                var reader_runner_cal_main = sqlrunner_cal_main.ExecuteReader();
                                if (reader_runner_cal_main.HasRows)
                                {
                                    while (reader_runner_cal_main.Read())
                                    {
                                        int user_id = (Int32)reader_runner_cal_main["user_id"];

                                        SqlCommand sqlUser2 = new SqlCommand("SELECT [balance],[exposure],[profit_loss],[admin_id],[mdl_id],[dl_id],[ocomm_rate] FROM users_client WHERE id='" + user_id + "' ", con);
                                        var dataUser2 = sqlUser2.ExecuteReader();
                                        dataUser2.Read();
                                        Double UserBalance = (Double)dataUser2["balance"];
                                        Double UserLiability = (Double)dataUser2["exposure"];
                                        Double ClientPL = (Double)dataUser2["profit_loss"];
                                        int Admin_id = (int)dataUser2["admin_id"];
                                        int UserMDId = (int)dataUser2["mdl_id"];
                                        int UserDistId = (int)dataUser2["dl_id"];
                                        Double comm_rate = 0;//(Double)dataUser2["ocomm_rate"];
                                        Double pcomm_rate = Math.Abs(comm_rate);

                                        SqlCommand sqlwme = new SqlCommand("SELECT [amount] FROM runner_cal WHERE market_code='" + betfair_id + "' AND user_id='" + user_id + "' AND is_result='1' ", con);
                                        var rowwme = sqlwme.ExecuteReader();
                                        var calamt = new List<Double>();
                                        while (rowwme.Read())
                                        {
                                            Double new_cal_amount = (Double)rowwme["amount"];
                                            new_cal_amount = Math.Round(new_cal_amount, 2);
                                            calamt.Add(new_cal_amount);
                                        }
                                        Double mincalamt = calamt.Min();
                                        Double mincalamt1 = Math.Abs(mincalamt);
                                        Double maximum_minus_value = 0;
                                        if (mincalamt >= 0)
                                        {
                                            maximum_minus_value = 0;
                                        }
                                        else
                                        {
                                            maximum_minus_value = mincalamt;
                                        }
                                        Double runner_cal = calamt[runner_posi_win];
                                        Double debit_balance = 0;
                                        Double credit_balance = 0;
                                        if (runner_cal < 0)
                                        {
                                            debit_balance = 0;
                                            credit_balance = Math.Abs(runner_cal); ;
                                        }
                                        else
                                        {
                                            debit_balance = runner_cal;
                                            credit_balance = 0;
                                        }
                                        Double commission = 0;
                                        Double dpcomm = 0;
                                        Double cpcomm = 0;
                                        string commdesc = "";
                                        if (comm_rate > 0)
                                        {
                                            Double comm = (credit_balance * comm_rate) / 100;
                                            commission = Math.Round(comm, 3);
                                            dpcomm = Math.Abs(commission);
                                            cpcomm = 0;
                                            commdesc = pcomm_rate + "% Commission Charged";
                                        }
                                        else if (comm_rate < 0)
                                        {
                                            Double comm = (debit_balance * comm_rate) / 100;
                                            commission = Math.Round(comm, 3);
                                            dpcomm = 0;
                                            cpcomm = Math.Abs(commission);
                                            commdesc = pcomm_rate + "% Commission Credited";
                                        }
                                        else
                                        {
                                            commission = 0;
                                            dpcomm = 0;
                                            cpcomm = 0;
                                            commdesc = pcomm_rate + "% Commission ";
                                        }

                                        Double user_net_balance = UserBalance - runner_cal + maximum_minus_value;
                                        Double updated_user_net_balance = user_net_balance + commission;
                                        Double updated_liability = UserLiability - maximum_minus_value;
                                        Double user_updated_net_balance = user_net_balance + updated_liability;
                                        Double after_comm_user_updated_net_balance = user_updated_net_balance + commission;
                                        Double NetWinLoss = credit_balance - debit_balance + commission;
                                        Double NetClientPL = ClientPL + NetWinLoss;
                                        Double adjust_bal = 0 - runner_cal + maximum_minus_value + commission;
                                        Double adjust_exp = 0 - maximum_minus_value;
                                        Double adjust_pl = NetWinLoss;

                                        SqlCommand sqlUserBalanceUpdate = new SqlCommand("UPDATE users_client SET balance= balance + '" + adjust_bal + "' , exposure= exposure + '" + adjust_exp + "' , profit_loss= profit_loss + '" + adjust_pl + "' WHERE id = '" + user_id + "' ", con);
                                        sqlUserBalanceUpdate.ExecuteNonQuery();

                                        SqlCommand sqlWinMatchExchangeUpdate = new SqlCommand("UPDATE runner_cal SET is_result='0' WHERE market_code='" + betfair_id + "' AND user_id='" + user_id + "' AND is_result='1' ", con);
                                        sqlWinMatchExchangeUpdate.ExecuteNonQuery();

                                        SqlCommand sqlLiveBetWon = new SqlCommand("UPDATE live_bet SET status='' , settled_time='" + time.ToString(format) + "' WHERE status!='' AND user_id='" + user_id + "' AND betfair_id='" + betfair_id + "' ", con);
                                        sqlLiveBetWon.ExecuteNonQuery();

                                        string AccStatementDescription = eventSportName + " / " + EventName + " / " + market_name + " [Settling Reverted]";

                                        SqlCommand sqlInsertUser = new SqlCommand("INSERT INTO user_account_statements (admin_id , md_id , dist_id , user_id , acc_stat_type ,  description ,  remark ,  debit ,  credit ,  balance ,  market_id ,  event_id ,  created , match_odds ) VALUES ('" + Admin_id + "','" + UserMDId + "','" + UserDistId + "','" + user_id + "','pl_coins','" + AccStatementDescription + "','','" + debit_balance + "','" + credit_balance + "','" + user_updated_net_balance + "','" + betfair_id + "','" + event_code + "','" + time.ToString(format) + "','1')", con);
                                        sqlInsertUser.ExecuteNonQuery();

                                        if (commission != 0)
                                        {
                                            string commissionDescription = commdesc + " (" + AccStatementDescription + ")";
                                            SqlCommand sqlInsertUserC = new SqlCommand("INSERT INTO user_account_statements (admin_id , md_id , dist_id , user_id , acc_stat_type ,  description ,  remark ,  debit ,  credit ,  balance ,  market_id ,  event_id ,  created , match_odds ) VALUES ('" + Admin_id + "','" + UserMDId + "','" + UserDistId + "','" + user_id + "','comm','" + commissionDescription + "','','" + dpcomm + "','" + cpcomm + "','" + after_comm_user_updated_net_balance + "','" + betfair_id + "','" + event_code + "','" + time.ToString(format) + "','1')", con);
                                            sqlInsertUserC.ExecuteNonQuery();
                                        }
                                        else
                                        {

                                        }

                                    }
                                    SqlCommand sqlMarketSettleUpdatefinal = new SqlCommand("UPDATE markets SET market_settle='' , result_value='' WHERE status='activate' AND result_value!='' AND market_settle='1' AND betfair_id = '" + betfair_id + "' ", con);
                                    sqlMarketSettleUpdatefinal.ExecuteNonQuery();

                                    /*string stmtcli = "SELECT COUNT(id) FROM live_bet where event_id = '"+event_code+"' AND status='activate' ";
                                    int countinplaycli = 0;
                                    using (SqlCommand cmdCountcli = new SqlCommand(stmtcli, con))
                                    {
                                        countinplaycli = (int)cmdCountcli.ExecuteScalar();
                                        if(countinplaycli == 0)
                                        {
                                            SqlCommand sqlLivematch = new SqlCommand("UPDATE matches SET status='CLOSED' WHERE event_code='" + event_code + "' ", con);
                                            sqlLivematch.ExecuteNonQuery();
                                        }
                                    }*/
                                    con.Close();
                                    return "Market Revert SuccessFully";
                                }
                                else
                                {
                                    con.Close();
                                    return "Error IN jh";
                                }
                            }
                        }
                        else
                        {
                            con.Close();
                            return "Market Already Settled";
                        }
                        con.Close();
                    }
                }
                else
                {
                    return "Data Missing";
                }
            }
            catch (Exception ex)
            {
                resultreturn = ex.ToString();
            }
            return resultreturn;
        }

        public string RunnerSettleTie(string event_code, string betfair_id, int SelectMatchWinneri, string market_name)
        {
            DateTime time = DateTime.Now;              // Use current time
            string format = "yyyy-MM-dd HH:mm:ss";
            string resultreturn = "Equal12";
            try
            {
                string eventSportName = FunctionDataController.GetSportsNameFromEvent(event_code);
                string EventName = FunctionDataController.getEventName(event_code);

                if (event_code != "" && betfair_id != "" && SelectMatchWinneri == 9999 && market_name != "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        SqlCommand sqlMarketUpdate = new SqlCommand("UPDATE markets SET status='deactivate' WHERE status='activate' AND market_settle!='1' AND betfair_id = '" + betfair_id + "' ", con);
                        int sqlMarketUpdatedone = sqlMarketUpdate.ExecuteNonQuery();
                        if (sqlMarketUpdatedone > 0)
                        {
                            SqlCommand sqlrunner_cal = new SqlCommand("SELECT id FROM runner_cal WHERE market_code='" + betfair_id + "'  AND event_code = '" + event_code + "' ", con);
                            var reader_runner_cal = sqlrunner_cal.ExecuteReader();
                            if (!reader_runner_cal.HasRows)
                            {
                                SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='1' , result_value='No Result' WHERE status='deactivate' AND result_value='' AND market_settle!='1' AND betfair_id = '" + betfair_id + "' ", con);
                                int sqlMarketSettleUpdatedone = sqlMarketSettleUpdate.ExecuteNonQuery();
                                if (sqlMarketSettleUpdatedone > 0)
                                {
                                    /*string stmtcli = "SELECT COUNT(id) FROM live_bet where event_id = '" + event_code + "' AND status='' ";
                                    int countinplaycli = 0;
                                    using (SqlCommand cmdCountcli = new SqlCommand(stmtcli, con))
                                    {
                                        countinplaycli = (int)cmdCountcli.ExecuteScalar();
                                        if (countinplaycli == 0)
                                        {
                                            SqlCommand sqlLivematch = new SqlCommand("UPDATE matches SET status='CLOSED' WHERE event_code='" + event_code + "' ", con);
                                            sqlLivematch.ExecuteNonQuery();
                                        }
                                    }*/
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

                                SqlCommand sqlrunner_cal_main = new SqlCommand("SELECT DISTINCT (user_id) FROM runner_cal WHERE market_code='" + betfair_id + "' AND is_result='0' AND event_code = '" + event_code + "' ", con);
                                var reader_runner_cal_main = sqlrunner_cal_main.ExecuteReader();
                                if (reader_runner_cal_main.HasRows)
                                {
                                    while (reader_runner_cal_main.Read())
                                    {
                                        int user_id = (Int32)reader_runner_cal_main["user_id"];

                                        SqlCommand sqlUser2 = new SqlCommand("SELECT [balance],[exposure],[profit_loss],[admin_id],[mdl_id],[dl_id],[ocomm_rate] FROM users_client WHERE id='" + user_id + "' ", con);
                                        var dataUser2 = sqlUser2.ExecuteReader();
                                        dataUser2.Read();
                                        Double UserBalance = (Double)dataUser2["balance"];
                                        Double UserLiability = (Double)dataUser2["exposure"];
                                        int Admin_id = (int)dataUser2["admin_id"];
                                        int UserMDId = (int)dataUser2["mdl_id"];
                                        int UserDistId = (int)dataUser2["dl_id"];

                                        SqlCommand sqlwme = new SqlCommand("SELECT [amount] FROM runner_cal WHERE market_code='" + betfair_id + "' AND user_id='" + user_id + "' AND is_result='0' ", con);
                                        var rowwme = sqlwme.ExecuteReader();
                                        var calamt = new List<Double>();
                                        while (rowwme.Read())
                                        {
                                            Double new_cal_amount = (Double)rowwme["amount"];
                                            new_cal_amount = Math.Round(new_cal_amount, 2);
                                            calamt.Add(new_cal_amount);
                                        }
                                        Double mincalamt = calamt.Min();
                                        Double mincalamt1 = Math.Abs(mincalamt);
                                        Double maximum_minus_value = 0;
                                        if (mincalamt >= 0)
                                        {
                                            maximum_minus_value = 0;
                                        }
                                        else
                                        {
                                            maximum_minus_value = mincalamt;
                                        }
                                        Double debit_balance = 0;
                                        Double credit_balance = 0;

                                        Double user_net_balance = UserBalance - maximum_minus_value;
                                        Double updated_liability = UserLiability + maximum_minus_value;
                                        Double user_updated_net_balance = user_net_balance + updated_liability;
                                        Double adjust_bal = 0 - maximum_minus_value;
                                        Double adjust_exp = 0 + maximum_minus_value;

                                        SqlCommand sqlUserBalanceUpdate = new SqlCommand("UPDATE users_client SET balance= balance + '" + adjust_bal + "' , exposure= exposure + '" + adjust_exp + "' WHERE id = '" + user_id + "' ", con);
                                        sqlUserBalanceUpdate.ExecuteNonQuery();

                                        SqlCommand sqlWinMatchExchangeUpdate = new SqlCommand("UPDATE runner_cal SET is_result='1' WHERE market_code='" + betfair_id + "' AND user_id='" + user_id + "' AND is_result='0' ", con);
                                        sqlWinMatchExchangeUpdate.ExecuteNonQuery();

                                        SqlCommand sqlLiveBetWon = new SqlCommand("UPDATE live_bet SET status='tie' , settled_time='" + time.ToString(format) + "' WHERE status='' AND user_id='" + user_id + "' AND betfair_id='" + betfair_id + "' ", con);
                                        sqlLiveBetWon.ExecuteNonQuery();

                                        string AccStatementDescription = eventSportName + " / " + EventName + " / " + market_name + "[No Result]";

                                        SqlCommand sqlInsertUser = new SqlCommand("INSERT INTO user_account_statements (admin_id , md_id , dist_id , user_id , acc_stat_type ,  description ,  remark ,  debit ,  credit ,  balance ,  market_id ,  event_id ,  created , match_odds ) VALUES ('" + Admin_id + "','" + UserMDId + "','" + UserDistId + "','" + user_id + "','pl_coins','" + AccStatementDescription + "','','" + debit_balance + "','" + credit_balance + "','" + user_updated_net_balance + "','" + betfair_id + "','" + event_code + "','" + time.ToString(format) + "','1')", con);
                                        sqlInsertUser.ExecuteNonQuery();

                                    }
                                    SqlCommand sqlMarketSettleUpdatefinal = new SqlCommand("UPDATE markets SET market_settle='1' , result_value='No Result' WHERE status='deactivate' AND result_value='' AND market_settle!='1' AND betfair_id = '" + betfair_id + "' ", con);
                                    sqlMarketSettleUpdatefinal.ExecuteNonQuery();

                                    /*string stmtcli = "SELECT COUNT(id) FROM live_bet where event_id = '"+event_code+"' AND status='activate' ";
                                    int countinplaycli = 0;
                                    using (SqlCommand cmdCountcli = new SqlCommand(stmtcli, con))
                                    {
                                        countinplaycli = (int)cmdCountcli.ExecuteScalar();
                                        if(countinplaycli == 0)
                                        {
                                            SqlCommand sqlLivematch = new SqlCommand("UPDATE matches SET status='CLOSED' WHERE event_code='" + event_code + "' ", con);
                                            sqlLivematch.ExecuteNonQuery();
                                        }
                                    }*/

                                    con.Close();
                                    return "Market Settle SuccessFully";
                                }
                                else
                                {
                                    con.Close();
                                    return "Error IN jh";
                                }
                            }
                        }
                        else
                        {
                            con.Close();
                            return "Market Already Settled";
                        }
                        con.Close();
                    }
                }
                else
                {
                    return "Data Missing";
                }
            }
            catch (Exception ex)
            {
                resultreturn = ex.ToString();
            }
            return resultreturn;
        }

        public ActionResult AutoSession()
        {
            DateTime time = DateTime.Now;              // Use current time
            string format = "yyyy-MM-dd HH:mm:ss";
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    SqlCommand sqlmatch = new SqlCommand("SELECT [event_code],[match_title] FROM matches WHERE is_fancy='1' AND betfair_id != '0' AND teama != 'nikhil' AND status='OPEN' AND match_time<'" + time.ToString(format) + "' ", con);
                    var reader_match = sqlmatch.ExecuteReader();
                    while (reader_match.Read())
                    {
                        string event_code = (string)reader_match["event_code"];
                        string match_title = (string)reader_match["match_title"];
                        SqlCommand sqlmarket = new SqlCommand("SELECT [betfair_id],[market_name] FROM markets WHERE event_code='" + event_code + "' AND type='sess' AND status='activate' AND result_value='' ", con);
                        var reader_market = sqlmarket.ExecuteReader();
                        while (reader_market.Read())
                        {
                            string market_name = (string)reader_market["market_name"];
                            string betfair_id = (string)reader_market["betfair_id"];
                            try
                            {
                                HttpClient client = new HttpClient();
                                client.BaseAddress = new Uri("http://api_link.com");
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                try
                                {
                                    HttpResponseMessage response = client.GetAsync("sess_result.php?event_code=" + event_code + "&sess_code=" + betfair_id).Result;
                                    if (response != null)
                                    {
                                        //string products = response.Content.ReadAsStringAsync().Result;
                                        var products = response.Content.ReadAsStringAsync().Result;
                                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                                        if (responseJson != null)
                                        {
                                            string sess_result = responseJson[0]["result_sess"];
                                            if (sess_result != "" && sess_result != null && sess_result != "9999" && sess_result != "Tie" && sess_result != "No Result")
                                            {
                                                int winnerresult = int.Parse(sess_result);
                                                string message = AutoSessionSettle(event_code, betfair_id, winnerresult, market_name);
                                                ViewBag.message = message;
                                                ViewBag.match_title = match_title + " [" + market_name + "] [" + winnerresult + "]";
                                                return View();
                                            }
                                            else
                                            {
                                                if (sess_result == "Tie" || sess_result == "No Result")
                                                {
                                                    int winnerresult = 9999;
                                                    string message = AutoSessionSettleTie(event_code, betfair_id, winnerresult, market_name);
                                                    ViewBag.message = message;
                                                    ViewBag.match_title = match_title + " [" + market_name + "] [" + winnerresult + "]";
                                                    return View();
                                                }
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ViewBag.message = ex.ToString();
                                    ViewBag.match_title = "";
                                }
                            }
                            catch (TaskCanceledException ex)
                            {
                                ViewBag.message = ex.ToString();
                                ViewBag.match_title = "";
                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.ToString();
                ViewBag.match_title = "";
            }
            return View();
        }
        public ActionResult AutoSessionGS()
        {
            DateTime time = DateTime.Now;              // Use current time
            string format = "yyyy-MM-dd HH:mm:ss";
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    SqlCommand sqlmatch = new SqlCommand("SELECT [event_code],[match_title] FROM matches WHERE sport_id='508' AND betfair_id != '0' AND teama != 'nikhil' AND status='OPEN' AND match_time<'" + time.ToString(format) + "' ", con);
                    var reader_match = sqlmatch.ExecuteReader();
                    while (reader_match.Read())
                    {
                        string event_code = (string)reader_match["event_code"];
                        string match_title = (string)reader_match["match_title"];
                        SqlCommand sqlmarket = new SqlCommand("SELECT [betfair_id],[market_name] FROM markets WHERE event_code='" + event_code + "' AND type='sess' AND status='activate' AND result_value='' ", con);
                        var reader_market = sqlmarket.ExecuteReader();
                        while (reader_market.Read())
                        {
                            string market_name = (string)reader_market["market_name"];
                            string betfair_id = (string)reader_market["betfair_id"];
                            try
                            {
                                HttpClient client = new HttpClient();
                                client.BaseAddress = new Uri("http://api_link.com");
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                try
                                {
                                    HttpResponseMessage response = client.GetAsync("sess_result_gs.php?event_code=" + event_code + "&sess_code=" + betfair_id).Result;
                                    if (response != null)
                                    {
                                        //string products = response.Content.ReadAsStringAsync().Result;
                                        var products = response.Content.ReadAsStringAsync().Result;
                                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                                        if (responseJson != null)
                                        {
                                            string sess_result = responseJson[0]["result_sess"];
                                            if (sess_result != "" && sess_result != null && sess_result != "9999" && sess_result != "Tie" && sess_result != "No Result")
                                            {
                                                int winnerresult = int.Parse(sess_result);
                                                string message = AutoSessionSettle(event_code, betfair_id, winnerresult, market_name);
                                                ViewBag.message = message;
                                                ViewBag.match_title = match_title + " [" + market_name + "] [" + winnerresult + "]";
                                                return View();
                                            }
                                            else
                                            {
                                                if (sess_result == "Tie" || sess_result == "No Result")
                                                {
                                                    int winnerresult = 9999;
                                                    string message = AutoSessionSettleTie(event_code, betfair_id, winnerresult, market_name);
                                                    ViewBag.message = message;
                                                    ViewBag.match_title = match_title + " [" + market_name + "] [" + winnerresult + "]";
                                                    return View();
                                                }
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ViewBag.message = ex.ToString();
                                    ViewBag.match_title = "";
                                }
                            }
                            catch (TaskCanceledException ex)
                            {
                                ViewBag.message = ex.ToString();
                                ViewBag.match_title = "";
                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.ToString();
                ViewBag.match_title = "";
            }
            return View();
        }
        public string AutoSessionSettle(string event_code, string betfair_id, int SelectMatchWinner, string market_name)
        {
            DateTime time = DateTime.Now;              // Use current time
            string format = "yyyy-MM-dd HH:mm:ss";
            string resultreturn = "Equal12";
            try
            {
                string eventSportName = FunctionDataController.GetSportsNameFromEvent(event_code);
                string EventName = FunctionDataController.getEventName(event_code);

                if (event_code != "" && betfair_id != "" && SelectMatchWinner > -1 && market_name != "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        var client_ids = new List<int>();
                        SqlCommand sqlFancy = new SqlCommand("SELECT DISTINCT user_id FROM fancy_exchange WHERE market_id = '" + betfair_id + "' AND event_id = '" + event_code + "' AND is_done='0' ", con);
                        var rowFancy = sqlFancy.ExecuteReader();
                        if (!rowFancy.HasRows)
                        {
                            SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='1' , result_value='" + SelectMatchWinner + "' WHERE status='activate' AND result_value='' AND market_settle!='1' AND betfair_id = '" + betfair_id + "' AND event_code = '" + event_code + "' ", con);
                            int sqlMarketSettleUpdatedone = sqlMarketSettleUpdate.ExecuteNonQuery();
                            if (sqlMarketSettleUpdatedone > 0)
                            {
                                /*string stmtcli = "SELECT COUNT(id) FROM live_bet where event_id = '" + event_code + "' AND status='' ";
                                int countinplaycli = 0;
                                using (SqlCommand cmdCountcli = new SqlCommand(stmtcli, con))
                                {
                                    countinplaycli = (int)cmdCountcli.ExecuteScalar();
                                    if (countinplaycli == 0)
                                    {
                                        SqlCommand sqlLivematch = new SqlCommand("UPDATE matches SET status='CLOSED' WHERE event_code='" + event_code + "' ", con);
                                        sqlLivematch.ExecuteNonQuery();
                                    }
                                }*/
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
                            string string_client_ids = "0";
                            while (rowFancy.Read())
                            {
                                int uid = (int)rowFancy["user_id"];
                                client_ids.Add(uid);
                                string_client_ids = string_client_ids + "," + uid;
                            }

                            SqlCommand marketde = new SqlCommand("UPDATE markets SET status='deactivate' WHERE status='activate' AND market_settle!='1' AND betfair_id = '" + betfair_id + "' AND event_code = '" + event_code + "' ", con);
                            int sqlMarketUpdatedone = marketde.ExecuteNonQuery();
                            if (sqlMarketUpdatedone > 0)
                            {
                                for (int i = 0; i < client_ids.Count; i++)
                                {
                                    int user_id = client_ids[i];
                                    Double total_liability = 0;
                                    Double availableLiability = 0;

                                    SqlCommand sql_new = new SqlCommand("SELECT [id] FROM live_bet_new WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_code + "' ", con);
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

                                        string StringLiveBetIdWon = "0";
                                        Double TotalPLLiveBetIdWon = 0;
                                        SqlCommand sqlWon = new SqlCommand("SELECT [total_value] , [id] FROM live_bet_new WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND user_id='" + client_ids[i] + "' AND ((field='Yes' AND rate<='" + SelectMatchWinner + "') OR (field='Not' AND rate>'" + SelectMatchWinner + "')) ", con);
                                        var dataWon = sqlWon.ExecuteReader();
                                        if (dataWon.HasRows)
                                        {
                                            while (dataWon.Read())
                                            {
                                                int won_ids = (int)dataWon["id"];
                                                StringLiveBetIdWon = StringLiveBetIdWon + "," + won_ids;
                                                TotalPLLiveBetIdWon = TotalPLLiveBetIdWon + (Double)dataWon["total_value"];
                                            }
                                        }
                                        string StringLiveBetIdLost = "0";
                                        Double TotalStakesLiveBetIdLost = 0;
                                        SqlCommand sqlLost = new SqlCommand("SELECT [stakes] , [id] FROM live_bet_new WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND user_id='" + client_ids[i] + "' AND ((field='Yes' AND rate>'" + SelectMatchWinner + "') OR (field='Not' AND rate<='" + SelectMatchWinner + "')) ", con);
                                        var dataLost = sqlLost.ExecuteReader();
                                        if (dataLost.HasRows)
                                        {
                                            while (dataLost.Read())
                                            {
                                                int lost_ids = (int)dataLost["id"];
                                                StringLiveBetIdLost = StringLiveBetIdLost + "," + lost_ids;
                                                TotalStakesLiveBetIdLost = TotalStakesLiveBetIdLost + (Double)dataLost["stakes"];
                                            }
                                        }

                                        SqlCommand sqlFancyExch = new SqlCommand("SELECT stakes FROM fancy_exchange WHERE market_id='" + betfair_id + "' AND event_id='" + event_code + "' AND user_id='" + client_ids[i] + "' AND is_done='0' ", con);
                                        var dataFancyExch = sqlFancyExch.ExecuteReader();
                                        if (dataFancyExch.HasRows)
                                        {
                                            while (dataFancyExch.Read())
                                            {
                                                total_liability = total_liability + (Double)dataFancyExch["stakes"];
                                            }
                                            availableLiability = UserLiability - total_liability;
                                        }

                                        Double NetWinLost = TotalPLLiveBetIdWon - TotalStakesLiveBetIdLost;
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

                                        Double adjust_bal = 0 + NetWinLost + total_liability;
                                        Double adjust_exp = 0 - total_liability;
                                        Double adjust_pl = NetWinLoss;

                                        SqlCommand sqlLiveBetWon = new SqlCommand("UPDATE live_bet_new SET status='won' WHERE user_id='" + user_id + "' AND betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND ((field='Yes' AND rate<='" + SelectMatchWinner + "') OR (field='Not' AND rate>'" + SelectMatchWinner + "')) ", con);
                                        sqlLiveBetWon.ExecuteNonQuery();

                                        SqlCommand sqlLiveBetLost = new SqlCommand("UPDATE live_bet_new SET status='lost' WHERE user_id='" + user_id + "' AND betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND ((field='Yes' AND rate>'" + SelectMatchWinner + "') OR (field='Not' AND rate<='" + SelectMatchWinner + "')) ", con);
                                        sqlLiveBetLost.ExecuteNonQuery();

                                        SqlCommand sqlFancyDone = new SqlCommand("UPDATE fancy_exchange SET is_done='1' WHERE user_id='" + user_id + "' AND event_id='" + event_code + "' AND market_id='" + betfair_id + "' ", con);
                                        sqlFancyDone.ExecuteNonQuery();

                                        SqlCommand sqlUserBalanceUpdate = new SqlCommand("UPDATE users_client SET balance= balance + '" + adjust_bal + "' , exposure= exposure + '" + adjust_exp + "' , profit_loss= profit_loss + '" + adjust_pl + "' WHERE id = '" + user_id + "' ", con);
                                        sqlUserBalanceUpdate.ExecuteNonQuery();

                                        string AccStatementDescription = eventSportName + " / " + EventName + " / " + market_name;

                                        SqlCommand sqlInsertUser = new SqlCommand("INSERT INTO user_account_statements (admin_id , md_id , dist_id , user_id , acc_stat_type ,  description ,  remark ,  debit ,  credit ,  balance ,  market_id ,  event_id ,  created , match_odds ) VALUES ('" + Admin_id + "','" + UserMDId + "','" + UserDistId + "','" + user_id + "','pl_coins','" + AccStatementDescription + "','','" + debit_balance + "','" + credit_balance + "','" + user_updated_net_balance + "','" + betfair_id + "','" + event_code + "','" + time.ToString(format) + "','2')", con);
                                        sqlInsertUser.ExecuteNonQuery();
                                    }
                                }
                                SqlCommand sqlBetStatusUpdate1 = new SqlCommand("UPDATE live_bet SET status='won' , settled_time='" + time.ToString(format) + "' WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND ((field='Yes' AND rate<='" + SelectMatchWinner + "') OR (field='Not' AND rate>'" + SelectMatchWinner + "')) ", con);
                                sqlBetStatusUpdate1.ExecuteNonQuery();

                                SqlCommand sqlBetStatusUpdate2 = new SqlCommand("UPDATE live_bet SET status='lost' , settled_time='" + time.ToString(format) + "' WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND ((field='Yes' AND rate>'" + SelectMatchWinner + "') OR (field='Not' AND rate<='" + SelectMatchWinner + "')) ", con);
                                sqlBetStatusUpdate2.ExecuteNonQuery();

                                SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='1' , result_value='" + SelectMatchWinner + "' WHERE status='deactivate' AND betfair_id = '" + betfair_id + "' AND event_code='" + event_code + "' AND result_value='' AND market_settle!='1'  ", con);
                                sqlMarketSettleUpdate.ExecuteNonQuery();

                                /*string stmtcli = "SELECT COUNT(id) FROM live_bet where event_id = '" + event_code + "' AND status='' ";
                                int countinplaycli = 0;
                                using (SqlCommand cmdCountcli = new SqlCommand(stmtcli, con))
                                {
                                    countinplaycli = (int)cmdCountcli.ExecuteScalar();
                                    if (countinplaycli == 0)
                                    {
                                        SqlCommand sqlLivematch = new SqlCommand("UPDATE matches SET status='CLOSED' WHERE event_code='" + event_code + "' ", con);
                                        sqlLivematch.ExecuteNonQuery();
                                    }
                                }*/
                                con.Close();
                                return "Market Settle SuccessFully";
                            }
                            else
                            {
                                con.Close();
                                return "Erro in Market Settle";
                            }
                        }
                    }
                }
                else
                {
                    return "Data Missing";
                }
            }
            catch (Exception ex)
            {
                resultreturn = ex.ToString();
            }
            return resultreturn;
        }

        public string SessionRevert(string event_code, string betfair_id, int SelectMatchWinner, string market_name)
        {
            DateTime time = DateTime.Now;              // Use current time
            string format = "yyyy-MM-dd HH:mm:ss";
            string resultreturn = "Equal12";
            try
            {
                string eventSportName = FunctionDataController.GetSportsNameFromEvent(event_code);
                string EventName = FunctionDataController.getEventName(event_code);

                if (event_code != "" && betfair_id != "" && SelectMatchWinner > -1 && market_name != "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        var client_ids = new List<int>();
                        SqlCommand sqlFancy = new SqlCommand("SELECT DISTINCT user_id FROM fancy_exchange WHERE market_id = '" + betfair_id + "' AND event_id = '" + event_code + "' AND is_done='1' ", con);
                        var rowFancy = sqlFancy.ExecuteReader();
                        if (!rowFancy.HasRows)
                        {
                            SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='' , result_value='' WHERE status='deactivate' AND result_value!='' AND market_settle='1' AND betfair_id = '" + betfair_id + "' AND event_code = '" + event_code + "' ", con);
                            int sqlMarketSettleUpdatedone = sqlMarketSettleUpdate.ExecuteNonQuery();
                            if (sqlMarketSettleUpdatedone > 0)
                            {
                                /*string stmtcli = "SELECT COUNT(id) FROM live_bet where event_id = '" + event_code + "' AND status='' ";
                                int countinplaycli = 0;
                                using (SqlCommand cmdCountcli = new SqlCommand(stmtcli, con))
                                {
                                    countinplaycli = (int)cmdCountcli.ExecuteScalar();
                                    if (countinplaycli == 0)
                                    {
                                        SqlCommand sqlLivematch = new SqlCommand("UPDATE matches SET status='CLOSED' WHERE event_code='" + event_code + "' ", con);
                                        sqlLivematch.ExecuteNonQuery();
                                    }
                                }*/
                                con.Close();
                                return "Market Revert SuccessFully";
                            }
                            else
                            {
                                con.Close();
                                return "Market Already Reverted";
                            }
                        }
                        else
                        {
                            string string_client_ids = "0";
                            while (rowFancy.Read())
                            {
                                int uid = (int)rowFancy["user_id"];
                                client_ids.Add(uid);
                                string_client_ids = string_client_ids + "," + uid;
                            }

                            SqlCommand marketde = new SqlCommand("UPDATE markets SET status='activate' WHERE status='deactivate' AND market_settle='1' AND betfair_id = '" + betfair_id + "' AND event_code = '" + event_code + "' ", con);
                            int sqlMarketUpdatedone = marketde.ExecuteNonQuery();
                            if (sqlMarketUpdatedone > 0)
                            {
                                for (int i = 0; i < client_ids.Count; i++)
                                {
                                    int user_id = client_ids[i];
                                    Double total_liability = 0;
                                    Double availableLiability = 0;

                                    SqlCommand sql_new = new SqlCommand("SELECT [id] FROM live_bet_new WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_code + "' ", con);
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

                                        string StringLiveBetIdWon = "0";
                                        Double TotalPLLiveBetIdWon = 0;
                                        SqlCommand sqlWon = new SqlCommand("SELECT [total_value] , [id] FROM live_bet_new WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND user_id='" + client_ids[i] + "' AND ((field='Yes' AND rate<='" + SelectMatchWinner + "') OR (field='Not' AND rate>'" + SelectMatchWinner + "')) ", con);
                                        var dataWon = sqlWon.ExecuteReader();
                                        if (dataWon.HasRows)
                                        {
                                            while (dataWon.Read())
                                            {
                                                int won_ids = (int)dataWon["id"];
                                                StringLiveBetIdWon = StringLiveBetIdWon + "," + won_ids;
                                                TotalPLLiveBetIdWon = TotalPLLiveBetIdWon + (Double)dataWon["total_value"];
                                            }
                                        }
                                        string StringLiveBetIdLost = "0";
                                        Double TotalStakesLiveBetIdLost = 0;
                                        SqlCommand sqlLost = new SqlCommand("SELECT [stakes] , [id] FROM live_bet_new WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND user_id='" + client_ids[i] + "' AND ((field='Yes' AND rate>'" + SelectMatchWinner + "') OR (field='Not' AND rate<='" + SelectMatchWinner + "')) ", con);
                                        var dataLost = sqlLost.ExecuteReader();
                                        if (dataLost.HasRows)
                                        {
                                            while (dataLost.Read())
                                            {
                                                int lost_ids = (int)dataLost["id"];
                                                StringLiveBetIdLost = StringLiveBetIdLost + "," + lost_ids;
                                                TotalStakesLiveBetIdLost = TotalStakesLiveBetIdLost + (Double)dataLost["stakes"];
                                            }
                                        }

                                        SqlCommand sqlFancyExch = new SqlCommand("SELECT stakes FROM fancy_exchange WHERE market_id='" + betfair_id + "' AND event_id='" + event_code + "' AND user_id='" + client_ids[i] + "' AND is_done='1' ", con);
                                        var dataFancyExch = sqlFancyExch.ExecuteReader();
                                        if (dataFancyExch.HasRows)
                                        {
                                            while (dataFancyExch.Read())
                                            {
                                                total_liability = total_liability + (Double)dataFancyExch["stakes"];
                                            }
                                            availableLiability = UserLiability + total_liability;
                                        }

                                        Double NetWinLost = -(TotalPLLiveBetIdWon - TotalStakesLiveBetIdLost);
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

                                        Double ClientNetBalance = UserBalance + NetWinLost - total_liability;
                                        Double user_updated_net_balance = ClientNetBalance + availableLiability;

                                        Double NetWinLoss = NetWinLost;
                                        Double NetClientPL = ClientPL + NetWinLoss;

                                        Double adjust_bal = 0 + NetWinLost - total_liability;
                                        Double adjust_exp = 0 + total_liability;
                                        Double adjust_pl = NetWinLoss;

                                        SqlCommand sqlLiveBetWon = new SqlCommand("UPDATE live_bet_new SET status='' WHERE user_id='" + user_id + "' AND betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' ", con);
                                        sqlLiveBetWon.ExecuteNonQuery();

                                        SqlCommand sqlFancyDone = new SqlCommand("UPDATE fancy_exchange SET is_done='0' WHERE user_id='" + user_id + "' AND event_id='" + event_code + "' AND market_id='" + betfair_id + "' ", con);
                                        sqlFancyDone.ExecuteNonQuery();

                                        SqlCommand sqlUserBalanceUpdate = new SqlCommand("UPDATE users_client SET balance= balance + '" + adjust_bal + "' , exposure= exposure + '" + adjust_exp + "' , profit_loss= profit_loss + '" + adjust_pl + "' WHERE id = '" + user_id + "' ", con);
                                        sqlUserBalanceUpdate.ExecuteNonQuery();

                                        string AccStatementDescription = eventSportName + " / " + EventName + " / " + market_name + " Settle Reverted";

                                        SqlCommand sqlInsertUser = new SqlCommand("INSERT INTO user_account_statements (admin_id , md_id , dist_id , user_id , acc_stat_type ,  description ,  remark ,  debit ,  credit ,  balance ,  market_id ,  event_id ,  created , match_odds ) VALUES ('" + Admin_id + "','" + UserMDId + "','" + UserDistId + "','" + user_id + "','pl_coins','" + AccStatementDescription + "','','" + debit_balance + "','" + credit_balance + "','" + user_updated_net_balance + "','" + betfair_id + "','" + event_code + "','" + time.ToString(format) + "','2')", con);
                                        sqlInsertUser.ExecuteNonQuery();
                                    }
                                }
                                SqlCommand sqlBetStatusUpdate1 = new SqlCommand("UPDATE live_bet SET status='' , settled_time='" + time.ToString(format) + "' WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' ", con);
                                sqlBetStatusUpdate1.ExecuteNonQuery();

                                SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='' , result_value='' WHERE status='activate' AND betfair_id = '" + betfair_id + "' AND event_code='" + event_code + "' AND result_value!='' AND market_settle='1'  ", con);
                                sqlMarketSettleUpdate.ExecuteNonQuery();

                                /*string stmtcli = "SELECT COUNT(id) FROM live_bet where event_id = '" + event_code + "' AND status='' ";
                                int countinplaycli = 0;
                                using (SqlCommand cmdCountcli = new SqlCommand(stmtcli, con))
                                {
                                    countinplaycli = (int)cmdCountcli.ExecuteScalar();
                                    if (countinplaycli == 0)
                                    {
                                        SqlCommand sqlLivematch = new SqlCommand("UPDATE matches SET status='CLOSED' WHERE event_code='" + event_code + "' ", con);
                                        sqlLivematch.ExecuteNonQuery();
                                    }
                                }*/
                                con.Close();
                                return "Market Revrted SuccessFully";
                            }
                            else
                            {
                                con.Close();
                                return "Erro in Market Revrted";
                            }
                        }
                    }
                }
                else
                {
                    return "Data Missing";
                }
            }
            catch (Exception ex)
            {
                resultreturn = ex.ToString();
            }
            return resultreturn;
        }

        public string AutoSessionSettleTie(string event_code, string betfair_id, int SelectMatchWinner, string market_name)
        {
            DateTime time = DateTime.Now;              // Use current time
            string format = "yyyy-MM-dd HH:mm:ss";
            string resultreturn = "Equal12";
            try
            {
                string eventSportName = FunctionDataController.GetSportsNameFromEvent(event_code);
                string EventName = FunctionDataController.getEventName(event_code);

                if (event_code != "" && betfair_id != "" && SelectMatchWinner == 9999 && market_name != "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        var client_ids = new List<int>();
                        SqlCommand sqlFancy = new SqlCommand("SELECT DISTINCT user_id FROM fancy_exchange WHERE market_id = '" + betfair_id + "' AND event_id = '" + event_code + "' AND is_done='0' ", con);
                        var rowFancy = sqlFancy.ExecuteReader();
                        if (!rowFancy.HasRows)
                        {
                            SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='1' , result_value='Tie/No Result' WHERE status='activate' AND result_value='' AND market_settle!='1' AND betfair_id = '" + betfair_id + "' AND event_code = '" + event_code + "' ", con);
                            int sqlMarketSettleUpdatedone = sqlMarketSettleUpdate.ExecuteNonQuery();
                            if (sqlMarketSettleUpdatedone > 0)
                            {
                                /*string stmtcli = "SELECT COUNT(id) FROM live_bet where event_id = '" + event_code + "' AND status='' ";
                                int countinplaycli = 0;
                                using (SqlCommand cmdCountcli = new SqlCommand(stmtcli, con))
                                {
                                    countinplaycli = (int)cmdCountcli.ExecuteScalar();
                                    if (countinplaycli == 0)
                                    {
                                        SqlCommand sqlLivematch = new SqlCommand("UPDATE matches SET status='CLOSED' WHERE event_code='" + event_code + "' ", con);
                                        sqlLivematch.ExecuteNonQuery();
                                    }
                                }*/
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
                            string string_client_ids = "0";
                            while (rowFancy.Read())
                            {
                                int uid = (int)rowFancy["user_id"];
                                client_ids.Add(uid);
                                string_client_ids = string_client_ids + "," + uid;
                            }

                            SqlCommand marketde = new SqlCommand("UPDATE markets SET status='deactivate' WHERE status='activate' AND market_settle!='1' AND betfair_id = '" + betfair_id + "' AND event_code = '" + event_code + "' ", con);
                            int sqlMarketUpdatedone = marketde.ExecuteNonQuery();
                            if (sqlMarketUpdatedone > 0)
                            {
                                for (int i = 0; i < client_ids.Count; i++)
                                {
                                    int user_id = client_ids[i];
                                    Double total_liability = 0;
                                    Double availableLiability = 0;

                                    SqlCommand sql_new = new SqlCommand("SELECT [id] FROM live_bet_new WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_code + "' ", con);
                                    var result_new = sql_new.ExecuteReader();
                                    if (result_new.HasRows)
                                    {
                                        SqlCommand sqlUser2 = new SqlCommand("SELECT [balance],[exposure],[profit_loss],[admin_id],[mdl_id],[dl_id] FROM users_client WHERE id='" + user_id + "' ", con);
                                        var dataUser2 = sqlUser2.ExecuteReader();
                                        dataUser2.Read();
                                        Double UserBalance = (Double)dataUser2["balance"];
                                        Double UserLiability = (Double)dataUser2["exposure"];
                                        int Admin_id = (int)dataUser2["admin_id"];
                                        int UserMDId = (int)dataUser2["mdl_id"];
                                        int UserDistId = (int)dataUser2["dl_id"];
                                        availableLiability = UserLiability;

                                        SqlCommand sqlFancyExch = new SqlCommand("SELECT stakes FROM fancy_exchange WHERE market_id='" + betfair_id + "' AND event_id='" + event_code + "' AND user_id='" + client_ids[i] + "' AND is_done='0' ", con);
                                        var dataFancyExch = sqlFancyExch.ExecuteReader();
                                        if (dataFancyExch.HasRows)
                                        {
                                            while (dataFancyExch.Read())
                                            {
                                                total_liability = total_liability + (Double)dataFancyExch["stakes"];
                                            }
                                            availableLiability = UserLiability - total_liability;
                                        }

                                        Double debit_balance = 0;
                                        Double credit_balance = 0;

                                        Double ClientNetBalance = UserBalance + total_liability;
                                        Double user_updated_net_balance = ClientNetBalance + availableLiability;
                                        Double adjust_bal = 0 + total_liability;
                                        Double adjust_exp = 0 - total_liability;

                                        SqlCommand sqlLiveBetWon = new SqlCommand("UPDATE live_bet_new SET status='tie' WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' ", con);
                                        sqlLiveBetWon.ExecuteNonQuery();

                                        SqlCommand sqlFancyDone = new SqlCommand("UPDATE fancy_exchange SET is_done='1' WHERE user_id='" + user_id + "' AND event_id='" + event_code + "' AND market_id='" + betfair_id + "' ", con);
                                        sqlFancyDone.ExecuteNonQuery();

                                        SqlCommand sqlUserBalanceUpdate = new SqlCommand("UPDATE users_client SET balance= balance + '" + adjust_bal + "' , exposure= exposure + '" + adjust_exp + "' WHERE id = '" + user_id + "' ", con);
                                        sqlUserBalanceUpdate.ExecuteNonQuery();

                                        string AccStatementDescription = eventSportName + " / " + EventName + " / " + market_name + "[Tie/No Result]";

                                        SqlCommand sqlInsertUser = new SqlCommand("INSERT INTO user_account_statements (admin_id , md_id , dist_id , user_id , acc_stat_type ,  description ,  remark ,  debit ,  credit ,  balance ,  market_id ,  event_id ,  created , match_odds ) VALUES ('" + Admin_id + "','" + UserMDId + "','" + UserDistId + "','" + user_id + "','pl_coins','" + AccStatementDescription + "','','" + debit_balance + "','" + credit_balance + "','" + user_updated_net_balance + "','" + betfair_id + "','" + event_code + "','" + time.ToString(format) + "','2')", con);
                                        sqlInsertUser.ExecuteNonQuery();
                                    }
                                }
                                SqlCommand sqlBetStatusUpdate1 = new SqlCommand("UPDATE live_bet SET status='tie' , settled_time='" + time.ToString(format) + "' WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' ", con);
                                sqlBetStatusUpdate1.ExecuteNonQuery();

                                SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='1' , result_value='Tie/No Result' WHERE status='deactivate' AND betfair_id = '" + betfair_id + "' AND event_code='" + event_code + "' AND result_value='' AND market_settle!='1'  ", con);
                                sqlMarketSettleUpdate.ExecuteNonQuery();

                                /*string stmtcli = "SELECT COUNT(id) FROM live_bet where event_id = '" + event_code + "' AND status='' ";
                                int countinplaycli = 0;
                                using (SqlCommand cmdCountcli = new SqlCommand(stmtcli, con))
                                {
                                    countinplaycli = (int)cmdCountcli.ExecuteScalar();
                                    if (countinplaycli == 0)
                                    {
                                        SqlCommand sqlLivematch = new SqlCommand("UPDATE matches SET status='CLOSED' WHERE event_code='" + event_code + "' ", con);
                                        sqlLivematch.ExecuteNonQuery();
                                    }
                                }*/
                                con.Close();
                                return "Market Settle SuccessFully";
                            }
                            else
                            {
                                con.Close();
                                return "Erro in Market Settle";
                            }
                        }
                    }
                }
                else
                {
                    return "Data Missing";
                }
            }
            catch (Exception ex)
            {
                resultreturn = ex.ToString();
            }
            return resultreturn;
        }

        public ActionResult AutoSessionR()
        {
            try
            {
                string betfair_id = Request.QueryString["betfair_id"];
                string event_code = Request.QueryString["event_code"];
                string message = AutoSessionSettleTieR(event_code, betfair_id);
                ViewBag.message = message;
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.ToString();
            }
            return View();
        }

        public string AutoSessionSettleTieR(string event_code, string betfair_id)
        {
            DateTime time = DateTime.Now;              // Use current time
            string format = "yyyy-MM-dd HH:mm:ss";
            string resultreturn = "Equal12";
            try
            {
                string market_name = FunctionDataController.GetMArketName(event_code, betfair_id);
                string getwin = FunctionDataController.GetWinner(event_code, betfair_id);
                int SelectMatchWinner = Int32.Parse(getwin);
                string eventSportName = FunctionDataController.GetSportsNameFromEvent(event_code);
                string EventName = FunctionDataController.getEventName(event_code);

                if (event_code != "" && betfair_id != "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        var client_ids = new List<int>();
                        SqlCommand sqlFancy = new SqlCommand("SELECT DISTINCT user_id FROM fancy_exchange WHERE market_id = '" + betfair_id + "' AND event_id = '" + event_code + "' AND is_done='1' ", con);
                        var rowFancy = sqlFancy.ExecuteReader();
                        if (!rowFancy.HasRows)
                        {
                            SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='' , result_value='' WHERE status='deactivate' AND result_value!='' AND market_settle='1' AND betfair_id = '" + betfair_id + "' AND event_code = '" + event_code + "' ", con);
                            int sqlMarketSettleUpdatedone = sqlMarketSettleUpdate.ExecuteNonQuery();
                            if (sqlMarketSettleUpdatedone > 0)
                            {
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
                            string string_client_ids = "0";
                            while (rowFancy.Read())
                            {
                                int uid = (int)rowFancy["user_id"];
                                client_ids.Add(uid);
                                string_client_ids = string_client_ids + "," + uid;
                            }

                            SqlCommand marketde = new SqlCommand("UPDATE markets SET status='activate' WHERE status='deactivate' AND market_settle='1' AND betfair_id = '" + betfair_id + "' AND event_code = '" + event_code + "' ", con);
                            int sqlMarketUpdatedone = marketde.ExecuteNonQuery();
                            if (sqlMarketUpdatedone > 0)
                            {
                                for (int i = 0; i < client_ids.Count; i++)
                                {
                                    int user_id = client_ids[i];
                                    Double total_liability = 0;
                                    Double availableLiability = 0;

                                    SqlCommand sql_new = new SqlCommand("SELECT [id] FROM live_bet_new WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_code + "' ", con);
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

                                        string StringLiveBetIdWon = "0";
                                        Double TotalPLLiveBetIdWon = 0;
                                        SqlCommand sqlWon = new SqlCommand("SELECT [total_value] , [id] FROM live_bet_new WHERE status!='' AND betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND user_id='" + client_ids[i] + "' AND ((field='Yes' AND rate<='" + SelectMatchWinner + "') OR (field='Not' AND rate>'" + SelectMatchWinner + "')) ", con);
                                        var dataWon = sqlWon.ExecuteReader();
                                        if (dataWon.HasRows)
                                        {
                                            while (dataWon.Read())
                                            {
                                                int won_ids = (int)dataWon["id"];
                                                StringLiveBetIdWon = StringLiveBetIdWon + "," + won_ids;
                                                TotalPLLiveBetIdWon = TotalPLLiveBetIdWon + (Double)dataWon["total_value"];
                                            }
                                        }
                                        string StringLiveBetIdLost = "0";
                                        Double TotalStakesLiveBetIdLost = 0;
                                        SqlCommand sqlLost = new SqlCommand("SELECT [stakes] , [id] FROM live_bet_new WHERE status!='' AND betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND user_id='" + client_ids[i] + "' AND ((field='Yes' AND rate>'" + SelectMatchWinner + "') OR (field='Not' AND rate<='" + SelectMatchWinner + "')) ", con);
                                        var dataLost = sqlLost.ExecuteReader();
                                        if (dataLost.HasRows)
                                        {
                                            while (dataLost.Read())
                                            {
                                                int lost_ids = (int)dataLost["id"];
                                                StringLiveBetIdLost = StringLiveBetIdLost + "," + lost_ids;
                                                TotalStakesLiveBetIdLost = TotalStakesLiveBetIdLost + (Double)dataLost["stakes"];
                                            }
                                        }

                                        SqlCommand sqlFancyExch = new SqlCommand("SELECT stakes FROM fancy_exchange WHERE market_id='" + betfair_id + "' AND event_id='" + event_code + "' AND user_id='" + client_ids[i] + "' AND is_done='1' ", con);
                                        var dataFancyExch = sqlFancyExch.ExecuteReader();
                                        if (dataFancyExch.HasRows)
                                        {
                                            while (dataFancyExch.Read())
                                            {
                                                total_liability = total_liability + (Double)dataFancyExch["stakes"];
                                            }
                                            availableLiability = UserLiability + total_liability;
                                        }

                                        Double NetWinLost = -(TotalPLLiveBetIdWon - TotalStakesLiveBetIdLost);
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

                                        Double ClientNetBalance = UserBalance + NetWinLost - total_liability;
                                        Double user_updated_net_balance = ClientNetBalance + availableLiability;

                                        Double NetWinLoss = NetWinLost;
                                        Double NetClientPL = ClientPL + NetWinLoss;
                                        Double adjust_bal = 0 + NetWinLost - total_liability;
                                        Double adjust_exp = 0 + total_liability;
                                        Double adjust_pl = NetWinLoss;

                                        SqlCommand sqlLiveBetWon = new SqlCommand("UPDATE live_bet_new SET status='' WHERE status!='' AND betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND ((field='Yes' AND rate<='" + SelectMatchWinner + "') OR (field='Not' AND rate>'" + SelectMatchWinner + "')) ", con);
                                        sqlLiveBetWon.ExecuteNonQuery();

                                        SqlCommand sqlLiveBetLost = new SqlCommand("UPDATE live_bet_new SET status='' WHERE status!='' AND betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND ((field='Yes' AND rate>'" + SelectMatchWinner + "') OR (field='Not' AND rate<='" + SelectMatchWinner + "')) ", con);
                                        sqlLiveBetLost.ExecuteNonQuery();

                                        SqlCommand sqlFancyDone = new SqlCommand("UPDATE fancy_exchange SET is_done='0' WHERE is_done='1' AND user_id='" + user_id + "' AND event_id='" + event_code + "' AND market_id='" + betfair_id + "' ", con);
                                        sqlFancyDone.ExecuteNonQuery();

                                        SqlCommand sqlUserBalanceUpdate = new SqlCommand("UPDATE users_client SET balance = balance + '" + adjust_bal + "' , exposure = exposure + '" + adjust_exp + "' , profit_loss = profit_loss + '" + adjust_pl + "' WHERE id = '" + user_id + "' ", con);
                                        sqlUserBalanceUpdate.ExecuteNonQuery();

                                        string AccStatementDescription = eventSportName + " / " + EventName + " / " + market_name + "[ Wrong Dr/Cr]";

                                        SqlCommand sqlInsertUser = new SqlCommand("INSERT INTO user_account_statements (admin_id , md_id , dist_id , user_id , acc_stat_type ,  description ,  remark ,  debit ,  credit ,  balance ,  market_id ,  event_id ,  created , match_odds ) VALUES ('" + Admin_id + "','" + UserMDId + "','" + UserDistId + "','" + user_id + "','pl_coins','" + AccStatementDescription + "','','" + debit_balance + "','" + credit_balance + "','" + user_updated_net_balance + "','" + betfair_id + "','" + event_code + "','" + time.ToString(format) + "','2')", con);
                                        sqlInsertUser.ExecuteNonQuery();
                                    }
                                }
                                SqlCommand sqlBetStatusUpdate1 = new SqlCommand("UPDATE live_bet SET status='' , settled_time='" + time.ToString(format) + "' WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' ", con);
                                sqlBetStatusUpdate1.ExecuteNonQuery();

                                SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='' , result_value='' WHERE status='activate' AND betfair_id = '" + betfair_id + "' AND event_code='" + event_code + "' AND result_value!='' AND market_settle='1'  ", con);
                                sqlMarketSettleUpdate.ExecuteNonQuery();
                                con.Close();
                                return "Market Settle SuccessFully";
                            }
                            else
                            {
                                con.Close();
                                return "Erro in Market Settle";
                            }
                        }
                    }
                }
                else
                {
                    return "Data Missing";
                }
            }
            catch (Exception ex)
            {
                resultreturn = ex.ToString();
            }
            return resultreturn;
        }
        public ActionResult AutoSessionRT()
        {
            try
            {
                string betfair_id = Request.QueryString["betfair_id"];
                string event_code = Request.QueryString["event_code"];
                string message = AutoSessionSettleTieRT(event_code, betfair_id);
                ViewBag.message = message;
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.ToString();
            }
            return View();
        }

        public string AutoSessionSettleTieRT(string event_code, string betfair_id)
        {
            DateTime time = DateTime.Now;              // Use current time
            string format = "yyyy-MM-dd HH:mm:ss";
            string resultreturn = "Equal12";
            try
            {
                string market_name = FunctionDataController.GetMArketName(event_code, betfair_id);

                string eventSportName = FunctionDataController.GetSportsNameFromEvent(event_code);
                string EventName = FunctionDataController.getEventName(event_code);

                if (event_code != "" && betfair_id != "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        var client_ids = new List<int>();
                        SqlCommand sqlFancy = new SqlCommand("SELECT DISTINCT user_id FROM fancy_exchange WHERE market_id = '" + betfair_id + "' AND event_id = '" + event_code + "' AND is_done='1' ", con);
                        var rowFancy = sqlFancy.ExecuteReader();
                        if (!rowFancy.HasRows)
                        {
                            SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='' , result_value='' WHERE status='deactivate' AND result_value!='' AND market_settle='1' AND betfair_id = '" + betfair_id + "' AND event_code = '" + event_code + "' ", con);
                            int sqlMarketSettleUpdatedone = sqlMarketSettleUpdate.ExecuteNonQuery();
                            if (sqlMarketSettleUpdatedone > 0)
                            {
                                con.Close();
                                return " " + market_name + " Market Revert SuccessFully";
                            }
                            else
                            {
                                con.Close();
                                return " " + market_name + " Market Already Reverted";
                            }
                        }
                        else
                        {
                            string string_client_ids = "0";
                            while (rowFancy.Read())
                            {
                                int uid = (int)rowFancy["user_id"];
                                client_ids.Add(uid);
                                string_client_ids = string_client_ids + "," + uid;
                            }

                            SqlCommand marketde = new SqlCommand("UPDATE markets SET status='activate' WHERE status='deactivate' AND market_settle='1' AND betfair_id = '" + betfair_id + "' AND event_code = '" + event_code + "' ", con);
                            int sqlMarketUpdatedone = marketde.ExecuteNonQuery();
                            if (sqlMarketUpdatedone > 0)
                            {
                                for (int i = 0; i < client_ids.Count; i++)
                                {
                                    int user_id = client_ids[i];
                                    Double total_liability = 0;
                                    Double availableLiability = 0;

                                    SqlCommand sql_new = new SqlCommand("SELECT [id] FROM live_bet_new WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_code + "' ", con);
                                    var result_new = sql_new.ExecuteReader();
                                    if (result_new.HasRows)
                                    {
                                        SqlCommand sqlUser2 = new SqlCommand("SELECT [balance],[exposure],[profit_loss],[admin_id],[mdl_id],[dl_id] FROM users_client WHERE id='" + user_id + "' ", con);
                                        var dataUser2 = sqlUser2.ExecuteReader();
                                        dataUser2.Read();
                                        Double UserBalance = (Double)dataUser2["balance"];
                                        Double UserLiability = (Double)dataUser2["exposure"];
                                        int Admin_id = (int)dataUser2["admin_id"];
                                        int UserMDId = (int)dataUser2["mdl_id"];
                                        int UserDistId = (int)dataUser2["dl_id"];
                                        availableLiability = UserLiability;

                                        SqlCommand sqlFancyExch = new SqlCommand("SELECT stakes FROM fancy_exchange WHERE market_id='" + betfair_id + "' AND event_id='" + event_code + "' AND user_id='" + client_ids[i] + "' AND is_done='1' ", con);
                                        var dataFancyExch = sqlFancyExch.ExecuteReader();
                                        if (dataFancyExch.HasRows)
                                        {
                                            while (dataFancyExch.Read())
                                            {
                                                total_liability = total_liability + (Double)dataFancyExch["stakes"];
                                            }
                                            availableLiability = UserLiability + total_liability;
                                        }

                                        Double debit_balance = 0;
                                        Double credit_balance = 0;

                                        Double ClientNetBalance = UserBalance - total_liability;
                                        Double user_updated_net_balance = ClientNetBalance + availableLiability;
                                        Double adjust_bal = 0 - total_liability;
                                        Double adjust_exp = 0 + total_liability;
                                        SqlCommand sqlLiveBetWon = new SqlCommand("UPDATE live_bet_new SET status='' WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' ", con);
                                        sqlLiveBetWon.ExecuteNonQuery();

                                        SqlCommand sqlFancyDone = new SqlCommand("UPDATE fancy_exchange SET is_done='0' WHERE user_id='" + user_id + "' AND event_id='" + event_code + "' AND market_id='" + betfair_id + "' ", con);
                                        sqlFancyDone.ExecuteNonQuery();

                                        SqlCommand sqlUserBalanceUpdate = new SqlCommand("UPDATE users_client SET balance = balance + '" + adjust_bal + "' , exposure = exposure + '" + adjust_exp + "' WHERE id = '" + user_id + "' ", con);
                                        sqlUserBalanceUpdate.ExecuteNonQuery();

                                        string AccStatementDescription = eventSportName + " / " + EventName + " / " + market_name + "[Wrong Dr/Cr]";

                                        SqlCommand sqlInsertUser = new SqlCommand("INSERT INTO user_account_statements (admin_id , md_id , dist_id , user_id , acc_stat_type ,  description ,  remark ,  debit ,  credit ,  balance ,  market_id ,  event_id ,  created , match_odds ) VALUES ('" + Admin_id + "','" + UserMDId + "','" + UserDistId + "','" + user_id + "','pl_coins','" + AccStatementDescription + "','','" + debit_balance + "','" + credit_balance + "','" + user_updated_net_balance + "','" + betfair_id + "','" + event_code + "','" + time.ToString(format) + "','2')", con);
                                        sqlInsertUser.ExecuteNonQuery();
                                    }
                                }
                                SqlCommand sqlBetStatusUpdate1 = new SqlCommand("UPDATE live_bet SET status='' , settled_time='" + time.ToString(format) + "' WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' ", con);
                                sqlBetStatusUpdate1.ExecuteNonQuery();

                                SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='' , result_value='' WHERE status='activate' AND betfair_id = '" + betfair_id + "' AND event_code='" + event_code + "' AND result_value!='' AND market_settle='1'  ", con);
                                sqlMarketSettleUpdate.ExecuteNonQuery();
                                con.Close();
                                return " " + market_name + " Market Revert SuccessFully";
                            }
                            else
                            {
                                con.Close();
                                return "Erro in " + market_name + "  Settle";
                            }
                        }
                    }
                }
                else
                {
                    return "Data Missing";
                }
            }
            catch (Exception ex)
            {
                resultreturn = ex.ToString();
            }
            return resultreturn;
        }

        /* public ActionResult AutoDLMDLCAL()
         {
             try
             {
                 DateTime time = DateTime.Now;              // Use current time
                 string format = "yyyy-MM-dd HH:mm:ss";
                 using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                 {
                     con.Open();
                     SqlCommand sqlmatch = new SqlCommand("SELECT [sport_id],[market_name],[event_code],[betfair_id] FROM markets WHERE dma_cal='no' AND market_settle='1' AND status = 'deactivate' ", con);
                     var reader_match = sqlmatch.ExecuteReader();
                     while (reader_match.Read())
                     {
                         string event_code = (string)reader_match["event_code"];
                         string betfair_id = (string)reader_match["betfair_id"];
                         string sport_id = (string)reader_match["sport_id"];
                         string market_name = (string)reader_match["market_name"];
                         string event_name = FunctionDataController.getEventName(event_code);
                         string eventSportName = FunctionDataController.GetSportsNameFromEvent(event_code);
                         string PLDescription = eventSportName + " / " + event_name + " / " + market_name;

                         SqlCommand sqlmarket = new SqlCommand("SELECT DISTINCT(dist_id) , md_id , admin_id FROM user_account_statements WHERE acc_stat_type='pl_coins' AND event_id='" + event_code + "' AND market_id='" + betfair_id + "' ", con);
                         var reader_market = sqlmarket.ExecuteReader();
                         if (reader_market.HasRows)
                         {
                             while (reader_market.Read())
                             {
                                 int dl_id = (int)reader_market["dist_id"];
                                 int md_id = (int)reader_market["md_id"];
                                 int admin_id = (int)reader_market["admin_id"];
                                 Double creditt = 0;
                                 Double debitt = 0;
                                 string is_credit = "no";
                                 string is_debit = "no";
                                 SqlCommand sqlcreditt = new SqlCommand("select  COALESCE(SUM(credit),0) as total_credit from user_account_statements WHERE acc_stat_type='pl_coins' AND event_id='" + event_code + "' AND market_id='" + betfair_id + "' AND dist_id='" + dl_id + "' ", con);
                                 var reader_creditt = sqlcreditt.ExecuteReader();
                                 if (reader_creditt.HasRows)
                                 {
                                     reader_creditt.Read();
                                     creditt = (Double)reader_creditt["total_credit"];
                                 }
                                 if (creditt > 0)
                                 {
                                     is_credit = "yes";
                                 }

                                 SqlCommand sqldebiitt = new SqlCommand("select  COALESCE(SUM(debit),0) as total_debit from user_account_statements WHERE acc_stat_type='pl_coins' AND event_id='" + event_code + "' AND market_id='" + betfair_id + "' AND  dist_id='" + dl_id + "' ", con);
                                 var reader_debitt = sqldebiitt.ExecuteReader();
                                 if (reader_debitt.HasRows)
                                 {
                                     reader_debitt.Read();
                                     debitt = (Double)reader_debitt["total_debit"];
                                 }
                                 if (debitt > 0)
                                 {
                                     is_debit = "yes";
                                 }

                                 Double Net_deb_cre = creditt - debitt;
                                 if (is_debit == "yes" || is_credit == "yes")
                                 {
                                     SqlCommand sqlPL = new SqlCommand("INSERT INTO pl_statement(admin_id,md_id, dist_id, sport_id, event_id, market_id, description, total_pl, dl_pl, mdl_pl, " +
                                         " admin_pl, total_plc, dl_plc, mdl_plc, admin_plc, created) VALUES ('" + admin_id + "','" + md_id + "','" + dl_id + "','" + sport_id + "', " +
                                         " '" + event_code + "','" + betfair_id + "','" + PLDescription + "','" + Net_deb_cre + "','0','0','0', " +
                                         " '0','0','0','0','" + time.ToString(format) + "') ", con);
                                     sqlPL.ExecuteNonQuery();
                                 }
                             }

                             SqlCommand sqlMarketdma = new SqlCommand("UPDATE markets SET dma_cal ='yes' WHERE dma_cal ='no' AND betfair_id = '" + betfair_id + "' AND event_code = '" + event_code + "' ", con);
                             int dqdj = sqlMarketdma.ExecuteNonQuery();
                             if (dqdj > 0)
                             {
                                *//* ViewBag.message = market_name + "[Success]";
                                 ViewBag.match_title = PLDescription + "[" + event_code + "]";*//*
                                 //return View();
                             }
                             else
                             {
                                 *//*ViewBag.message = market_name + "[Error]";
                                 ViewBag.match_title = PLDescription + "[" + event_code + "]";*//*
                                 //return View();
                             }
                         }
                         else
                         {

                         }
                     }
                     con.Close();
                 }
             }
             catch (Exception ex)
             {
                 ViewBag.message = ex.ToString();
                 ViewBag.match_title = "";
             }
             return View();
         }*/
        public ActionResult AutoDLMDLCALC()
        {
            try
            {
                DateTime time = DateTime.Now;              // Use current time
                string format = "yyyy-MM-dd HH:mm:ss";
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    SqlCommand sqlmatch = new SqlCommand("SELECT TOP(50) [sport_id],[market_name],[event_code],[betfair_id] FROM markets WHERE sport_id=7888 AND dma_cal='no' AND market_settle='1' AND status = 'deactivate' ", con);
                    var reader_match = sqlmatch.ExecuteReader();
                    while (reader_match.Read())
                    {
                        string event_code = (string)reader_match["event_code"];
                        string betfair_id = (string)reader_match["betfair_id"];
                        string sport_id = (string)reader_match["sport_id"];
                        string market_name = (string)reader_match["market_name"];
                        string event_name = FunctionDataController.getEventName(event_code);
                        string eventSportName = FunctionDataController.GetSportsNameFromEvent(event_code);
                        string PLDescription = eventSportName + " / " + event_name + " / " + market_name;

                        SqlCommand sqlmarket = new SqlCommand("SELECT DISTINCT(dist_id) , md_id , admin_id FROM user_account_statements WHERE acc_stat_type='pl_coins' AND event_id='" + event_code + "' AND market_id='" + betfair_id + "' ", con);
                        var reader_market = sqlmarket.ExecuteReader();
                        if (reader_market.HasRows)
                        {
                            while (reader_market.Read())
                            {
                                int dl_id = (int)reader_market["dist_id"];
                                int md_id = (int)reader_market["md_id"];
                                int admin_id = (int)reader_market["admin_id"];
                                Double creditt = 0;
                                Double debitt = 0;
                                string is_credit = "no";
                                string is_debit = "no";
                                SqlCommand sqlcreditt = new SqlCommand("select  COALESCE(SUM(credit),0) as total_credit from user_account_statements WHERE acc_stat_type='pl_coins' AND event_id='" + event_code + "' AND market_id='" + betfair_id + "' AND dist_id='" + dl_id + "' ", con);
                                var reader_creditt = sqlcreditt.ExecuteReader();
                                if (reader_creditt.HasRows)
                                {
                                    reader_creditt.Read();
                                    creditt = (Double)reader_creditt["total_credit"];
                                }
                                if (creditt > 0)
                                {
                                    is_credit = "yes";
                                }

                                SqlCommand sqldebiitt = new SqlCommand("select  COALESCE(SUM(debit),0) as total_debit from user_account_statements WHERE acc_stat_type='pl_coins' AND event_id='" + event_code + "' AND market_id='" + betfair_id + "' AND  dist_id='" + dl_id + "' ", con);
                                var reader_debitt = sqldebiitt.ExecuteReader();
                                if (reader_debitt.HasRows)
                                {
                                    reader_debitt.Read();
                                    debitt = (Double)reader_debitt["total_debit"];
                                }
                                if (debitt > 0)
                                {
                                    is_debit = "yes";
                                }

                                Double Net_deb_cre = creditt - debitt;
                                if (is_debit == "yes" || is_credit == "yes")
                                {
                                    SqlCommand sqlPL = new SqlCommand("INSERT INTO pl_statement(admin_id,md_id, dist_id, sport_id, event_id, market_id, description, total_pl, dl_pl, mdl_pl, " +
                                        " admin_pl, total_plc, dl_plc, mdl_plc, admin_plc, created) VALUES ('" + admin_id + "','" + md_id + "','" + dl_id + "','" + sport_id + "', " +
                                        " '" + event_code + "','" + betfair_id + "','" + PLDescription + "','" + Net_deb_cre + "','0','0','0', " +
                                        " '0','0','0','0','" + time.ToString(format) + "') ", con);
                                    sqlPL.ExecuteNonQuery();
                                }
                            }

                            SqlCommand sqlMarketdma = new SqlCommand("UPDATE markets SET dma_cal ='yes' WHERE dma_cal ='no' AND betfair_id = '" + betfair_id + "' AND event_code = '" + event_code + "' ", con);
                            int dqdj = sqlMarketdma.ExecuteNonQuery();
                            if (dqdj > 0)
                            {
                                /* ViewBag.message = market_name + "[Success]";
                                 ViewBag.match_title = PLDescription + "[" + event_code + "]";*/
                                //return View();
                            }
                            else
                            {
                                /*ViewBag.message = market_name + "[Error]";
                                ViewBag.match_title = PLDescription + "[" + event_code + "]";*/
                                //return View();
                            }
                        }
                        else
                        {

                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.ToString();
                ViewBag.match_title = "";
            }
            return View();
        }

        public ActionResult AutoDLMDLCALO()
        {
            try
            {
                DateTime time = DateTime.Now;              // Use current time
                string format = "yyyy-MM-dd HH:mm:ss";
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    SqlCommand sqlmatch = new SqlCommand("SELECT TOP(50) [sport_id],[market_name],[event_code],[betfair_id] FROM markets WHERE sport_id!=7888 AND dma_cal='no' AND market_settle='1' AND status = 'deactivate' ", con);
                    var reader_match = sqlmatch.ExecuteReader();
                    while (reader_match.Read())
                    {
                        string event_code = (string)reader_match["event_code"];
                        string betfair_id = (string)reader_match["betfair_id"];
                        string sport_id = (string)reader_match["sport_id"];
                        string market_name = (string)reader_match["market_name"];
                        string event_name = FunctionDataController.getEventName(event_code);
                        string eventSportName = FunctionDataController.GetSportsNameFromEvent(event_code);
                        string PLDescription = eventSportName + " / " + event_name + " / " + market_name;

                        SqlCommand sqlmarket = new SqlCommand("SELECT DISTINCT(dist_id) , md_id , admin_id FROM user_account_statements WHERE acc_stat_type='pl_coins' AND event_id='" + event_code + "' AND market_id='" + betfair_id + "' ", con);
                        var reader_market = sqlmarket.ExecuteReader();
                        if (reader_market.HasRows)
                        {
                            while (reader_market.Read())
                            {
                                int dl_id = (int)reader_market["dist_id"];
                                int md_id = (int)reader_market["md_id"];
                                int admin_id = (int)reader_market["admin_id"];
                                Double creditt = 0;
                                Double debitt = 0;
                                string is_credit = "no";
                                string is_debit = "no";
                                SqlCommand sqlcreditt = new SqlCommand("select  COALESCE(SUM(credit),0) as total_credit from user_account_statements WHERE acc_stat_type='pl_coins' AND event_id='" + event_code + "' AND market_id='" + betfair_id + "' AND dist_id='" + dl_id + "' ", con);
                                var reader_creditt = sqlcreditt.ExecuteReader();
                                if (reader_creditt.HasRows)
                                {
                                    reader_creditt.Read();
                                    creditt = (Double)reader_creditt["total_credit"];
                                }
                                if (creditt > 0)
                                {
                                    is_credit = "yes";
                                }

                                SqlCommand sqldebiitt = new SqlCommand("select  COALESCE(SUM(debit),0) as total_debit from user_account_statements WHERE acc_stat_type='pl_coins' AND event_id='" + event_code + "' AND market_id='" + betfair_id + "' AND  dist_id='" + dl_id + "' ", con);
                                var reader_debitt = sqldebiitt.ExecuteReader();
                                if (reader_debitt.HasRows)
                                {
                                    reader_debitt.Read();
                                    debitt = (Double)reader_debitt["total_debit"];
                                }
                                if (debitt > 0)
                                {
                                    is_debit = "yes";
                                }

                                Double Net_deb_cre = creditt - debitt;
                                if (is_debit == "yes" || is_credit == "yes")
                                {
                                    SqlCommand sqlPL = new SqlCommand("INSERT INTO pl_statement(admin_id,md_id, dist_id, sport_id, event_id, market_id, description, total_pl, dl_pl, mdl_pl, " +
                                        " admin_pl, total_plc, dl_plc, mdl_plc, admin_plc, created) VALUES ('" + admin_id + "','" + md_id + "','" + dl_id + "','" + sport_id + "', " +
                                        " '" + event_code + "','" + betfair_id + "','" + PLDescription + "','" + Net_deb_cre + "','0','0','0', " +
                                        " '0','0','0','0','" + time.ToString(format) + "') ", con);
                                    sqlPL.ExecuteNonQuery();
                                }
                            }

                            SqlCommand sqlMarketdma = new SqlCommand("UPDATE markets SET dma_cal ='yes' WHERE dma_cal ='no' AND betfair_id = '" + betfair_id + "' AND event_code = '" + event_code + "' ", con);
                            int dqdj = sqlMarketdma.ExecuteNonQuery();
                            if (dqdj > 0)
                            {
                                /* ViewBag.message = market_name + "[Success]";
                                 ViewBag.match_title = PLDescription + "[" + event_code + "]";*/
                                //return View();
                            }
                            else
                            {
                                /*ViewBag.message = market_name + "[Error]";
                                ViewBag.match_title = PLDescription + "[" + event_code + "]";*/
                                //return View();
                            }
                        }
                        else
                        {

                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.ToString();
                ViewBag.match_title = "";
            }
            return View();
        }

        public ActionResult AutoDLMDLCAL()
        {
            DateTime time = DateTime.Now;              // Use current time
            string format = "yyyy-MM-dd HH:mm:ss";
            Double total_coin_rate = 100;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                con.Open();
                SqlCommand sqlmatch = new SqlCommand("SELECT [sport_id],[market_name],[event_code],[betfair_id] FROM markets WHERE dma_cal='no' AND market_settle='1' AND status = 'deactivate' ", con);
                var reader_match = sqlmatch.ExecuteReader();
                while (reader_match.Read())
                {
                    string event_code = (string)reader_match["event_code"];
                    string betfair_id = (string)reader_match["betfair_id"];
                    string sport_id = (string)reader_match["sport_id"];
                    string market_name = (string)reader_match["market_name"];
                    string event_name = getEventName(event_code);
                    string eventSportName = GetSportsNameFromEvent(event_code);
                    string PLDescription = eventSportName + " / " + event_name + " / " + market_name;
                    int Ad_id = 1;
                    int DL_id = 0;
                    int MDL_id = 0;
                    int user_id = 0;
                    Double AdminProfitLoss = getAdminPL(Ad_id);
                    Double TotalAdminProfit = 0;

                    SqlCommand sqlmarket = new SqlCommand("SELECT DISTINCT[md_id],user_id FROM user_account_statements WHERE acc_stat_type='pl_coins' AND event_id='" + event_code + "' AND market_id='" + betfair_id + "' ", con);
                    var reader_market = sqlmarket.ExecuteReader();
                    if (reader_market.HasRows)
                    {
                        while (reader_market.Read())
                        {
                            int md_id = (int)reader_market["md_id"];
                            user_id = (int)reader_market["user_id"];
                            SqlCommand sqlMDL = new SqlCommand("SELECT [profit_loss],[admin_profit_loss],[coin_rate] FROM masterdistributors WHERE id='" + md_id + "' ", con);
                            var dataMDL = sqlMDL.ExecuteReader();
                            dataMDL.Read();
                            Double MDL_profit_loss = (Double)dataMDL["profit_loss"];
                            Double admin_profit_loss = (Double)dataMDL["admin_profit_loss"];
                            Double MDLTCoinRate = (Double)dataMDL["coin_rate"];
                            Double TotalMDLProfit = 0;
                            Double total_admin_profit = 0;
                            SqlCommand sqldist = new SqlCommand("SELECT [id],[admin_id],[coin_rate],[profit_loss],[mdl_profit_loss] FROM distributors WHERE md_id='" + md_id + "' ", con);
                            var reader_dist = sqldist.ExecuteReader();
                            while (reader_dist.Read())
                            {
                                int dl_id = (int)reader_dist["id"];
                                int admin_id = (int)reader_dist["admin_id"];
                                Double AdminCoinRate = total_coin_rate- MDLTCoinRate;
                                Double dl_profit_loss = (Double)reader_dist["profit_loss"];
                                Double mdl_profit_loss = (Double)reader_dist["mdl_profit_loss"];
                                Double DLCoinRate = (Double)reader_dist["coin_rate"];

                                Double creditt = 0;
                                Double debitt = 0;
                                string is_credit = "no";
                                string is_debit = "no";
                                SqlCommand sqlcreditt = new SqlCommand("select  COALESCE(SUM(credit),0) as total_credit,COALESCE(SUM(debit),0) as total_debit from user_account_statements WHERE acc_stat_type='pl_coins' AND event_id='" + event_code + "' AND market_id='" + betfair_id + "' AND user_id='" + user_id + "' ", con);
                                var reader_creditt = sqlcreditt.ExecuteReader();
                                reader_creditt.Read();
                                creditt = (Double)reader_creditt["total_credit"];
                                if (creditt > 0)
                                {
                                    is_credit = "yes";
                                }
                                debitt = (Double)reader_creditt["total_debit"];
                                if (debitt > 0)
                                {
                                    is_debit = "yes";
                                }

                                Double Net_deb_cre = creditt - debitt;
                                Double dl_pl = Math.Round((Net_deb_cre * DLCoinRate) / total_coin_rate,3);
                                Double DebitDPL = 0;
                                Double CreditDPL = 0;

                                if (dl_pl < 0)
                                {
                                    DebitDPL = Math.Abs(dl_pl);
                                    CreditDPL = 0;
                                }
                                else
                                {
                                    DebitDPL = 0;
                                    CreditDPL = dl_pl;
                                }
                                Double MDLCR = MDLTCoinRate - DLCoinRate;
                                Double mdl_pl = Math.Round((Net_deb_cre* MDLCR) /total_coin_rate, 3);
                                Double admin_pl = Math.Round((Net_deb_cre * AdminCoinRate) / total_coin_rate,3);
                                Double DLNetRefPL = dl_profit_loss + dl_pl;
                                Double MDLNetRefPL = mdl_profit_loss + mdl_pl + admin_pl;
                                TotalMDLProfit = TotalMDLProfit + mdl_pl;
                                total_admin_profit = total_admin_profit + admin_pl;
                                TotalAdminProfit = TotalAdminProfit + admin_pl;
                                Double sumC = 0;
                                Double sumAdminC = 0;
                                Double sumMDLC = 0;
                                Double sumDLC = 0;
                                if (is_debit == "yes" || is_credit == "yes")
                                {
                                    SqlCommand sqlDLPL = new SqlCommand("UPDATE distributors SET profit_loss='" + DLNetRefPL + "' , mdl_profit_loss='" + MDLNetRefPL + "' WHERE id = '" + dl_id + "' ", con);
                                    sqlDLPL.ExecuteNonQuery();
                                    SqlCommand sqlDPLS = new SqlCommand("INSERT INTO dist_account_statements(admin_id,md_id,dist_id,user_id,acc_stat_type, description,remark, debit, credit, " +
                                        " balance , event_id, market_id, created) VALUES ('" + admin_id + "','" + md_id + "','" + dl_id + "','" + user_id + "','pl_coins','" + PLDescription + "','','" + DebitDPL + "','" + CreditDPL + "'," +
                                        "'" + DLNetRefPL + "','" + event_code + "','" + betfair_id + "','" + time.ToString(format) + "')  ", con);
                                    sqlDPLS.ExecuteNonQuery();
                                    SqlCommand sqlPL = new SqlCommand("INSERT INTO pl_statement(admin_id,md_id, dist_id, sport_id, event_id, market_id, description, total_pl, dl_pl, mdl_pl, " +
                                        " admin_pl, total_plc, dl_plc, mdl_plc, admin_plc, created,user_id) VALUES ('" + admin_id + "','" + md_id + "','" + dl_id + "','" + sport_id + "', " +
                                        " '" + event_code + "','" + betfair_id + "','" + PLDescription + "','" + Net_deb_cre + "','" + dl_pl + "','" + mdl_pl + "','" + admin_pl + "', " +
                                        " '" + sumC + "','" + sumDLC + "','" + sumMDLC + "','" + sumAdminC + "','" + time.ToString(format) + "','" + user_id + "') ", con);
                                    sqlPL.ExecuteNonQuery();
                                }
                            }
                            Double NetMDLProfitLoss = MDL_profit_loss + TotalMDLProfit;
                            NetMDLProfitLoss = Math.Round(NetMDLProfitLoss, 3);
                            Double sumAgentMPL = Math.Round(TotalMDLProfit, 3);
                            Double DebitMPL = 0;
                            Double CreditMPL = 0;

                            if (sumAgentMPL < 0)
                            {
                                DebitMPL = Math.Abs(sumAgentMPL);
                                CreditMPL = 0;
                            }
                            else
                            {
                                DebitMPL = 0;
                                CreditMPL = sumAgentMPL;
                            }
                            Double Net_admin_profit_loss = admin_profit_loss + total_admin_profit;
                            Net_admin_profit_loss = Math.Round(Net_admin_profit_loss, 3);
                            SqlCommand sqlMDPL = new SqlCommand("UPDATE masterdistributors SET profit_loss = '" + NetMDLProfitLoss + "' ,admin_profit_loss ='" + Net_admin_profit_loss + "' WHERE id = '" + md_id + "' ", con);
                            sqlMDPL.ExecuteNonQuery();
                            SqlCommand sqlMPLS = new SqlCommand("INSERT INTO md_account_statements(admin_id,md_id,dist_id,user_id,acc_stat_type, description,remark, debit, credit, " +
                                " balance , event_id, market_id, created) VALUES ('" + Ad_id + "','" + md_id + "','" + DL_id + "','" + user_id + "','pl_coins','" + PLDescription + "','','" + DebitMPL + "','" + CreditMPL + "'," +
                                "'" + NetMDLProfitLoss + "','" + event_code + "','" + betfair_id + "','" + time.ToString(format) + "')  ", con);
                            sqlMPLS.ExecuteNonQuery();
                        }

                        Double NetAdminProfitLoss = AdminProfitLoss + TotalAdminProfit;
                        NetAdminProfitLoss = Math.Round(NetAdminProfitLoss, 3);
                        Double sumSuperAdPL = Math.Round(TotalAdminProfit, 3);
                        Double DebitAPL = 0;
                        Double CreditAPL = 0;

                        if (sumSuperAdPL < 0)
                        {
                            DebitAPL = Math.Abs(sumSuperAdPL);
                            CreditAPL = 0;
                        }
                        else
                        {
                            DebitAPL = 0;
                            CreditAPL = sumSuperAdPL;
                        }
                        SqlCommand sqlAdminPL = new SqlCommand("UPDATE Admin SET profit_loss ='" + NetAdminProfitLoss + "' WHERE id = '" + Ad_id + "' ", con);
                        sqlAdminPL.ExecuteNonQuery();
                        SqlCommand sqlAPLS = new SqlCommand("INSERT INTO admin_account_statements(admin_id,md_id,dist_id,user_id,acc_stat_type, description,remark, debit, credit, " +
                            " balance , event_id, market_id, created) VALUES ('" + Ad_id + "','" + MDL_id + "','" + DL_id + "','" + user_id + "','pl_coins','" + PLDescription + "','','" + DebitAPL + "','" + CreditAPL + "'," +
                            "'" + NetAdminProfitLoss + "','" + event_code + "','" + betfair_id + "','" + time.ToString(format) + "')  ", con);
                        sqlAPLS.ExecuteNonQuery();

                        SqlCommand sqlMarketdma = new SqlCommand("UPDATE markets SET dma_cal ='yes' WHERE dma_cal ='no' AND betfair_id = '" + betfair_id + "' AND event_code = '" + event_code + "' ", con);
                        int dqdj = sqlMarketdma.ExecuteNonQuery();
                        if (dqdj > 0)
                        {
                            ViewBag.message = market_name + "[Success]";
                            ViewBag.match_title = event_name + "[" + event_code + "]";
                            return View();
                        }
                        else
                        {
                            ViewBag.message = market_name + "[Error]";
                            ViewBag.match_title = event_name + "[" + event_code + "]";
                            return View();
                        }
                    }
                    else
                    {

                    }
                }
                con.Close();
            }

            return View();
        }
        public Double getAdminPL(int Ad_id)
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
        public Double getAdminCoinRate(int Ad_id)
        {
            Double pl_amount = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT coin_rate from admin where id='" + Ad_id + "'"))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        reader.Read();
                        pl_amount = (Double)reader["coin_rate"];
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                string fgg = ex.ToString();
            }

            return pl_amount;
        }

        public ActionResult AutoTP()
        {
            try
            {
                DateTime time = DateTime.Now;              // Use current time
                string format = "yyyy-MM-dd HH:mm:ss";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    SqlCommand sqlmatch = new SqlCommand();
                    sqlmatch.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlmatch.CommandText = "AutoTp";
                    //sqlmatch.Parameters.Add("@id", System.Data.SqlDbType.Int).Value="nik";
                    sqlmatch.Connection = con;
                    var reader_match = sqlmatch.ExecuteReader();
                    while (reader_match.Read())
                    {
                        string event_code = (string)reader_match["event_id"];
                        string betfair_id = (string)reader_match["betfair_id"];
                        string evv_name = "";

                        if (event_code == "20202020")
                        {
                            evv_name = "TeenPatiT20Result";

                            string match_title = evv_name + " ( " + event_code + " )";
                            try
                            {
                                HttpClient client = new HttpClient();
                                client.BaseAddress = new Uri("http://diamond8exch.com/");
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                try
                                {
                                    HttpResponseMessage response = client.GetAsync("CasinoData/" + evv_name).Result;
                                    if (response != null)
                                    {
                                        var products = response.Content.ReadAsStringAsync().Result;
                                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                                        var datares = responseJson["data"];
                                        for (int i = 0; i < datares.Count; i++)
                                        {
                                            if (datares[i]["mid"] == betfair_id)
                                            {
                                                int resultwin = datares[i]["result"];
                                                string SelectMatchWinner = resultwin.ToString();
                                                string message = RunnerSettleTP(betfair_id, event_code, SelectMatchWinner);
                                                ViewBag.message = message;
                                                ViewBag.match_title = match_title + " ( Round : " + datares[i]["mid"] + " )";
                                                return View();
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ViewBag.message = ex.ToString();
                                    ViewBag.match_title = "";
                                }
                            }
                            catch (TaskCanceledException ex)
                            {
                                ViewBag.message = ex.ToString();
                                ViewBag.match_title = "";
                            }
                        }
                        else
                        {
                            if (event_code == "30303030")
                            {
                                evv_name = "TeenPatiT20BResult";
                            }
                            else if (event_code == "40404040")
                            {
                                evv_name = "TeenPatiT20PokerResult";
                            }
                            else if (event_code == "50505050")
                            {
                                evv_name = "dt20Result";
                            }
                            else if (event_code == "51515151")
                            {
                                evv_name = "dtl20Result";
                            }
                            else if (event_code == "60606060")
                            {
                                evv_name = "Lucky7Result";
                            }
                            else if (event_code == "61616161")
                            {
                                evv_name = "lucky7BResult";
                            }
                            if (evv_name != "")
                            {
                                string match_title = evv_name + " ( " + event_code + " )";
                                try
                                {
                                    HttpClient client = new HttpClient();
                                    client.BaseAddress = new Uri("http://skyinplay99.com/");
                                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                    try
                                    {
                                        HttpResponseMessage response = client.GetAsync("CasinoData/" + evv_name).Result;
                                        if (response != null)
                                        {
                                            var products = response.Content.ReadAsStringAsync().Result;
                                            dynamic responseJson = JsonConvert.DeserializeObject(products);
                                            var datares = responseJson["data"];
                                            for (int i = 0; i < datares.Count; i++)
                                            {
                                                if (datares[i]["mid"] == betfair_id)
                                                {
                                                    int resultwin = datares[i]["result"];
                                                    string SelectMatchWinner = resultwin.ToString();
                                                    string message = RunnerSettleTP(betfair_id, event_code, SelectMatchWinner);
                                                    ViewBag.message = message;
                                                    ViewBag.match_title = match_title + " ( Round : " + datares[i]["mid"] + " )";
                                                    return View();
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ViewBag.message = ex.ToString();
                                        ViewBag.match_title = "";
                                    }
                                }
                                catch (TaskCanceledException ex)
                                {
                                    ViewBag.message = ex.ToString();
                                    ViewBag.match_title = "";
                                }

                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.ToString();
                ViewBag.match_title = "";
            }
            return View();
        }

        public string GetSportsNameFromEvent(string event_code)
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

        public string getSportID(string event_code)
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

        public string getEventName(string EveCodeM)
        {
            string MAtchTG = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT match_title FROM matches where event_code= '" + EveCodeM + "' "))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    MAtchTG = (string)reader["match_title"];
                    con.Close();
                }
            }
            return MAtchTG;
        }

        public ActionResult AutoTP7AB()
        {
            try
            {
                DateTime time = DateTime.Now;              // Use current time
                string format = "yyyy-MM-dd HH:mm:ss";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    SqlCommand sqlmatch = new SqlCommand();
                    sqlmatch.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlmatch.CommandText = "AutoTp";
                    //sqlmatch.Parameters.Add("@id", System.Data.SqlDbType.Int).Value="nik";
                    sqlmatch.Connection = con;
                    var reader_match = sqlmatch.ExecuteReader();
                    while (reader_match.Read())
                    {
                        string event_code = (string)reader_match["event_id"];
                        string betfair_id = (string)reader_match["betfair_id"];
                        string evv_name = "";

                        if (event_code == "60606060")
                        {
                            evv_name = "lucky7Result";
                        }
                        else if (event_code == "61616161")
                        {
                            evv_name = "lucky7BResult";
                        }
                        if (evv_name != "")
                        {
                            string match_title = evv_name + " ( " + event_code + " )";
                            try
                            {
                                HttpClient client = new HttpClient();
                                client.BaseAddress = new Uri("http://api_link.com/");
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                try
                                {
                                    HttpResponseMessage response = client.GetAsync("tpresult.php?type=" + evv_name).Result;
                                    if (response != null)
                                    {
                                        var products = response.Content.ReadAsStringAsync().Result;
                                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                                        var datares = responseJson["data"];
                                        for (int i = 0; i < datares.Count; i++)
                                        {
                                            if (datares[i]["mid"] == betfair_id)
                                            {
                                                int resultwin = datares[i]["result"];
                                                string SelectMatchWinner = resultwin.ToString();
                                                string message = RunnerSettleTP(betfair_id, event_code, SelectMatchWinner);
                                                ViewBag.message = message;
                                                ViewBag.match_title = match_title + " ( Round : " + datares[i]["mid"] + " )";
                                                return View();
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ViewBag.message = ex.ToString();
                                    ViewBag.match_title = "";
                                }
                            }
                            catch (TaskCanceledException ex)
                            {
                                ViewBag.message = ex.ToString();
                                ViewBag.match_title = "";
                            }

                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.ToString();
                ViewBag.match_title = "";
            }
            return View();
        }
        public ActionResult AutoTP7()
        {
            try
            {
                DateTime time = DateTime.Now;              // Use current time
                string format = "yyyy-MM-dd HH:mm:ss";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    SqlCommand sqlmatch = new SqlCommand();
                    sqlmatch.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlmatch.CommandText = "AutoTp";
                    //sqlmatch.Parameters.Add("@id", System.Data.SqlDbType.Int).Value="nik";
                    sqlmatch.Connection = con;
                    var reader_match = sqlmatch.ExecuteReader();
                    while (reader_match.Read())
                    {
                        string event_code = (string)reader_match["event_id"];
                        string betfair_id = (string)reader_match["betfair_id"];
                        string evv_name = "";

                        if (event_code == "20202020")
                        {
                            evv_name = "TeenPatiT20Result";

                            string match_title = evv_name + " ( " + event_code + " )";
                            try
                            {
                                HttpClient client = new HttpClient();
                                client.BaseAddress = new Uri("http://diamond8exch.com/");
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                try
                                {
                                    HttpResponseMessage response = client.GetAsync("CasinoData/" + evv_name).Result;
                                    if (response != null)
                                    {
                                        var products = response.Content.ReadAsStringAsync().Result;
                                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                                        var datares = responseJson["data"];
                                        for (int i = 0; i < datares.Count; i++)
                                        {
                                            if (datares[i]["mid"] == betfair_id)
                                            {
                                                int resultwin = datares[i]["result"];
                                                string SelectMatchWinner = resultwin.ToString();
                                                string message = RunnerSettleTP(betfair_id, event_code, SelectMatchWinner);
                                                ViewBag.message = message;
                                                ViewBag.match_title = match_title + " ( Round : " + datares[i]["mid"] + " )";
                                                return View();
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ViewBag.message = ex.ToString();
                                    ViewBag.match_title = "";
                                }
                            }
                            catch (TaskCanceledException ex)
                            {
                                ViewBag.message = ex.ToString();
                                ViewBag.match_title = "";
                            }
                        }
                        else
                        {
                            if (event_code == "30303030")
                            {
                                evv_name = "TeenPatiT20Result";
                            }
                            else if (event_code == "40404040")
                            {
                                evv_name = "TeenPatiT20PokerResult";
                            }
                            else if (event_code == "50505050")
                            {
                                evv_name = "dt20Result";
                            }
                            else if (event_code == "51515151")
                            {
                                evv_name = "dtl20Result";
                            }
                            /*else if (event_code == "60606060")
                            {
                                evv_name = "lucky7Result";
                            }
                            else if (event_code == "61616161")
                            {
                                evv_name = "lucky7BResult";
                            }*/
                            if (evv_name != "")
                            {
                                string match_title = evv_name + " ( " + event_code + " )";
                                try
                                {
                                    HttpClient client = new HttpClient();
                                    client.BaseAddress = new Uri("http://api_link.com/");
                                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                    try
                                    {
                                        HttpResponseMessage response = client.GetAsync("tpresult.php?type=" + evv_name).Result;
                                        if (response != null)
                                        {
                                            var products = response.Content.ReadAsStringAsync().Result;
                                            dynamic responseJson = JsonConvert.DeserializeObject(products);
                                            var datares = responseJson["data"];
                                            for (int i = 0; i < datares.Count; i++)
                                            {
                                                if (datares[i]["mid"] == betfair_id)
                                                {
                                                    int resultwin = datares[i]["result"];
                                                    string SelectMatchWinner = resultwin.ToString();
                                                    string message = RunnerSettleTP(betfair_id, event_code, SelectMatchWinner);
                                                    ViewBag.message = message;
                                                    ViewBag.match_title = match_title + " ( Round : " + datares[i]["mid"] + " )";
                                                    return View();
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ViewBag.message = ex.ToString();
                                        ViewBag.match_title = "";
                                    }
                                }
                                catch (TaskCanceledException ex)
                                {
                                    ViewBag.message = ex.ToString();
                                    ViewBag.match_title = "";
                                }

                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.ToString();
                ViewBag.match_title = "";
            }
            return View();
        }

        public ActionResult AutoTPSKY()
        {
            try
            {
                DateTime time = DateTime.Now;              // Use current time
                string format = "yyyy-MM-dd HH:mm:ss";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    SqlCommand sqlmatch = new SqlCommand("SELECT DISTINCT [betfair_id] , event_id FROM live_bet WHERE status='' AND odds_type='TP' ", con);
                    var reader_match = sqlmatch.ExecuteReader();
                    while (reader_match.Read())
                    {
                        string event_code = (string)reader_match["event_id"];
                        string betfair_id = (string)reader_match["betfair_id"];
                        string evv_name = "";
                        if (event_code == "30303030")
                        {
                            evv_name = "TeenPatiT20Result";

                            string match_title = evv_name + " ( " + event_code + " )";
                            try
                            {
                                HttpClient client = new HttpClient();
                                client.BaseAddress = new Uri("http://bet24exch.com/");
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                try
                                {
                                    HttpResponseMessage response = client.GetAsync("tpresult.php?id=" + betfair_id).Result;
                                    if (response != null)
                                    {
                                        string products = response.Content.ReadAsStringAsync().Result;
                                        if (products != "no data")
                                        {
                                            string SelectMatchWinner = products;
                                            string message = RunnerSettleTP(betfair_id, event_code, SelectMatchWinner);
                                            ViewBag.message = message;
                                            ViewBag.match_title = match_title + " ( Round : " + betfair_id + " )";
                                            return View();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ViewBag.message = ex.ToString();
                                    ViewBag.match_title = "";
                                }
                            }
                            catch (TaskCanceledException ex)
                            {
                                ViewBag.message = ex.ToString();
                                ViewBag.match_title = "";
                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.ToString();
                ViewBag.match_title = "";
            }
            return View();
        }
        public string RunnerSettleTP(string id, string event_code, string winner)
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
                if (event_code == "20202020" || event_code == "30303030")
                {
                    if (winner == "3")
                    {
                        winner = "2";
                    }
                }
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
                                        string nr_des = "";
                                        if (winner == "0")
                                        {
                                            nr_des = " [Tie]";
                                            if (event_code == "60606060" || event_code == "61616161")
                                            {
                                                NetWinLost = total_liability / 2;
                                                NetWinLost = -(Math.Round(NetWinLost));
                                            }
                                            else
                                            {
                                                NetWinLost = 0;
                                            }
                                        }
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
                                        Double adjust_bal = 0 + NetWinLost + total_liability;
                                        Double adjust_exp = 0 - total_liability;
                                        Double adjust_pl = NetWinLoss;
                                        if (nr_des != "")
                                        {
                                            SqlCommand sqlLiveBetLost = new SqlCommand("UPDATE live_bet SET status='tie' WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' ", con);
                                            sqlLiveBetLost.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand sqlLiveBetWon = new SqlCommand("UPDATE live_bet SET status='won' WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND runner_posi='" + winner + "' ", con);
                                            sqlLiveBetWon.ExecuteNonQuery();

                                            SqlCommand sqlLiveBetLost = new SqlCommand("UPDATE live_bet SET status='lost' WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND runner_posi!='" + winner + "' ", con);
                                            sqlLiveBetLost.ExecuteNonQuery();
                                        }

                                        SqlCommand sqlUserBalanceUpdate = new SqlCommand("UPDATE users_client SET balance = balance + '" + adjust_bal + "' , exposure = exposure + '" + adjust_exp + "' , profit_loss = profit_loss + '" + adjust_pl + "' WHERE id = '" + user_id + "' ", con);
                                        sqlUserBalanceUpdate.ExecuteNonQuery();

                                        string AccStatementDescription = eventSportName + " / " + EventName + " / " + market_name + nr_des;

                                        SqlCommand sqlInsertUser = new SqlCommand("INSERT INTO user_account_statements (admin_id , md_id , dist_id , user_id , acc_stat_type ,  description ,  remark ,  debit ,  credit ,  balance ,  market_id ,  event_id ,  created , match_odds ) VALUES ('" + Admin_id + "','" + UserMDId + "','" + UserDistId + "','" + user_id + "','pl_coins','" + AccStatementDescription + "','','" + debit_balance + "','" + credit_balance + "','" + user_updated_net_balance + "','" + betfair_id + "','" + event_code + "','" + time.ToString(format) + "','2')", con);
                                        sqlInsertUser.ExecuteNonQuery();
                                    }
                                }

                                SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='1' , result_value='" + winner + "' WHERE status='deactivate' AND betfair_id = '" + betfair_id + "' AND event_code='" + event_code + "' AND result_value='' AND market_settle!='1'  ", con);
                                sqlMarketSettleUpdate.ExecuteNonQuery();

                                con.Close();
                                return "Market Settle SuccessFully";
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
                ReturnMessage = ex.ToString();
            }
            return ReturnMessage;
        }
        public string TeenRevert(string id, string event_code, string winner)
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
                if (event_code == "20202020" || event_code == "30303030")
                {
                    if (winner == "3")
                    {
                        winner = "2";
                    }
                }
                if (event_code != "" && betfair_id != "" && winner != "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        SqlCommand sqlFancy = new SqlCommand("SELECT DISTINCT user_id FROM live_bet WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_code + "' ", con);
                        var rowFancy = sqlFancy.ExecuteReader();
                        if (!rowFancy.HasRows)
                        {
                            SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='' ,status='activate' , result_value='' WHERE status='deactivate' AND result_value!='' AND market_settle='1' AND betfair_id = '" + betfair_id + "' AND event_code = '" + event_code + "' ", con);
                            int sqlMarketSettleUpdatedone = sqlMarketSettleUpdate.ExecuteNonQuery();
                            if (sqlMarketSettleUpdatedone > 0)
                            {
                                SqlCommand cmd4 = new SqlCommand("UPDATE runner_market SET status='activate' where market_id='" + betfair_id + "' ", con);
                                cmd4.ExecuteNonQuery();
                                con.Close();
                                return "Market Reverted SuccessFully";
                            }
                            else
                            {
                                con.Close();
                                return "Market Already Reverted";
                            }
                        }
                        else
                        {
                            SqlCommand marketde = new SqlCommand("UPDATE markets SET status='activate' WHERE status='deactivate' AND market_settle='1' AND betfair_id = '" + betfair_id + "' AND event_code = '" + event_code + "' ", con);
                            int sqlMarketUpdatedone = marketde.ExecuteNonQuery();
                            SqlCommand cmd41 = new SqlCommand("UPDATE runner_market SET status='activate' where market_id='" + betfair_id + "' ", con);
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
                                        availableLiability = UserLiability + total_liability;

                                        Double NetWinLost = -(TotalPLWon - TotalStakesLost);
                                        string nr_des = "";
                                        if (winner == "0")
                                        {
                                            nr_des = " [Tie]";
                                            if (event_code == "60606060" || event_code == "61616161")
                                            {
                                                NetWinLost = -(total_liability / 2);
                                                NetWinLost = (Math.Round(NetWinLost));
                                            }
                                            else
                                            {
                                                NetWinLost = 0;
                                            }
                                        }
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

                                        Double ClientNetBalance = UserBalance + NetWinLost - total_liability;
                                        Double user_updated_net_balance = ClientNetBalance + availableLiability;

                                        Double NetWinLoss = NetWinLost;
                                        Double NetClientPL = ClientPL + NetWinLoss;
                                        Double adjust_bal = 0 + NetWinLost - total_liability;
                                        Double adjust_exp = 0 + total_liability;
                                        Double adjust_pl = NetWinLoss;
                                        if (nr_des != "")
                                        {
                                            SqlCommand sqlLiveBetLost = new SqlCommand("UPDATE live_bet SET status='' WHERE user_id='" + user_id + "' AND betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' ", con);
                                            sqlLiveBetLost.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand sqlLiveBetWon = new SqlCommand("UPDATE live_bet SET status='' WHERE user_id='" + user_id + "' AND betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND runner_posi='" + winner + "' ", con);
                                            sqlLiveBetWon.ExecuteNonQuery();

                                            SqlCommand sqlLiveBetLost = new SqlCommand("UPDATE live_bet SET status='' WHERE user_id='" + user_id + "' AND betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND runner_posi!='" + winner + "' ", con);
                                            sqlLiveBetLost.ExecuteNonQuery();
                                        }

                                        SqlCommand sqlUserBalanceUpdate = new SqlCommand("UPDATE users_client SET balance = balance + '" + adjust_bal + "' , exposure = exposure + '" + adjust_exp + "' , profit_loss = profit_loss + '" + adjust_pl + "' WHERE id = '" + user_id + "' ", con);
                                        sqlUserBalanceUpdate.ExecuteNonQuery();

                                        string AccStatementDescription = eventSportName + " / " + EventName + " / " + market_name + nr_des + " [Settling Revert]";

                                        SqlCommand sqlInsertUser = new SqlCommand("INSERT INTO user_account_statements (admin_id , md_id , dist_id , user_id , acc_stat_type ,  description ,  remark ,  debit ,  credit ,  balance ,  market_id ,  event_id ,  created , match_odds ) VALUES ('" + Admin_id + "','" + UserMDId + "','" + UserDistId + "','" + user_id + "','pl_coins','" + AccStatementDescription + "','','" + debit_balance + "','" + credit_balance + "','" + user_updated_net_balance + "','" + betfair_id + "','" + event_code + "','" + time.ToString(format) + "','2')", con);
                                        sqlInsertUser.ExecuteNonQuery();
                                    }
                                }

                                SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='' , result_value='' WHERE status='activate' AND betfair_id = '" + betfair_id + "' AND event_code='" + event_code + "' AND result_value!='' AND market_settle='1'  ", con);
                                sqlMarketSettleUpdate.ExecuteNonQuery();

                                con.Close();
                                return "Market Reverted SuccessFully";
                            }
                            else
                            {
                                con.Close();
                                return "Erro in Market Revert";
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
                ReturnMessage = ex.ToString();
            }
            return ReturnMessage;
        }

        public string RunnerSettleTPR(string id, string event_code, string winner)
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
                /*if (event_code == "20202020" || event_code == "30303030")
                {
                    if (winner == "3")
                    {
                        winner = "2";
                    }
                }*/
                if (event_code != "" && betfair_id != "" && winner != "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();
                        SqlCommand sqlFancy = new SqlCommand("SELECT DISTINCT user_id FROM live_bet WHERE betfair_id = '" + betfair_id + "' AND event_id = '" + event_code + "' ", con);
                        var rowFancy = sqlFancy.ExecuteReader();
                        if (!rowFancy.HasRows)
                        {
                            SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='0' ,status='activate' , result_value='' WHERE status='deactivate' AND result_value!='' AND market_settle='1' AND betfair_id = '" + betfair_id + "' AND event_code = '" + event_code + "' ", con);
                            int sqlMarketSettleUpdatedone = sqlMarketSettleUpdate.ExecuteNonQuery();
                            if (sqlMarketSettleUpdatedone > 0)
                            {
                                SqlCommand cmd4 = new SqlCommand("UPDATE runner_market SET status='activate' where market_id='" + betfair_id + "' ", con);
                                cmd4.ExecuteNonQuery();
                                con.Close();
                                return "Market Revert SuccessFully";
                            }
                            else
                            {
                                con.Close();
                                return "Market Already reverted";
                            }
                        }
                        else
                        {
                            SqlCommand marketde = new SqlCommand("UPDATE markets SET status='activate' WHERE status='deactivate' AND market_settle='1' AND betfair_id = '" + betfair_id + "' AND event_code = '" + event_code + "' ", con);
                            int sqlMarketUpdatedone = marketde.ExecuteNonQuery();
                            SqlCommand cmd41 = new SqlCommand("UPDATE runner_market SET status='activate' where market_id='" + betfair_id + "' ", con);
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
                                        availableLiability = UserLiability + total_liability;

                                        Double NetWinLost = -(TotalPLWon - TotalStakesLost);
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

                                        Double ClientNetBalance = UserBalance + NetWinLost - total_liability;
                                        Double user_updated_net_balance = ClientNetBalance + availableLiability;

                                        Double NetWinLoss = NetWinLost;
                                        Double NetClientPL = ClientPL + NetWinLoss;
                                        Double adjust_bal = 0 + NetWinLost - total_liability;
                                        Double adjust_exp = 0 + total_liability;
                                        Double adjust_pl = NetWinLoss;
                                        //return "wdvfdsf";
                                        SqlCommand sqlLiveBetWon = new SqlCommand("UPDATE live_bet SET status='' WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND runner_posi='" + winner + "' ", con);
                                        sqlLiveBetWon.ExecuteNonQuery();

                                        SqlCommand sqlLiveBetLost = new SqlCommand("UPDATE live_bet SET status='' WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND runner_posi!='" + winner + "' ", con);
                                        sqlLiveBetLost.ExecuteNonQuery();

                                        SqlCommand sqlUserBalanceUpdate = new SqlCommand("UPDATE users_client SET balance = balance + '" + adjust_bal + "' , exposure = exposure + '" + adjust_exp + "' , profit_loss = profit_loss + '" + adjust_pl + "' WHERE id = '" + user_id + "' ", con);
                                        sqlUserBalanceUpdate.ExecuteNonQuery();

                                        string AccStatementDescription = eventSportName + " / " + EventName + " / " + market_name + " (Wrong Dr/Cr)";

                                        SqlCommand sqlInsertUser = new SqlCommand("INSERT INTO user_account_statements (admin_id , md_id , dist_id , user_id , acc_stat_type ,  description ,  remark ,  debit ,  credit ,  balance ,  market_id ,  event_id ,  created , match_odds ) VALUES ('" + Admin_id + "','" + UserMDId + "','" + UserDistId + "','" + user_id + "','pl_coins','" + AccStatementDescription + "','','" + debit_balance + "','" + credit_balance + "','" + user_updated_net_balance + "','" + betfair_id + "','" + event_code + "','" + time.ToString(format) + "','2')", con);
                                        sqlInsertUser.ExecuteNonQuery();
                                    }
                                }

                                SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='0' , result_value='' WHERE status='activate' AND betfair_id = '" + betfair_id + "' AND event_code='" + event_code + "' AND result_value!='' AND market_settle='1'  ", con);
                                sqlMarketSettleUpdate.ExecuteNonQuery();

                                con.Close();
                                return "Market Settle SuccessFully";
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
                ReturnMessage = ex.ToString();
            }
            return ReturnMessage;
        }
        public ActionResult AutoTPTie()
        {
            string SelectMatchWinner = "No Result";
            try
            {
                if (Request.QueryString["ev_code"] != null && Request.QueryString["bid"] != null && Request.QueryString["winner"] != null)
                {
                    string event_code = Request.QueryString["ev_code"];
                    string betfair_id = Request.QueryString["bid"];
                    string evv_name = "TeenPatiT20Result";
                    string match_title = evv_name + " ( " + event_code + " )";
                    string winner = Request.QueryString["winner"];
                    if (winner == "0")
                    {
                        SelectMatchWinner = "No Result";
                        string message = RunnerSettleTPTie(betfair_id, event_code, SelectMatchWinner);
                        ViewBag.message = message;
                        ViewBag.match_title = match_title + " ( Round : " + betfair_id + " )";
                        return View();
                    }
                    else
                    {
                        SelectMatchWinner = winner;
                        string message = RunnerSettleTP(betfair_id, event_code, SelectMatchWinner);
                        ViewBag.message = message;
                        ViewBag.match_title = match_title + " ( Round : " + betfair_id + " )";
                        return View();
                    }
                }
                else
                {
                    ViewBag.message = "Enter details";
                    ViewBag.match_title = "";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.ToString();
                ViewBag.match_title = "";
            }

            return View();

        }
        public string RunnerSettleTPTie(string id, string event_code, string winner)
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

                                        SqlCommand sqlFancyExch = new SqlCommand("SELECT COALESCE(SUM(stakes),0) as tot_stak FROM live_bet WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' AND user_id='" + uid + "' ", con);
                                        var dataFancyExch = sqlFancyExch.ExecuteReader();
                                        if (dataFancyExch.HasRows)
                                        {
                                            dataFancyExch.Read();
                                            total_liability = (Double)dataFancyExch["tot_stak"];
                                        }
                                        availableLiability = UserLiability - total_liability;

                                        Double NetWinLost = 0;
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
                                        Double adjust_bal = 0 + NetWinLost + total_liability;
                                        Double adjust_exp = 0 - total_liability;
                                        Double adjust_pl = NetWinLoss;
                                        SqlCommand sqlLiveBetWon = new SqlCommand("UPDATE live_bet SET status='tie' WHERE betfair_id='" + betfair_id + "' AND event_id='" + event_code + "' ", con);
                                        sqlLiveBetWon.ExecuteNonQuery();

                                        SqlCommand sqlUserBalanceUpdate = new SqlCommand("UPDATE users_client SET balance = balance + '" + adjust_bal + "' , exposure = exposure + '" + adjust_exp + "' , profit_loss = profit_loss + '" + adjust_pl + "' WHERE id = '" + user_id + "' ", con);
                                        sqlUserBalanceUpdate.ExecuteNonQuery();

                                        string AccStatementDescription = eventSportName + " / " + EventName + " / " + market_name + "(No Result)";

                                        SqlCommand sqlInsertUser = new SqlCommand("INSERT INTO user_account_statements (admin_id , md_id , dist_id , user_id , acc_stat_type ,  description ,  remark ,  debit ,  credit ,  balance ,  market_id ,  event_id ,  created , match_odds ) VALUES ('" + Admin_id + "','" + UserMDId + "','" + UserDistId + "','" + user_id + "','pl_coins','" + AccStatementDescription + "','','" + debit_balance + "','" + credit_balance + "','" + user_updated_net_balance + "','" + betfair_id + "','" + event_code + "','" + time.ToString(format) + "','2')", con);
                                        sqlInsertUser.ExecuteNonQuery();
                                    }
                                }

                                SqlCommand sqlMarketSettleUpdate = new SqlCommand("UPDATE markets SET market_settle='1' , result_value='" + winner + "' WHERE status='deactivate' AND betfair_id = '" + betfair_id + "' AND event_code='" + event_code + "' AND result_value='' AND market_settle!='1'  ", con);
                                sqlMarketSettleUpdate.ExecuteNonQuery();

                                con.Close();
                                return "Market Settle SuccessFully";
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
                ReturnMessage = ex.ToString();
            }
            return ReturnMessage;
        }

        public ActionResult AutoTPOn()
        {
            try
            {
                DateTime time = DateTime.Now;              // Use current time
                string format = "yyyy-MM-dd HH:mm:ss";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    SqlCommand sqlmatch = new SqlCommand();
                    sqlmatch.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlmatch.CommandText = "AutoTp";
                    //sqlmatch.Parameters.Add("@id", System.Data.SqlDbType.Int).Value="nik";
                    sqlmatch.Connection = con;
                    var reader_match = sqlmatch.ExecuteReader();
                    if (reader_match.HasRows)
                    {
                        while (reader_match.Read())
                        {
                            string event_code = (string)reader_match["event_id"];
                            string betfair_id = (string)reader_match["betfair_id"];
                            string evv_name = "";
                            if (event_code == "31313131")
                            {
                                evv_name = "TeenPatiOneDayResult";

                                string match_title = evv_name + " ( " + event_code + " )";
                                try
                                {
                                    HttpClient client = new HttpClient();
                                    client.BaseAddress = new Uri("http://skyinplay99.com/");
                                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                    try
                                    {
                                        HttpResponseMessage response = client.GetAsync("CasinoData/TeenPatiMuflisResult").Result;
                                        if (response != null)
                                        {
                                            var products = response.Content.ReadAsStringAsync().Result;
                                            dynamic responseJson = JsonConvert.DeserializeObject(products);
                                            var datares = responseJson["data"];
                                            for (int i = 0; i < datares.Count; i++)
                                            {
                                                if (datares[i]["mid"] == betfair_id)
                                                {
                                                    int SelectMatchWinner = datares[i]["result"];
                                                    string market_name = "Round : " + datares[i]["mid"];
                                                    if (SelectMatchWinner != 0)
                                                    {
                                                        if(SelectMatchWinner == 3)
                                                        {
                                                            SelectMatchWinner = 2;
                                                        }
                                                        string message = RunnerSettle(event_code, betfair_id, SelectMatchWinner, market_name);
                                                        ViewBag.message = message;
                                                        ViewBag.match_title = match_title + " ( Round : " + datares[i]["mid"] + " )";
                                                        return View();
                                                    }
                                                    else if (SelectMatchWinner == 0)
                                                    {
                                                        SelectMatchWinner = 9999;
                                                        string message = RunnerSettleTie(event_code, betfair_id, SelectMatchWinner, market_name);
                                                        ViewBag.message = message;
                                                        ViewBag.match_title = match_title;
                                                        return View();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ViewBag.message = ex.ToString();
                                        ViewBag.match_title = "";
                                    }
                                }
                                catch (TaskCanceledException ex)
                                {
                                    ViewBag.message = ex.ToString();
                                    ViewBag.match_title = "";
                                }
                            }
                            else if(event_code == "5676756767")
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
                                string match_title = evv_name + " ( " + event_code + " )";
                                for (int i = 0; i < 10; i++)
                                {
                                    string data = response.Content;
                                    JObject jObject = JObject.Parse(data);
                                    string round_id = jObject.SelectToken("data["+i+"].round_id").ToString();
                                    if (round_id == betfair_id)
                                    {
                                        int SelectMatchWinner = 0;
                                        string market_name = "Round : " + round_id;
                                        string player_name = jObject.SelectToken("data[" + i + "].player_name").ToString();
                                        if (player_name != "")
                                        {
                                            if(player_name == "A")
                                            {
                                                SelectMatchWinner = 1;
                                            }
                                            else if (player_name == "B")
                                            {
                                                SelectMatchWinner = 2;
                                            }
                                            string message = RunnerSettle(event_code, betfair_id, SelectMatchWinner, market_name);
                                            ViewBag.message = message;
                                            ViewBag.match_title = match_title + " ( Round : " + round_id + " )";
                                            return View();
                                        }
                                        else if (SelectMatchWinner == 0)
                                        {
                                            SelectMatchWinner = 9999;
                                            string message = RunnerSettleTie(event_code, betfair_id, SelectMatchWinner, market_name);
                                            ViewBag.message = message;
                                            ViewBag.match_title = match_title;
                                            return View();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                try
                                {
                                    if (event_code == "52525252")
                                    {
                                        evv_name = "dt6Result";
                                    }
                                    else if (event_code == "41414141")
                                    {
                                        evv_name = "PokerResult";
                                    }

                                    if (evv_name != "")
                                    {
                                        string match_title = evv_name + " ( " + event_code + " )";
                                        try
                                        {
                                            HttpClient client = new HttpClient();
                                            client.BaseAddress = new Uri("http://api_link.com/");
                                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                            try
                                            {
                                                HttpResponseMessage response = client.GetAsync("tpresult.php?type=" + evv_name).Result;
                                                if (response != null)
                                                {
                                                    var products = response.Content.ReadAsStringAsync().Result;
                                                    dynamic responseJson = JsonConvert.DeserializeObject(products);
                                                    var datares = responseJson["data"];
                                                    for (int i = 0; i < datares.Count; i++)
                                                    {
                                                        if (datares[i]["mid"] == betfair_id)
                                                        {
                                                            int SelectMatchWinner = datares[i]["result"];
                                                            string market_name = "Round : " + datares[i]["mid"];
                                                            if (SelectMatchWinner != 0)
                                                            {
                                                                if (event_code == "41414141")
                                                                {
                                                                    if (SelectMatchWinner == 11)
                                                                    {
                                                                        SelectMatchWinner = 1;
                                                                    }
                                                                    else if (SelectMatchWinner == 21)
                                                                    {
                                                                        SelectMatchWinner = 2;
                                                                    }
                                                                }
                                                                string message = RunnerSettle(event_code, betfair_id, SelectMatchWinner, market_name);
                                                                ViewBag.message = message;
                                                                ViewBag.match_title = match_title + " ( Round : " + datares[i]["mid"] + " )";
                                                                return View();
                                                            }
                                                            else if (SelectMatchWinner == 0)
                                                            {
                                                                SelectMatchWinner = 9999;
                                                                string message = RunnerSettleTie(event_code, betfair_id, SelectMatchWinner, market_name);
                                                                ViewBag.message = message;
                                                                ViewBag.match_title = match_title;
                                                                return View();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                ViewBag.message = ex.ToString();
                                                ViewBag.match_title = "";
                                            }
                                        }
                                        catch (TaskCanceledException ex)
                                        {
                                            ViewBag.message = ex.ToString();
                                            ViewBag.match_title = "";
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.ToString();
                ViewBag.match_title = "";
            }
            return View();
        }

        public string eventttt(string sportsid, string betfair_id)
        {
            string hu = "";
            try
            {
                DateTime time = DateTime.Now;              // Use current time
                string format = "yyyy-MM-dd HH:mm:ss";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    con.Open();
                    SqlCommand sqlmatch = new SqlCommand("MarketName", con);
                    sqlmatch.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlmatch.Parameters.Add("event_code", System.Data.SqlDbType.NVarChar).Value = sportsid;
                    sqlmatch.Parameters.Add("betfair_id", System.Data.SqlDbType.NVarChar).Value = betfair_id;
                    sqlmatch.Connection = con;
                    var reader_match = sqlmatch.ExecuteReader();
                    if (reader_match.HasRows)
                    {
                        reader_match.Read();
                        hu = (string)reader_match["match_title"];
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.ToString();
                ViewBag.match_title = "";
            }
            return hu;
        }

    }
}