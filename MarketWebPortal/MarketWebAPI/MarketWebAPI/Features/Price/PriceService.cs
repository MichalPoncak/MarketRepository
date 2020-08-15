using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MarketWebAPI.Features.Price
{
    public interface IPriceService
    {
        List<PriceOutput> GetPrices();
        public int InsertPrices(string createdBy, Collection<PriceInput> prices);
    }

    public class PriceService : IPriceService
    {
        private IPriceDataAccess priceDataAccess { get; }

        public PriceService(IPriceDataAccess priceDataAccess)
        {
            this.priceDataAccess = priceDataAccess;
        }

        public List<PriceOutput> GetPrices()
        {
            var prices = new List<PriceOutput>();
            List<PriceOutputDto> pricesDal = priceDataAccess.GetPrices();

            if (pricesDal != null && pricesDal.Count > 0)
            {
                prices = pricesDal.Select<PriceOutputDto, PriceOutput>(
                    x => new PriceOutput
                    {
                        PriceID = x.PriceID,
                        Date = x.Date,
                        MarketPrice = x.MarketPrice,
                        CreatedTimeStamp = x.CreatedTimeStamp,
                        CreatedBy = x.CreatedBy,
                        UpdatedTimeStamp = x.UpdatedTimeStamp,
                        UpdatedBy = x.UpdatedBy
                    }).ToList();
            }

            return prices;
        }

        public int InsertPrices(string createdBy, Collection<PriceInput> prices)
        {
            var marketPrices = new Collection<PriceInputDto>(prices.Select(a => new PriceInputDto
            {
                PriceDate = a.Date,
                MarketPrice = a.MarketPrice,
                CreatedBy = createdBy,
                UpdatedBy = createdBy
            }).ToList());

            return priceDataAccess.InsertPrices(
                createdBy,
                createdBy,
                marketPrices);
        }
    }
}
