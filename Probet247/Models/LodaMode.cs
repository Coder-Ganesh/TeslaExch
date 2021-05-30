using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Probet247.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Card
    {
        public string name { get; set; }
        public List<object> cards { get; set; }
        public int selectionId { get; set; }
    }

    public class LastResult
    {
        public string mid { get; set; }
        public int result { get; set; }
        public string selection_name { get; set; }
        public string sortName { get; set; }
    }

    public class AvailableToBack
    {
        public double? price { get; set; }
        public int? size { get; set; }
        public object line { get; set; }
        public string status { get; set; }
    }

    public class AvailableToLay
    {
        public object price { get; set; }
        public object size { get; set; }
        public object line { get; set; }
        public string status { get; set; }
    }

    public class Ex
    {
        public List<AvailableToBack> availableToBack { get; set; }
        public List<AvailableToLay> availableToLay { get; set; }
    }

    public class Runner
    {
        public Ex ex { get; set; }
        public string status { get; set; }
        public int handicap { get; set; }
        public string runnerName { get; set; }
        public int selectionId { get; set; }
        public int sortPriority { get; set; }
        public int totalMatched { get; set; }
        public int liability_type { get; set; }
        public int lastPriceTraded { get; set; }
        public int card { get; set; }
        public int profit_loss { get; set; }
    }

    public class CardData
    {
        public string name { get; set; }
        public List<string> cards { get; set; }
        public int winner { get; set; }
        public int selectionId { get; set; }
    }

    public class Market
    {
        public int id { get; set; }
        public string sport_id { get; set; }
        public string sport_name { get; set; }
        public string series_id { get; set; }
        public string series_name { get; set; }
        public string match_id { get; set; }
        public string match_name { get; set; }
        public string market_id { get; set; }
        public string name { get; set; }
        public List<Runner> runners { get; set; }
        public int max_bet_liability { get; set; }
        public int max_bet { get; set; }
        public int min_bet { get; set; }
        public int max_market_liability { get; set; }
        public int max_market_profit { get; set; }
        public int back_rate_diff { get; set; }
        public int lay_rate_diff { get; set; }
        public string odds_style { get; set; }
        public string favourite_team { get; set; }
        public string is_active { get; set; }
        public string is_visible { get; set; }
        public string game_type { get; set; }
        public string is_match_odds { get; set; }
        public string result { get; set; }
        public string result_selection_id { get; set; }
        public string is_abandoned { get; set; }
        public string odds_mode { get; set; }
        public string is_bookmaker { get; set; }
        public List<CardData> card_data { get; set; }
        public List<object> self_deactive_user_ids { get; set; }
        public List<object> deactive_user_ids { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }

    public class Data
    {
        public int id { get; set; }
        public string sport_id { get; set; }
        public string sport_name { get; set; }
        public string series_id { get; set; }
        public string series_name { get; set; }
        public string match_id { get; set; }
        public string name { get; set; }
        public DateTime match_date { get; set; }
        public DateTime open_date { get; set; }
        public object start_date { get; set; }
        public int max_odd_limit { get; set; }
        public int min_odd_limit { get; set; }
        public int max_volume_limit { get; set; }
        public int min_volume_limit { get; set; }
        public int min_stack { get; set; }
        public int max_stack { get; set; }
        public int back_rate_diff { get; set; }
        public int lay_rate_diff { get; set; }
        public string odds_style { get; set; }
        public string favourite_team { get; set; }
        public object score_board_json { get; set; }
        public string game_type { get; set; }
        public string score_type { get; set; }
        public string is_cup { get; set; }
        public string score_key { get; set; }
        public string tv_key { get; set; }
        public string is_completed { get; set; }
        public string is_active { get; set; }
        public List<object> self_deactive_user_ids { get; set; }
        public List<object> deactive_user_ids { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int timer { get; set; }
        public List<Card> cards { get; set; }
        public List<LastResult> last_result { get; set; }
        public List<Market> markets { get; set; }
        public List<object> fancy { get; set; }
    }

    public class Body
    {
        public string match_id { get; set; }
    }

    public class Root
    {
        public int code { get; set; }
        public bool error { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
        public Body body { get; set; }
    }


}