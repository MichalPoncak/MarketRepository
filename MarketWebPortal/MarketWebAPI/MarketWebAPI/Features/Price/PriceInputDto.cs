using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketWebAPI.Features.Price
{
    public class PriceInputDto
    {
        public DateTime PriceDate { get; set; }
        public decimal MarketPrice { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
