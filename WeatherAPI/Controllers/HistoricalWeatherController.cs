using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WeatherAPI.Models;
using MongoDB.Driver;
using WeatherAPI.App_Start;

namespace WeatherAPI.Controllers
{
    public class HistoricalWeatherController : ApiController
    {
        private MongoDBContext dBContext;
        private IMongoCollection<HistoricalWeatherModel> historicalWeatherCollection;

        public HistoricalWeatherController()
        {
            dBContext = new MongoDBContext();
            historicalWeatherCollection = dBContext.database.GetCollection<HistoricalWeatherModel>("historicalWeatherCollection");
        }

        [Route("api/timemachine")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(double lon, double lat, Int64 dt)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api.openweathermap.org/data/2.5/onecall/timemachine?lat={lat}&lon={lon}&dt={dt}&lang=vi&appid=4c666f2882f9c811602409fdc691518e&units=metric"),
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                String rawJSON = await response.Content.ReadAsStringAsync();
                HistoricalWeatherModel result = JsonConvert.DeserializeObject<HistoricalWeatherModel>(rawJSON);
                var replaceResult = historicalWeatherCollection.ReplaceOne(x => x.lon == lon && x.lat == lat, result, new ReplaceOptions() { IsUpsert = true });
            }

            try
            {
                var result = historicalWeatherCollection.Find(x => x.lon == lon && x.lat == lat && x.current.dt == dt).FirstOrDefault();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
