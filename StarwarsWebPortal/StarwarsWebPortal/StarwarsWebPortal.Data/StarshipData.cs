using Microsoft.Extensions.Options;
using StarwarsWebPortal.Models;
using StarwarsWebPortal.Utility;
using StarwarsWebPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarwarsWebPortal.Data
{
    public interface IStarshipData
    {
        WebAPIOutputModel<StarshipViewModel> GetStarships(int pageNumber);
    }

    public class StarshipData : IStarshipData
    {
        private readonly IWebAPIClientManager _client;
        private readonly WebAPISettingsModel _config;

        public StarshipData(IOptions<WebAPISettingsModel> config)
        {
            _config = config.Value;
            _client = new WebAPIClientManager(_config.WebAPIURL);
        }

        public StarshipData(IWebAPIClientManager APIClient,
            IOptions<WebAPISettingsModel> config)
        {
            _config = config.Value;
            _client = APIClient;
        }

        public WebAPIOutputModel<StarshipViewModel> GetStarships(int pageNumber)
        {
            _client.SetURIPath(_config.WebAPIComponents[0]);
            _client.AddURIQstringParam(
                "page",
                new List<string>() { pageNumber.ToString() });
            var output = new WebAPIOutputModel<StarshipViewModel>();
            var _temp = _client.Get<StarshipViewModel>(null);
            var _error = _client.GetAPIError();
            output.TransResult = new WebAPILogModel()
            {
                APIMessage = _error.APIMessage,
                APIStatusCode = _error.APIStatusCode,
                APISystemExceptionList = _error.APISystemExceptionList
            };

            if (_temp.Count > 0)
            {
                output.DataList.Add(_temp);
            }

            return output;
        }
    }
}
