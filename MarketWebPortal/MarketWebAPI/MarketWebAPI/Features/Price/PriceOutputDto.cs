using MarketWebAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MarketWebAPI.Features.Price
{
    public class PriceOutputDto
    {
        public static PriceOutputDto FromDataReader(IDataReader reader)
        {
            var fee = new PriceOutputDto();
            fee.PriceID = reader.GetValueOrDefault<int>("pric_PriceID");
            fee.Date = reader.GetValueOrDefault<DateTime>("pric_Date");
            fee.MarketPrice = reader.GetValueOrDefault<decimal>("pric_MarketPrice");
            fee.CreatedTimeStamp = reader.GetValueOrDefault<DateTime>("pric_CreatedTimeStamp");
            fee.CreatedBy = reader.GetValueOrDefault<string>("pric_CreatedBy");
            fee.UpdatedTimeStamp = reader.GetValueOrDefault<DateTime>("pric_UpdatedTimeStamp");
            fee.UpdatedBy = reader.GetValueOrDefault<string>("pric_UpdatedBy");
            return fee;
        }

        public int PriceID { get; set; }
        public DateTime Date { get; set; }
        public decimal MarketPrice { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedTimeStamp { get; set; }
        public string UpdatedBy { get; set; }
    }
}
