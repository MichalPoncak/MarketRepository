using MarketWebPortal.Model;
using MarketWebPortal.Utility;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketWebPortal.Data
{
    public interface IPriceData 
    {
        WebAPIOutput<PriceModel> GetPrices();
        WebAPILog AddPrices(string createdBy, List<PriceModel> Param);
    }

    public class PriceData : BaseData, IPriceData
    {
        private readonly IWebAPIClientManager _client;
        private readonly WebAPISettings _config;

        public PriceData(IOptions<WebAPISettings> config)
        {
            _config = config.Value;
            _client = new WebAPIClientManager(_config.WebAPIURL);
        }

        public PriceData(IWebAPIClientManager APIClient, IOptions<WebAPISettings> config)
        {
            _config = config.Value;
            _client = APIClient;
        }

        public WebAPIOutput<PriceModel> GetPrices()
        {
            _client.SetURIPath(_config.WebAPIComponents[0]);
            var output = new WebAPIOutput<PriceModel>();
            var _temp = _client.Get<List<PriceModel>>(null);
            var _error = _client.GetAPIError();
            output.TransResult = new WebAPILog()
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

        public WebAPILog AddPrices(string createdBy, List<PriceModel> prices)
        {
            _client.SetURIPath(_config.WebAPIComponents[0]);
            _client.AddHeaderParams("authorization", "");
            _client.AddHeaderParams("CreatedBy", createdBy);

            WebAPIResponse _retVal = _client.Post<List<PriceModel>, string>(prices);

            if (_retVal.ErrorList.Count > 0)
            {
                return new WebAPILog()
                {
                    CreatedID = "0",
                    ReturnMessage = _retVal.ErrorList.FirstOrDefault()
                };
            }
            else
            {
                return new WebAPILog()
                {
                    CreatedID = _retVal.CreatedID
                };
            }
        }
    }
}
