using MarketWebAPI.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace MarketWebAPI.Features.Price
{
    public interface IPriceDataAccess
    {
        List<PriceOutputDto> GetPrices();
        public int InsertPrices(
            string CreatedBy,
            string UpdatedBy,
            Collection<PriceInputDto> prices);
    }

    public class PriceDataAccess : IPriceDataAccess
    {
        private readonly IDataAccessHelper dataAccess;
        private readonly SqlParameterHelper parameterHelper;

        public PriceDataAccess(IDataAccessHelper dataAccess)
        {
            this.dataAccess = dataAccess;
            parameterHelper = new SqlParameterHelper();
        }

        public List<PriceOutputDto> GetPrices()
        {
            var parameterList = new List<DbParameter>(); // no params needed

            IDataReader dataReader = dataAccess.GetDataReader("spPriceGet", parameterList, CommandType.StoredProcedure);

            List<PriceOutputDto> prices;

            using (IDataReader reader = dataReader)
            {
                prices = reader.Select(PriceOutputDto.FromDataReader).ToList();
            }

            return prices;
        }

        public int InsertPrices(
            string CreatedBy,
            string UpdatedBy,
            Collection<PriceInputDto> prices)
        {
            var parameterList = new List<DbParameter>();
            parameterList.Add(parameterHelper.GetParameter("CreatedBy", CreatedBy));
            parameterList.Add(parameterHelper.GetParameter("UpdatedBy", UpdatedBy));
            parameterList.Add(parameterHelper.GetParameter("PricesJson", Newtonsoft.Json.JsonConvert.SerializeObject(prices)));

            var insertedIdParam = parameterHelper.GetParameter("InsertedID", SqlDbType.Int, ParameterDirection.Output);
            parameterList.Add(insertedIdParam);

            dataAccess.ExecuteNonQuery("spPriceBulkInsert", parameterList, CommandType.StoredProcedure);

            return (int)insertedIdParam.Value;
        }
    }
}
