using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WeatherAPI.Models
{
    public class AlertModel
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId _id { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
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
        public Alert[] alerts;
    }
}