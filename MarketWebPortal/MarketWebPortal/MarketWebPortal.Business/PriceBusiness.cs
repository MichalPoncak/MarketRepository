using MarketWebPortal.Data;
using MarketWebPortal.Model;
using MarketWebPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketWebPortal.Business
{
    public interface IPriceBusiness
    {
        WebAPIOutput<PriceModel> GetPrices();
        MaxPriceViewModel GetMaxPrice(DateTime startDate, DateTime endDate);
        public WebAPILog AddPrices(string createdBy, List<PriceModel> prices);
    }

    public class PriceBusiness : IPriceBusiness
    {
        private IPriceData priceData { get; }

        public PriceBusiness(IPriceData priceData) => this.priceData = priceData;

        public WebAPIOutput<PriceModel> GetPrices() => priceData.GetPrices();

        public MaxPriceViewModel GetMaxPrice(DateTime startDate, DateTime endDate)
        {
            WebAPIOutput<PriceModel> pricesDal = priceData.GetPrices();

            var maxPriceOutput = new MaxPriceViewModel();

            List<PriceModel> prices = pricesDal.DataList;

            List<PriceModel> pricesFilteredByDateRange = prices.Where(x => x.Date >= startDate && x.Date <= endDate).ToList();

            var groupPricesByHour = pricesFilteredByDateRange
                .GroupBy(x => new { x.Date.Date, x.Date.Hour })
                .Select(grp => new
                {
                    GroupedFields = grp.Key,
                    PriceDate = grp.First().Date,
                    PriceHour = grp.First().Date.Hour,
                    Price = grp.Sum(p => p.MarketPriceEX1),
                    RecordCount = grp.Count()
                }).ToList();

            if (groupPricesByHour.Count >= 1)
            {
                double maxPrice = groupPricesByHour.Max(x => x.Price);

                DateTime maxPriceAggregatedRecordDate = groupPricesByHour
                    .Where(x => x.Price == maxPrice)
                    .Select(x => x.PriceDate).First();

                List<PriceModel> maxPriceGranularRecords = pricesFilteredByDateRange
                    .Where(x => x.Date.Date == maxPriceAggregatedRecordDate.Date &&
                    x.Date.Hour == maxPriceAggregatedRecordDate.Hour).ToList();

                var mostExpensiveHour = groupPricesByHour.Select(x => x.Price).Max();

                maxPriceOutput.MaxHourlyPrice = mostExpensiveHour;
                maxPriceOutput.GranularPrices = maxPriceGranularRecords;
            }
            else
            {
                maxPriceOutput.MaxHourlyPrice = 0;
            }

            return maxPriceOutput;
        }

        public WebAPILog AddPrices(string createdBy, List<PriceModel> prices) =>
            priceData.AddPrices(createdBy, prices);
    }
}
