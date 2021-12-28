using Newtonsoft.Json;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WeatherAPI.App_Start;
using WeatherAPI.Models;
using System.Threading;

namespace WeatherAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private MongoDBContext dBContext;
        private IMongoCollection<CurrentWeatherModel> currentWeatherCollection;
        private IMongoCollection<DailyWeatherModel> dailyWeatherCollection;
        private IMongoCollection<HourlyWeatherModel> hourlyWeatherCollection;

        protected void Application_Start()
        {
            dBContext = new MongoDBContext();
            currentWeatherCollection = dBContext.database.GetCollection<CurrentWeatherModel>("currentWeather");
            dailyWeatherCollection = dBContext.database.GetCollection<DailyWeatherModel>("dailyWeather");
            hourlyWeatherCollection = dBContext.database.GetCollection<HourlyWeatherModel>("hourlyWeather");

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var updateCurrentTimer = new Timer(UpdateCurrentWeather);
            var updateDailyWeatherTimer = new Timer(UpdateDailyWeather);
            var updateHourlyWeatherTimer = new Timer(UpdateHourlyWeather);

            DateTime now = DateTime.Now;
            DateTime tomorrow = DateTime.Today.AddDays(1).Date;
            DateTime nextHour = new DateTime(now.Year, now.Month, now.Day, now.Hour + 1, 0, 0);
            int durationUntilTomorrow = Convert.ToInt32((tomorrow - now).TotalMilliseconds);
            int durationUntilNextHour = Convert.ToInt32((nextHour - now).TotalMilliseconds);

            updateCurrentTimer.Change(0, 60 * 60 * 1000);
            updateDailyWeatherTimer.Change(durationUntilTomorrow, 24 * 60 * 60 * 1000);
            updateHourlyWeatherTimer.Change(durationUntilNextHour, 60 * 60 * 1000);
        }

        private async void UpdateCurrentWeather(object e)
        {
            var client = new HttpClient();
            string[] locations = new string[] { "Mát-xcơ-va", "Luân Đôn", "Hà Nội", "Thành phố Hồ Chí Minh", "Dubai", "Edmonton" };
            foreach (string location in locations)
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://api.openweathermap.org/data/2.5/weather?q={location}&lang=vi&appid=27da6525ff3320b503e537afbcf0dbb9&units=metric"),
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    String rawJSON = await response.Content.ReadAsStringAsync();
                    CurrentWeatherModel result = JsonConvert.DeserializeObject<CurrentWeatherModel>(rawJSON);
                    var replaceResult = currentWeatherCollection.ReplaceOne(x => x.name == location, result, new ReplaceOptions() { IsUpsert = true });
                }

            }
        }

        private async void UpdateDailyWeather(object e)
        {
            var client = new HttpClient();
            LocationModel[] locations = new LocationModel[]
            {
                new LocationModel() { name = "Hà Nội", lon = 105.8412, lat = 21.0245 },
                new LocationModel() { name = "Mát-xcơ-va", lon = 37.6156, lat = 55.7522 },
                new LocationModel() { name = "Thành phố Hồ Chí Minh", lon = 106.6667, lat = 10.75 },
                new LocationModel() { name = "Luân Đôn", lon = -0.1257, lat = 51.5085 },
                new LocationModel() { name = "Dubai", lon = 55.3047, lat = 25.2582 },
                new LocationModel() { name = "Edmonton", lon = -113.4687, lat = 53.5501 }
            };

            foreach (LocationModel location in locations)
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://api.openweathermap.org/data/2.5/onecall?lon={location.lon}&lat={location.lat}&exclude=current,minutely,hourly,alerts&lang=vi&appid=ed5c6f17638fc6a4f9fd6868e59c6c35&units=metric"),
                };
                using (var response = await client.SendAsync(request))
                {
                    try
                    {
                        response.EnsureSuccessStatusCode();
                        String rawJSON = await response.Content.ReadAsStringAsync();
                        DailyWeatherModel result = JsonConvert.DeserializeObject<DailyWeatherModel>(rawJSON);
                        var replaceResult = dailyWeatherCollection.ReplaceOne(x => x.lon == location.lon && x.lat == location.lat, result, new ReplaceOptions() { IsUpsert = true });
                    }
                    catch
                    {

                    }
                }

            }
        }

        private async void UpdateHourlyWeather(object e)
        {
            var client = new HttpClient();
            LocationModel[] locations = new LocationModel[]
            {
                new LocationModel() { name = "Hà Nội", lon = 105.8412, lat = 21.0245 },
                new LocationModel() { name = "Mát-xcơ-va", lon = 37.6156, lat = 55.7522 },
                new LocationModel() { name = "Thành phố Hồ Chí Minh", lon = 106.6667, lat = 10.75 },
                new LocationModel() { name = "Luân Đôn", lon = -0.1257, lat = 51.5085 },
                new LocationModel() { name = "Dubai", lon = 55.3047, lat = 25.2582 },
                new LocationModel() { name = "Edmonton", lon = -113.4687, lat = 53.5501 }
            };

            foreach (LocationModel location in locations)
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://api.openweathermap.org/data/2.5/onecall?lon={location.lon}&lat={location.lat}&exclude=current,minutely,daily,alerts&lang=vi&appid=81befcfa5c26506142c9163f48174025&units=metric"),
                };
                using (var response = await client.SendAsync(request))
                {
                    try
                    {
                        response.EnsureSuccessStatusCode();
                        String rawJSON = await response.Content.ReadAsStringAsync();
                        HourlyWeatherModel result = JsonConvert.DeserializeObject<HourlyWeatherModel>(rawJSON);
                        var replaceResult = hourlyWeatherCollection.ReplaceOne(x => x.lon == location.lon && x.lat == location.lat, result, new ReplaceOptions() { IsUpsert = true });
                    }
                    catch
                    {

                    }
                }

            }
        }
    }
}
