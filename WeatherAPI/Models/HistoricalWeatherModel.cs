using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeatherAPI.Models
{
    public class HistoricalWeatherModel
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId _id { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public class Current
        {
            public Int64 dt { get; set; }
            public Int64 sunrise { get; set; }
            public Int64 sunset { get; set; }
            public double temp { get; set; }
            public double feels_like { get; set; }
            public int pressure { get; set; }
            public int humidity { get; set; }
            public double dew_point { get; set; }
            public int cloud { get; set; }
            public double uvi { get; set; }
            public int visibility { get; set; }
            public double wind_speed { get; set; }
            public double wind_gust { get; set; }
            public int wind_deg { get; set; }
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
            public Int64 dt { get; set; }
            public double temp { get; set; }
            public double feels_like { get; set; }
            public int pressure { get; set; }
            public int humidity { get; set; }
            public double dew_point { get; set; }
            public int cloud { get; set; }
            public int visibility { get; set; }
            public double wind_speed { get; set; }
            public double wind_gust { get; set; }
            public int wind_deg { get; set; }
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
            public class Weather
            {
                public int id { get; set; }
                public string main { get; set; }
                public string description { get; set; }
                public string icon { get; set; }
            }
            public Weather[] weather { get; set; }
        }
        public Hourly[] hourly { get; set; }
    }
}