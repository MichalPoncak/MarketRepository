using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherWebPortal.Data;
using WeatherWebPortal.Models;

namespace WeatherWebPortal.Business
{
    public interface IWeatherBusiness
    {
        WebAPIOutputModel<WeatherModel> GetWeather(string WOEIDLocation, DateTime date, int daysForward);
    }

    public class WeatherBusiness : IWeatherBusiness
    {
        private IWeatherData weatherData { get; }

        public WeatherBusiness(IWeatherData weatherData) => this.weatherData = weatherData;

        public WebAPIOutputModel<WeatherModel> GetWeather(string WOEIDLocation, DateTime date, int daysForward)
        {
            var result = new WebAPIOutputModel<WeatherModel>();

            for (int i = 0; i <= daysForward; i++)
            {
                List<WeatherModel> dailyWeatherForecast = weatherData.GetWeather(WOEIDLocation, date.AddDays(i)).DataList;
                result.DataList.AddRange(dailyWeatherForecast);
            }

            return result;
        }
    }
}
