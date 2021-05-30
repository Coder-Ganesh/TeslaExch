using BertfairLive1.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Probet247.Controllers
{
    public class CheckController : Controller
    {

        // GET: Check
        private SqlConnection con;
        private SqlConnection con2;
        DateTime CurrentTimes = DateTime.Now;

        // GET: Check
        public ActionResult Index()
        {
            return View();
        }
        private void connection2()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            con2 = new SqlConnection(constr);
        }
        [HttpPost]
        public ActionResult BetPlaceM(BetPlaceD obj)
        {
            string GetBetfairId = "";
            string GetEventCode = "";
            string GetstackValue = "";
            int Getindex = 999;
            string GetBoxtype = "";
            string GetOdds = "";
            string GetRunnerName = "";
            string GetMarketName = "";

            string SendResponse = "Error In Submitting Bet";
            GetBetfairId = obj.BetfairId;
            GetEventCode = obj.EventCode;
            GetstackValue = obj.stackValue;
            Getindex = obj.RunnerIndex;
            GetBoxtype = obj.BoxType;
            GetOdds = obj.OddsValue;
            GetRunnerName = obj.RunnerNameget;
            GetMarketName = obj.MarketName;
            float GetOddsFloat = float.Parse(GetOdds, CultureInfo.InvariantCulture.NumberFormat);
            string ad_id = (string)System.Web.HttpContext.Current.Session["AdminID"];
            string spppID = GetsportEventId(GetEventCode);
            int delay_time = 1000;
            if (ad_id != "14")
            {
                delay_time = GetMODelay(GetEventCode);
            }
           
            if(GetOdds =="0" || GetOdds == "")
            {
                SendResponse = "Error in place BET";
                return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
            }
            Thread.Sleep(delay_time);
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = client.GetAsync("exchange/odds/market/2/29278528/" + GetBetfairId).Result;
                    if (response != null)
                    {
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        Double totalMatched = responseJson[0].totalMatched;
                        string SpoRTIDS = FunctionDataController.GetEventTypeId(GetEventCode);
                        if (SpoRTIDS != "4")
                        {
                            if (totalMatched < 60 && GetMarketName != "Match Odds")
                            {
                                SendResponse = "Low liquidity Bet not Allow Can't Place Bet";
                                return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        var GetRunner = responseJson[0].runners;
                        int CheckRunner = GetRunner.Count;

                        string MainStatus = responseJson[0].status;
                        string EvnetMarketId = responseJson[0].marketId;

                        if (MainStatus == "OPEN")
                        {
                            int GetfieldPos = Getindex - 1;
                            string MatchStatus = responseJson[0].runners[GetfieldPos].status;
                            if (MatchStatus == "ACTIVE")
                            {

                                if (GetBoxtype == "back")
                                {
                                    if (GetRunner[GetfieldPos].ex.availableToBack.Count > 0)
                                    {
                                        float CheckOutRate = GetRunner[GetfieldPos].ex.availableToBack[0].price;
                                        if (GetOddsFloat <= CheckOutRate)
                                        {
                                            if (GetMarketName == "To Win the Toss")
                                            {
                                                SendResponse = PlaceBetToss(CheckOutRate, GetBetfairId, GetEventCode, GetstackValue,
                                                Getindex, GetBoxtype, GetOdds, GetRunnerName, GetMarketName);
                                            }
                                            else
                                            {
                                                SendResponse = PlaceBet(CheckOutRate, GetBetfairId, GetEventCode, GetstackValue,
                                                Getindex, GetBoxtype, GetOdds, GetRunnerName, GetMarketName);
                                            }
                                        }
                                        else
                                        {
                                            SendResponse = "Not Equal";
                                        }
                                    }
                                }
                                else if (GetBoxtype == "lay")
                                {
                                    if (GetRunner[GetfieldPos].ex.availableToLay.Count > 0)
                                    {
                                        float CheckOutRate = GetRunner[GetfieldPos].ex.availableToLay[0].price;
                                        if (GetOddsFloat >= CheckOutRate)
                                        {
                                            if (GetMarketName == "To Win the Toss")
                                            {
                                                SendResponse = PlaceBetToss(CheckOutRate, GetBetfairId, GetEventCode, GetstackValue,
                                                Getindex, GetBoxtype, GetOdds, GetRunnerName, GetMarketName);
                                            }
                                            else
                                            {
                                                SendResponse = PlaceBet(CheckOutRate, GetBetfairId, GetEventCode, GetstackValue,
                                                Getindex, GetBoxtype, GetOdds, GetRunnerName, GetMarketName);
                                            }
                                        }
                                        else
                                        {
                                            SendResponse = "Not Equal";
                                        }
                                    }
                                }
                                else
                                {
                                }
                                if (SendResponse == "Equal")
                                {

                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    SendResponse = "Error in place bet";
                    return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (TaskCanceledException ex)
            {
                SendResponse = "Error in place bet";
                return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult BetPlaceMGS(BetPlaceD obj)
        {
            string GetBetfairId = "";
            string GetEventCode = "";
            string GetstackValue = "";
            int Getindex = 999;
            string GetBoxtype = "";
            string GetOdds = "";
            string GetRunnerName = "";
            string GetMarketName = "";

            string SendResponse = "Error In Submitting Bet";
            GetBetfairId = obj.BetfairId;
            GetEventCode = obj.EventCode;
            GetstackValue = obj.stackValue;
            Getindex = obj.RunnerIndex;
            GetBoxtype = obj.BoxType;
            GetOdds = obj.OddsValue;
            GetRunnerName = obj.RunnerNameget;
            GetMarketName = obj.MarketName;
            float GetOddsFloat = float.Parse(GetOdds, CultureInfo.InvariantCulture.NumberFormat);
            int delay_time = GetMODelay(GetEventCode);
            Thread.Sleep(delay_time);
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = client.GetAsync("sky_mo.php?event_id=" + GetEventCode).Result;
                    if (response != null)
                    {
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        Double totalMatched = 100000;//responseJson[0].totalMatched;
                        string SpoRTIDS = FunctionDataController.GetEventTypeId(GetEventCode);
                        var GetRunner = responseJson.runner;
                        int CheckRunner = GetRunner.Count;

                        string MainStatus = responseJson.status;
                        string EvnetMarketId = responseJson.marketId;

                        if (MainStatus == "OPEN")
                        {
                            int GetfieldPos = Getindex - 1;
                            if (GetBoxtype == "back")
                            {
                                float CheckOutRate = GetRunner[GetfieldPos].rate1;
                                if (GetOddsFloat <= CheckOutRate)
                                {
                                    SendResponse = PlaceBet(CheckOutRate, GetBetfairId, GetEventCode, GetstackValue,
                                        Getindex, GetBoxtype, GetOdds, GetRunnerName, GetMarketName);
                                }
                                else
                                {
                                    SendResponse = "Not Equal";
                                }
                            }
                            else if (GetBoxtype == "lay")
                            {
                                float CheckOutRate = GetRunner[GetfieldPos].rate2;
                                if (GetOddsFloat >= CheckOutRate)
                                {
                                    SendResponse = PlaceBet(CheckOutRate, GetBetfairId, GetEventCode, GetstackValue,
                                        Getindex, GetBoxtype, GetOdds, GetRunnerName, GetMarketName);
                                }
                                else
                                {
                                    SendResponse = "Not Equal";
                                }
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            SendResponse = "market Suspended";
                        }

                    }
                }
                catch (Exception ex)
                {
                    SendResponse = "Error in place bet";
                    return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (TaskCanceledException ex)
            {
                SendResponse = "Error in place bet";
                return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BetPlaceBM(BetPlaceD obj)
        {
            string GetBetfairId = "";
            string GetEventCode = "";
            string GetstackValue = "";
            int Getindex = 999;
            string GetBoxtype = "";
            string GetOdds = "";
            string GetRunnerName = "";
            string GetMarketName = "";

            string SendResponse = "Error In Submitting Bet";
            GetBetfairId = obj.BetfairId;
            GetEventCode = obj.EventCode;
            GetstackValue = obj.stackValue;
            Getindex = obj.RunnerIndex;
            GetBoxtype = obj.BoxType;
            GetOdds = obj.OddsValue;
            GetRunnerName = obj.RunnerNameget;
            GetMarketName = obj.MarketName;
            float GetOddsFloat = float.Parse(GetOdds, CultureInfo.InvariantCulture.NumberFormat);
            int delay_time = GetBMDelay(GetEventCode);
            Thread.Sleep(delay_time);
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = client.GetAsync("sky_mo.php?event_id=" + GetEventCode).Result;
                    if (response != null)
                    {
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        Double totalMatched = 100000;//responseJson[0].totalMatched;
                        string SpoRTIDS = FunctionDataController.GetEventTypeId(GetEventCode);
                        var GetRunner = responseJson.runner;
                        int CheckRunner = GetRunner.Count;

                        string MainStatus = responseJson.status;
                        string EvnetMarketId = responseJson.marketId;

                        if (MainStatus == "OPEN")
                        {
                            int GetfieldPos = Getindex - 1;
                            if (GetBoxtype == "back")
                            {
                                float CheckOutRate = GetRunner[GetfieldPos].rate1;
                                if (GetOddsFloat <= CheckOutRate)
                                {
                                    SendResponse = PlaceBetBM(CheckOutRate, GetBetfairId, GetEventCode, GetstackValue,
                                        Getindex, GetBoxtype, GetOdds, GetRunnerName, GetMarketName);
                                }
                                else
                                {
                                    SendResponse = "Not Equal";
                                }
                            }
                            else if (GetBoxtype == "lay")
                            {
                                float CheckOutRate = GetRunner[GetfieldPos].rate2;
                                if (GetOddsFloat >= CheckOutRate)
                                {
                                    SendResponse = PlaceBetBM(CheckOutRate, GetBetfairId, GetEventCode, GetstackValue,
                                        Getindex, GetBoxtype, GetOdds, GetRunnerName, GetMarketName);
                                }
                                else
                                {
                                    SendResponse = "Not Equal";
                                }
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            SendResponse = "market Suspended";
                        }

                    }
                }
                catch (Exception ex)
                {
                    SendResponse = "Error in place bet";
                    return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (TaskCanceledException ex)
            {
                SendResponse = "Error in place bet";
                return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult BetPlaceMX(BetPlaceD obj)
        {
            string GetBetfairId = "";
            string GetEventCode = "";
            string GetstackValue = "";
            int Getindex = 999;
            string GetBoxtype = "";
            string GetOdds = "";
            string GetRunnerName = "";
            string GetMarketName = "";
            string type = "LiveFeed";

            string SendResponse = "Error In Submitting Bet";
            GetBetfairId = obj.BetfairId;
            GetEventCode = obj.EventCode;
            GetstackValue = obj.stackValue;
            Getindex = obj.RunnerIndex;
            GetBoxtype = obj.BoxType;
            GetOdds = obj.OddsValue;
            GetRunnerName = obj.RunnerNameget;
            GetMarketName = obj.MarketName;
            type = obj.OddsVolume;
            float GetOddsFloat = float.Parse(GetOdds, CultureInfo.InvariantCulture.NumberFormat);
            int delay_time = GetMODelay(GetEventCode);
            Thread.Sleep(delay_time);
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://1xbet.mobi/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync(type + "/GetGameZip?id=" + GetBetfairId + "&lng=en&cfview=0&isSubGames=true&GroupEvents=true&countevents=600&grMode=2").Result;  // Blocking call! 
                    if (response != null)
                    {
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        var isresult = responseJson["Value"]["GE"];
                        if (responseJson["Value"]["I"] != null)
                        {
                            string SpoRTIDS = responseJson["Value"]["SI"];
                            string EvnetMarketId = responseJson["Value"]["I"];
                            foreach (var item in isresult)
                            {
                                int GE = item["G"];

                                String Back1 = "";
                                String Back2 = "";
                                String LayOdds = "";
                                String LayOdds1 = "";

                                if (GE == 1 || GE == 38)
                                {
                                    var E1 = item["E"];
                                    int ELength1 = E1.Count;
                                    if (SpoRTIDS == "99")
                                    {
                                        if (ELength1 == 3)
                                        {
                                            Boolean filedALock = false;
                                            Boolean filedBLock = false;
                                            if (E1[0][0]["B"] != null)
                                            {
                                                filedALock = E1[0][0]["B"];
                                            }

                                            if (E1[2][0]["B"] != null)
                                            {
                                                filedBLock = E1[2][0]["B"];
                                            }
                                            if (filedBLock == true && filedALock == true)
                                            {
                                                SendResponse = "Market Suspended";
                                                return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                            }
                                            else
                                            {
                                                Double filedArate = E1[0][0]["C"];
                                                Double filedBrate = E1[2][0]["C"];

                                                if (filedALock == true && filedBLock == false)
                                                {
                                                    Back2 = filedBrate.ToString();
                                                    Double LayOdds11111 = System.Math.Round((Double.Parse(Back2) + 0.10), 2);
                                                    LayOdds1 = LayOdds11111.ToString();
                                                }
                                                else if (filedALock == false && filedBLock == true)
                                                {
                                                    Back1 = filedArate.ToString();
                                                    Double LayOOODS = System.Math.Round((Double.Parse(Back1) + 0.10), 2);
                                                    LayOdds = LayOOODS.ToString();
                                                }
                                                else
                                                {
                                                    if (filedArate < 2 && filedBrate < 2)
                                                    {
                                                        Back1 = filedArate.ToString();
                                                        Back2 = filedBrate.ToString();
                                                    }
                                                    else
                                                    {
                                                        if (filedArate < filedBrate)
                                                        {
                                                            Back1 = filedArate.ToString();
                                                            Double LayOOODS = System.Math.Round((Double.Parse(Back1) + 0.05), 2);
                                                            LayOdds = LayOOODS.ToString();
                                                            Double Back22222 = System.Math.Round(((1 / (filedArate - 1)) + 1), 2);
                                                            Back2 = Back22222.ToString();
                                                            Double LayOdds11111 = System.Math.Round((Double.Parse(Back2) + 0.05), 2);
                                                            LayOdds1 = LayOdds11111.ToString();
                                                        }
                                                        else if (filedArate > filedBrate)
                                                        {
                                                            Double Back11111 = System.Math.Round(((1 / (filedBrate - 1)) + 1), 2);
                                                            Back1 = Back11111.ToString();
                                                            Double LayOOODS = System.Math.Round((Double.Parse(Back1) + 0.05), 2);
                                                            LayOdds = LayOOODS.ToString();
                                                            Back2 = filedBrate.ToString();
                                                            Double LayOdds11111 = System.Math.Round((Double.Parse(Back2) + 0.05), 2);
                                                            LayOdds1 = LayOdds11111.ToString();
                                                        }
                                                        else
                                                        {
                                                            Back1 = filedArate.ToString();
                                                            Back2 = filedBrate.ToString();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            SendResponse = "Market Suspended";
                                            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    else if (SpoRTIDS == "10" || SpoRTIDS == "21" || SpoRTIDS == "3")
                                    {
                                        if (ELength1 == 2)
                                        {
                                            Boolean filedALock = false;
                                            Boolean filedBLock = false;
                                            if (E1[0][0]["B"] != null)
                                            {
                                                filedALock = E1[0][0]["B"];
                                            }

                                            if (E1[1][0]["B"] != null)
                                            {
                                                filedBLock = E1[1][0]["B"];
                                            }
                                            if (filedBLock == true && filedALock == true)
                                            {
                                                SendResponse = "Market Suspended";
                                                return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                            }
                                            else
                                            {
                                                Double filedArate = E1[0][0]["C"];
                                                Double filedBrate = E1[1][0]["C"];

                                                if (filedALock == true && filedBLock == false)
                                                {
                                                    Back2 = filedBrate.ToString();
                                                    Double LayOdds11111 = System.Math.Round((Double.Parse(Back2) + 0.10), 2);
                                                    LayOdds1 = LayOdds11111.ToString();
                                                }
                                                else if (filedALock == false && filedBLock == true)
                                                {
                                                    Back1 = filedArate.ToString();
                                                    Double LayOOODS = System.Math.Round((Double.Parse(Back1) + 0.10), 2);
                                                    LayOdds = LayOOODS.ToString();
                                                }
                                                else
                                                {
                                                    if (filedArate < 2 && filedBrate < 2)
                                                    {
                                                        Back1 = filedArate.ToString();
                                                        Back2 = filedBrate.ToString();
                                                    }
                                                    else
                                                    {
                                                        if (filedArate < filedBrate)
                                                        {
                                                            if (filedArate > 1.04 && filedArate < 1.10)
                                                            {
                                                                filedArate = filedArate - 0.01;
                                                            }
                                                            else if (filedArate > 1.09 && filedArate < 1.15)
                                                            {
                                                                filedArate = filedArate + 0.01;
                                                            }
                                                            else if (filedArate > 1.14 && filedArate < 1.20)
                                                            {
                                                                filedArate = filedArate - 0.01;
                                                            }
                                                            else if (filedArate > 1.19 && filedArate < 1.30)
                                                            {
                                                                filedArate = filedArate + 0.02;
                                                            }
                                                            else if (filedArate > 1.29 && filedArate < 1.50)
                                                            {
                                                                filedArate = filedArate - 0.02;
                                                            }
                                                            else if (filedArate > 1.49 && filedArate < 1.60)
                                                            {
                                                                filedArate = filedArate + 0.03;
                                                            }
                                                            else if (filedArate > 1.59 && filedArate < 1.80)
                                                            {
                                                                filedArate = filedArate - 0.02;
                                                            }
                                                            else if (filedArate > 1.79 && filedArate < 1.90)
                                                            {
                                                                filedArate = filedArate - 0.04;
                                                            }
                                                            else if (filedArate > 1.89 && filedArate < 1.97)
                                                            {
                                                                filedArate = filedArate + 0.02;
                                                            }
                                                            Back1 = filedArate.ToString();
                                                            Double LayOOODS = System.Math.Round((Double.Parse(Back1) + 0.10), 2);
                                                            LayOdds = LayOOODS.ToString();
                                                        }
                                                        else if (filedArate > filedBrate)
                                                        {
                                                            if (filedBrate > 1.04 && filedBrate < 1.10)
                                                            {
                                                                filedBrate = filedBrate - 0.01;
                                                            }
                                                            else if (filedBrate > 1.09 && filedBrate < 1.15)
                                                            {
                                                                filedBrate = filedBrate + 0.01;
                                                            }
                                                            else if (filedBrate > 1.14 && filedBrate < 1.20)
                                                            {
                                                                filedBrate = filedBrate - 0.01;
                                                            }
                                                            else if (filedBrate > 1.19 && filedBrate < 1.30)
                                                            {
                                                                filedBrate = filedBrate + 0.02;
                                                            }
                                                            else if (filedBrate > 1.29 && filedBrate < 1.50)
                                                            {
                                                                filedBrate = filedBrate - 0.02;
                                                            }
                                                            else if (filedBrate > 1.49 && filedBrate < 1.60)
                                                            {
                                                                filedBrate = filedBrate + 0.03;
                                                            }
                                                            else if (filedBrate > 1.59 && filedBrate < 1.80)
                                                            {
                                                                filedBrate = filedBrate - 0.02;
                                                            }
                                                            else if (filedBrate > 1.79 && filedBrate < 1.90)
                                                            {
                                                                filedBrate = filedBrate - 0.04;
                                                            }
                                                            else if (filedBrate > 1.89 && filedBrate < 1.97)
                                                            {
                                                                filedBrate = filedBrate + 0.02;
                                                            }
                                                            Back2 = filedBrate.ToString();
                                                            Double LayOdds11111 = System.Math.Round((Double.Parse(Back2) + 0.10), 2);
                                                            LayOdds1 = LayOdds11111.ToString();
                                                        }
                                                        else
                                                        {
                                                            Back1 = filedArate.ToString();
                                                            Back2 = filedBrate.ToString();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (ELength1 == 2)
                                        {
                                            Boolean filedALock = false;
                                            Boolean filedBLock = false;
                                            if (E1[0][0]["B"] != null)
                                            {
                                                filedALock = E1[0][0]["B"];
                                            }

                                            if (E1[1][0]["B"] != null)
                                            {
                                                filedBLock = E1[1][0]["B"];
                                            }
                                            if (filedBLock == true && filedALock == true)
                                            {
                                                SendResponse = "Market Suspended";
                                                return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                            }
                                            else
                                            {
                                                Double filedArate = E1[0][0]["C"];
                                                Double filedBrate = E1[1][0]["C"];

                                                if (filedALock == true && filedBLock == false)
                                                {
                                                    Back2 = filedBrate.ToString();
                                                    Double LayOdds11111 = System.Math.Round((Double.Parse(Back2) + 0.10), 2);
                                                    LayOdds1 = LayOdds11111.ToString();
                                                }
                                                else if (filedALock == false && filedBLock == true)
                                                {
                                                    Back1 = filedArate.ToString();
                                                    Double LayOOODS = System.Math.Round((Double.Parse(Back1) + 0.10), 2);
                                                    LayOdds = LayOOODS.ToString();
                                                }
                                                else
                                                {
                                                    if (filedArate < 2 && filedBrate < 2)
                                                    {
                                                        Back1 = filedArate.ToString();
                                                        Back2 = filedBrate.ToString();
                                                    }
                                                    else
                                                    {
                                                        if (filedArate < filedBrate)
                                                        {
                                                            Back1 = filedArate.ToString();
                                                            Double LayOOODS = System.Math.Round((Double.Parse(Back1) + 0.05), 2);
                                                            LayOdds = LayOOODS.ToString();
                                                            Double Back22222 = System.Math.Round(((1 / (LayOOODS - 1)) + 1), 2);
                                                            Back2 = Back22222.ToString();
                                                            Double LayOdds11111 = System.Math.Round((Double.Parse(Back2) + 0.30), 2);
                                                            LayOdds1 = LayOdds11111.ToString();
                                                        }
                                                        else if (filedArate > filedBrate)
                                                        {
                                                            Back2 = filedBrate.ToString();
                                                            Double LayOdds11111 = System.Math.Round((Double.Parse(Back2) + 0.05), 2);
                                                            LayOdds1 = LayOdds11111.ToString();
                                                            Double Back11111 = System.Math.Round(((1 / (LayOdds11111 - 1)) + 1), 2);
                                                            Back1 = Back11111.ToString();
                                                            Double LayOOODS = System.Math.Round((Double.Parse(Back1) + 0.30), 2);
                                                            LayOdds = LayOOODS.ToString();
                                                        }
                                                        else
                                                        {
                                                            Back1 = filedArate.ToString();
                                                            Back2 = filedBrate.ToString();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (GetBoxtype == "back")
                                {
                                    float CheckOutRate = 0;
                                    if (Getindex == 1)
                                    {
                                        if (Back1 != "" && Back1 != "0")
                                        {
                                            CheckOutRate = float.Parse(Back1);
                                        }
                                    }
                                    else if (Getindex == 2)
                                    {
                                        if (Back2 != "" && Back2 != "0")
                                        {
                                            CheckOutRate = float.Parse(Back2);
                                        }
                                    }
                                    if (CheckOutRate != 0)
                                    {
                                        if (GetOddsFloat <= CheckOutRate)
                                        {
                                            SendResponse = PlaceBet(CheckOutRate, GetBetfairId, GetEventCode, GetstackValue,
                                                Getindex, GetBoxtype, GetOdds, GetRunnerName, GetMarketName);
                                            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                        }
                                        else
                                        {
                                            SendResponse = "Odds Changed";
                                        }
                                    }
                                    else
                                    {
                                        SendResponse = "Odds Changed";
                                    }
                                }
                                else if (GetBoxtype == "lay")
                                {
                                    float CheckOutRate = 0;
                                    if (Getindex == 1)
                                    {
                                        if (LayOdds != "" && LayOdds != "0")
                                        {
                                            CheckOutRate = float.Parse(LayOdds);
                                        }
                                    }
                                    else if (Getindex == 2)
                                    {
                                        if (LayOdds1 != "" && LayOdds1 != "0")
                                        {
                                            CheckOutRate = float.Parse(LayOdds1);
                                        }
                                    }
                                    if (CheckOutRate != 0)
                                    {
                                        if (GetOddsFloat >= CheckOutRate)
                                        {
                                            SendResponse = PlaceBet(CheckOutRate, GetBetfairId, GetEventCode, GetstackValue,
                                                Getindex, GetBoxtype, GetOdds, GetRunnerName, GetMarketName);
                                            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                        }
                                        else
                                        {
                                            SendResponse = "Odds Changed";
                                        }
                                    }
                                    else
                                    {
                                        SendResponse = "Odds Changed";
                                    }
                                }
                                else
                                {
                                    SendResponse = "Cant Place Bet";
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SendResponse = "Error in place bet";
                    return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (TaskCanceledException ex)
            {
                SendResponse = "Error in place bet";
                return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
        }

        public string PlaceBet(float betable_rate, string GetBetfairId, string GetEventCode, string GetstackValue,
                                int Getindex, string GetBoxtype, string GetOdds, string GetRunnerName, string GetMarketName)
        {
            string gjhgf = "Error in Place Bet";
            try
            {
                long Time_Stamp = ((DateTimeOffset)CurrentTimes).ToUnixTimeSeconds();
                DateTime MatchTTime = GetMatchTimeDB(GetEventCode);
                string SpoRTIDS = FunctionDataController.GetEventTypeId(GetEventCode);
                long unixTime = ((DateTimeOffset)MatchTTime).ToUnixTimeSeconds();
                long diff = unixTime - Time_Stamp;
                if (diff > 1800 && SpoRTIDS == "4")
                {
                    return "Cant Place Bet Before 30 Minute";
                }
                else if (diff > 0 && SpoRTIDS != "4")
                {
                    return "Cant Place Bet Before Inplay";
                }
                string CheckMarketIsInsertOrNot = "";
                string user_ids = (string)System.Web.HttpContext.Current.Session["UIDS"];
                string hash_keys = (string)System.Web.HttpContext.Current.Session["hash_keyS"];
                string agm_id = (string)System.Web.HttpContext.Current.Session["AgentMID"];
                string ad_id = (string)System.Web.HttpContext.Current.Session["AdminID"];
                //string UserName = (string)System.Web.HttpContext.Current.Session["UserName"];
                //return user_ids+"--"+ hash_keys+"--"+ UserName;
                int user_id = Int32.Parse(user_ids);
                string hash_key = hash_keys;
                Double user_bal = 0;
                Double user_exp = 0;
                string runner = "";
                int dist_id = 0;
                int md_id = 0;
                int admin_id = 0;
                Double minstakes = 1;
                Double maxStakes = 200000;
                string Inr_coin = GetIsInr(ad_id);
                /*if (SpoRTIDS != "4")
                {
                    minstakes = 5;
                }*/
                if (Inr_coin == "yes")
                {
                    minstakes = 100;
                    maxStakes = 500000;
                }
                else
                {
                    minstakes = 1;
                    maxStakes = 500000;
                }
                Double max_profit_limit = GetMOLimit(GetEventCode);
                /*if (Inr_coin == "yes")
                {
                    if (SpoRTIDS == "4")
                    {
                        max_profit_limit = 40000;
                    }
                    else
                    {
                        max_profit_limit = 40000;
                    }
                }
                else
                {
                    if (SpoRTIDS == "4")
                    {
                        max_profit_limit = 0;
                    }
                    else if (SpoRTIDS == "2")
                    {
                        max_profit_limit = 100;
                    }
                    else if (SpoRTIDS == "1")
                    {
                        max_profit_limit = 200;
                    }
                    else
                    {
                        max_profit_limit = 50;
                    }
                }*/
                float Getstack = float.Parse(GetstackValue, CultureInfo.InvariantCulture.NumberFormat);
                var calamt = new List<Double>();
                var calnewamt = new List<Double>();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd22 = new SqlCommand("SELECT id, status FROM markets WHERE betfair_id='" + GetBetfairId + "' ", con))
                    {
                        cmd22.Connection = con;
                        con.Open();
                        var readermmmarkets = cmd22.ExecuteReader();
                        if (readermmmarkets.HasRows)
                        {
                            readermmmarkets.Read();
                            string status_mk = (string)readermmmarkets["status"];
                            if (status_mk == "activate")
                            {
                                CheckMarketIsInsertOrNot = "Success";
                            }
                            else
                            {
                                return "Market Closed";
                            }
                        }
                        else
                        {
                            CheckMarketIsInsertOrNot = FunctionDataController.AddMarkets(GetBetfairId, GetEventCode);
                        }
                        con.Close();
                    }
                }

                if (CheckMarketIsInsertOrNot == "Success")
                {
                    if (Getstack < minstakes)
                    {
                        return "Minimum Stakes Limit is " + minstakes;
                    }
                    if (Getstack > maxStakes)
                    {
                        return "Maximum Stakes Limit is " + maxStakes;
                    }

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        using (var cmd = new SqlCommand("SELECT [exposure_limit],[balance],[exposure],[dl_id],[mdl_id],[admin_id] FROM users_client WHERE id='" + user_id + "' and hash_key='" + hash_key + "' AND status='activate' "))
                        {
                            cmd.Connection = con;
                            con.Open();
                            var reader = cmd.ExecuteReader();

                            if (reader.HasRows)
                            {
                                reader.Read();
                                user_bal = (Double)reader["balance"];
                                user_exp = (Double)reader["exposure"];
                                dist_id = (int)reader["dl_id"];
                                md_id = (int)reader["mdl_id"];
                                admin_id = (int)reader["admin_id"];
                                Double exposure_limit = (Double)reader["exposure_limit"];

                                float stakes = 0;
                                float profit = 0;
                                float Get_total = Getstack * betable_rate;
                                float Get_total_val = Get_total - Getstack;
                                if (GetBoxtype == "back")
                                {
                                    stakes = Getstack;
                                    profit = Get_total_val;
                                }
                                else if (GetBoxtype == "lay")
                                {
                                    stakes = Get_total_val;
                                    profit = Getstack;
                                }
                                int runner_pos1 = Getindex;
                                SqlCommand markets = new SqlCommand("SELECT [amount],[runner_pos],[runner] FROM runner_cal WHERE user_id='" + user_id + "' AND market_code='" + GetBetfairId + "'  AND event_code = '" + GetEventCode + "'  ", con);
                                var reader_markets = markets.ExecuteReader();
                                if (reader_markets.HasRows)
                                {
                                    while (reader_markets.Read())
                                    {
                                        Double calamount = (Double)reader_markets["amount"];
                                        int cal_pos = (Int32)reader_markets["runner_pos"];
                                        //int runner_pos1 = runner_pos+1;
                                        Double new_cal_amount = 0;
                                        if (runner_pos1 == cal_pos)
                                        {
                                            runner = (string)reader_markets["runner"];
                                        }
                                        if (runner_pos1 == cal_pos && GetBoxtype == "back")
                                        {
                                            new_cal_amount = calamount + Get_total_val;
                                        }
                                        else if (runner_pos1 == cal_pos && GetBoxtype == "lay")
                                        {
                                            new_cal_amount = calamount - Get_total_val;
                                        }
                                        else if (runner_pos1 != cal_pos && GetBoxtype == "back")
                                        {
                                            new_cal_amount = calamount - Getstack;
                                        }
                                        else if (runner_pos1 != cal_pos && GetBoxtype == "lay")
                                        {
                                            new_cal_amount = calamount + Getstack;
                                        }

                                        new_cal_amount = Math.Round(new_cal_amount, 2);

                                        calamt.Add(calamount);
                                        calnewamt.Add(new_cal_amount);
                                    }
                                }
                                else
                                {
                                    SqlCommand runner2 = new SqlCommand("SELECT [runner_name],[sortPriority] FROM runner_market WHERE market_id='" + GetBetfairId + "' ", con);
                                    var reader_runner = runner2.ExecuteReader();
                                    if (reader_runner.HasRows)
                                    {
                                        while (reader_runner.Read())
                                        {
                                            string runnername = (string)reader_runner["runner_name"];
                                            string cal_pos1 = (string)reader_runner["sortPriority"];
                                            int cal_pos = Int32.Parse(cal_pos1);
                                            //int runner_pos1 = runner_pos+1;
                                            Double new_cal_amount = 0;
                                            if (runner_pos1 == cal_pos)
                                            {
                                                runner = runnername;
                                            }
                                            if (runner_pos1 == cal_pos && GetBoxtype == "back")
                                            {
                                                new_cal_amount = Get_total_val;
                                            }
                                            else if (runner_pos1 == cal_pos && GetBoxtype == "lay")
                                            {
                                                new_cal_amount = -Get_total_val;
                                            }
                                            else if (runner_pos1 != cal_pos && GetBoxtype == "back")
                                            {
                                                new_cal_amount = -Getstack;
                                            }
                                            else if (runner_pos1 != cal_pos && GetBoxtype == "lay")
                                            {
                                                new_cal_amount = Getstack;
                                            }

                                            new_cal_amount = Math.Round(new_cal_amount, 2);

                                            calamt.Add(0);
                                            calnewamt.Add(new_cal_amount);
                                        }
                                    }
                                }
                                Double mincalamt = calamt.Min();
                                if (mincalamt > 0)
                                {
                                    mincalamt = 0;
                                }
                                Double minnewcalamt = calnewamt.Min();
                                if (minnewcalamt > 0)
                                {
                                    minnewcalamt = 0;
                                }
                                Double maxnewcalamt = calnewamt.Max();
                                if (maxnewcalamt < 0)
                                {
                                    maxnewcalamt = 0;
                                }
                                /*if (max_profit_limit > 0)
                                {
                                    if (maxnewcalamt > max_profit_limit)
                                    {
                                        return "Maximum Profit limit is " + max_profit_limit + " Coins";
                                    }
                                }*/
                                if (maxnewcalamt > exposure_limit)
                                {
                                    return "Maximum Exposure limit is " + exposure_limit + " Coins";
                                }
                                if (maxnewcalamt > max_profit_limit)
                                {
                                    return "Maximum Profit limit is " + max_profit_limit + " Coins";
                                }

                                Double new_user_exp = user_exp + mincalamt - minnewcalamt;
                                Double new_user_bal = user_bal - mincalamt + minnewcalamt;
                                Double adjust_exp = 0 + mincalamt - minnewcalamt;
                                Double adjust_bal = 0 - mincalamt + minnewcalamt;
                                if (new_user_bal < 0)
                                {
                                    return "Insufficient Balance";
                                }
                                DateTime time = DateTime.Now;
                                string format1 = "yyyy-MM-dd HH:mm:ss";

                                SqlCommand sqlcal1 = new SqlCommand("SELECT [id] FROM runner_cal WHERE user_id='" + user_id + "'  AND market_code='" + GetBetfairId + "'  AND event_code = '" + GetEventCode + "'  ", con);
                                var datacal1 = sqlcal1.ExecuteReader();
                                if (datacal1.HasRows)
                                {
                                    int wi = 0;
                                    while (datacal1.Read())
                                    {

                                        int cal_id = (int)datacal1["id"];
                                        Double new_cal_amount1 = calnewamt[wi];

                                        SqlCommand sqlcalu = new SqlCommand("UPDATE runner_cal SET amount ='" + new_cal_amount1 + "' WHERE user_id='" + user_id + "' AND id = '" + cal_id + "' ", con);
                                        sqlcalu.ExecuteNonQuery();
                                        wi++;
                                    }
                                }
                                else
                                {
                                    SqlCommand sqlcal_run = new SqlCommand("SELECT [runner_name],[sortPriority] FROM runner_market WHERE market_id='" + GetBetfairId + "' ", con);
                                    var datacal_run = sqlcal_run.ExecuteReader();
                                    if (datacal_run.HasRows)
                                    {
                                        int wi = 0;
                                        while (datacal_run.Read())
                                        {
                                            string runnername = (string)datacal_run["runner_name"];
                                            string runnerpos = (string)datacal_run["sortPriority"];
                                            Double new_cal_amount1 = calnewamt[wi];

                                            SqlCommand sqlcalu1 = new SqlCommand("INSERT INTO runner_cal(user_id,dist_id,md_id,admin_id, event_id, market_id, event_code, " +
                                                " market_code, runner, runner_pos, amount, is_result, created) " +
                                                " VALUES ('" + user_id + "','" + dist_id + "' ,'" + md_id + "' ,'" + admin_id + "' ,'" + GetEventCode + "', '" + GetBetfairId + "', " +
                                                " '" + GetEventCode + "','" + GetBetfairId + "','" + runnername + "', '" + runnerpos + "','" + new_cal_amount1 + "','0','" + time.ToString(format1) + "')", con);
                                            sqlcalu1.ExecuteNonQuery();
                                            wi++;
                                        }
                                    }
                                }
                                string field = GetBoxtype + "(" + GetRunnerName + ")";
                                SqlCommand cmdlive_bet = new SqlCommand("INSERT INTO live_bet (user_id ,dist_id ,md_id ,admin_id ,event_id ,betfair_id , " +
                                    " field ,rate ,stakes ,total_value ,session_rate ,logic ,field_pos ,team_name ,runner_posi ,status ,place_time ,settled_time, " +
                                    " odds_type ,input_stakes ,input_pl ,before_bal ,after_bal ,before_exp ,after_exp)" +
                                    "VALUES ('" + user_id + "' ,'" + dist_id + "' ,'" + md_id + "' ,'" + admin_id + "' ,'" + GetEventCode + "' ,'" + GetBetfairId + "' , " +
                                    " '" + field + "' ,'" + betable_rate + "' ,'" + stakes + "' ,'" + profit + "' ,'' ,'' ,'" + GetBoxtype + "' ,'" + GetRunnerName + "' , " +
                                    " '" + Getindex + "' ,'' ,'" + time.ToString(format1) + "','','MO' ,'" + Getstack + "' ,'" + Get_total + "' ,'" + user_bal + "' , " +
                                    " '" + new_user_bal + "' ,'" + user_exp + "' ,'" + new_user_exp + "' ) ", con);
                                cmdlive_bet.ExecuteNonQuery();
                                SqlCommand cmduser = new SqlCommand("UPDATE users_client SET balance = balance + '" + adjust_bal + "' , exposure = exposure + '" + adjust_exp + "' WHERE id = '" + user_id + "' ", con);
                                cmduser.ExecuteNonQuery();
                                con.Close();
                                gjhgf = "Bet Submitted Successfully";

                            }
                            else
                            {
                                gjhgf = "Please Login Again to Place Bet";
                            }
                            con.Close();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gjhgf = "Error in Place Bet";
            }
            return gjhgf;
        }
        public string PlaceBetBM(float betable_rate, string GetBetfairId, string GetEventCode, string GetstackValue,
                                int Getindex, string GetBoxtype, string GetOdds, string GetRunnerName, string GetMarketName)
        {
            string gjhgf = "Error in Place Bet";
            try
            {
                long Time_Stamp = ((DateTimeOffset)CurrentTimes).ToUnixTimeSeconds();
                DateTime MatchTTime = GetMatchTimeDB(GetEventCode);
                string SpoRTIDS = FunctionDataController.GetEventTypeId(GetEventCode);
                long unixTime = ((DateTimeOffset)MatchTTime).ToUnixTimeSeconds();
                long diff = unixTime - Time_Stamp;
                if (diff > 1800 && SpoRTIDS == "4")
                {
                    return "Cant Place Bet Before 30 Minute";
                }
                else if (diff > 0 && SpoRTIDS != "4")
                {
                    return "Cant Place Bet Before Inplay";
                }
                string CheckMarketIsInsertOrNot = "";
                string user_ids = (string)System.Web.HttpContext.Current.Session["UIDS"];
                string hash_keys = (string)System.Web.HttpContext.Current.Session["hash_keyS"];
                string agm_id = (string)System.Web.HttpContext.Current.Session["AgentMID"];
                string ad_id = (string)System.Web.HttpContext.Current.Session["AdminID"];
                //string UserName = (string)System.Web.HttpContext.Current.Session["UserName"];
                //return user_ids+"--"+ hash_keys+"--"+ UserName;
                int user_id = Int32.Parse(user_ids);
                string hash_key = hash_keys;
                Double user_bal = 0;
                Double user_exp = 0;
                string runner = "";
                int dist_id = 0;
                int md_id = 0;
                int admin_id = 0;
                Double minstakes = 1;
                Double maxStakes = 200000;
                string Inr_coin = "yes";// GetIsInr(agm_id);
                if (SpoRTIDS != "4")
                {
                    minstakes = 5;
                }
                if (Inr_coin == "yes")
                {
                    minstakes = 100;
                    maxStakes = 50000;
                }
                Double max_profit_limit = GetBMLimit(GetEventCode);
                /*if (Inr_coin == "yes")
                {
                    if (SpoRTIDS == "4")
                    {
                        max_profit_limit = 40000;
                    }
                    else
                    {
                        max_profit_limit = 40000;
                    }
                }
                else
                {
                    if (SpoRTIDS == "4")
                    {
                        max_profit_limit = 0;
                    }
                    else if (SpoRTIDS == "2")
                    {
                        max_profit_limit = 100;
                    }
                    else if (SpoRTIDS == "1")
                    {
                        max_profit_limit = 200;
                    }
                    else
                    {
                        max_profit_limit = 50;
                    }
                }*/
                float Getstack = float.Parse(GetstackValue, CultureInfo.InvariantCulture.NumberFormat);
                var calamt = new List<Double>();
                var calnewamt = new List<Double>();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd22 = new SqlCommand("SELECT id, status FROM markets WHERE betfair_id='" + GetBetfairId + "' ", con))
                    {
                        cmd22.Connection = con;
                        con.Open();
                        var readermmmarkets = cmd22.ExecuteReader();
                        if (readermmmarkets.HasRows)
                        {
                            readermmmarkets.Read();
                            string status_mk = (string)readermmmarkets["status"];
                            if (status_mk == "activate")
                            {
                                CheckMarketIsInsertOrNot = "Success";
                            }
                            else
                            {
                                return "Market Closed";
                            }
                        }
                        else
                        {
                            CheckMarketIsInsertOrNot = FunctionDataController.AddMarkets(GetBetfairId, GetEventCode);
                        }
                        con.Close();
                    }
                }

                if (CheckMarketIsInsertOrNot == "Success")
                {
                    if (Getstack < minstakes)
                    {
                        return "Minimum Stakes Limit is " + minstakes;
                    }
                    if (Getstack > maxStakes)
                    {
                        return "Maximum Stakes Limit is " + maxStakes;
                    }

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        using (var cmd = new SqlCommand("SELECT [exposure_limit],[balance],[exposure],[dl_id],[mdl_id],[admin_id] FROM users_client WHERE id='" + user_id + "' and hash_key='" + hash_key + "' AND status='activate' "))
                        {
                            cmd.Connection = con;
                            con.Open();
                            var reader = cmd.ExecuteReader();

                            if (reader.HasRows)
                            {
                                reader.Read();
                                user_bal = (Double)reader["balance"];
                                user_exp = (Double)reader["exposure"];
                                dist_id = (int)reader["dl_id"];
                                md_id = (int)reader["mdl_id"];
                                admin_id = (int)reader["admin_id"];
                                Double exposure_limit = (Double)reader["exposure_limit"];

                                float stakes = 0;
                                float profit = 0;
                                float Get_total = Getstack * betable_rate;
                                float Get_total_val = Get_total - Getstack;
                                if (GetBoxtype == "back")
                                {
                                    stakes = Getstack;
                                    profit = Get_total_val;
                                }
                                else if (GetBoxtype == "lay")
                                {
                                    stakes = Get_total_val;
                                    profit = Getstack;
                                }
                                int runner_pos1 = Getindex;
                                SqlCommand markets = new SqlCommand("SELECT [amount],[runner_pos],[runner] FROM runner_cal WHERE user_id='" + user_id + "' AND market_code='" + GetBetfairId + "'  AND event_code = '" + GetEventCode + "'  ", con);
                                var reader_markets = markets.ExecuteReader();
                                if (reader_markets.HasRows)
                                {
                                    while (reader_markets.Read())
                                    {
                                        Double calamount = (Double)reader_markets["amount"];
                                        int cal_pos = (Int32)reader_markets["runner_pos"];
                                        //int runner_pos1 = runner_pos+1;
                                        Double new_cal_amount = 0;
                                        if (runner_pos1 == cal_pos)
                                        {
                                            runner = (string)reader_markets["runner"];
                                        }
                                        if (runner_pos1 == cal_pos && GetBoxtype == "back")
                                        {
                                            new_cal_amount = calamount + Get_total_val;
                                        }
                                        else if (runner_pos1 == cal_pos && GetBoxtype == "lay")
                                        {
                                            new_cal_amount = calamount - Get_total_val;
                                        }
                                        else if (runner_pos1 != cal_pos && GetBoxtype == "back")
                                        {
                                            new_cal_amount = calamount - Getstack;
                                        }
                                        else if (runner_pos1 != cal_pos && GetBoxtype == "lay")
                                        {
                                            new_cal_amount = calamount + Getstack;
                                        }

                                        new_cal_amount = Math.Round(new_cal_amount, 2);

                                        calamt.Add(calamount);
                                        calnewamt.Add(new_cal_amount);
                                    }
                                }
                                else
                                {
                                    SqlCommand runner2 = new SqlCommand("SELECT [runner_name],[sortPriority] FROM runner_market WHERE market_id='" + GetBetfairId + "' ", con);
                                    var reader_runner = runner2.ExecuteReader();
                                    if (reader_runner.HasRows)
                                    {
                                        while (reader_runner.Read())
                                        {
                                            string runnername = (string)reader_runner["runner_name"];
                                            string cal_pos1 = (string)reader_runner["sortPriority"];
                                            int cal_pos = Int32.Parse(cal_pos1);
                                            //int runner_pos1 = runner_pos+1;
                                            Double new_cal_amount = 0;
                                            if (runner_pos1 == cal_pos)
                                            {
                                                runner = runnername;
                                            }
                                            if (runner_pos1 == cal_pos && GetBoxtype == "back")
                                            {
                                                new_cal_amount = Get_total_val;
                                            }
                                            else if (runner_pos1 == cal_pos && GetBoxtype == "lay")
                                            {
                                                new_cal_amount = -Get_total_val;
                                            }
                                            else if (runner_pos1 != cal_pos && GetBoxtype == "back")
                                            {
                                                new_cal_amount = -Getstack;
                                            }
                                            else if (runner_pos1 != cal_pos && GetBoxtype == "lay")
                                            {
                                                new_cal_amount = Getstack;
                                            }

                                            new_cal_amount = Math.Round(new_cal_amount, 2);

                                            calamt.Add(0);
                                            calnewamt.Add(new_cal_amount);
                                        }
                                    }
                                }
                                Double mincalamt = calamt.Min();
                                if (mincalamt > 0)
                                {
                                    mincalamt = 0;
                                }
                                Double minnewcalamt = calnewamt.Min();
                                if (minnewcalamt > 0)
                                {
                                    minnewcalamt = 0;
                                }
                                Double maxnewcalamt = calnewamt.Max();
                                if (maxnewcalamt < 0)
                                {
                                    maxnewcalamt = 0;
                                }
                                /*if (max_profit_limit > 0)
                                {
                                    if (maxnewcalamt > max_profit_limit)
                                    {
                                        return "Maximum Profit limit is " + max_profit_limit + " Coins";
                                    }
                                }*/
                                if (maxnewcalamt > exposure_limit)
                                {
                                    return "Maximum Exposure limit is " + exposure_limit + " Coins";
                                }
                                if (maxnewcalamt > max_profit_limit)
                                {
                                    return "Maximum Profit limit is " + max_profit_limit + " Coins";
                                }

                                Double new_user_exp = user_exp + mincalamt - minnewcalamt;
                                Double new_user_bal = user_bal - mincalamt + minnewcalamt;
                                Double adjust_exp = 0 + mincalamt - minnewcalamt;
                                Double adjust_bal = 0 - mincalamt + minnewcalamt;
                                if (new_user_bal < 0)
                                {
                                    return "Insufficient Balance";
                                }
                                DateTime time = DateTime.Now;
                                string format1 = "yyyy-MM-dd HH:mm:ss";

                                SqlCommand sqlcal1 = new SqlCommand("SELECT [id] FROM runner_cal WHERE user_id='" + user_id + "'  AND market_code='" + GetBetfairId + "'  AND event_code = '" + GetEventCode + "'  ", con);
                                var datacal1 = sqlcal1.ExecuteReader();
                                if (datacal1.HasRows)
                                {
                                    int wi = 0;
                                    while (datacal1.Read())
                                    {

                                        int cal_id = (int)datacal1["id"];
                                        Double new_cal_amount1 = calnewamt[wi];

                                        SqlCommand sqlcalu = new SqlCommand("UPDATE runner_cal SET amount ='" + new_cal_amount1 + "' WHERE user_id='" + user_id + "' AND id = '" + cal_id + "' ", con);
                                        sqlcalu.ExecuteNonQuery();
                                        wi++;
                                    }
                                }
                                else
                                {
                                    SqlCommand sqlcal_run = new SqlCommand("SELECT [runner_name],[sortPriority] FROM runner_market WHERE market_id='" + GetBetfairId + "' ", con);
                                    var datacal_run = sqlcal_run.ExecuteReader();
                                    if (datacal_run.HasRows)
                                    {
                                        int wi = 0;
                                        while (datacal_run.Read())
                                        {
                                            string runnername = (string)datacal_run["runner_name"];
                                            string runnerpos = (string)datacal_run["sortPriority"];
                                            Double new_cal_amount1 = calnewamt[wi];

                                            SqlCommand sqlcalu1 = new SqlCommand("INSERT INTO runner_cal(user_id,dist_id,md_id,admin_id, event_id, market_id, event_code, " +
                                                " market_code, runner, runner_pos, amount, is_result, created) " +
                                                " VALUES ('" + user_id + "','" + dist_id + "' ,'" + md_id + "' ,'" + admin_id + "' ,'" + GetEventCode + "', '" + GetBetfairId + "', " +
                                                " '" + GetEventCode + "','" + GetBetfairId + "','" + runnername + "', '" + runnerpos + "','" + new_cal_amount1 + "','0','" + time.ToString(format1) + "')", con);
                                            sqlcalu1.ExecuteNonQuery();
                                            wi++;
                                        }
                                    }
                                }
                                string field = GetBoxtype + "(" + GetRunnerName + ")";
                                SqlCommand cmdlive_bet = new SqlCommand("INSERT INTO live_bet (user_id ,dist_id ,md_id ,admin_id ,event_id ,betfair_id , " +
                                    " field ,rate ,stakes ,total_value ,session_rate ,logic ,field_pos ,team_name ,runner_posi ,status ,place_time ,settled_time, " +
                                    " odds_type ,input_stakes ,input_pl ,before_bal ,after_bal ,before_exp ,after_exp)" +
                                    "VALUES ('" + user_id + "' ,'" + dist_id + "' ,'" + md_id + "' ,'" + admin_id + "' ,'" + GetEventCode + "' ,'" + GetBetfairId + "' , " +
                                    " '" + field + "' ,'" + betable_rate + "' ,'" + stakes + "' ,'" + profit + "' ,'' ,'' ,'" + GetBoxtype + "' ,'" + GetRunnerName + "' , " +
                                    " '" + Getindex + "' ,'' ,'" + time.ToString(format1) + "','','MO' ,'" + Getstack + "' ,'" + Get_total + "' ,'" + user_bal + "' , " +
                                    " '" + new_user_bal + "' ,'" + user_exp + "' ,'" + new_user_exp + "' ) ", con);
                                cmdlive_bet.ExecuteNonQuery();
                                SqlCommand cmduser = new SqlCommand("UPDATE users_client SET balance = balance + '" + adjust_bal + "' , exposure = exposure + '" + adjust_exp + "' WHERE id = '" + user_id + "' ", con);
                                cmduser.ExecuteNonQuery();
                                con.Close();
                                gjhgf = "Bet Submitted Successfully";

                            }
                            else
                            {
                                gjhgf = "Please Login Again to Place Bet";
                            }
                            con.Close();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gjhgf = "Error in Place Bet";
            }
            return gjhgf;
        }

        public string PlaceBetToss(float betable_rate, string GetBetfairId, string GetEventCode, string GetstackValue,
                                int Getindex, string GetBoxtype, string GetOdds, string GetRunnerName, string GetMarketName)
        {
            string gjhgf = "Error in Place Bet";
            try
            {
                long Time_Stamp = ((DateTimeOffset)CurrentTimes).ToUnixTimeSeconds();
                DateTime MatchTTime = GetMatchTimeDB(GetEventCode);
                string SpoRTIDS = FunctionDataController.GetEventTypeId(GetEventCode);
                long unixTime = ((DateTimeOffset)MatchTTime).ToUnixTimeSeconds();
                long diff = unixTime - Time_Stamp;
                if (diff < 3600 && SpoRTIDS == "4")
                {
                    return "Place Bet only Before 60 Minute of Match Start";
                }
                string CheckMarketIsInsertOrNot = "";
                //[SessionState(SessionStateBehavior.Default)]
                string user_ids = (string)System.Web.HttpContext.Current.Session["UIDS"];
                string hash_keys = (string)System.Web.HttpContext.Current.Session["hash_keyS"];
                string agm_id = (string)System.Web.HttpContext.Current.Session["AgentMID"];
                //string UserName = (string)System.Web.HttpContext.Current.Session["UserName"];
                //return user_ids+"--"+ hash_keys+"--"+ UserName;
                int user_id = Int32.Parse(user_ids);
                string hash_key = hash_keys;
                Double user_bal = 0;
                Double user_exp = 0;
                string runner = "";
                int dist_id = 0;
                int md_id = 0;
                int admin_id = 0;
                Double minstakes = 1;
                Double maxStakes = 1000;
                string Inr_coin = "yes";//GetIsInr(agm_id);
                if (Inr_coin == "yes")
                {
                    minstakes = 100;
                    maxStakes = 10000;
                }
                Double max_profit_limit = GetMOLimit(GetEventCode);
                float Getstack = float.Parse(GetstackValue, CultureInfo.InvariantCulture.NumberFormat);
                var calamt = new List<Double>();
                var calnewamt = new List<Double>();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd22 = new SqlCommand("SELECT id , status FROM markets WHERE betfair_id='" + GetBetfairId + "'", con))
                    {
                        cmd22.Connection = con;
                        con.Open();
                        var readermmmarkets = cmd22.ExecuteReader();
                        if (readermmmarkets.HasRows)
                        {
                            readermmmarkets.Read();
                            string status_mk = (string)readermmmarkets["status"];
                            if (status_mk == "activate")
                            {
                                CheckMarketIsInsertOrNot = "Success";
                            }
                            else
                            {
                                return "Market Closed";
                            }
                        }
                        else
                        {
                            CheckMarketIsInsertOrNot = FunctionDataController.AddMarkets(GetBetfairId, GetEventCode);
                        }
                        con.Close();
                    }
                }

                if (CheckMarketIsInsertOrNot == "Success")
                {
                    if (Getstack < minstakes)
                    {
                        return "Minimum Stakes Limit is " + minstakes;
                    }
                    if (Getstack > maxStakes)
                    {
                        return "Maximum Stakes Limit is " + maxStakes;
                    }

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        using (var cmd = new SqlCommand("SELECT [exposure_limit],[balance],[exposure],[dl_id],[mdl_id],[admin_id] FROM users_client WHERE id='" + user_id + "' and hash_key='" + hash_key + "' AND status='activate' "))
                        {
                            cmd.Connection = con;
                            con.Open();
                            var reader = cmd.ExecuteReader();

                            if (reader.HasRows)
                            {
                                reader.Read();
                                user_bal = (Double)reader["balance"];
                                user_exp = (Double)reader["exposure"];
                                Double exposure_limit = (Double)reader["exposure_limit"];
                                dist_id = (int)reader["dl_id"];
                                md_id = (int)reader["mdl_id"];
                                admin_id = (int)reader["admin_id"];

                                float stakes = 0;
                                float profit = 0;
                                float Get_total = Getstack * betable_rate;
                                float Get_total_val = Get_total - Getstack;
                                if (GetBoxtype == "back")
                                {
                                    stakes = Getstack;
                                    profit = Get_total_val;
                                }
                                else if (GetBoxtype == "lay")
                                {
                                    stakes = Get_total_val;
                                    profit = Getstack;
                                }
                                int runner_pos1 = Getindex;
                                SqlCommand markets = new SqlCommand("SELECT [amount],[runner_pos],[runner] FROM runner_cal WHERE user_id='" + user_id + "' AND market_code='" + GetBetfairId + "'  AND event_code = '" + GetEventCode + "'  ", con);
                                var reader_markets = markets.ExecuteReader();
                                if (reader_markets.HasRows)
                                {
                                    while (reader_markets.Read())
                                    {
                                        Double calamount = (Double)reader_markets["amount"];
                                        int cal_pos = (Int32)reader_markets["runner_pos"];
                                        //int runner_pos1 = runner_pos+1;
                                        Double new_cal_amount = 0;
                                        if (runner_pos1 == cal_pos)
                                        {
                                            runner = (string)reader_markets["runner"];
                                        }
                                        if (runner_pos1 == cal_pos && GetBoxtype == "back")
                                        {
                                            new_cal_amount = calamount + Get_total_val;
                                        }
                                        else if (runner_pos1 == cal_pos && GetBoxtype == "lay")
                                        {
                                            new_cal_amount = calamount - Get_total_val;
                                        }
                                        else if (runner_pos1 != cal_pos && GetBoxtype == "back")
                                        {
                                            new_cal_amount = calamount - Getstack;
                                        }
                                        else if (runner_pos1 != cal_pos && GetBoxtype == "lay")
                                        {
                                            new_cal_amount = calamount + Getstack;
                                        }

                                        new_cal_amount = Math.Round(new_cal_amount, 2);

                                        calamt.Add(calamount);
                                        calnewamt.Add(new_cal_amount);
                                    }
                                }
                                else
                                {
                                    SqlCommand runner2 = new SqlCommand("SELECT [runner_name],[sortPriority] FROM runner_market WHERE market_id='" + GetBetfairId + "' ", con);
                                    var reader_runner = runner2.ExecuteReader();
                                    if (reader_runner.HasRows)
                                    {
                                        while (reader_runner.Read())
                                        {
                                            string runnername = (string)reader_runner["runner_name"];
                                            string cal_pos1 = (string)reader_runner["sortPriority"];
                                            int cal_pos = Int32.Parse(cal_pos1);
                                            //int runner_pos1 = runner_pos+1;
                                            Double new_cal_amount = 0;
                                            if (runner_pos1 == cal_pos)
                                            {
                                                runner = runnername;
                                            }
                                            if (runner_pos1 == cal_pos && GetBoxtype == "back")
                                            {
                                                new_cal_amount = Get_total_val;
                                            }
                                            else if (runner_pos1 == cal_pos && GetBoxtype == "lay")
                                            {
                                                new_cal_amount = -Get_total_val;
                                            }
                                            else if (runner_pos1 != cal_pos && GetBoxtype == "back")
                                            {
                                                new_cal_amount = -Getstack;
                                            }
                                            else if (runner_pos1 != cal_pos && GetBoxtype == "lay")
                                            {
                                                new_cal_amount = Getstack;
                                            }

                                            new_cal_amount = Math.Round(new_cal_amount, 2);

                                            calamt.Add(0);
                                            calnewamt.Add(new_cal_amount);
                                        }
                                    }
                                }
                                Double mincalamt = calamt.Min();
                                if (mincalamt > 0)
                                {
                                    mincalamt = 0;
                                }
                                Double minnewcalamt = calnewamt.Min();
                                if (minnewcalamt > 0)
                                {
                                    minnewcalamt = 0;
                                }
                                Double maxnewcalamt = calnewamt.Max();
                                if (maxnewcalamt < 0)
                                {
                                    maxnewcalamt = 0;
                                }

                                if (maxnewcalamt > exposure_limit)
                                {
                                    return "Maximum Exposure limit is " + exposure_limit + " Coins";
                                }

                                if (maxnewcalamt > max_profit_limit)
                                {
                                    return "Maximum Profit limit is " + max_profit_limit + " Coins";
                                }
                                Double new_user_exp = user_exp + mincalamt - minnewcalamt;
                                Double new_user_bal = user_bal - mincalamt + minnewcalamt;
                                Double adjust_exp = 0 + mincalamt - minnewcalamt;
                                Double adjust_bal = 0 - mincalamt + minnewcalamt;
                                if (new_user_bal < 0)
                                {
                                    return "Insufficient Balance";
                                }
                                DateTime time = DateTime.Now;
                                string format1 = "yyyy-MM-dd HH:mm:ss";

                                SqlCommand sqlcal1 = new SqlCommand("SELECT [id] FROM runner_cal WHERE user_id='" + user_id + "'  AND market_code='" + GetBetfairId + "'  AND event_code = '" + GetEventCode + "'  ", con);
                                var datacal1 = sqlcal1.ExecuteReader();
                                if (datacal1.HasRows)
                                {
                                    int wi = 0;
                                    while (datacal1.Read())
                                    {

                                        int cal_id = (int)datacal1["id"];
                                        Double new_cal_amount1 = calnewamt[wi];

                                        SqlCommand sqlcalu = new SqlCommand("UPDATE runner_cal SET amount ='" + new_cal_amount1 + "' WHERE user_id='" + user_id + "' AND id = '" + cal_id + "' ", con);
                                        sqlcalu.ExecuteNonQuery();
                                        wi++;
                                    }
                                }
                                else
                                {
                                    SqlCommand sqlcal_run = new SqlCommand("SELECT [runner_name],[sortPriority] FROM runner_market WHERE market_id='" + GetBetfairId + "' ", con);
                                    var datacal_run = sqlcal_run.ExecuteReader();
                                    if (datacal_run.HasRows)
                                    {
                                        int wi = 0;
                                        while (datacal_run.Read())
                                        {
                                            string runnername = (string)datacal_run["runner_name"];
                                            string runnerpos = (string)datacal_run["sortPriority"];
                                            Double new_cal_amount1 = calnewamt[wi];

                                            SqlCommand sqlcalu1 = new SqlCommand("INSERT INTO runner_cal(user_id,dist_id,md_id,admin_id, event_id, market_id, event_code, " +
                                                " market_code, runner, runner_pos, amount, is_result, created) " +
                                                " VALUES ('" + user_id + "','" + dist_id + "' ,'" + md_id + "' ,'" + admin_id + "' ,'" + GetEventCode + "', '" + GetBetfairId + "', " +
                                                " '" + GetEventCode + "','" + GetBetfairId + "','" + runnername + "', '" + runnerpos + "','" + new_cal_amount1 + "','0','" + time.ToString(format1) + "')", con);
                                            sqlcalu1.ExecuteNonQuery();
                                            wi++;
                                        }
                                    }
                                }
                                string field = GetBoxtype + "(" + GetRunnerName + ")";
                                SqlCommand cmdlive_bet = new SqlCommand("INSERT INTO live_bet (user_id ,dist_id ,md_id ,admin_id ,event_id ,betfair_id , " +
                                    " field ,rate ,stakes ,total_value ,session_rate ,logic ,field_pos ,team_name ,runner_posi ,status ,place_time ,settled_time, " +
                                    " odds_type ,input_stakes ,input_pl ,before_bal ,after_bal ,before_exp ,after_exp)" +
                                    "VALUES ('" + user_id + "' ,'" + dist_id + "' ,'" + md_id + "' ,'" + admin_id + "' ,'" + GetEventCode + "' ,'" + GetBetfairId + "' , " +
                                    " '" + field + "' ,'" + betable_rate + "' ,'" + stakes + "' ,'" + profit + "' ,'' ,'' ,'" + GetBoxtype + "' ,'" + GetRunnerName + "' , " +
                                    " '" + Getindex + "' ,'' ,'" + time.ToString(format1) + "','','MO' ,'" + Getstack + "' ,'" + Get_total + "' ,'" + user_bal + "' , " +
                                    " '" + new_user_bal + "' ,'" + user_exp + "' ,'" + new_user_exp + "' ) ", con);
                                cmdlive_bet.ExecuteNonQuery();
                                SqlCommand cmduser = new SqlCommand("UPDATE users_client SET balance = balance + '" + adjust_bal + "' , exposure = exposure + '" + adjust_exp + "' WHERE id = '" + user_id + "' ", con);
                                cmduser.ExecuteNonQuery();
                                con.Close();
                                gjhgf = "Bet Submitted Successfully";

                            }
                            else
                            {
                                gjhgf = "Please Login Again to Place Bet";
                            }
                            con.Close();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gjhgf = "Error in Place bet";
            }
            return gjhgf;
        }
        public string CLo(string username, string password)
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
                    Random rand = new Random();
                    string rand_hash = rand.Next(1000, 10000000).ToString();
                    string new_hash = "dsfgfhgfdhjghjdfggfdh";
                    com.CommandText = "SELECT hash_key,dl_id,mdl_id,admin_id,id FROM users_client WHERE  username='" + username + "' and password='" + password + "' AND status='activate'";
                    dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        ViewBag.Login = "Success";
                        Session["UserName"] = username.ToString();
                        dr.Read();
                        string hash_key = (string)dr["hash_key"];
                        int Agent_Id = (int)dr["dl_id"];
                        int AgentM_Id = (int)dr["mdl_id"];
                        int Admin_Id = (int)dr["admin_id"];
                        int UID = (int)dr["id"];
                        string cuid = UID.ToString();
                        if (cuid == "8")
                        {
                            Session["hash_keyS"] = hash_key;
                        }
                        else
                        {
                            Session["hash_keyS"] = new_hash;
                        }
                        Session["UIDS"] = UID.ToString();
                        Session["AgentID"] = Agent_Id.ToString();
                        Session["AgentMID"] = AgentM_Id.ToString();
                        Session["AdminID"] = Admin_Id.ToString();
                        Session["is_panel_log"] = username.ToString();

                        if (cuid == "8")
                        {

                        }
                        else
                        {
                            SqlCommand update_hash = new SqlCommand("UPDATE users_client set hash_key='" + new_hash + "' where id='" + UID + "'", con2);
                            update_hash.ExecuteNonQuery();
                        }

                        gjhgf = "Success";
                    }
                    else
                    {
                        SqlCommand com1 = new SqlCommand();
                        SqlDataReader dr1;
                        com1.Connection = con2;
                        com1.CommandText = "SELECT hash_key,id,md_id,admin_id FROM distributors WHERE username='" + username + "' and password='" + password + "' AND status='activate'";
                        dr1 = com1.ExecuteReader();
                        if (dr1.HasRows)
                        {
                            string hash_key = "";
                            int login_user_id = 0;
                            int Mlogin_user_id = 0;
                            int Alogin_user_id = 0;
                            while (dr1.Read())
                            {
                                hash_key = (string)dr1["hash_key"];
                                login_user_id = (int)dr1["id"];
                                Mlogin_user_id = (int)dr1["md_id"];
                                Alogin_user_id = (int)dr1["admin_id"];
                            }
                            Session["DL_UserName"] = username.ToString();
                            Session["DL_hash_key"] = hash_key.ToString();
                            Session["DL_login_user_id"] = login_user_id.ToString();
                            Session["MDL_login_user_id"] = Mlogin_user_id.ToString();
                            Session["Admin_login_user_id"] = Alogin_user_id.ToString();
                            gjhgf = "Agent";
                        }
                        else
                        {
                            SqlCommand com2 = new SqlCommand();
                            SqlDataReader dr2;
                            com2.Connection = con2;
                            com2.CommandText = "SELECT hash_key,id,admin_id FROM masterdistributors WHERE  username='" + username + "' and password='" + password + "'";
                            dr2 = com2.ExecuteReader();
                            if (dr2.HasRows)
                            {
                                string hash_key = "";
                                int login_user_id = 0;
                                int login_admin_id = 0;
                                while (dr2.Read())
                                {
                                    hash_key = (string)dr2["hash_key"];
                                    login_user_id = (int)dr2["id"];
                                    login_admin_id = (int)dr2["admin_id"];
                                }
                                Session["MDL_UserName"] = username.ToString();
                                Session["MDL_hash_key"] = hash_key.ToString();
                                Session["MDL_login_user_id"] = login_user_id.ToString();
                                Session["MDL_admin_user_id"] = login_admin_id.ToString();
                                Session["is_panel_log"] = username.ToString();
                                gjhgf = "AgentM";
                            }
                            else
                            {
                                SqlCommand com3 = new SqlCommand();
                                SqlDataReader dr3;
                                com3.Connection = con2;
                                com3.CommandText = "SELECT id ,hash_key FROM admin WHERE username='" + username + "' and password='" + password + "' AND status='activate' AND site_name='Probet247'";
                                dr3 = com3.ExecuteReader();
                                if (dr3.HasRows)
                                {
                                    string hash_key = "";
                                    int login_user_id = 0;
                                    while (dr3.Read())
                                    {
                                        hash_key = (string)dr3["hash_key"];
                                        login_user_id = (int)dr3["id"];
                                    }
                                    Session["Admin_UserName"] = username.ToString();
                                    Session["Admin_hash_key"] = hash_key.ToString();
                                    Session["Admin_login_user_id"] = login_user_id.ToString();
                                    Session["is_panel_log"] = username.ToString();
                                    gjhgf = "Admin";
                                }
                                else
                                {
                                    gjhgf = "Failed";
                                }
                            }
                        }
                    }
                    con2.Close();
                }
            }
            catch (Exception ex)
            {
                gjhgf = "Error";
            }
            return gjhgf;
        }

        [HttpPost]
        public ActionResult BetPlaceMS(BetPlaceD obj)
        {
            string GetBetfairId = "";
            string GetEventCode = "";
            string GetstackValue = "";
            int Getindex = 999;
            string GetBoxtype = "";
            string GetOdds = "";
            string GetVolume = "";
            string GetRunnerName = "";
            string GetMarketName = "";
            string SeBFI = "";

            string SendResponse = "";
            GetBetfairId = obj.BetfairId;
            GetEventCode = obj.EventCode;
            GetVolume = obj.OddsVolume;
            GetstackValue = obj.stackValue;
            Getindex = obj.RunnerIndex;
            GetBoxtype = obj.BoxType;
            GetOdds = obj.OddsValue;
            GetRunnerName = obj.RunnerNameget;
            GetMarketName = obj.MarketName;
            SeBFI = obj.SeBFI;
            int delay_time = GetSessDelay(GetEventCode);
            Thread.Sleep(delay_time);
            string ad_id = (string)System.Web.HttpContext.Current.Session["AdminID"];           
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://bet-fair.co/exchange/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("SessionDataAView?Event_Code=" + GetEventCode + "&neck=0").Result;
                    if (response != null)
                    {
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson1 = JsonConvert.DeserializeObject(products);
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        for (int i = 0; i < responseJson.Count; i++)
                        {
                            string selectioniii = responseJson[i].SelectionId;
                            if (selectioniii == GetBetfairId)
                            {
                                string GameStatus = responseJson[i].GameStatus;
                                if (GameStatus == "")
                                {
                                    if (GetBoxtype == "Not")
                                    {
                                        string CheckOutRate = responseJson[i].LayPrice1;
                                        Double vtyugh = Double.Parse(GetOdds);
                                        string LaySize1 = responseJson[i].LaySize1;
                                        Double SessionRate = Double.Parse(LaySize1);
                                        String Session_Name = responseJson[i].RunnerName;
                                        if (GetOdds == CheckOutRate && LaySize1 == GetVolume && GetMarketName == Session_Name)
                                        {
                                            SendResponse = PlaceBetS(vtyugh, SessionRate, GetBetfairId, GetEventCode, GetstackValue,
                                                    Getindex, GetBoxtype, GetOdds, GetRunnerName, GetMarketName);
                                        }
                                        else
                                        {
                                            SendResponse = "Odds or Volume Changed";
                                        }
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                    else if (GetBoxtype == "Yes")
                                    {
                                        string CheckOutRate = responseJson[i].BackPrice1;
                                        Double vtyugh = Double.Parse(GetOdds);
                                        string BackSize1 = responseJson[i].BackSize1;
                                        Double SessionRate = Double.Parse(BackSize1);
                                        String Session_Name = responseJson[i].RunnerName;
                                        if (GetOdds == CheckOutRate && BackSize1 == GetVolume && GetMarketName == Session_Name)
                                        {
                                            SendResponse = PlaceBetS(vtyugh, SessionRate, GetBetfairId, GetEventCode, GetstackValue,
                                                    Getindex, GetBoxtype, GetOdds, GetRunnerName, GetMarketName);
                                        }
                                        else
                                        {
                                            SendResponse = "Odds or Volume Changed";
                                        }
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        SendResponse = "Error in Submit Bet";
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                else
                                {
                                    SendResponse = "Market Suspended";
                                    return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                }

                            }
                        }
                    }
                    else
                    {
                        SendResponse = "Error in Submitting Bet";
                    }
                }
                catch (Exception)
                {
                    SendResponse = "Error in Place Bet";
                    return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                    //System.Diagnostics.Debug.WriteLine("dfghghgfhf" + ex);
                }
            }
            catch (TaskCanceledException)
            {
                SendResponse = "Error in Place Bet";
                return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult BetPlaceBinary(BetPlaceD obj)
        {
            string GetBetfairId = "";
            string GetEventCode = "";
            string GetstackValue = "";
            int Getindex = 999;
            string GetBoxtype = "";
            string GetOdds = "";
            string GetVolume = "";
            string GetRunnerName = "";
            string GetMarketName = "";

            string SendResponse = "";
            GetBetfairId = obj.BetfairId;
            GetEventCode = obj.EventCode;
            GetVolume = obj.OddsVolume;
            GetstackValue = obj.stackValue;
            Getindex = obj.RunnerIndex;
            GetBoxtype = obj.BoxType;
            GetOdds = obj.OddsValue;
            GetRunnerName = obj.RunnerNameget;
            GetMarketName = obj.MarketName;

            int delay_time = GetSessDelay(GetEventCode);
            Thread.Sleep(delay_time);
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = client.GetAsync("binary_sess.php?id=" + GetEventCode).Result;
                    if (response != null)
                    {
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        for (int i = 0; i < responseJson.Count; i++)
                        {
                            string selectioniii = responseJson[i].SelectionId;
                            if (selectioniii == GetBetfairId)
                            {
                                string GameStatus = responseJson[i].GameStatus;
                                if (GameStatus == "")
                                {
                                    if (GetBoxtype == "Not")
                                    {
                                        string CheckOutRate = responseJson[i].LayPrice1;
                                        Double vtyugh = Double.Parse(GetOdds);
                                        Double vtyugh11 = Double.Parse(CheckOutRate);
                                        string LaySize1 = responseJson[i].LaySize1;
                                        Double SessionRate = Double.Parse(LaySize1);
                                        String Session_Name = responseJson[i].RunnerName;
                                        if (GetMarketName == Session_Name)
                                        {
                                            SendResponse = PlaceBetS(vtyugh11, SessionRate, GetBetfairId, GetEventCode, GetstackValue,
                                                    Getindex, GetBoxtype, GetOdds, GetRunnerName, GetMarketName);
                                        }
                                        else
                                        {
                                            SendResponse = "Different Market";
                                        }
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                    else if (GetBoxtype == "Yes")
                                    {
                                        string CheckOutRate = responseJson[i].BackPrice1;
                                        Double vtyugh = Double.Parse(GetOdds);
                                        Double vtyugh11 = Double.Parse(CheckOutRate);
                                        string BackSize1 = responseJson[i].BackSize1;
                                        Double SessionRate = Double.Parse(BackSize1);
                                        String Session_Name = responseJson[i].RunnerName;
                                        if (GetMarketName == Session_Name)
                                        {
                                            SendResponse = PlaceBetS(vtyugh11, SessionRate, GetBetfairId, GetEventCode, GetstackValue,
                                                    Getindex, GetBoxtype, GetOdds, GetRunnerName, GetMarketName);
                                        }
                                        else
                                        {
                                            SendResponse = "Different Market";
                                        }
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        SendResponse = "Error in Submit Bet";
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                else
                                {
                                    SendResponse = "Market Suspended";
                                    return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                }

                            }
                        }
                    }
                    else
                    {
                        SendResponse = "Error in Submitting Bet";
                    }
                }
                catch (Exception ex)
                {
                    SendResponse = "Error in Place Bet";
                    return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                    //System.Diagnostics.Debug.WriteLine("dfghghgfhf" + ex);
                }
            }
            catch (TaskCanceledException ex)
            {
                SendResponse = "Error in Place Bet";
                return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult BetPlaceGS(BetPlaceD obj)
        {
            string GetBetfairId = "";
            string GetEventCode = "";
            string GetstackValue = "";
            int Getindex = 999;
            string GetBoxtype = "";
            string GetOdds = "";
            string GetVolume = "";
            string GetRunnerName = "";
            string GetMarketName = "";

            string SendResponse = "";
            GetBetfairId = obj.BetfairId;
            GetEventCode = obj.EventCode;
            GetVolume = obj.OddsVolume;
            GetstackValue = obj.stackValue;
            Getindex = obj.RunnerIndex;
            GetBoxtype = obj.BoxType;
            GetOdds = obj.OddsValue;
            GetRunnerName = obj.RunnerNameget;
            GetMarketName = obj.MarketName;

            int delay_time = GetSessDelay(GetEventCode);
            Thread.Sleep(delay_time);
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://api_link.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = client.GetAsync("sky_sess.php?event_id=" + GetEventCode).Result;
                    if (response != null)
                    {
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        for (int i = 0; i < responseJson.Count; i++)
                        {
                            string selectioniii = responseJson[i].SelectionId;
                            if (selectioniii == GetBetfairId)
                            {
                                string GameStatus = responseJson[i].GameStatus;
                                if (GameStatus == "")
                                {
                                    if (GetBoxtype == "Not")
                                    {
                                        string CheckOutRate = responseJson[i].LayPrice1;
                                        Double vtyugh = Double.Parse(GetOdds);
                                        string LaySize1 = responseJson[i].LaySize1;
                                        Double SessionRate = Double.Parse(LaySize1);
                                        String Session_Name = responseJson[i].RunnerName;
                                        if (GetOdds == CheckOutRate && LaySize1 == GetVolume && GetMarketName == Session_Name)
                                        {
                                            SendResponse = PlaceBetS(vtyugh, SessionRate, GetBetfairId, GetEventCode, GetstackValue,
                                                    Getindex, GetBoxtype, GetOdds, GetRunnerName, GetMarketName);
                                        }
                                        else
                                        {
                                            SendResponse = "Odds or Volume Changed";
                                        }
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                    else if (GetBoxtype == "Yes")
                                    {
                                        string CheckOutRate = responseJson[i].BackPrice1;
                                        Double vtyugh = Double.Parse(GetOdds);
                                        string BackSize1 = responseJson[i].BackSize1;
                                        Double SessionRate = Double.Parse(BackSize1);
                                        String Session_Name = responseJson[i].RunnerName;
                                        if (GetOdds == CheckOutRate && BackSize1 == GetVolume && GetMarketName == Session_Name)
                                        {
                                            SendResponse = PlaceBetS(vtyugh, SessionRate, GetBetfairId, GetEventCode, GetstackValue,
                                                    Getindex, GetBoxtype, GetOdds, GetRunnerName, GetMarketName);
                                        }
                                        else
                                        {
                                            SendResponse = "Odds or Volume Changed";
                                        }
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        SendResponse = "Error in Submit Bet";
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                else
                                {
                                    SendResponse = "Market Suspended";
                                    return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                }

                            }
                        }
                    }
                    else
                    {
                        SendResponse = "Error in Submitting Bet";
                    }
                }
                catch (Exception ex)
                {
                    SendResponse = "Error in Place Bet";
                    return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                    //System.Diagnostics.Debug.WriteLine("dfghghgfhf" + ex);
                }
            }
            catch (TaskCanceledException ex)
            {
                SendResponse = "Error in Place Bet";
                return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
        }

        public string PlaceBetS(Double betable_rate, Double session_rate, string GetBetfairId, string GetEventCode, string GetstackValue,
                                int Getindex, string GetBoxtype, string GetOdds, string GetRunnerName, string GetMarketName)
        {
            string gjhgf = "Error in Submitting Bet";
            try
            {
                string user_ids = (string)System.Web.HttpContext.Current.Session["UIDS"];
                string hash_keys = (string)System.Web.HttpContext.Current.Session["hash_keyS"];
                string agm_id = (string)System.Web.HttpContext.Current.Session["AgentMID"];
                string ad_id = (string)System.Web.HttpContext.Current.Session["AdminID"];
                int user_id = Int32.Parse(user_ids);
                string hash_key = hash_keys;
                Double user_bal = 0;
                Double user_exp = 0;
                Double updated_exposure = 0;
                Double UserNetBalance = 0;
                Double adjust_exp = 0;
                Double adjust_bal = 0;
                Double bet_before_balance = 0;
                int dist_id = 0;
                int md_id = 0;
                int admin_id = 0;
                Double minstakes = 1;
                Double maxStakes = 20000;
                string Inr_coin = GetIsInr(ad_id);
                if (Inr_coin == "yes")
                {
                    minstakes = 100;
                    maxStakes = 250000;
                }
                else
                {
                    minstakes = 1;
                    maxStakes = 25000;
                }
                Double Getstack = Double.Parse(GetstackValue, CultureInfo.InvariantCulture.NumberFormat);
                Double input_stakes = Getstack;
                Double TotalProfitLost = 0;
                DateTime time = DateTime.Now;
                string format1 = "yyyy-MM-dd HH:mm:ss";

                if (Getstack < minstakes)
                {
                    return "Minimum Stakes Limit is " + minstakes;
                }
                if (Getstack > maxStakes)
                {
                    return "Maximum Stakes Limit is " + maxStakes;
                }
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT [exposure_limit],[balance],[exposure],[dl_id],[mdl_id],[admin_id] FROM users_client WHERE id='" + user_id + "' and hash_key='" + hash_key + "' AND status='activate' "))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            reader.Read();
                            user_bal = (Double)reader["balance"];
                            bet_before_balance = user_bal;
                            user_exp = (Double)reader["exposure"];
                            Double exposure_limit = (Double)reader["exposure_limit"];
                            dist_id = (int)reader["dl_id"];
                            md_id = (int)reader["mdl_id"];
                            admin_id = (int)reader["admin_id"];

                            using (var cmd1 = new SqlCommand("SELECT id FROM matches WHERE status='OPEN' AND event_code = '" + GetEventCode + "'", con))
                            {
                                var dataM = cmd1.ExecuteReader();
                                if (dataM.HasRows)
                                {
                                    using (var cmd11 = new SqlCommand("SELECT betfair_id, status FROM markets WHERE market_name = '" + GetMarketName + "'  AND event_code = '" + GetEventCode + "'", con))
                                    {
                                        var dataMM = cmd11.ExecuteReader();
                                        if (dataMM.HasRows)
                                        {
                                            dataMM.Read();
                                            GetBetfairId = (string)dataMM["betfair_id"];
                                            string status_mk = (string)dataMM["status"];
                                            if (status_mk == "activate")
                                            {
                                            }
                                            else
                                            {
                                                return "Market Closed";
                                            }
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("INSERT INTO  markets (market_name,back1,lay1,back2,lay2,back3,lay3,status,result_value," +
                                              "market_settle,created,league_id,sport_id,betfair_id,event_code,nested_market,runner_name_done,type) " +
                                              "VALUES ('" + GetMarketName + "' ,'' ,'' ,'' ,'' ,'' ,'' ,'activate' ,'' ,'' ,'" + time.ToString(format1) + "' ,'' ,'4' ,'" + GetBetfairId + "' ,'" + GetEventCode + "' ,'' ,'done','sess') ", con);
                                            int runquery1 = cmd3.ExecuteNonQuery();
                                            if (runquery1 == 1)
                                            {

                                            }
                                            else
                                            {
                                                return "Market not available";
                                            }
                                        }

                                        using (var cmd111 = new SqlCommand("SELECT id FROM markets WHERE status='activate' AND market_name = '" + GetMarketName + "'  AND event_code = '" + GetEventCode + "'", con))
                                        {
                                            var sqlMarket = cmd111.ExecuteReader();
                                            if (sqlMarket.HasRows)
                                            {

                                                string field = "";
                                                string field_pos = "";
                                                string notyes_query = "";
                                                if (GetBoxtype == "Not")
                                                {
                                                    field_pos = "a";
                                                    field = "Not";
                                                    TotalProfitLost = Getstack;
                                                    Getstack = (Getstack * session_rate) / 100;
                                                    notyes_query = "SELECT stakes,id FROM fancy_exchange WHERE user_id='" + user_id + "' AND event_id='" + GetEventCode + "' AND market_id='" + GetBetfairId + "' AND field_name='Yes' AND field_rate<='" + betable_rate + "' ";
                                                }
                                                else if (GetBoxtype == "Yes")
                                                {
                                                    field_pos = "b";
                                                    field = "Yes";
                                                    Getstack = Getstack;
                                                    TotalProfitLost = (Getstack * session_rate) / 100;
                                                    notyes_query = "SELECT stakes,id FROM fancy_exchange WHERE user_id='" + user_id + "' AND event_id='" + GetEventCode + "' AND market_id='" + GetBetfairId + "' AND field_name='Not' AND field_rate>='" + betable_rate + "' ";
                                                }
                                                else { }

                                                Double balance = Getstack;
                                                UserNetBalance = user_bal - balance;
                                                double totalbettp = gettptotal(user_id, GetBetfairId, GetEventCode);
                                                Double max_profit_limit = GetSessLimit(GetEventCode);
                                                double vyvgj = totalbettp + TotalProfitLost;
                                                if (vyvgj > exposure_limit)
                                                {
                                                    return "Maximum Exposure limit is " + exposure_limit + " Coins";
                                                }
                                                if (vyvgj > max_profit_limit)
                                                {
                                                    return "Maximum Profit limit is " + max_profit_limit + " Coins";
                                                }
                                                // Start Place Bet For NOt/Yes Session
                                                if (GetBoxtype == "Not" || GetBoxtype == "Yes")
                                                {
                                                    using (var cmd22 = new SqlCommand("SELECT id FROM fancy_exchange WHERE user_id='" + user_id + "' AND event_id='" + GetEventCode + "' AND market_id='" + GetBetfairId + "'", con))
                                                    {
                                                        var fancy_exchange = cmd22.ExecuteReader();
                                                        if (fancy_exchange.HasRows)
                                                        {
                                                            var session_not_fancy_exchange_ids = new List<Double>();
                                                            var session_not_fancy_exchange_amounts = new List<Double>();

                                                            using (var sqlFancyNot = new SqlCommand(notyes_query, con))
                                                            {
                                                                var rowFancyNot = sqlFancyNot.ExecuteReader();
                                                                if (rowFancyNot.HasRows)
                                                                {
                                                                    Double totalStakeNot = 0;
                                                                    while (rowFancyNot.Read())
                                                                    {
                                                                        totalStakeNot = totalStakeNot + (Double)rowFancyNot["stakes"];
                                                                        int session_not_fancy_exchange_ids1 = (int)rowFancyNot["id"];
                                                                        Double session_not_fancy_exchange_amounts1 = (Double)rowFancyNot["stakes"];

                                                                        session_not_fancy_exchange_ids.Add(session_not_fancy_exchange_ids1);
                                                                        session_not_fancy_exchange_amounts.Add(session_not_fancy_exchange_amounts1);
                                                                    }
                                                                    if (totalStakeNot >= Getstack)
                                                                    {

                                                                        Double used_esposure = Getstack;
                                                                        UserNetBalance = user_bal + used_esposure;
                                                                        updated_exposure = user_exp - Getstack;
                                                                        adjust_bal = 0 + used_esposure;
                                                                        adjust_exp = 0 - Getstack;

                                                                        if (UserNetBalance < 0)
                                                                        {
                                                                            con.Close();
                                                                            return "Insufficient Balance";
                                                                        }

                                                                        Double arrayValueFind = Getstack;
                                                                        var arrayFancyExchange = new List<Double>();
                                                                        Double arrayValueSum = 0;
                                                                        arrayFancyExchange = session_not_fancy_exchange_amounts;

                                                                        for (int i = 0; i < arrayFancyExchange.Count; i++)
                                                                        {
                                                                            arrayValueSum = arrayValueSum + arrayFancyExchange[i];
                                                                            if (arrayValueSum == arrayValueFind)
                                                                            {
                                                                                for (int j = 0; j <= i; j++)
                                                                                {
                                                                                    arrayFancyExchange[j] = 0;
                                                                                }
                                                                                break;
                                                                            }
                                                                            else if (arrayValueSum > arrayValueFind)
                                                                            {
                                                                                Double arrayTempValue = arrayValueSum - arrayValueFind;
                                                                                arrayFancyExchange[i] = arrayTempValue;
                                                                                for (int j = 0; j <= i - 1; j++)
                                                                                {
                                                                                    arrayFancyExchange[j] = 0;
                                                                                }
                                                                                break;
                                                                            }
                                                                        }
                                                                        var arrayFancyExchange1 = new List<Double>();
                                                                        arrayFancyExchange1 = arrayFancyExchange;

                                                                        for (int i = 0; i < arrayFancyExchange1.Count; i++)
                                                                        {
                                                                            SqlCommand sqlFancyNotUpdate = new SqlCommand("UPDATE fancy_exchange SET stakes='" + arrayFancyExchange1[i] + "' WHERE id='" + session_not_fancy_exchange_ids[i] + "' ", con);
                                                                            sqlFancyNotUpdate.ExecuteNonQuery();
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Double difference = Getstack - totalStakeNot;
                                                                        UserNetBalance = user_bal + totalStakeNot - difference;
                                                                        updated_exposure = user_exp + difference - totalStakeNot;
                                                                        adjust_bal = 0 + totalStakeNot - difference;
                                                                        adjust_exp = 0 + difference - totalStakeNot;

                                                                        if (UserNetBalance < 0)
                                                                        {
                                                                            con.Close();
                                                                            return "Insufficient Balance";
                                                                        }

                                                                        for (int i1 = 0; i1 < session_not_fancy_exchange_ids.Count; i1++)
                                                                        {
                                                                            var sqlFancyNotUpdate = new SqlCommand("UPDATE fancy_exchange SET stakes='0' WHERE id='" + session_not_fancy_exchange_ids[i1] + "' ", con);
                                                                            sqlFancyNotUpdate.ExecuteNonQuery();
                                                                        }

                                                                        var sqlFancyExchInsert = new SqlCommand("INSERT INTO fancy_exchange(user_id, market_id, event_id, field_name, field_rate,stakes, created,modified,is_done) VALUES " +
                                                                          "('" + user_id + "','" + GetBetfairId + "','" + GetEventCode + "','" + field + "','" + betable_rate + "','" + difference + "','" + time.ToString(format1) + "','" + time.ToString(format1) + "','0') ", con);
                                                                        sqlFancyExchInsert.ExecuteNonQuery();
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    updated_exposure = user_exp + Getstack;
                                                                    UserNetBalance = user_bal - Getstack;
                                                                    adjust_exp = 0 + Getstack;
                                                                    adjust_bal = 0 - Getstack;

                                                                    if (UserNetBalance < 0)
                                                                    {
                                                                        con.Close();
                                                                        return "Insufficient Balance";
                                                                    }
                                                                    SqlCommand sqlFancyExchInsert = new SqlCommand("INSERT INTO fancy_exchange(user_id, market_id, event_id, field_name, field_rate,stakes, created,modified,is_done) VALUES " +
                                                                      "('" + user_id + "','" + GetBetfairId + "','" + GetEventCode + "','" + field + "','" + betable_rate + "','" + Getstack + "','" + time.ToString(format1) + "','" + time.ToString(format1) + "','0') ", con);
                                                                    sqlFancyExchInsert.ExecuteNonQuery();
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            updated_exposure = user_exp + Getstack;
                                                            UserNetBalance = user_bal - Getstack;
                                                            adjust_exp = 0 + Getstack;
                                                            adjust_bal = 0 - Getstack;

                                                            if (UserNetBalance < 0)
                                                            {
                                                                con.Close();
                                                                return "Insufficient Balance";
                                                            }
                                                            SqlCommand sqlFancyExchInsert = new SqlCommand("INSERT INTO fancy_exchange(user_id, market_id, event_id, field_name, field_rate,stakes, created,modified,is_done) VALUES " +
                                                              "('" + user_id + "','" + GetBetfairId + "','" + GetEventCode + "','" + field + "','" + betable_rate + "','" + Getstack + "','" + time.ToString(format1) + "','" + time.ToString(format1) + "','0') ", con);
                                                            sqlFancyExchInsert.ExecuteNonQuery();

                                                        }

                                                        SqlCommand sqlInsert = new SqlCommand("INSERT INTO live_bet (user_id ,dist_id ,md_id ,admin_id ,event_id ,betfair_id , " +
                                                          " field ,rate ,stakes ,total_value ,session_rate ,logic ,field_pos ,team_name ,runner_posi ,status ,place_time ,settled_time, " +
                                                          " odds_type ,input_stakes ,input_pl ,before_bal ,after_bal ,before_exp ,after_exp)" +
                                                          "VALUES ('" + user_id + "' ,'" + dist_id + "' ,'" + md_id + "' ,'" + admin_id + "' ,'" + GetEventCode + "' ,'" + GetBetfairId + "' , " +
                                                          " '" + field + "' ,'" + betable_rate + "' ,'" + Getstack + "' ,'" + TotalProfitLost + "' ,'" + session_rate + "' ,'' ,'" + field_pos + "' ,'' , " +
                                                          " '' ,'' ,'" + time.ToString(format1) + "','','sess' ,'" + Getstack + "' ,'" + TotalProfitLost + "' ,'" + bet_before_balance + "' , " +
                                                          " '" + UserNetBalance + "' ,'" + user_exp + "' ,'" + updated_exposure + "' ) ", con);
                                                        int runquery = sqlInsert.ExecuteNonQuery();
                                                        if (runquery == 1)
                                                        {
                                                            SqlCommand sqlUserBalanceUpdate = new SqlCommand("UPDATE users_client SET balance = balance + '" + adjust_bal + "' , exposure = exposure + '" + adjust_exp + "' WHERE id='" + user_id + "'", con);
                                                            sqlUserBalanceUpdate.ExecuteNonQuery();

                                                            SqlCommand sqlanaly = new SqlCommand("INSERT INTO live_bet_new(user_id, dist_id, md_id, admin_id, event_id, betfair_id, field, rate, stakes, total_value,status, input_stakes, created, odds_type) VALUES" +
                                                              " ('" + user_id + "','" + dist_id + "','" + md_id + "','" + admin_id + "','" + GetEventCode + "','" + GetBetfairId + "','" + field + "','" + betable_rate + "','" + Getstack + "','" + TotalProfitLost + "','','" + input_stakes + "','" + time.ToString(format1) + "','sess')", con);
                                                            sqlanaly.ExecuteNonQuery();

                                                            con.Close();
                                                            return "Success";
                                                        }
                                                        else
                                                        {
                                                            con.Close();
                                                            return "Error";
                                                        }
                                                    }
                                                }
                                                // End Place Bet For NOt/Yes Session
                                                else
                                                {
                                                    updated_exposure = user_exp + Getstack;
                                                    UserNetBalance = user_bal - Getstack;
                                                    adjust_exp = 0 + Getstack;
                                                    adjust_bal = 0 - Getstack;

                                                    if (UserNetBalance < 0)
                                                    {
                                                        con.Close();
                                                        return "Insufficient Balance";
                                                    }
                                                    string field_pos1 = "a";
                                                    if (GetBoxtype == "even")
                                                    {
                                                        field_pos1 = "b";
                                                    }
                                                    SqlCommand sqlInsert = new SqlCommand("INSERT INTO live_bet (user_id ,dist_id ,md_id ,admin_id ,event_id ,betfair_id , " +
                                                      " field ,rate ,stakes ,total_value ,session_rate ,logic ,field_pos ,team_name ,runner_posi ,status ,place_time ,settled_time, " +
                                                      " odds_type ,input_stakes ,input_pl ,before_bal ,after_bal ,before_exp ,after_exp)" +
                                                      "VALUES ('" + user_id + "' ,'" + dist_id + "' ,'" + md_id + "' ,'" + admin_id + "' ,'" + GetEventCode + "' ,'" + GetBetfairId + "' , " +
                                                      " '" + GetBoxtype + "' ,'" + betable_rate + "' ,'" + Getstack + "' ,'" + TotalProfitLost + "' ,'" + session_rate + "' ,'' ,'" + field_pos1 + "' ,'' , " +
                                                      " '' ,'' ,'" + time.ToString(format1) + "','','fancy' ,'" + Getstack + "' ,'" + TotalProfitLost + "' ,'" + bet_before_balance + "' , " +
                                                      " '" + UserNetBalance + "' ,'" + user_exp + "' ,'" + updated_exposure + "' ) ", con);
                                                    int runquery1 = sqlInsert.ExecuteNonQuery();
                                                    if (runquery1 == 1)
                                                    {
                                                        SqlCommand sqlUserBalanceUpdate = new SqlCommand("UPDATE users_client SET balance = balance + '" + adjust_bal + "' , exposure = exposure + '" + adjust_exp + "' WHERE id='" + user_id + "'", con);
                                                        sqlUserBalanceUpdate.ExecuteNonQuery();

                                                        SqlCommand sqlanaly = new SqlCommand("INSERT INTO live_bet_new(user_id, dist_id, md_id, admin_id, event_id, betfair_id, field, rate, stakes, total_value,status, input_stakes, created, odds_type) VALUES" +
                                                          " ('" + user_id + "','" + dist_id + "','" + md_id + "','" + admin_id + "','" + GetEventCode + "','" + GetBetfairId + "','" + GetBoxtype + "','" + betable_rate + "','" + Getstack + "','" + TotalProfitLost + "','','" + input_stakes + "','" + time.ToString(format1) + "','fancy')", con);
                                                        sqlanaly.ExecuteNonQuery();

                                                        con.Close();
                                                        return "Success";
                                                    }
                                                    else
                                                    {
                                                        con.Close();
                                                        return "Error";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                con.Close();
                                                return "Market Not Available";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    gjhgf = "Error";
                                }
                            }
                        }
                        else
                        {
                            gjhgf = "Please Login Again to Place Bet";
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                gjhgf = "Error in Place Bet";
            }
            return gjhgf;
        }
        [HttpPost]
        public ActionResult BetPlaceMSP(BetPlaceD obj)
        {
            string GetBetfairId = "";
            string GetEventCode = "";
            string GetEventType = "";
            string GetstackValue = "";
            int Getindex = 999;
            string GetBoxtype = "";
            string GetOdds = "";
            string GetVolume = "";
            string GetRunnerName = "";
            string GetMarketName = "";

            string SendResponse = "";
            GetBetfairId = obj.BetfairId;
            GetEventCode = obj.EventCode;
            GetEventType = obj.EventType;
            GetVolume = obj.OddsVolume;
            GetstackValue = obj.stackValue;
            Getindex = obj.RunnerIndex;
            GetBoxtype = obj.BoxType;
            GetOdds = obj.OddsValue;
            GetRunnerName = obj.RunnerNameget;
            GetMarketName = obj.MarketName;
            string xid = FunctionDataController.Getxid(GetEventCode);

            int delay_time = GetSessDelay(GetEventCode);
            Thread.Sleep(delay_time);
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://probet247.com/exchange/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = client.GetAsync("GetNewsData?xid=" + xid + "&type=" + GetEventType).Result;
                    if (response != null)
                    {
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        for (int i = 0; i < responseJson.Count; i++)
                        {
                            string selectioniii = responseJson[i].TempId;
                            string selectioniii_v = responseJson[i].SelectionId;
                            if (selectioniii == GetBetfairId)
                            {
                                string GameStatus = responseJson[i].GameStatus;
                                if (GameStatus == "")
                                {
                                    if (GetBoxtype == "Not")
                                    {
                                        string CheckOutRate = responseJson[i].LayPrice1;
                                        Double vtyugh = Double.Parse(GetOdds);
                                        string LaySize1 = responseJson[i].LaySize1;
                                        Double SessionRate = Double.Parse(LaySize1);
                                        String Session_Name = responseJson[i].RunnerName;
                                        if (GetOdds == CheckOutRate && LaySize1 == GetVolume && GetMarketName == Session_Name)
                                        {
                                            SendResponse = PlaceBetSP(vtyugh, SessionRate, selectioniii_v, GetEventCode, GetstackValue,
                                                    Getindex, GetBoxtype, GetOdds, GetRunnerName, Session_Name);
                                        }
                                        else
                                        {
                                            SendResponse = "Odds Changed";
                                        }
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                    else if (GetBoxtype == "Yes")
                                    {
                                        string CheckOutRate = responseJson[i].BackPrice1;
                                        Double vtyugh = Double.Parse(GetOdds);
                                        string BackSize1 = responseJson[i].BackSize1;
                                        Double SessionRate = Double.Parse(BackSize1);
                                        String Session_Name = responseJson[i].RunnerName;
                                        if (GetOdds == CheckOutRate && BackSize1 == GetVolume && GetMarketName == Session_Name)
                                        {
                                            SendResponse = PlaceBetSP(vtyugh, SessionRate, selectioniii_v, GetEventCode, GetstackValue,
                                                    Getindex, GetBoxtype, GetOdds, Session_Name, GetMarketName);
                                        }
                                        else
                                        {
                                            SendResponse = "Odds Changed";
                                        }
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        SendResponse = "Error in Submit Bet";
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                else
                                {
                                    SendResponse = "Market Suspended";
                                    return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                    else
                    {
                        SendResponse = "Error in Submitting Bet";
                    }
                }
                catch (Exception ex)
                {
                    SendResponse = "Error in Place Bet";
                    return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (TaskCanceledException ex)
            {
                SendResponse = "Error in Place Bet";
                return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
        }

        public string PlaceBetSP(Double betable_rate, Double session_rate, string GetBetfairId, string GetEventCode, string GetstackValue,
                                int Getindex, string GetBoxtype, string GetOdds, string GetRunnerName, string GetMarketName)
        {
            string gjhgf = "Error in Submitting Bet";
            try
            {
                string user_ids = (string)System.Web.HttpContext.Current.Session["UIDS"];
                string hash_keys = (string)System.Web.HttpContext.Current.Session["hash_keyS"];
                string agm_id = (string)System.Web.HttpContext.Current.Session["AgentMID"];
                int user_id = Int32.Parse(user_ids);
                string hash_key = hash_keys;
                Double user_bal = 0;
                Double user_exp = 0;
                Double updated_exposure = 0;
                Double UserNetBalance = 0;
                Double adjust_exp = 0;
                Double adjust_bal = 0;
                Double bet_before_balance = 0;
                int dist_id = 0;
                int md_id = 0;
                int admin_id = 0;
                Double minstakes = 1;
                Double maxStakes = 200;
                string Inr_coin = "yes";//GetIsInr(agm_id);
                if (Inr_coin == "yes")
                {
                    minstakes = 100;
                    maxStakes = 25000;
                }
                Double Getstack = Double.Parse(GetstackValue, CultureInfo.InvariantCulture.NumberFormat);
                Double input_stakes = Getstack;
                Double TotalProfitLost = 0;
                DateTime time = DateTime.Now;
                string format1 = "yyyy-MM-dd HH:mm:ss";

                if (Getstack < minstakes)
                {
                    return "Minimum Stakes Limit is " + minstakes;
                }
                if (Getstack > maxStakes)
                {
                    return "Maximum Stakes Limit is " + maxStakes;
                }

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    using (var cmd = new SqlCommand("SELECT [exposure_limit],[balance],[exposure],[dl_id],[mdl_id],[admin_id] FROM users_client WHERE id='" + user_id + "' and hash_key='" + hash_key + "' AND status='activate' "))
                    {
                        cmd.Connection = con;
                        con.Open();
                        var reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            reader.Read();
                            user_bal = (Double)reader["balance"];
                            bet_before_balance = user_bal;
                            user_exp = (Double)reader["exposure"];
                            Double exposure_limit = (Double)reader["exposure_limit"];
                            dist_id = (int)reader["dl_id"];
                            md_id = (int)reader["mdl_id"];
                            admin_id = (int)reader["admin_id"];

                            using (var cmd1 = new SqlCommand("SELECT sport_id FROM matches WHERE status='OPEN' AND event_code = '" + GetEventCode + "'", con))
                            {
                                var dataM = cmd1.ExecuteReader();
                                if (dataM.HasRows)
                                {
                                    dataM.Read();
                                    string sport_id = (string)dataM["sport_id"];
                                    if (sport_id != "4")
                                    {
                                        if (GetMarketName == "Total Point" || GetMarketName == "Total Point " || GetMarketName.Contains("Total Point [") || GetMarketName == "Total Goals" || GetMarketName == "Total Goals " || GetMarketName.Contains("Total Goals ["))
                                        {
                                            GetMarketName = GetMarketName + " - " + betable_rate;
                                        }
                                    }
                                    using (var cmd11 = new SqlCommand("SELECT betfair_id, status FROM markets WHERE market_name = '" + GetMarketName + "'  AND event_code = '" + GetEventCode + "'", con))
                                    {
                                        var dataMM = cmd11.ExecuteReader();
                                        if (dataMM.HasRows)
                                        {
                                            dataMM.Read();
                                            GetBetfairId = (string)dataMM["betfair_id"];
                                            string status_mk = (string)dataMM["status"];
                                            if (status_mk == "activate")
                                            {
                                            }
                                            else
                                            {
                                                return "Market Closed";
                                            }
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("INSERT INTO  markets (market_name,back1,lay1,back2,lay2,back3,lay3,status,result_value," +
                                              "market_settle,created,league_id,sport_id,betfair_id,event_code,nested_market,runner_name_done,type,x_check_type) " +
                                              "VALUES ('" + GetMarketName + "' ,'' ,'' ,'' ,'' ,'' ,'' ,'activate' ,'' ,'' ,'" + time.ToString(format1) + "' ,'' ,'" + sport_id + "' ,'" + GetBetfairId + "' ,'" + GetEventCode + "' ,'' ,'done','sess','ges') ", con);
                                            int runquery1 = cmd3.ExecuteNonQuery();
                                            if (runquery1 == 1)
                                            {

                                            }
                                            else
                                            {
                                                return "Market Not Available";
                                            }
                                        }

                                        using (var cmd111 = new SqlCommand("SELECT id FROM markets WHERE status='activate' AND market_name = '" + GetMarketName + "'  AND event_code = '" + GetEventCode + "'", con))
                                        {
                                            var sqlMarket = cmd111.ExecuteReader();
                                            if (sqlMarket.HasRows)
                                            {
                                                string field = "";
                                                string field_pos = "";
                                                if (GetBoxtype == "Not")
                                                {
                                                    field_pos = "a";
                                                    field = "Not";
                                                    TotalProfitLost = Getstack;
                                                    Getstack = (Getstack * session_rate) / 100;
                                                }
                                                else if (GetBoxtype == "Yes")
                                                {
                                                    field_pos = "b";
                                                    field = "Yes";
                                                    Getstack = Getstack;
                                                    TotalProfitLost = (Getstack * session_rate) / 100;
                                                }
                                                else { }

                                                Double balance = Getstack;
                                                UserNetBalance = user_bal - balance;

                                                double totalbettp = gettptotal(user_id, GetBetfairId, GetEventCode);
                                                Double max_profit_limit = GetSessLimit(GetEventCode);
                                                double vyvgj = totalbettp + TotalProfitLost;
                                                if (vyvgj > exposure_limit)
                                                {
                                                    return "Maximum Exposure limit is " + exposure_limit + " Coins";
                                                }
                                                if (vyvgj > max_profit_limit)
                                                {
                                                    return "Maximum Profit limit is " + max_profit_limit + " Coins";
                                                }
                                                if (GetBoxtype == "Not" || GetBoxtype == "Yes")
                                                {
                                                    updated_exposure = user_exp + Getstack;
                                                    UserNetBalance = user_bal - Getstack;
                                                    adjust_exp = 0 + Getstack;
                                                    adjust_bal = 0 - Getstack;

                                                    if (UserNetBalance < 0)
                                                    {
                                                        con.Close();
                                                        return "Insufficient Balance";
                                                    }
                                                    SqlCommand sqlFancyExchInsert = new SqlCommand("INSERT INTO fancy_exchange(user_id, market_id, event_id, field_name, field_rate,stakes, created,modified,is_done) VALUES " +
                                                        "('" + user_id + "','" + GetBetfairId + "','" + GetEventCode + "','" + field + "','" + betable_rate + "','" + Getstack + "','" + time.ToString(format1) + "','" + time.ToString(format1) + "','0') ", con);
                                                    sqlFancyExchInsert.ExecuteNonQuery();

                                                    SqlCommand sqlInsert = new SqlCommand("INSERT INTO live_bet (user_id ,dist_id ,md_id ,admin_id ,event_id ,betfair_id , " +
                                                        " field ,rate ,stakes ,total_value ,session_rate ,logic ,field_pos ,team_name ,runner_posi ,status ,place_time ,settled_time, " +
                                                        " odds_type ,input_stakes ,input_pl ,before_bal ,after_bal ,before_exp ,after_exp)" +
                                                        "VALUES ('" + user_id + "' ,'" + dist_id + "' ,'" + md_id + "' ,'" + admin_id + "' ,'" + GetEventCode + "' ,'" + GetBetfairId + "' , " +
                                                        " '" + field + "' ,'" + betable_rate + "' ,'" + Getstack + "' ,'" + TotalProfitLost + "' ,'" + session_rate + "' ,'' ,'" + field_pos + "' ,'' , " +
                                                        " '' ,'' ,'" + time.ToString(format1) + "','','sess' ,'" + Getstack + "' ,'" + TotalProfitLost + "' ,'" + bet_before_balance + "' , " +
                                                        " '" + UserNetBalance + "' ,'" + user_exp + "' ,'" + updated_exposure + "' ) ", con);
                                                    int runquery = sqlInsert.ExecuteNonQuery();
                                                    if (runquery == 1)
                                                    {
                                                        SqlCommand sqlUserBalanceUpdate = new SqlCommand("UPDATE users_client SET balance = balance + '" + adjust_bal + "' , exposure = exposure + '" + adjust_exp + "' WHERE id='" + user_id + "'", con);
                                                        sqlUserBalanceUpdate.ExecuteNonQuery();

                                                        SqlCommand sqlanaly = new SqlCommand("INSERT INTO live_bet_new(user_id, dist_id, md_id, admin_id, event_id, betfair_id, field, rate, stakes, total_value,status, input_stakes, created, odds_type) VALUES" +
                                                            " ('" + user_id + "','" + dist_id + "','" + md_id + "','" + admin_id + "','" + GetEventCode + "','" + GetBetfairId + "','" + field + "','" + betable_rate + "','" + Getstack + "','" + TotalProfitLost + "','','" + input_stakes + "','" + time.ToString(format1) + "','sess')", con);
                                                        sqlanaly.ExecuteNonQuery();

                                                        con.Close();
                                                        return "Success";
                                                    }
                                                    else
                                                    {
                                                        con.Close();
                                                        return "Error";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                con.Close();
                                                return "Market Not Available";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    gjhgf = "Error";
                                }
                            }
                        }
                        else
                        {
                            gjhgf = "Please Login Again to Place Bet";
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return gjhgf;
        }
        public DateTime GetMatchTimeDB(string Eve_Code)
        {

            DateTime MAtchTG = new DateTime();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT match_time FROM matches where event_code= '" + Eve_Code + "' "))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        MAtchTG = (DateTime)reader["match_time"];
                        con.Close();
                        return MAtchTG;
                    }
                }
            }
            return MAtchTG;
        }

        public String teenpattioddsrate(string Eve_Code, string GetBetfairId, int Getindex)
        {

            string odds_rate = "";
            string evv_name = "";
            int i = 999;
            if (Eve_Code == "20202020")
            {
                evv_name = "TeenPatiT20";
                if (Getindex == 1)
                {
                    i = 0;
                }
                else if (Getindex == 2)
                {
                    i = 2;
                }
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://diamond8exch.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("CasinoData/" + evv_name).Result;
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        if (responseJson != null)
                        {
                            var t1 = responseJson.t1;
                            string autotime = t1[0]["autotime"];
                            int autotime1 = int.Parse(autotime);
                            var t2 = responseJson.t2;
                            string betfair_id = t2[i]["mid"];
                            string status = t2[i]["gstatus"];
                            if (status == "1" && betfair_id == GetBetfairId && autotime1 > 4)
                            {
                                odds_rate = t2[i]["rate"];
                            }
                            else
                            {
                                odds_rate = "0";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        odds_rate = "0";
                    }
                }
                catch (TaskCanceledException ex)
                {
                    odds_rate = "0";
                }
            }
            else if (Eve_Code == "30303030")
            {
                evv_name = "TeenPatiT20B";
                if (Getindex == 1)
                {
                    i = 0;
                }
                else if (Getindex == 2)
                {
                    i = 2;
                }
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://skyinplay99.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("CasinoData/" + evv_name).Result;
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        if (responseJson != null)
                        {
                            var t1 = responseJson.t1;
                            string autotime = t1[0]["autotime"];
                            int autotime1 = int.Parse(autotime);
                            var t2 = responseJson.t2;
                            string betfair_id = t2[i]["mid"];
                            string status = t2[i]["gstatus"];
                            if (status == "1" && betfair_id == GetBetfairId && autotime1 > 4)
                            {
                                odds_rate = t2[i]["rate"];
                            }
                            else
                            {
                                odds_rate = "0";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        odds_rate = "0";
                    }
                }
                catch (TaskCanceledException ex)
                {
                    odds_rate = "0";
                }
            }

            else if (Eve_Code == "50505050")
            {
                evv_name = "dt20";
                if (Getindex == 1)
                {
                    i = 0;
                }
                else if (Getindex == 2)
                {
                    i = 1;
                }
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://api_link.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("t20api.php?type=" + evv_name).Result;
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        if (responseJson != null)
                        {
                            var t1 = responseJson.data.t1;
                            string autotime = t1[0]["autotime"];
                            int autotime1 = int.Parse(autotime);
                            var t2 = responseJson.data.t2;
                            string betfair_id = t2[i]["mid"];
                            string status = t2[i]["gstatus"];
                            if (status == "1" && betfair_id == GetBetfairId && autotime1 > 4)
                            {
                                odds_rate = t2[i]["rate"];
                            }
                            else
                            {
                                odds_rate = "0";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        odds_rate = "0";
                    }
                }
                catch (TaskCanceledException ex)
                {
                    odds_rate = "0";
                }
            }

            else if (Eve_Code == "51515151")
            {
                evv_name = "dtl20";
                if (Getindex == 1)
                {
                    i = 0;
                }
                else if (Getindex == 21)
                {
                    i = 18;
                }
                else if (Getindex == 41)
                {
                    i = 36;
                }
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://api_link.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("t20api.php?type=" + evv_name).Result;
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        if (responseJson != null)
                        {
                            var t1 = responseJson.data.t1;
                            string autotime = t1[0]["autotime"];
                            int autotime1 = int.Parse(autotime);
                            var t2 = responseJson.data.t2;
                            string betfair_id = t2[i]["mid"];
                            string status = t2[i]["gstatus"];
                            if (status == "1" && betfair_id == GetBetfairId && autotime1 > 4)
                            {
                                odds_rate = t2[i]["b1"];
                            }
                            else
                            {
                                odds_rate = "0";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        odds_rate = "0";
                    }
                }
                catch (TaskCanceledException ex)
                {
                    odds_rate = "0";
                }
            }

            else if (Eve_Code == "60606060")
            {
                evv_name = "lucky7";
                if (Getindex == 1)
                {
                    i = 0;
                }
                else if (Getindex == 2)
                {
                    i = 1;
                }

                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://skyinplay99.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("CasinoData/Lucky7").Result;
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        if (responseJson != null)
                        {
                            var t1 = responseJson.data.t1;
                            string autotime = t1[0]["autotime"];
                            int autotime1 = int.Parse(autotime);
                            var t2 = responseJson.data.t2;
                            string betfair_id = t2[i]["mid"];
                            string status = t2[i]["gstatus"];
                            if (status == "1" && betfair_id == GetBetfairId && autotime1 > 4)
                            {
                                odds_rate = t2[i]["rate"];
                            }
                            else
                            {
                                odds_rate = "0";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        odds_rate = "0";
                    }
                }
                catch (TaskCanceledException ex)
                {
                    odds_rate = "0";
                }
            }

            else if (Eve_Code == "61616161")
            {
                evv_name = "lucky7B";
                if (Getindex == 1)
                {
                    i = 0;
                }
                else if (Getindex == 2)
                {
                    i = 1;
                }

                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://api_link.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("t20api.php?type=" + evv_name).Result;
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        if (responseJson != null)
                        {
                            var t1 = responseJson.data.t1;
                            string autotime = t1[0]["autotime"];
                            int autotime1 = int.Parse(autotime);
                            var t2 = responseJson.data.t2;
                            string betfair_id = t2[i]["mid"];
                            string status = t2[i]["gstatus"];
                            if (status == "1" && betfair_id == GetBetfairId && autotime1 > 4)
                            {
                                odds_rate = t2[i]["rate"];
                            }
                            else
                            {
                                odds_rate = "0";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        odds_rate = "0";
                    }
                }
                catch (TaskCanceledException ex)
                {
                    odds_rate = "0";
                }
            }

            else if (Eve_Code == "40404040")
            {
                evv_name = "TeenPatiT20Poker";
                if (Getindex == 11)
                {
                    i = 0;
                }
                else if (Getindex == 21)
                {
                    i = 1;
                }

                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://api_link.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("t20api.php?type=" + evv_name).Result;
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        if (responseJson != null)
                        {
                            var t1 = responseJson.data.t1;
                            string autotime = t1[0]["autotime"];
                            int autotime1 = int.Parse(autotime);

                           

                            var t2 = responseJson.data.t2;
                            string betfair_id = t2[i]["mid"];
                            string status = t2[i]["gstatus"];
                            if (status == "1" && betfair_id == GetBetfairId && autotime1 > 4)
                            {
                                odds_rate = t2[i]["rate"];
                            }
                            else
                            {
                                odds_rate = "0";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        odds_rate = "0";
                    }
                }
                catch (TaskCanceledException ex)
                {
                    odds_rate = "0";
                }
            }


            return odds_rate;
        }

        public ActionResult BetPlaceMTP(BetPlaceD obj)
        {
            string SendResponse = "";
            try
            {
                string CheckMarketIsInsertOrNot = "";
                string GetBetfairId = "";
                string GetEventCode = "";
                string GetstackValue = "";
                int Getindex = 999;
                string GetBoxtype = "";
                string GetOdds = "";
                string GetRunnerName = "";
                string GetMarketName = "";

                GetBetfairId = obj.BetfairId;
                GetEventCode = obj.EventCode;
                GetstackValue = obj.stackValue;
                Getindex = obj.RunnerIndex;
                GetBoxtype = obj.BoxType;
                GetOdds = obj.OddsValue;
                GetRunnerName = obj.RunnerNameget;
                GetMarketName = obj.MarketName;
                float GetOddsFloat = float.Parse(GetOdds, CultureInfo.InvariantCulture.NumberFormat);
                if (GetEventCode == "51515151")
                {
                    if (Getindex == 2)
                    {
                        Getindex = 21;
                    }
                    else if (Getindex == 3)
                    {
                        Getindex = 41;
                    }
                }
                else if (GetEventCode == "40404040")
                {
                    if (Getindex == 1)
                    {
                        Getindex = 11;
                    }
                    else if (Getindex == 2)
                    {
                        Getindex = 21;
                    }
                }

                int delay_time = GetMODelay(GetEventCode);
                Thread.Sleep(delay_time);
                string odds_rate = teenpattioddsrate(GetEventCode, GetBetfairId, Getindex);
                if (odds_rate == "0")
                {
                    return Json(new { success = true, responseText = "Market Suspended" }, JsonRequestBehavior.AllowGet);
                }
                else if (odds_rate == GetOdds && odds_rate != "" && odds_rate != "0")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        using (var cmd22 = new SqlCommand("SELECT id, status FROM markets WHERE betfair_id='" + GetBetfairId + "'", con))
                        {
                            cmd22.Connection = con;
                            con.Open();
                            var dataMM = cmd22.ExecuteReader();
                            if (dataMM.HasRows)
                            {
                                dataMM.Read();
                                string status_mk = (string)dataMM["status"];
                                if (status_mk == "activate")
                                {
                                }
                                else
                                {
                                    SendResponse = "Market Closed";
                                    return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                }
                                CheckMarketIsInsertOrNot = "Success";
                            }
                            else
                            {
                                CheckMarketIsInsertOrNot = OtherController.Submitdfx(GetEventCode, GetBetfairId);
                            }
                            con.Close();
                        }
                    }

                    if (CheckMarketIsInsertOrNot == "Success")
                    {
                        float betable_rate = float.Parse(odds_rate, CultureInfo.InvariantCulture.NumberFormat);
                        string user_ids = (string)System.Web.HttpContext.Current.Session["UIDS"];
                        string hash_keys = (string)System.Web.HttpContext.Current.Session["hash_keyS"];
                        string agm_id = (string)System.Web.HttpContext.Current.Session["AgentMID"];
                        int user_id = Int32.Parse(user_ids);
                        string hash_key = hash_keys;
                        Double user_bal = 0;
                        Double user_exp = 0;
                        int dist_id = 0;
                        int md_id = 0;
                        int admin_id = 0;
                        Double minstakes = 1;
                        Double maxStakes = 250;
                        string Inr_coin = "yes";// GetIsInr(agm_id);
                        if (Inr_coin == "yes")
                        {
                            minstakes = 100;
                            maxStakes = 25000;
                        }
                        float Getstack = float.Parse(GetstackValue, CultureInfo.InvariantCulture.NumberFormat);
                        var calamt = new List<Double>();
                        var calnewamt = new List<Double>();

                        if (Getstack < minstakes)
                        {
                            SendResponse = "Minimum Stakes Limit is " + minstakes;
                            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                        }
                        if (Getstack > maxStakes)
                        {
                            SendResponse = "Maximum Stakes Limit is " + maxStakes;
                            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                        }

                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            using (var cmd = new SqlCommand("SELECT [exposure_limit],[balance],[exposure],[dl_id],[mdl_id],[admin_id] FROM users_client WHERE id='" + user_id + "' and hash_key='" + hash_key + "' AND status='activate' "))
                            {
                                cmd.Connection = con;
                                con.Open();
                                var reader = cmd.ExecuteReader();

                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    user_bal = (Double)reader["balance"];
                                    user_exp = (Double)reader["exposure"];
                                    Double exposure_limit = (Double)reader["exposure_limit"];
                                    dist_id = (int)reader["dl_id"];
                                    md_id = (int)reader["mdl_id"];
                                    admin_id = (int)reader["admin_id"];

                                    float stakes = 0;
                                    float profit = 0;
                                    float Get_total = Getstack * betable_rate;
                                    float Get_total_val = Get_total - Getstack;
                                    if (GetBoxtype == "back")
                                    {
                                        stakes = Getstack;
                                        profit = Get_total_val;
                                    }
                                    else if (GetBoxtype == "lay")
                                    {
                                        stakes = Get_total_val;
                                        profit = Getstack;
                                    }
                                    double totalbettp = gettptotal(user_id, GetBetfairId, GetEventCode);
                                    Double max_profit_limit = GetMOLimit(GetEventCode);
                                    double vyvgj = totalbettp + profit;
                                    if (vyvgj > exposure_limit)
                                    {
                                        SendResponse = "Maximum Exposure limit is " + exposure_limit + " Coins";
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                    if (vyvgj > max_profit_limit)
                                    {
                                        SendResponse = "Maximum Profit limit is " + max_profit_limit + " Coins";
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                    Double new_user_exp = user_exp + stakes;
                                    Double new_user_bal = user_bal - stakes;
                                    Double adjust_exp = 0 + stakes;
                                    Double adjust_bal = 0 - stakes;
                                    if (new_user_bal < 0)
                                    {
                                        SendResponse = "Insufficient Balance";
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                    DateTime time = DateTime.Now;
                                    string format1 = "yyyy-MM-dd HH:mm:ss";

                                    string field = GetBoxtype + "(" + GetRunnerName + ")";
                                    SqlCommand cmdlive_bet = new SqlCommand("INSERT INTO live_bet (user_id ,dist_id ,md_id ,admin_id ,event_id ,betfair_id , " +
                                        " field ,rate ,stakes ,total_value ,session_rate ,logic ,field_pos ,team_name ,runner_posi ,status ,place_time ,settled_time, " +
                                        " odds_type ,input_stakes ,input_pl ,before_bal ,after_bal ,before_exp ,after_exp)" +
                                        "VALUES ('" + user_id + "' ,'" + dist_id + "' ,'" + md_id + "' ,'" + admin_id + "' ,'" + GetEventCode + "' ,'" + GetBetfairId + "' , " +
                                        " '" + field + "' ,'" + betable_rate + "' ,'" + stakes + "' ,'" + profit + "' ,'' ,'' ,'" + GetBoxtype + "' ,'" + GetRunnerName + "' , " +
                                        " '" + Getindex + "' ,'' ,'" + time.ToString(format1) + "','','TP' ,'" + Getstack + "' ,'" + Get_total + "' ,'" + user_bal + "' , " +
                                        " '" + new_user_bal + "' ,'" + user_exp + "' ,'" + new_user_exp + "' ) ", con);
                                    cmdlive_bet.ExecuteNonQuery();
                                    SqlCommand cmduser = new SqlCommand("UPDATE users_client SET balance = balance + '" + adjust_bal + "' , exposure = exposure + '" + adjust_exp + "' WHERE id = '" + user_id + "' ", con);
                                    cmduser.ExecuteNonQuery();
                                    con.Close();
                                    SendResponse = "Bet Submitted Successfully";

                                }
                                else
                                {
                                    SendResponse = "Please Login Again to Place any Bet";
                                }
                                con.Close();

                            }
                        }
                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = true, responseText = CheckMarketIsInsertOrNot }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    string vgf = "Odds Changed";
                    return Json(new { success = true, responseText = vgf }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                SendResponse = "Error in Place Bet";
            }
            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult BetPlaceMTPNik(BetPlaceD obj)
        {
            string SendResponse = "";
            try
            {
                string CheckMarketIsInsertOrNot = "";
                string GetBetfairId = "";
                string GetEventCode = "";
                string GetstackValue = "";
                int Getindex = 999;
                string GetBoxtype = "";
                string GetOdds = "";
                string GetRunnerName = "";
                string GetMarketName = "";

                GetBetfairId = obj.BetfairId;
                GetEventCode = obj.EventCode;
                GetstackValue = obj.stackValue;
                Getindex = obj.RunnerIndex;
                GetBoxtype = obj.BoxType;
                GetOdds = "2.00";
                GetRunnerName = obj.RunnerNameget;
                GetMarketName = obj.MarketName;
                float GetOddsFloat = float.Parse(GetOdds, CultureInfo.InvariantCulture.NumberFormat);
                if (GetEventCode == "51515151")
                {
                    if (Getindex == 2)
                    {
                        Getindex = 21;
                    }
                    else if (Getindex == 3)
                    {
                        Getindex = 41;
                    }
                }
                else if (GetEventCode == "40404040")
                {
                    if (Getindex == 1)
                    {
                        Getindex = 11;
                    }
                    else if (Getindex == 2)
                    {
                        Getindex = 21;
                    }
                }

                int delay_time = GetMODelay(GetEventCode);
                Thread.Sleep(100);
                string odds_rate = GetOdds;//teenpattioddsrate(GetEventCode, GetBetfairId, Getindex);
                if (odds_rate == "0")
                {
                    return Json(new { success = true, responseText = "Market Suspended" }, JsonRequestBehavior.AllowGet);
                }
                else if (odds_rate == GetOdds && odds_rate != "" && odds_rate != "0")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionBFPBF"].ToString()))
                    {
                        using (var cmd22 = new SqlCommand("SELECT id, status FROM markets WHERE betfair_id='" + GetBetfairId + "'", con))
                        {
                            cmd22.Connection = con;
                            con.Open();
                            var dataMM = cmd22.ExecuteReader();
                            if (dataMM.HasRows)
                            {
                                dataMM.Read();
                                string status_mk = (string)dataMM["status"];
                                if (status_mk == "activate")
                                {
                                }
                                else
                                {
                                    SendResponse = "Market Closed";
                                    return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                }
                                CheckMarketIsInsertOrNot = "Success";
                            }
                            else
                            {
                                CheckMarketIsInsertOrNot = OtherController.Submitdfxs(GetEventCode, GetBetfairId);
                            }
                            con.Close();
                        }
                    }

                    if (CheckMarketIsInsertOrNot == "Success")
                    {
                        float betable_rate = float.Parse(odds_rate, CultureInfo.InvariantCulture.NumberFormat);
                        string user_ids = "4966";
                        string hash_keys = "0B0BBE0CA5A0CD13991B837FFFF6CF3D";
                        string agm_id = "2";
                        int user_id = Int32.Parse(user_ids);
                        string hash_key = hash_keys;
                        Double user_bal = 0;
                        Double user_exp = 0;
                        int dist_id = 0;
                        int md_id = 0;
                        int admin_id = 0;
                        Double minstakes = 1;
                        Double maxStakes = 250;
                        string Inr_coin = "yes";// GetIsInr(agm_id);
                        if (Inr_coin == "yes")
                        {
                            minstakes = 100;
                            maxStakes = 25000;
                        }
                        float Getstack = float.Parse(GetstackValue, CultureInfo.InvariantCulture.NumberFormat);
                        var calamt = new List<Double>();
                        var calnewamt = new List<Double>();

                        if (Getstack < minstakes)
                        {
                            SendResponse = "Minimum Stakes Limit is " + minstakes;
                            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                        }
                        if (Getstack > maxStakes)
                        {
                            SendResponse = "Maximum Stakes Limit is " + maxStakes;
                            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                        }

                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionBFPBF"].ToString()))
                        {
                            using (var cmd = new SqlCommand("SELECT [exposure_limit],[balance],[exposure],[dl_id],[mdl_id],[admin_id] FROM users_client WHERE id='" + user_id + "' and hash_key='" + hash_key + "' AND status='activate' "))
                            {
                                cmd.Connection = con;
                                con.Open();
                                var reader = cmd.ExecuteReader();

                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    user_bal = (Double)reader["balance"];
                                    user_exp = (Double)reader["exposure"];
                                    Double exposure_limit = (Double)reader["exposure_limit"];
                                    dist_id = (int)reader["dl_id"];
                                    md_id = (int)reader["mdl_id"];
                                    admin_id = (int)reader["admin_id"];

                                    float stakes = 0;
                                    float profit = 0;
                                    float Get_total = Getstack * betable_rate;
                                    float Get_total_val = Get_total - Getstack;
                                    if (GetBoxtype == "back")
                                    {
                                        stakes = Getstack;
                                        profit = Get_total_val;
                                    }
                                    else if (GetBoxtype == "lay")
                                    {
                                        stakes = Get_total_val;
                                        profit = Getstack;
                                    }
                                    double totalbettp = gettptotal(user_id, GetBetfairId, GetEventCode);
                                    Double max_profit_limit = GetMOLimit(GetEventCode);
                                    double vyvgj = totalbettp + profit;
                                    if (vyvgj > exposure_limit)
                                    {
                                        SendResponse = "Maximum Exposure limit is " + exposure_limit + " Coins";
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                    if (vyvgj > max_profit_limit)
                                    {
                                        SendResponse = "Maximum Profit limit is " + max_profit_limit + " Coins";
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                    Double new_user_exp = user_exp + stakes;
                                    Double new_user_bal = user_bal - stakes;
                                    Double adjust_exp = 0 + stakes;
                                    Double adjust_bal = 0 - stakes;
                                    if (new_user_bal < 0)
                                    {
                                        SendResponse = "Insufficient Balance";
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                    DateTime time = DateTime.Now;
                                    string format1 = "yyyy-MM-dd HH:mm:ss";

                                    string field = GetBoxtype + "(" + GetRunnerName + ")";
                                    SqlCommand cmdlive_bet = new SqlCommand("INSERT INTO live_bet (user_id ,dist_id ,md_id ,admin_id ,event_id ,betfair_id , " +
                                        " field ,rate ,stakes ,total_value ,session_rate ,logic ,field_pos ,team_name ,runner_posi ,status ,place_time ,settled_time, " +
                                        " odds_type ,input_stakes ,input_pl ,before_bal ,after_bal ,before_exp ,after_exp)" +
                                        "VALUES ('" + user_id + "' ,'" + dist_id + "' ,'" + md_id + "' ,'" + admin_id + "' ,'" + GetEventCode + "' ,'" + GetBetfairId + "' , " +
                                        " '" + field + "' ,'" + betable_rate + "' ,'" + stakes + "' ,'" + profit + "' ,'' ,'' ,'" + GetBoxtype + "' ,'" + GetRunnerName + "' , " +
                                        " '" + Getindex + "' ,'' ,'" + time.ToString(format1) + "','','TP' ,'" + Getstack + "' ,'" + Get_total + "' ,'" + user_bal + "' , " +
                                        " '" + new_user_bal + "' ,'" + user_exp + "' ,'" + new_user_exp + "' ) ", con);
                                    cmdlive_bet.ExecuteNonQuery();
                                    SqlCommand cmduser = new SqlCommand("UPDATE users_client SET balance = balance + '" + adjust_bal + "' , exposure = exposure + '" + adjust_exp + "' WHERE id = '" + user_id + "' ", con);
                                    cmduser.ExecuteNonQuery();
                                    con.Close();
                                    SendResponse = "Bet Submitted Successfully";

                                }
                                else
                                {
                                    SendResponse = "Please Login Again to Place any Bet";
                                }
                                con.Close();

                            }
                        }
                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = true, responseText = CheckMarketIsInsertOrNot }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    string vgf = "Odds Changed";
                    return Json(new { success = true, responseText = vgf }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                SendResponse = "Error in Place Bet";
            }
            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult BetPlaceMTPNikBF(BetPlaceD obj)
        {
            string SendResponse = "";
            try
            {
                string CheckMarketIsInsertOrNot = "";
                string GetBetfairId = "";
                string GetEventCode = "";
                string GetstackValue = "";
                int Getindex = 999;
                string GetBoxtype = "";
                string GetOdds = "";
                string GetRunnerName = "";
                string GetMarketName = "";

                GetBetfairId = obj.BetfairId;
                GetEventCode = obj.EventCode;
                GetstackValue = obj.stackValue;
                Getindex = obj.RunnerIndex;
                GetBoxtype = obj.BoxType;
                GetOdds = obj.OddsValue;
                GetRunnerName = obj.RunnerNameget;
                GetMarketName = obj.MarketName;
                float GetOddsFloat = float.Parse(GetOdds, CultureInfo.InvariantCulture.NumberFormat);
                if (GetEventCode == "51515151")
                {
                    if (Getindex == 2)
                    {
                        Getindex = 21;
                    }
                    else if (Getindex == 3)
                    {
                        Getindex = 41;
                    }
                }
                else if (GetEventCode == "40404040")
                {
                    if (Getindex == 1)
                    {
                        Getindex = 11;
                    }
                    else if (Getindex == 2)
                    {
                        Getindex = 21;
                    }
                }

                int delay_time = GetMODelay(GetEventCode);
                Thread.Sleep(100);
                string odds_rate = GetOdds;//teenpattioddsrate(GetEventCode, GetBetfairId, Getindex);
                if (odds_rate == "0")
                {
                    return Json(new { success = true, responseText = "Market Suspended" }, JsonRequestBehavior.AllowGet);
                }
                else if (odds_rate == GetOdds && odds_rate != "" && odds_rate != "0")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionBFF"].ToString()))
                    {
                        using (var cmd22 = new SqlCommand("SELECT id, status FROM markets WHERE betfair_id='" + GetBetfairId + "'", con))
                        {
                            cmd22.Connection = con;
                            con.Open();
                            var dataMM = cmd22.ExecuteReader();
                            if (dataMM.HasRows)
                            {
                                dataMM.Read();
                                string status_mk = (string)dataMM["status"];
                                if (status_mk == "activate")
                                {
                                }
                                else
                                {
                                    SendResponse = "Market Closed";
                                    return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                }
                                CheckMarketIsInsertOrNot = "Success";
                            }
                            else
                            {
                                CheckMarketIsInsertOrNot = OtherController.Submitdfxsb(GetEventCode, GetBetfairId);
                            }
                            con.Close();
                        }
                    }

                    if (CheckMarketIsInsertOrNot == "Success")
                    {
                        float betable_rate = float.Parse(odds_rate, CultureInfo.InvariantCulture.NumberFormat);
                        string user_ids = "2909";
                        string hash_keys = "AFF85D46FB0C5A835D6EF73516350B10";
                        string agm_id = "2";
                        int user_id = Int32.Parse(user_ids);
                        string hash_key = hash_keys;
                        Double user_bal = 0;
                        Double user_exp = 0;
                        int dist_id = 0;
                        int md_id = 0;
                        int admin_id = 0;
                        Double minstakes = 1;
                        Double maxStakes = 250;
                        string Inr_coin = "yes";// GetIsInr(agm_id);
                        if (Inr_coin == "yes")
                        {
                            minstakes = 100;
                            maxStakes = 25000;
                        }
                        float Getstack = float.Parse(GetstackValue, CultureInfo.InvariantCulture.NumberFormat);
                        var calamt = new List<Double>();
                        var calnewamt = new List<Double>();

                        if (Getstack < minstakes)
                        {
                            SendResponse = "Minimum Stakes Limit is " + minstakes;
                            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                        }
                        if (Getstack > maxStakes)
                        {
                            SendResponse = "Maximum Stakes Limit is " + maxStakes;
                            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                        }

                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionBFF"].ToString()))
                        {
                            using (var cmd = new SqlCommand("SELECT [exposure_limit],[balance],[exposure],[dl_id],[mdl_id],[admin_id] FROM users_client WHERE id='" + user_id + "' and hash_key='" + hash_key + "' AND status='activate' "))
                            {
                                cmd.Connection = con;
                                con.Open();
                                var reader = cmd.ExecuteReader();

                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    user_bal = (Double)reader["balance"];
                                    user_exp = (Double)reader["exposure"];
                                    Double exposure_limit = (Double)reader["exposure_limit"];
                                    dist_id = (int)reader["dl_id"];
                                    md_id = (int)reader["mdl_id"];
                                    admin_id = (int)reader["admin_id"];

                                    float stakes = 0;
                                    float profit = 0;
                                    float Get_total = Getstack * betable_rate;
                                    float Get_total_val = Get_total - Getstack;
                                    if (GetBoxtype == "back")
                                    {
                                        stakes = Getstack;
                                        profit = Get_total_val;
                                    }
                                    else if (GetBoxtype == "lay")
                                    {
                                        stakes = Get_total_val;
                                        profit = Getstack;
                                    }
                                    double totalbettp = gettptotal(user_id, GetBetfairId, GetEventCode);
                                    Double max_profit_limit = GetMOLimit(GetEventCode);
                                    double vyvgj = totalbettp + profit;
                                    if (vyvgj > exposure_limit)
                                    {
                                        SendResponse = "Maximum Exposure limit is " + exposure_limit + " Coins";
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                    if (vyvgj > max_profit_limit)
                                    {
                                        SendResponse = "Maximum Profit limit is " + max_profit_limit + " Coins";
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                    Double new_user_exp = user_exp + stakes;
                                    Double new_user_bal = user_bal - stakes;
                                    Double adjust_exp = 0 + stakes;
                                    Double adjust_bal = 0 - stakes;
                                    if (new_user_bal < 0)
                                    {
                                        SendResponse = "Insufficient Balance";
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                    DateTime time = DateTime.Now;
                                    string format1 = "yyyy-MM-dd HH:mm:ss";

                                    string field = GetBoxtype + "(" + GetRunnerName + ")";
                                    SqlCommand cmdlive_bet = new SqlCommand("INSERT INTO live_bet (user_id ,dist_id ,md_id ,admin_id ,event_id ,betfair_id , " +
                                        " field ,rate ,stakes ,total_value ,session_rate ,logic ,field_pos ,team_name ,runner_posi ,status ,place_time ,settled_time, " +
                                        " odds_type ,input_stakes ,input_pl ,before_bal ,after_bal ,before_exp ,after_exp)" +
                                        "VALUES ('" + user_id + "' ,'" + dist_id + "' ,'" + md_id + "' ,'" + admin_id + "' ,'" + GetEventCode + "' ,'" + GetBetfairId + "' , " +
                                        " '" + field + "' ,'" + betable_rate + "' ,'" + stakes + "' ,'" + profit + "' ,'' ,'' ,'" + GetBoxtype + "' ,'" + GetRunnerName + "' , " +
                                        " '" + Getindex + "' ,'' ,'" + time.ToString(format1) + "','','TP' ,'" + Getstack + "' ,'" + Get_total + "' ,'" + user_bal + "' , " +
                                        " '" + new_user_bal + "' ,'" + user_exp + "' ,'" + new_user_exp + "' ) ", con);
                                    cmdlive_bet.ExecuteNonQuery();
                                    SqlCommand cmduser = new SqlCommand("UPDATE users_client SET balance = balance + '" + adjust_bal + "' , exposure = exposure + '" + adjust_exp + "' WHERE id = '" + user_id + "' ", con);
                                    cmduser.ExecuteNonQuery();
                                    con.Close();
                                    SendResponse = "Bet Submitted Successfully";

                                }
                                else
                                {
                                    SendResponse = "Please Login Again to Place any Bet";
                                }
                                con.Close();

                            }
                        }
                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = true, responseText = CheckMarketIsInsertOrNot }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    string vgf = "Odds Changed";
                    return Json(new { success = true, responseText = vgf }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                SendResponse = "Error in Place Bet";
            }
            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BetPlaceMTPNikBart(BetPlaceD obj)
        {
            string SendResponse = "";
            try
            {
                string CheckMarketIsInsertOrNot = "";
                string GetBetfairId = "";
                string GetEventCode = "";
                string GetstackValue = "";
                int Getindex = 999;
                string GetBoxtype = "";
                string GetOdds = "";
                string GetRunnerName = "";
                string GetMarketName = "";

                GetBetfairId = obj.BetfairId;
                GetEventCode = obj.EventCode;
                GetstackValue = obj.stackValue;
                Getindex = obj.RunnerIndex;
                GetBoxtype = obj.BoxType;
                GetOdds = "2.00";
                GetRunnerName = obj.RunnerNameget;
                GetMarketName = obj.MarketName;
                float GetOddsFloat = float.Parse(GetOdds, CultureInfo.InvariantCulture.NumberFormat);
                if (GetEventCode == "51515151")
                {
                    if (Getindex == 2)
                    {
                        Getindex = 21;
                    }
                    else if (Getindex == 3)
                    {
                        Getindex = 41;
                    }
                }
                else if (GetEventCode == "40404040")
                {
                    if (Getindex == 1)
                    {
                        Getindex = 11;
                    }
                    else if (Getindex == 2)
                    {
                        Getindex = 21;
                    }
                }

                int delay_time = GetMODelay(GetEventCode);
                Thread.Sleep(100);
                string odds_rate = GetOdds;//teenpattioddsrate(GetEventCode, GetBetfairId, Getindex);
                if (odds_rate == "0")
                {
                    return Json(new { success = true, responseText = "Market Suspended" }, JsonRequestBehavior.AllowGet);
                }
                else if (odds_rate == GetOdds && odds_rate != "" && odds_rate != "0")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionBart"].ToString()))
                    {
                        using (var cmd22 = new SqlCommand("SELECT id, status FROM markets WHERE betfair_id='" + GetBetfairId + "'", con))
                        {
                            cmd22.Connection = con;
                            con.Open();
                            var dataMM = cmd22.ExecuteReader();
                            if (dataMM.HasRows)
                            {
                                dataMM.Read();
                                string status_mk = (string)dataMM["status"];
                                if (status_mk == "activate")
                                {
                                }
                                else
                                {
                                    SendResponse = "Market Closed";
                                    return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                }
                                CheckMarketIsInsertOrNot = "Success";
                            }
                            else
                            {
                                CheckMarketIsInsertOrNot = OtherController.Submitdfxsbar(GetEventCode, GetBetfairId);
                            }
                            con.Close();
                        }
                    }

                    if (CheckMarketIsInsertOrNot == "Success")
                    {
                        float betable_rate = float.Parse(odds_rate, CultureInfo.InvariantCulture.NumberFormat);
                        string user_ids = "2329";
                        string hash_keys = "F733B54A9F0873F5ABE35226D82AF5FC";
                        string agm_id = "2";
                        int user_id = Int32.Parse(user_ids);
                        string hash_key = hash_keys;
                        Double user_bal = 0;
                        Double user_exp = 0;
                        int dist_id = 0;
                        int md_id = 0;
                        int admin_id = 0;
                        Double minstakes = 1;
                        Double maxStakes = 250;
                        string Inr_coin = "yes";// GetIsInr(agm_id);
                        if (Inr_coin == "yes")
                        {
                            minstakes = 100;
                            maxStakes = 25000;
                        }
                        float Getstack = float.Parse(GetstackValue, CultureInfo.InvariantCulture.NumberFormat);
                        var calamt = new List<Double>();
                        var calnewamt = new List<Double>();

                        if (Getstack < minstakes)
                        {
                            SendResponse = "Minimum Stakes Limit is " + minstakes;
                            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                        }
                        if (Getstack > maxStakes)
                        {
                            SendResponse = "Maximum Stakes Limit is " + maxStakes;
                            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                        }

                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionBart"].ToString()))
                        {
                            using (var cmd = new SqlCommand("SELECT [exposure_limit],[balance],[exposure],[dl_id],[mdl_id],[admin_id] FROM users_client WHERE id='" + user_id + "' and hash_key='" + hash_key + "' AND status='activate' "))
                            {
                                cmd.Connection = con;
                                con.Open();
                                var reader = cmd.ExecuteReader();

                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    user_bal = (Double)reader["balance"];
                                    user_exp = (Double)reader["exposure"];
                                    Double exposure_limit = (Double)reader["exposure_limit"];
                                    dist_id = (int)reader["dl_id"];
                                    md_id = (int)reader["mdl_id"];
                                    admin_id = (int)reader["admin_id"];

                                    float stakes = 0;
                                    float profit = 0;
                                    float Get_total = Getstack * betable_rate;
                                    float Get_total_val = Get_total - Getstack;
                                    if (GetBoxtype == "back")
                                    {
                                        stakes = Getstack;
                                        profit = Get_total_val;
                                    }
                                    else if (GetBoxtype == "lay")
                                    {
                                        stakes = Get_total_val;
                                        profit = Getstack;
                                    }
                                    double totalbettp = gettptotal(user_id, GetBetfairId, GetEventCode);
                                    Double max_profit_limit = GetMOLimit(GetEventCode);
                                    double vyvgj = totalbettp + profit;
                                    if (vyvgj > exposure_limit)
                                    {
                                        SendResponse = "Maximum Exposure limit is " + exposure_limit + " Coins";
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                    if (vyvgj > max_profit_limit)
                                    {
                                        SendResponse = "Maximum Profit limit is " + max_profit_limit + " Coins";
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                    Double new_user_exp = user_exp + stakes;
                                    Double new_user_bal = user_bal - stakes;
                                    Double adjust_exp = 0 + stakes;
                                    Double adjust_bal = 0 - stakes;
                                    if (new_user_bal < 0)
                                    {
                                        SendResponse = "Insufficient Balance";
                                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                                    }
                                    DateTime time = DateTime.Now;
                                    string format1 = "yyyy-MM-dd HH:mm:ss";

                                    string field = GetBoxtype + "(" + GetRunnerName + ")";
                                    SqlCommand cmdlive_bet = new SqlCommand("INSERT INTO live_bet (user_id ,dist_id ,md_id ,admin_id ,event_id ,betfair_id , " +
                                        " field ,rate ,stakes ,total_value ,session_rate ,logic ,field_pos ,team_name ,runner_posi ,status ,place_time ,settled_time, " +
                                        " odds_type ,input_stakes ,input_pl ,before_bal ,after_bal ,before_exp ,after_exp)" +
                                        "VALUES ('" + user_id + "' ,'" + dist_id + "' ,'" + md_id + "' ,'" + admin_id + "' ,'" + GetEventCode + "' ,'" + GetBetfairId + "' , " +
                                        " '" + field + "' ,'" + betable_rate + "' ,'" + stakes + "' ,'" + profit + "' ,'' ,'' ,'" + GetBoxtype + "' ,'" + GetRunnerName + "' , " +
                                        " '" + Getindex + "' ,'' ,'" + time.ToString(format1) + "','','TP' ,'" + Getstack + "' ,'" + Get_total + "' ,'" + user_bal + "' , " +
                                        " '" + new_user_bal + "' ,'" + user_exp + "' ,'" + new_user_exp + "' ) ", con);
                                    cmdlive_bet.ExecuteNonQuery();
                                    SqlCommand cmduser = new SqlCommand("UPDATE users_client SET balance = balance + '" + adjust_bal + "' , exposure = exposure + '" + adjust_exp + "' WHERE id = '" + user_id + "' ", con);
                                    cmduser.ExecuteNonQuery();
                                    con.Close();
                                    SendResponse = "Bet Submitted Successfully";

                                }
                                else
                                {
                                    SendResponse = "Please Login Again to Place any Bet";
                                }
                                con.Close();

                            }
                        }
                        return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = true, responseText = CheckMarketIsInsertOrNot }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    string vgf = "Odds Changed";
                    return Json(new { success = true, responseText = vgf }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                SendResponse = "Error in Place Bet";
            }
            return Json(new { success = true, responseText = SendResponse }, JsonRequestBehavior.AllowGet);
        }

        public String GetIsInr(string md_id)
        {

            string MAtchTG = "yes";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT is_inr FROM admin where id= '" + md_id + "' "))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        MAtchTG = (string)reader["is_inr"];
                        con.Close();
                        return MAtchTG;
                    }
                }
            }
            return MAtchTG;
        }
        public Double GetBMLimit(string ev_code)
        {
            Double BMLimit = 10000;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT max_limit_mo_bm FROM matches where event_code= '" + ev_code + "' "))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        BMLimit = (Double)reader["max_limit_mo_bm"];
                        con.Close();
                        return BMLimit;
                    }
                }
            }
            return BMLimit;
        }
        public Double GetMOLimit(string ev_code)
        {
            Double MOLimit = 10000;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT max_limit_mo FROM matches where event_code= '" + ev_code + "' "))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        MOLimit = (Double)reader["max_limit_mo"];
                        con.Close();
                        return MOLimit;
                    }
                }
            }
            return MOLimit;
        }
        public Double GetSessLimit(string ev_code)
        {
            Double SessLimit = 10000;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT max_limit_sess FROM matches where event_code= '" + ev_code + "' "))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        SessLimit = (Double)reader["max_limit_sess"];
                        con.Close();
                        return SessLimit;
                    }
                }
            }
            return SessLimit;
        }
        public int GetBMDelay(string ev_code)
        {
            int BMDelay = 2000;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT bm_delay FROM matches where event_code= '" + ev_code + "' "))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        BMDelay = (int)reader["bm_delay"];
                        con.Close();
                        return BMDelay;
                    }
                }
            }
            return BMDelay;
        }
        public int GetMODelay(string ev_code)
        {
            int MODelay = 10000;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT mo_delay FROM matches where event_code= '" + ev_code + "' "))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        MODelay = (int)reader["mo_delay"];
                        con.Close();
                        return MODelay;
                    }
                }
            }
            return MODelay;
        }
        public int GetSessDelay(string ev_code)
        {
            int SessDelay = 1500;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT sess_delay FROM matches where event_code= '" + ev_code + "' "))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        SessDelay = (int)reader["sess_delay"];
                        con.Close();
                        return SessDelay;
                    }
                }
            }
            return SessDelay;
        }

        public String GetsportEventId(string id)
        {

            string MAtchTG = "1";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT sport_id FROM matches where event_code= '" + id + "' "))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        MAtchTG = (string)reader["sport_id"];
                        con.Close();
                        return MAtchTG;
                    }
                }
            }
            return MAtchTG;
        }

        public double gettptotal(int user_id, string bf_id, string ev_id)
        {

            double MAtchTG = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (var cmd = new SqlCommand("SELECT total_value FROM live_bet where event_id='" + ev_id + "' AND status='' And betfair_id='" + bf_id + "' AND user_id= '" + user_id + "' "))
                {
                    cmd.Connection = con;
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MAtchTG += (double)reader["total_value"];
                        }
                    }
                    con.Close();
                }
            }
            return MAtchTG;
        }

        public string PlaceBetTPO(string EventCode, string BetfairId,
           string OddsValue, string stackValue, string BoxType, string RunnerNameget,
           string RunnerIndex)
        {
            string gjhgf = "Error in Place Bet";
            string GetEventCode = EventCode;
            string GetBetfairId = BetfairId;
            string GetBoxtype = BoxType;
            string GetRunnerName = RunnerNameget;
            string GetstackValue = stackValue;

            int Getindex = Int32.Parse(RunnerIndex);
            string uid = (string)System.Web.HttpContext.Current.Session["UIDS"];
            string hashkey = (string)System.Web.HttpContext.Current.Session["hash_keyS"];
            if (OddsValue != "")
            {
                float betable_rate = float.Parse(OddsValue);
                try
                {
                    string SpoRTIDS = FunctionDataController.GetEventTypeId(GetEventCode);

                    string CheckMarketIsInsertOrNot = "";
                    string agm_id = (string)System.Web.HttpContext.Current.Session["AgentMID"];
                    string ad_id = (string)System.Web.HttpContext.Current.Session["AdminID"];
                    int user_id = Int32.Parse(uid);
                    string hash_key = hashkey;
                    Double user_bal = 0;
                    Double user_exp = 0;
                    string runner = "";
                    int dist_id = 0;
                    int md_id = 0;
                    int admin_id = 0;
                    Double minstakes = 1;
                    Double maxStakes = 100;
                    string Inr_coin = GetIsInr(agm_id);
                    int delay_time = GetMODelay(GetEventCode);
                    Thread.Sleep(delay_time);

                    if (Inr_coin == "yes")
                    {
                        minstakes = 100;
                        maxStakes = 10000;

                    }
                    Double max_profit_limit = GetMOLimit(GetEventCode);
                    if (Inr_coin != "yes")
                    {
                        max_profit_limit = max_profit_limit / 100;
                    }

                    float Getstack = float.Parse(GetstackValue, CultureInfo.InvariantCulture.NumberFormat);
                    var calamt = new List<Double>();
                    var calnewamt = new List<Double>();

                    string odds_rate = CheckOddsFor(GetEventCode, GetBetfairId, Getindex, BoxType);
                    if (odds_rate == "0")
                    {
                        gjhgf = "Market Suspended";
                    }
                    else if (odds_rate == OddsValue && odds_rate != "" && odds_rate != "0")
                    {

                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                        {
                            using (var cmd22 = new SqlCommand("SELECT id, status FROM markets WHERE betfair_id='" + GetBetfairId + "'", con))
                            {
                                cmd22.Connection = con;
                                con.Open();
                                var fancy_exchange = cmd22.ExecuteReader();
                                if (fancy_exchange.HasRows)
                                {
                                    fancy_exchange.Read();
                                    string status_mk = (string)fancy_exchange["status"];
                                    if (status_mk == "activate")
                                    {
                                        CheckMarketIsInsertOrNot = "Success";
                                    }
                                    else
                                    {
                                        return "Market Closed";
                                    }

                                }
                                else
                                {
                                    CheckMarketIsInsertOrNot = OtherController.Submitdfx(GetEventCode, GetBetfairId);
                                }
                                con.Close();
                            }
                        }

                        if (CheckMarketIsInsertOrNot == "Success")
                        {
                            if (Getstack < minstakes)
                            {
                                return "Minimum Stakes Limit is " + minstakes;
                            }
                            if (Getstack > maxStakes)
                            {
                                return "Maximum Stakes Limit is " + maxStakes;
                            }

                            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                            {
                                using (var cmd = new SqlCommand("SELECT [exposure_limit],[balance],[exposure],[dl_id],[mdl_id],[admin_id] FROM users_client WHERE id='" + user_id + "' and hash_key='" + hash_key + "' AND status='activate' "))
                                {
                                    cmd.Connection = con;
                                    con.Open();
                                    var reader = cmd.ExecuteReader();

                                    if (reader.HasRows)
                                    {
                                        reader.Read();
                                        user_bal = (Double)reader["balance"];
                                        user_exp = (Double)reader["exposure"];
                                        dist_id = (int)reader["dl_id"];
                                        md_id = (int)reader["mdl_id"];
                                        admin_id = (int)reader["admin_id"];
                                        Double exposure_limit = (Double)reader["exposure_limit"];

                                        float stakes = 0;
                                        float profit = 0;
                                        float Get_total = Getstack * betable_rate;
                                        float Get_total_val = Get_total - Getstack;
                                        if (GetBoxtype == "back")
                                        {
                                            stakes = Getstack;
                                            profit = Get_total_val;
                                        }
                                        else if (GetBoxtype == "lay")
                                        {
                                            stakes = Get_total_val;
                                            profit = Getstack;
                                        }
                                        int runner_pos1 = Getindex;
                                        SqlCommand markets = new SqlCommand("SELECT [amount],[runner_pos],[runner] FROM runner_cal WHERE user_id='" + user_id + "' AND market_code='" + GetBetfairId + "'  AND event_code = '" + GetEventCode + "'  ", con);
                                        var reader_markets = markets.ExecuteReader();
                                        if (reader_markets.HasRows)
                                        {
                                            while (reader_markets.Read())
                                            {
                                                Double calamount = (Double)reader_markets["amount"];
                                                int cal_pos = (Int32)reader_markets["runner_pos"];
                                                //int runner_pos1 = runner_pos+1;
                                                Double new_cal_amount = 0;
                                                if (runner_pos1 == cal_pos)
                                                {
                                                    runner = (string)reader_markets["runner"];
                                                }
                                                if (runner_pos1 == cal_pos && GetBoxtype == "back")
                                                {
                                                    new_cal_amount = calamount + Get_total_val;
                                                }
                                                else if (runner_pos1 == cal_pos && GetBoxtype == "lay")
                                                {
                                                    new_cal_amount = calamount - Get_total_val;
                                                }
                                                else if (runner_pos1 != cal_pos && GetBoxtype == "back")
                                                {
                                                    new_cal_amount = calamount - Getstack;
                                                }
                                                else if (runner_pos1 != cal_pos && GetBoxtype == "lay")
                                                {
                                                    new_cal_amount = calamount + Getstack;
                                                }

                                                new_cal_amount = Math.Round(new_cal_amount, 2);

                                                calamt.Add(calamount);
                                                calnewamt.Add(new_cal_amount);
                                            }
                                        }
                                        else
                                        {
                                            SqlCommand runner2 = new SqlCommand("SELECT [runner_name],[sortPriority] FROM runner_market WHERE market_id='" + GetBetfairId + "' ", con);
                                            var reader_runner = runner2.ExecuteReader();
                                            if (reader_runner.HasRows)
                                            {
                                                while (reader_runner.Read())
                                                {
                                                    string runnername = (string)reader_runner["runner_name"];
                                                    string cal_pos1 = (string)reader_runner["sortPriority"];
                                                    int cal_pos = Int32.Parse(cal_pos1);
                                                    //int runner_pos1 = runner_pos+1;
                                                    Double new_cal_amount = 0;
                                                    if (runner_pos1 == cal_pos)
                                                    {
                                                        runner = runnername;
                                                    }
                                                    if (runner_pos1 == cal_pos && GetBoxtype == "back")
                                                    {
                                                        new_cal_amount = Get_total_val;
                                                    }
                                                    else if (runner_pos1 == cal_pos && GetBoxtype == "lay")
                                                    {
                                                        new_cal_amount = -Get_total_val;
                                                    }
                                                    else if (runner_pos1 != cal_pos && GetBoxtype == "back")
                                                    {
                                                        new_cal_amount = -Getstack;
                                                    }
                                                    else if (runner_pos1 != cal_pos && GetBoxtype == "lay")
                                                    {
                                                        new_cal_amount = Getstack;
                                                    }

                                                    new_cal_amount = Math.Round(new_cal_amount, 2);

                                                    calamt.Add(0);
                                                    calnewamt.Add(new_cal_amount);
                                                }
                                            }
                                        }
                                        Double mincalamt = calamt.Min();
                                        if (mincalamt > 0)
                                        {
                                            mincalamt = 0;
                                        }
                                        Double minnewcalamt = calnewamt.Min();
                                        if (minnewcalamt > 0)
                                        {
                                            minnewcalamt = 0;
                                        }
                                        Double maxnewcalamt = calnewamt.Max();
                                        if (maxnewcalamt < 0)
                                        {
                                            maxnewcalamt = 0;
                                        }
                                        if (maxnewcalamt > exposure_limit)
                                        {
                                            return "Maximum Exposure limit is " + exposure_limit + " Coins";
                                        }
                                        if (maxnewcalamt > max_profit_limit)
                                        {
                                            return "Maximum Profit limit is " + max_profit_limit + " Coins";
                                        }

                                        Double new_user_exp = user_exp + mincalamt - minnewcalamt;
                                        Double new_user_bal = user_bal - mincalamt + minnewcalamt;
                                        Double adjust_exp = 0 + mincalamt - minnewcalamt;
                                        Double adjust_bal = 0 - mincalamt + minnewcalamt;
                                        if (new_user_bal < 0)
                                        {
                                            return "Insufficient Balance";
                                        }
                                        DateTime time = DateTime.Now;
                                        string format1 = "yyyy-MM-dd HH:mm:ss";

                                        SqlCommand sqlcal1 = new SqlCommand("SELECT [id] FROM runner_cal WHERE user_id='" + user_id + "'  AND market_code='" + GetBetfairId + "'  AND event_code = '" + GetEventCode + "'  ", con);
                                        var datacal1 = sqlcal1.ExecuteReader();
                                        if (datacal1.HasRows)
                                        {
                                            int wi = 0;
                                            while (datacal1.Read())
                                            {

                                                int cal_id = (int)datacal1["id"];
                                                Double new_cal_amount1 = calnewamt[wi];

                                                SqlCommand sqlcalu = new SqlCommand("UPDATE runner_cal SET amount ='" + new_cal_amount1 + "' WHERE user_id='" + user_id + "' AND id = '" + cal_id + "' ", con);
                                                sqlcalu.ExecuteNonQuery();
                                                wi++;
                                            }
                                        }
                                        else
                                        {
                                            SqlCommand sqlcal_run = new SqlCommand("SELECT [runner_name],[sortPriority] FROM runner_market WHERE market_id='" + GetBetfairId + "' ", con);
                                            var datacal_run = sqlcal_run.ExecuteReader();
                                            if (datacal_run.HasRows)
                                            {
                                                int wi = 0;
                                                while (datacal_run.Read())
                                                {
                                                    string runnername = (string)datacal_run["runner_name"];
                                                    string runnerpos = (string)datacal_run["sortPriority"];
                                                    Double new_cal_amount1 = calnewamt[wi];

                                                    SqlCommand sqlcalu1 = new SqlCommand("INSERT INTO runner_cal(user_id,dist_id,md_id,admin_id, event_id, market_id, event_code, " +
                                                        " market_code, runner, runner_pos, amount, is_result, created) " +
                                                        " VALUES ('" + user_id + "','" + dist_id + "' ,'" + md_id + "' ,'" + admin_id + "' ,'" + GetEventCode + "', '" + GetBetfairId + "', " +
                                                        " '" + GetEventCode + "','" + GetBetfairId + "','" + runnername + "', '" + runnerpos + "','" + new_cal_amount1 + "','0','" + time.ToString(format1) + "')", con);
                                                    sqlcalu1.ExecuteNonQuery();
                                                    wi++;
                                                }
                                            }
                                        }
                                        string field = GetBoxtype + "(" + GetRunnerName + ")";
                                        SqlCommand cmdlive_bet = new SqlCommand("INSERT INTO live_bet (user_id ,dist_id ,md_id ,admin_id ,event_id ,betfair_id , " +
                                            " field ,rate ,stakes ,total_value ,session_rate ,logic ,field_pos ,team_name ,runner_posi ,status ,place_time ,settled_time, " +
                                            " odds_type ,input_stakes ,input_pl ,before_bal ,after_bal ,before_exp ,after_exp)" +
                                            "VALUES ('" + user_id + "' ,'" + dist_id + "' ,'" + md_id + "' ,'" + admin_id + "' ,'" + GetEventCode + "' ,'" + GetBetfairId + "' , " +
                                            " '" + field + "' ,'" + betable_rate + "' ,'" + stakes + "' ,'" + profit + "' ,'' ,'' ,'" + GetBoxtype + "' ,'" + GetRunnerName + "' , " +
                                            " '" + Getindex + "' ,'' ,'" + time.ToString(format1) + "','','TP' ,'" + Getstack + "' ,'" + Get_total + "' ,'" + user_bal + "' , " +
                                            " '" + new_user_bal + "' ,'" + user_exp + "' ,'" + new_user_exp + "' ) ", con);
                                        cmdlive_bet.ExecuteNonQuery();
                                        SqlCommand cmduser = new SqlCommand("UPDATE users_client SET balance = balance + '" + adjust_bal + "' , exposure = exposure + '" + adjust_exp + "' WHERE id = '" + user_id + "' ", con);
                                        cmduser.ExecuteNonQuery();
                                        con.Close();
                                        gjhgf = "Bet Submitted Successfully";

                                    }
                                    else
                                    {
                                        gjhgf = "Please Login Again to Place Bet";
                                    }
                                    con.Close();

                                }
                            }
                        }
                    }
                    else
                    {
                        gjhgf = "Odds Changed";
                    }
                }
                catch (Exception ex)
                {
                    gjhgf = "Error in Place Bet";
                }
            }
            else
            {
                gjhgf = "Error in Place Bet";
            }

            return gjhgf;
        }

        public string CheckOddsFor(string Eve_Code, string GetBetfairId, int Getindex, string Boxtype)
        {
            string odds_rate = "";
            string evv_name = "";
            int i = 999;
            if (Eve_Code == "31313131")
            {
                if (Getindex == 1)
                {
                    i = 0;
                }
                else if (Getindex == 2)
                {
                    i = 1;
                }
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://skyinplay99.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("CasinoData/TeenPatiMuflis").Result;
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        if (responseJson != null)
                        {
                            var t2 = responseJson.t1;
                            var t3 = responseJson.t2;
                            string autotime = t2[0]["autotime"];
                            int autotime1 = int.Parse(autotime);
                            string betfair_id = t3[i]["mid"];
                            string status = t3[i]["gstatus"];
                            if (status == "1" && betfair_id == GetBetfairId && autotime1 > 4)
                            {
                                odds_rate = t3[i]["rate"];
                            }
                            else
                            {
                                odds_rate = "0";
                            }
                        }
                        else
                        {
                            odds_rate = "0";
                        }
                    }
                    catch (Exception ex)
                    {
                        string ewx = ex.ToString();
                        odds_rate = "0";

                    }
                }
                catch (TaskCanceledException ex)
                {
                    odds_rate = "0";
                }
            }
            else if (Eve_Code == "52525252")
            {
                evv_name = "dt6";
                if (Getindex == 1)
                {
                    i = 0;
                }
                else if (Getindex == 2)
                {
                    i = 1;
                }
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://api_link.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("t20api.php?type=" + evv_name).Result;
                        var products = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(products);
                        if (responseJson != null)
                        {
                            var t1 = responseJson.data.t1;
                            string autotime = t1[0]["autotime"];
                            int autotime1 = int.Parse(autotime);
                            var t2 = responseJson.data.t2;
                            string betfair_id = t2[i]["mid"];
                            string status = t2[i]["gstatus"];
                            if (status == "ACTIVE" && betfair_id == GetBetfairId && autotime1 > 4)
                            {
                                if (Boxtype == "back")
                                {
                                    odds_rate = t2[i]["b1"];
                                }
                                else if (Boxtype == "lay")
                                {
                                    odds_rate = t2[i]["l1"];
                                }
                                else
                                {
                                    odds_rate = "0";
                                }
                            }
                            else
                            {
                                odds_rate = "0";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        odds_rate = "0";
                    }
                }
                catch (TaskCanceledException ex)
                {
                    odds_rate = "0";
                }
            }


            else if (Eve_Code == "5676756767")
            {
                if (Getindex == 1)
                {
                    i = 0;
                }
                else if (Getindex == 2)
                {
                    i = 1;
                }
                try
                {
                    var client = new RestClient("https://betexch.uk/api/v1/matchDetails");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("sec-ch-ua", "\"Google Chrome\";v=\"87\", \" Not;A Brand\";v=\"99\", \"Chromium\";v=\"87\"");
                    request.AddHeader("Accept", "application/json, text/plain, */*");
                    request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOnsiaWQiOjE5MjcsInJvbGUiOiJBZG1pblRlc3QiLCJwYXJlbnRfaWQiOiIxODU5LDE4NTgsMTg1NywxLDAiLCJ1c2VyX3R5cGVfaWQiOjV9LCJpYXQiOjE2MDkyNDA5NDcsImV4cCI6MTYxMDEwNDk0N30.BetHC7zqmpfnSOMj-kf_Je8b1viIUPpECer4WC5nt6Y");
                    request.AddHeader("sec-ch-ua-mobile", "?0");
                    client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", "{\"match_id\":\"56767\",\"market_id\":\"-100.56767\",\"user_id\":1864,\"user_type_id\":5}", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    if (response != null)
                    {
                        string data = response.Content;
                        JObject jObject = JObject.Parse(data);
                        string betfair_id = jObject.SelectToken("data.match[0].roundId").ToString();
                        string fvhbgfh = jObject.SelectToken("data.match[0].runners[" + i + "].lay").ToString();
                        string backConts = jObject.SelectToken("data.match[0].runners[" + i + "].back").ToString();
                        string autotime = jObject.SelectToken("data.match[0].autotime").ToString();
                        int autotime1 = int.Parse(autotime);
                        string status = jObject.SelectToken("data.match[0].status").ToString();
                        if (status == "OPEN" && betfair_id == GetBetfairId && autotime1 > 4)
                        {
                            if (Boxtype == "back")
                            {
                                if (backConts.Contains("price"))
                                {
                                    odds_rate = jObject.SelectToken("data.match[0].runners[" + i + "].back[0].price").ToString();
                                }
                            }
                            else if (Boxtype == "lay")
                            {
                                if (fvhbgfh.Contains("price"))
                                {
                                    odds_rate = jObject.SelectToken("data.match[0].runners[" + i + "].lay[0].price").ToString();
                                }
                            }
                            else
                            {
                                odds_rate = "0";
                            }
                        }
                        else
                        {
                            odds_rate = "0";
                        }
                    }
                }
                catch(Exception ex)
                {

                }
                
            }

            return odds_rate;
        }
    }
}