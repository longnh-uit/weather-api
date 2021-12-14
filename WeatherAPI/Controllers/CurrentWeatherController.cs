using Newtonsoft.Json;
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
                    RequestUri = new Uri($"https://community-open-weather-map.p.rapidapi.com/find?q={location}&cnt=1&mode=null&lon=0&type=link%2C%20accurate&lat=0&units=metric"),
                    Headers =
                {
                    { "x-rapidapi-host", "community-open-weather-map.p.rapidapi.com" },
                    { "x-rapidapi-key", "b99908da4bmsh2b013b728fd8ad3p112d00jsn94d5bec39aa1" },
                },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    String rawJSON = await response.Content.ReadAsStringAsync();
                    rawJSON = rawJSON.Substring(rawJSON.IndexOf("list") + 7);
                    rawJSON = rawJSON.Remove(rawJSON.Length - 2);
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
