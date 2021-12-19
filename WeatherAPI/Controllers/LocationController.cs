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
using System.Text.RegularExpressions;
using System.Text;

namespace WeatherAPI.Controllers
{
    public class LocationController : ApiController
    {
        private MongoDBContext dBContext;
        private IMongoCollection<LocationModel> locationCollection;

        private string convertToUnsign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public LocationController()
        {
            dBContext = new MongoDBContext();
            locationCollection = dBContext.database.GetCollection<LocationModel>("location");
        }

        [Route("api/getlocation")]
        [HttpGet]
        public IHttpActionResult GetLocationList()
        {
            List<LocationModel> locations = locationCollection.Find(_ => true).ToList();
            return Ok(locations);
        }

        [Route("api/searchlocation")]
        [HttpGet]
        public IHttpActionResult SearchLocation(string q)
        {
            List<LocationModel> locations = locationCollection.Find(_ => true).ToList(),
                                result = new List<LocationModel>();
            List<string> locationNameUnsign = new List<string>();

            if (q == "") return NotFound();

            q = q.ToLower();

            foreach (LocationModel location in locations)
            {
                locationNameUnsign.Add(convertToUnsign(location.name).ToLower());
            }

            int i;

            for (i = 0; i < locationNameUnsign.Count; i++)
            {
                if (locationNameUnsign[i].Contains(q)) result.Add(locations[i]);
            }

            return Ok(result);

        }
    }
}
