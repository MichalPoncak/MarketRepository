using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherWebPortal.Models
{
    public class WebAPISettingsModel
    {
        public string WebAPIURL { get; set; }
        public string[] WebAPIComponents { get; set; }
    }
}
