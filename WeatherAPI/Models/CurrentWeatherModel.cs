using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeatherAPI.Models
{
    public class CurrentWeatherModel
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId _id { get; set; }
        public string name { get; set; }
        public class CoordClass
        {
            public double lat { get; set; }
            public double lon { get; set; }
        }
        public CoordClass coord { get; set; }
        public class MainClass
        {
            public double temp { get; set; }
            public double feels_like { get; set; }
            public double temp_min { get; set; }
            public double temp_max { get; set; }
            public double pressure { get; set; }
            public double humidity { get; set; }
        }
        public MainClass main { get; set; }
        public Int64 dt { get; set; }
        public class WindClass
        {
            public double speed { get; set; }
            public double deg { get; set; }
        }
        public WindClass wind { get; set; }
        public class SysClass
        {
            public string country { get; set; }
        }
        public SysClass sys { get; set; }
        public class RainClass
        {
            [JsonProperty("1h")]
            public double _1h { get; set; }
        }
        public RainClass rain { get; set; }
        public class SnowClass
        {
            [JsonProperty("1h")]
            public double _1h { get; set; }
        }
        public SnowClass snow { get; set; }
        public class CloudsClass
        {
            public double all { get; set; }
        }
        public CloudsClass clouds { get; set; }
        public class WeatherClass
        {
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }
        public WeatherClass[] weather { get; set; }

    }
}