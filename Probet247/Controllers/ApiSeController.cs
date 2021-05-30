using RBetfair.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Probet247.Controllers
{
    public class ApiSeController : Controller
    {
        // GET: ApiSe
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SessionSetR()
        {
            string data = Request.QueryString["data"];
            List<NotSettleMN> messages = new List<NotSettleMN>();
            messages = GetOtherMarketsName(data);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<NotSettleMN> GetOtherMarketsName(string Event_code)
        {
            var messages = new List<NotSettleMN>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    string query = "select market_name,event_code,betfair_id,x_check_type from markets where type='sess' and status='activate' and event_code='" + Event_code + "'";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        SqlDataReader dr;
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            string market_name = (string)dr["market_name"];
                            string event_code = (string)dr["event_code"];
                            string betfair_id = (string)dr["betfair_id"];
                            string type = (string)dr["x_check_type"];

                            messages.Add(item: new NotSettleMN
                            {
                                Marketname = market_name,
                                event_code = event_code,
                                betfair_id = betfair_id,
                                type = type
                            });
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return messages;
        }


        public JsonResult SessionDetail()
        {
            List<NotSettleMN> messages = new List<NotSettleMN>();
            messages = SessionDetail1();
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<NotSettleMN> SessionDetail1()
        {
            var messages = new List<NotSettleMN>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    string query = "select sport_id,created,market_name,event_code,betfair_id,x_check_type from markets where type='sess' and status='activate' and sport_id!='4' ";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        SqlDataReader dr;
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            string market_name = (string)dr["market_name"];
                            string event_code = (string)dr["event_code"];
                            string event_name = FunctionDataController.getEventName(event_code);
                            string betfair_id = (string)dr["betfair_id"];
                            string type = (string)dr["x_check_type"];
                            string sport_id = (string)dr["sport_id"];
                            DateTime created = (DateTime)dr["created"];
                            string format = "yyyy-MM-dd HH:mm:ss";

                            messages.Add(item: new NotSettleMN
                            {
                                Marketname = market_name,
                                event_code = event_code,
                                betfair_id = betfair_id,
                                event_name = event_name,
                                type = type,
                                sport_id = sport_id,
                                created = created.ToString(format),
                            });
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return messages;
        }

        public JsonResult x_code_data()
        {
            string data = Request.QueryString["data"];
            List<NotSettleMN> messages = new List<NotSettleMN>();
            messages = Get_X_Code_Data(data);
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public List<NotSettleMN> Get_X_Code_Data(string Event_code)
        {
            var messages = new List<NotSettleMN>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    string query = "select x_code,x_type from matches where event_code='" + Event_code + "'";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        SqlDataReader dr;
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            string event_code = (string)dr["x_code"];
                            string type = (string)dr["x_type"];

                            messages.Add(item: new NotSettleMN
                            {
                                event_code = event_code,
                                type = type
                            });
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return messages;
        }

        public string DeleteMatches()
        {
            string ygy = "";
            try
            {
                DateTime time = DateTime.Now;
                string format = "yyyy-MM-dd HH:mm:ss";
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("select TOP(1000) event_code,betfair_id from matches where bet_data='no' AND status!='OPEN' AND sport_id not in(567,7888) AND match_time < '" + time.ToString(format) + "' order by match_time ", con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string event_code = (string)reader["event_code"];
                            string betfair_id = (string)reader["betfair_id"];
                            using (var cmdLB = new SqlCommand("select id from live_bet where event_id='" + event_code + "'", con))
                            {
                                var readerLB = cmdLB.ExecuteReader();
                                if (!readerLB.HasRows)
                                {

                                    SqlCommand sqldeleteRuMarkets = new SqlCommand("delete from runner_market  WHERE market_id='" + betfair_id + "' ", con);
                                    sqldeleteRuMarkets.ExecuteNonQuery();

                                    SqlCommand sqldeleteMarkets = new SqlCommand("delete from markets  WHERE event_code='" + event_code + "' ", con);
                                    sqldeleteMarkets.ExecuteNonQuery();

                                    SqlCommand sqldeleteMatches = new SqlCommand("delete from matches  WHERE event_code='" + event_code + "' ", con);
                                    sqldeleteMatches.ExecuteNonQuery();

                                    ygy += event_code + ",";
                                }
                                else
                                {
                                    SqlCommand sqlupdateMatches = new SqlCommand("UPDATE matches set bet_data='yes' WHERE event_code='" + event_code + "' ", con);
                                    sqlupdateMatches.ExecuteNonQuery();
                                }
                            }
                        }

                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ygy = ex.ToString();
            }

            return ygy;
        }

        public String DeleteMarkets()
        {
            string ygy = "";
            try
            {
                string creat_t = Request.QueryString["creat_t"];
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("select TOP(500) betfair_id from markets where result_value='' AND market_settle = '' AND status='activate' AND sport_id = 7888 AND created < '" + creat_t + "' order by created ", con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string betfair_id = (string)reader["betfair_id"];
                                using (var cmdLB = new SqlCommand("select id from live_bet where betfair_id='" + betfair_id + "'", con))
                                {
                                    var readerLB = cmdLB.ExecuteReader();
                                    if (!readerLB.HasRows)
                                    {

                                        SqlCommand sqldeleteRuMarkets = new SqlCommand("delete from runner_market  WHERE market_id='" + betfair_id + "' ", con);
                                        sqldeleteRuMarkets.ExecuteNonQuery();

                                        SqlCommand sqldeleteMarkets = new SqlCommand("delete from markets  WHERE betfair_id='" + betfair_id + "' ", con);
                                        sqldeleteMarkets.ExecuteNonQuery();

                                        ygy += betfair_id + ",";
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
                ygy = ex.ToString();
            }

            return ygy;
        }
        public string is_tv(string cric_id)
        {
            string tv_ch = "0";
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("select tv_ch from matches where cric_id='" + cric_id + "' ", con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            tv_ch = "1";
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                tv_ch = "0";
            }
            return tv_ch;
        }

    }
}