using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WeatherAPI.Models;
using MongoDB.Driver;
using WeatherAPI.App_Start;


namespace WeatherAPI.Controllers
{
    public class ContactController : ApiController
    {
        private MongoDBContext dBContext;
        private IMongoCollection<ContactModel> contactCollection;

        public ContactController()
        {
            dBContext = new MongoDBContext();
            contactCollection = dBContext.database.GetCollection<ContactModel>("contact");
        }

        [Route("api/contact")]
        [HttpPost]
        public IHttpActionResult SaveContact(ContactModel contact)
        {
            contactCollection.InsertOne(contact);
            return Ok("Thành công!");
        }
    }
}
