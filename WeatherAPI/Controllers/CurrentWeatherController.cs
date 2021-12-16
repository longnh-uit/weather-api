﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WeatherAPI.Models;
using MongoDB.Driver;
using WeatherAPI.App_Start;
using MongoDB.Bson;

namespace WeatherAPI.Controllers
{
    public class CurrentWeatherController : ApiController
    {
        private MongoDBContext dBContext;
        private IMongoCollection<CurrentWeatherModel> currentWeatherCollection;

        public CurrentWeatherController()
        {
            dBContext = new MongoDBContext();
            currentWeatherCollection = dBContext.database.GetCollection<CurrentWeatherModel>("currentWeather");
        }

        // GET: api/updateweather
        [Route("api/updateweather")]
        [HttpGet]
        async public Task<IHttpActionResult> UpdateWeather()
        {
            var client = new HttpClient();
            string[] locations = new string[] { "Moscow", "London", "Ha Noi", "Thanh pho Ho Chi Minh", "Dubai" };
            foreach (string location in locations)
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://api.openweathermap.org/data/2.5/weather?q={location}&appid=27da6525ff3320b503e537afbcf0dbb9&units=metric"),
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    String rawJSON = await response.Content.ReadAsStringAsync();
                    CurrentWeatherModel result = JsonConvert.DeserializeObject<CurrentWeatherModel>(rawJSON);
                    var replaceResult = currentWeatherCollection.ReplaceOne(x => x.name == location, result, new ReplaceOptions() { IsUpsert = true });
                }

            }

            return Ok();
        }

        // GET: api/currentweather?name=ejafiesf
        [Route("api/currentweather")]
        [HttpGet]
        public IHttpActionResult Get(string name)
        {
            try
            {
                var result = currentWeatherCollection.Find(x => x.name == name).FirstOrDefault();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
