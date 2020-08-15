using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MarketWebAPI.Features.Price
{
    public class PriceOutput
    {
        public int PriceID { get; set; }
        public DateTime Date { get; set; }
        [JsonPropertyName("MarketPriceEX1")]
        public decimal MarketPrice { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedTimeStamp { get; set; }
        public string UpdatedBy { get; set; }
    }
}
