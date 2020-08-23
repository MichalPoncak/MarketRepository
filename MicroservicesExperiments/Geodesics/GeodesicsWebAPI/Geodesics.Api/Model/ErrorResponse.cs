using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Geodesics.Api.Model
{
    public class ErrorResponse
    {
        public IEnumerable<Error> Errors { get; set; }

        public class Error
        {
            public string UserMessage { get; set; }
            public string InternalMessage { get; set; }
            public string Code { get; set; }
            public string MoreInfo { get; set; }
        }
    }
}
