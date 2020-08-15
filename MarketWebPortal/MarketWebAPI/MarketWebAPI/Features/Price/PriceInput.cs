using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MarketWebAPI.Features.Price
{
    public class PriceInput
    {
        public DateTime Date { get; set; }
        [JsonPropertyName("MarketPriceEX1")]
        public decimal MarketPrice { get; set; }
    }
}
