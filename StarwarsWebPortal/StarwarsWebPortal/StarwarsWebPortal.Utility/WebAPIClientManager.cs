using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace StarwarsWebPortal.Utility
{
    public interface IWebAPIClientManager
    {
        bool SetURIPath(string URIPath);
        bool AddURIParam(string ParameterName, string ParameterValue);
        bool AddHeaderParams(string ParameterName, string ParameterValue);
        bool AddURIQstringParam(string ParameterName, List<string> ParameterValue);
        bool SetBodyValue(object ParamObj);
        T Get<T>(T Param);
        WebAPIError GetAPIError();
    }

    public class WebAPIClientManager : IWebAPIClientManager
    {
        private HttpClient _httpClient;
        private Dictionary<string, string> _uriParams;
        private Dictionary<string, List<string>> _uriQueryParamCollection;
        private Dictionary<string, string> _headerParams;
        private string _uriPath;
        private string webAPIURL;

        public WebAPIError WebAPIError { get; set; }

        public WebAPIClientManager(string webAPIURL)
        {
            _uriParams = new Dictionary<string, string>();
            _uriQueryParamCollection = new Dictionary<string, List<string>>();
            _headerParams = new Dictionary<string, string>();
            _uriPath = string.Empty;
            this.webAPIURL = webAPIURL;
            WebAPIError = new WebAPIError() { APIStatusCode = 200 };

            InitHttpClient();
        }

        private string GenerateCompleteURI()
        {
            string retVal = string.Empty;
            retVal += webAPIURL;
            retVal += _uriPath;

            foreach (var _kvp in _uriParams)
            {
                retVal += string.Format("/{0}/{1}", _kvp.Key, Uri.EscapeDataString(_kvp.Value));
            }

            if (_uriQueryParamCollection.Count > 0) retVal += "?";

            foreach (var _kvp in _uriQueryParamCollection)
            {
                string _temp = string.Empty;

                foreach (var _val in _kvp.Value)
                {
                    _temp += string.Format("{0}={1}&", _kvp.Key, Uri.EscapeDataString(_val));
                }
                _temp.TrimEnd(new char[] { '&' });
                retVal += _temp;
            }

            return retVal;
        }

        private void InitHttpClient()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("CreatedBy", "MarketWebPortal");
        }

        public bool SetURIPath(string URIPath)
        {
            _uriPath = URIPath;
            _uriParams.Clear();
            return true;
        }

        public bool AddURIParam(string ParameterName, string ParameterValue)
        {
            bool retVal = true;

            try
            {
                _uriParams.Add(ParameterName, ParameterValue);
            }
            catch
            {
                retVal = false;
            }

            return retVal;
        }

        public bool AddHeaderParams(string ParameterName, string ParameterValue)
        {
            bool retVal = true;

            try
            {
                if (!_httpClient.DefaultRequestHeaders.Contains(ParameterName))
                {
                    _headerParams.Add(ParameterName, ParameterValue);
                    _httpClient.DefaultRequestHeaders.Add(ParameterName, ParameterValue);
                }
            }
            catch
            {
                retVal = false;
            }

            return retVal;
        }

        public bool AddURIQstringParam(string ParameterName, List<string> ParameterValue)
        {
            bool retVal = true;

            try
            {
                _uriQueryParamCollection.Add(ParameterName, ParameterValue);
            }
            catch
            {
                retVal = false;
            }

            return retVal;
        }

        public bool SetBodyValue(object ParamObj)
        {
            return true;
        }

        public T Get<T>(T Param)
        {
            T _retVal = Param; // Param is only used to assign a default value to a generic type

            string _fullURI = GenerateCompleteURI(); // EX: "http://swapi.dev/api/starships/?page=1&";
            HttpResponseMessage _response = _httpClient.GetAsync(_fullURI).Result;

            if (_response.IsSuccessStatusCode)
            {
                // note: this line can crash if an error occurs in the model class
                _retVal = _response.Content.ReadAsAsync<T>().Result; // WebApi.Client nuget package neede for ReadAsAsync
            }
            else
            {
                WebAPIError = _response.Content.ReadAsAsync<WebAPIError>().Result;
            }

            return _retVal;
        }

        public WebAPIError GetAPIError()
        {
            return this.WebAPIError;
        }
    }

    public class WebAPIResponse
    {
        [JsonProperty(PropertyName = "errors")]
        public List<string> ErrorList { get; set; }
        public WebAPIResponse()
        {
            ErrorList = new List<string>();
        }
        public string CreatedID { get; set; }
    }

    public class WebAPIError
    {
        [JsonProperty(PropertyName = "StatusCode")]
        public int APIStatusCode { get; set; }
        [JsonProperty(PropertyName = "Message")]
        public string APIMessage { get; set; }
        private List<Exception> _apiSystemExceptionList { get; set; }
        [JsonProperty(PropertyName = "SystemExceptionList")]
        public List<Exception> APISystemExceptionList
        {
            get
            {
                if (_apiSystemExceptionList == null) _apiSystemExceptionList = new List<Exception>();
                return _apiSystemExceptionList;
            }
            set { _apiSystemExceptionList = value; }
        }
    }
}
