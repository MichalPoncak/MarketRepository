using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherWebPortal.Models;
using WeatherWebPortal.Utility;

namespace WeatherWebPortal.Data
{
    public interface IWeatherData
    {
        WebAPIOutputModel<WeatherModel> GetWeather(string WOEIDLocation, DateTime date);
    }

    public class WeatherData : IWeatherData
    {
        private readonly IWebAPIClientManager _client;
        private readonly WebAPISettingsModel _config;

        public WeatherData(IOptions<WebAPISettingsModel> config)
        {
            _config = config.Value;
            _client = new WebAPIClientManager(_config.WebAPIURL);
        }

        public WeatherData(IWebAPIClientManager APIClient,
            IOptions<WebAPISettingsModel> config)
        {
            _config = config.Value;
            _client = APIClient;
        }

        public WebAPIOutputModel<WeatherModel> GetWeather(string WOEIDLocation, DateTime date)
        {
            _client.SetURIPath(_config.WebAPIComponents[0]);
            _client.AddURIParam(WOEIDLocation, date.ToString("yyyy/MM/dd"));
            var output = new WebAPIOutputModel<WeatherModel>();
            var _temp = _client.Get<List<WeatherModel>>(null);
            var _error = _client.GetAPIError();
            output.TransResult = new WebAPILogModel()
            {
                APIMessage = _error.APIMessage,
                APIStatusCode = _error.APIStatusCode,
                APISystemExceptionList = _error.APISystemExceptionList
            };

            if (_temp.Count > 0)
            {
                output.DataList.AddRange(_temp);
            }

            return output;
        }
    }
}
