using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherAPI.Models
{
    public class DatabaseSetting
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}