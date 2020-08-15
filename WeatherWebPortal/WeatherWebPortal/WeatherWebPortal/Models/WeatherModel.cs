using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherWebPortal.Models
{
    public class WeatherModel
    {
        private string weatherStateAbbreviation;
        private DateTime applicableDate;

        public string ApplicableDate { get; set; }

        [JsonProperty(PropertyName = "applicable_date")]
        public DateTime ApplicableDateTime
        {
            set
            {
                applicableDate = value;
                ApplicableDate = applicableDate.ToString("d");
            }
        }
        [JsonProperty(PropertyName = "weather_state_name")]
        public string WeatherStateName { get; set; }
        [JsonProperty(PropertyName = "weather_state_abbr")]
        public string WeatherStateAbbreviation
        {
            get
            {
                return weatherStateAbbreviation;
            }

            set
            {
                weatherStateAbbreviation = value;
                IconLink = "<img src='https://www.metaweather.com/static/img/weather/" + weatherStateAbbreviation + ".svg' alt='Snow' style='width: 32px'>";
            }
        }
        public string IconLink { get; set; }
    }
}
