using System;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WeatherAPI.Models;
using MongoDB.Driver;
using WeatherAPI.App_Start;


namespace WeatherAPI.Controllers
{
    public class HourlyWeatherController : ApiController
    {
        private MongoDBContext dBContext;
        private IMongoCollection<HourlyWeatherModel> hourlyWeatherCollection;

        public HourlyWeatherController()
        {
            dBContext = new MongoDBContext();
            hourlyWeatherCollection = dBContext.database.GetCollection<HourlyWeatherModel>("hourlyWeather");
        }

        [Route("api/hourlyweather")]
        [HttpGet]
        public IHttpActionResult Get(double lon, double lat)
        {
            try
            {
                var result = hourlyWeatherCollection.Find(x => x.lon == lon && x.lat == lat).FirstOrDefault();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
