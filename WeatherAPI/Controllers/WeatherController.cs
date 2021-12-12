using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WeatherAPI.Models;
using MongoDB.Driver;
using WeatherAPI.App_Start;
using MongoDB.Bson;

namespace WeatherAPI.Controllers
{
    public class WeatherController : ApiController
    {
        private MongoDBContext dBContext;
        private IMongoCollection<Weather> weatherCollection;

        public WeatherController()
        {
            dBContext = new MongoDBContext();
            weatherCollection = dBContext.database.GetCollection<Weather>("weather");
        }

        // GET: api/Weather
        public IEnumerable<Weather> Get()
        {
            List<Weather> weathers = weatherCollection.AsQueryable().ToList();
            return weathers.Where(p => true);
        }

        // GET: api/Weather/5
        public Weather Get(string id)
        {
            var weatherId = new ObjectId(id);
            Weather weather = weatherCollection.AsQueryable<Weather>().SingleOrDefault(x => x.Id == weatherId);
            return weather;
        }

        // POST: api/Weather
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Weather/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Weather/5
        public void Delete(int id)
        {
        }
    }
}
