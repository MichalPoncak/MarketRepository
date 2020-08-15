using MarketWebPortal.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWebPortal.ViewModels
{
    public class MaxPriceViewModel
    {
        public List<PriceModel> GranularPrices { get; set; } = new List<PriceModel>();
        public double MaxHourlyPrice { get; set; }
    }
}
