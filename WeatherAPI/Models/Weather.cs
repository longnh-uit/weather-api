using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeatherAPI.Models
{
    public class Weather
    {
        [BsonId]
        public ObjectId Id { get; set; }
        
        [BsonElement("Main")]
        public string Main { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }

        [BsonElement("Icon")]
        public string Icon { get; set; }
    }
}