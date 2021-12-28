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
    public class DailyWeatherController : ApiController
    {
        private MongoDBContext dBContext;
        private IMongoCollection<DailyWeatherModel> dailyWeatherCollection;

        public DailyWeatherController()
        {
            dBContext = new MongoDBContext();
            dailyWeatherCollection = dBContext.database.GetCollection<DailyWeatherModel>("dailyWeather");
        }

        [Route("api/dailyweather")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(double lon, double lat)
        {
            try
            {
                var result = dailyWeatherCollection.Find(x => x.lon == lon && x.lat == lat).FirstOrDefault();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
