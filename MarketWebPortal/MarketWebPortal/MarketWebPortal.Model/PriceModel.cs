using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace MarketWebPortal.Model
{
    public class PriceModel
    {
        public DateTime Date { get; set; }
        public double MarketPriceEX1 { get; set; }
    }
}
