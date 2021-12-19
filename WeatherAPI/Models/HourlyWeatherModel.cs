using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeatherAPI.Models
{
    public class HourlyWeatherModel
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId _id { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public class Current
        {
            public int dt { get; set; }
            public int sunrise { get; set; }
            public int sunset { get; set; }
            public double temp { get; set; }
            public double feels_like { get; set; }
            public int pressure { get; set; }
            public int humidity { get; set; }
            public double dew_point { get; set; }
            public double uvi { get; set; }
            public int cloud { get; set; }
            public int visibility { get; set; }
            public int win_deg { get; set; }
            public double win_gust { get; set; }
            public class Weather
            {
                public int id { get; set; }
                public string main { get; set; }
                public string description { get; set; }
                public string icon { get; set; }
            }
            public Weather[] weather { get; set; }
        }
        public Current current { get; set; }
        public class Hourly
        {
            public int dt { get; set; }
            public double temp { get; set; }
            public double feels_like { get; set; }
            public int pressure { get; set; }
            public int humidity { get; set; }
            public double dew_point { get; set; }
            public double win_speed { get; set; }
            public int win_deg { get; set; }
            public double win_gust { get; set; }
            public class Weather
            {
                public int id { get; set; }
                public string main { get; set; }
                public string description { get; set; }
                public string icon { get; set; }
            }
            public Weather[] weather { get; set; }
            public int clouds { get; set; }
            public double pop { get; set; }
            public class Rain
            {
                [JsonProperty("1h")]
                public double _1h { get; set; }
            }
            public Rain rain { get; set; }
            public class Snow
            {
                [JsonProperty("1h")]
                public double _1h { get; set; }
            }
            public Snow snow { get; set; }
            public double uvi { get; set; }
        }
        public Hourly[] hourly { get; set; }
        public class Alert
        {
            public string sender_name { get; set; }
            [JsonProperty("event")]
            public string event_name { get; set; }
            public int start { get; set; }
            public int end { get; set; }
            public string description { get; set; }
            public string[] tags { get; set; }
        }
    }

}