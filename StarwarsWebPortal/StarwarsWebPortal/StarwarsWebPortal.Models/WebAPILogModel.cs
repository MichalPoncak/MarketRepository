using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarwarsWebPortal.Models
{
    public class WebAPILogModel
    {
        [JsonProperty(PropertyName = "StatusCode")]
        public int APIStatusCode { get; set; }
        [JsonProperty(PropertyName = "Message")]
        public string APIMessage { get; set; }
        [JsonProperty(PropertyName = "SystemExceptionList")]
        public List<Exception> APISystemExceptionList { get; set; }
    }
}
