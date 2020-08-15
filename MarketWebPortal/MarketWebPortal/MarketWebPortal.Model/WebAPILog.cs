using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketWebPortal.Model
{
    public class WebAPILog
    {
        public string SQLState { get; set; }
        public int SQLCode { get; set; }
        public string ReturnMessage { get; set; }
        public string SQLTransactionError { get; set; }
        public bool IsSessionTimedOut { get; set; }
        public string CreatedID { get; set; }
        [JsonProperty(PropertyName = "userMessage")]
        public string UserMessage { get; set; }
        [JsonProperty(PropertyName = "internalMessage")]
        public string InternalMessage { get; set; }
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }
        [JsonProperty(PropertyName = "moreInfo")]
        public string MoreInfo { get; set; }
        [JsonProperty(PropertyName = "StatusCode")]
        public int APIStatusCode { get; set; }
        [JsonProperty(PropertyName = "Message")]
        public string APIMessage { get; set; }
        [JsonProperty(PropertyName = "SystemExceptionList")]
        public List<Exception> APISystemExceptionList { get; set; }
        public bool IsSuccessful
        {
            get
            {
                int _state;
                bool isNumber = int.TryParse(SQLState, out _state);

                return (SQLCode == 0)
                    && string.IsNullOrEmpty(SQLTransactionError)
                    && ((isNumber && _state == 0) || (!isNumber))
                    && (APISystemExceptionList == null || APISystemExceptionList.Count.Equals(0));
            }
        }
    }
}
