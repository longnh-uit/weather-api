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
    public class AlertController : ApiController
    {
        private MongoDBContext dBContext;
        private IMongoCollection<AlertModel> alertCollection;
        
        public AlertController()
        {
            dBContext = new MongoDBContext();
            alertCollection = dBContext.database.GetCollection<AlertModel>("alert");
        }

        [Route("api/alert")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(double lon, double lat)
        {
            var client = new HttpClient();
            LocationModel[] locations = new LocationModel[]
            {
                new LocationModel() { name = "Hà Nội", lon = 105.8412, lat = 21.0245 },
                new LocationModel() { name = "Mát-xcơ-va", lon = 37.6156, lat = 55.7522 },
                new LocationModel() { name = "Thành phố Hồ Chí Minh", lon = 106.6667, lat = 10.75 },
                new LocationModel() { name = "Luân Đôn", lon = -0.1257, lat = 51.5085 },
                new LocationModel() { name = "Dubai", lon = 55.3047, lat = 25.2582 },
                new LocationModel() { name = "Edmonton", lon = -113.4687, lat = 53.5501 },
                new LocationModel() { name = "Quảng Trị", lon = 107.2, lat = 16.75 },
                new LocationModel() { name = "Bình Định", lon = 109.1117, lat = 13.886 }
            };

            foreach (LocationModel location in locations)
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://api.openweathermap.org/data/2.5/onecall?lon={location.lon}&lat={location.lat}&exclude=current,minutely,hourly,daily&lang=vi&appid=27da6525ff3320b503e537afbcf0dbb9&units=metric"),
                };
                using (var response = await client.SendAsync(request))
                {
                    try
                    {
                        response.EnsureSuccessStatusCode();
                        String rawJSON = await response.Content.ReadAsStringAsync();
                        AlertModel result = JsonConvert.DeserializeObject<AlertModel>(rawJSON);
                        var replaceResult = alertCollection.ReplaceOne(x => x.lon == location.lon && x.lat == location.lat, result, new ReplaceOptions() { IsUpsert = true });
                    }
                    catch { }
                }

            }

            try
            {
                var result = alertCollection.Find(x => x.lon == lon && x.lat == lat).FirstOrDefault();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}