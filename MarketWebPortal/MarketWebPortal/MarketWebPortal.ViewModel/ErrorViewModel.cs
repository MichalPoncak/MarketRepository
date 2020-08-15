using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWebPortal.ViewModel
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public int APIStatusCode { get; set; }
        public string APIMessage { get; set; }
        public List<Exception> APISystemExceptionList { get; set; }
    }
}
