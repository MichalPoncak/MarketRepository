using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MarketWebPortal.ViewModels;
using MarketWebPortal.Business;
using MarketWebPortal.Model;
using System.IO;
using Microsoft.AspNetCore.Http;
using Aspose.Cells;
using System.Data;
using System.Reflection;

namespace MarketWebPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPriceBusiness priceBusiness;

        public HomeController(ILogger<HomeController> logger, IPriceBusiness priceBusiness)
        {
            _logger = logger;
            this.priceBusiness = priceBusiness;
        }

        public IActionResult Index()
        {
            WebAPIOutput<PriceModel> prices = priceBusiness.GetPrices();

            var priceBarChartData = new PriceBarChartViewModel();
            priceBarChartData.MinPrice = (prices.DataList.Count >= 1) ? prices.DataList.Select(x => x.MarketPriceEX1).Min() : 0;
            priceBarChartData.MaxPrice = (prices.DataList.Count >= 1) ? prices.DataList.Select(x => x.MarketPriceEX1).Max() : 0;
            priceBarChartData.AveragePrice = (prices.DataList.Count >= 1) ? prices.DataList.Select(x => x.MarketPriceEX1).Average() : 0;

            return View(priceBarChartData);
        }

        [HttpPost]
        public MaxPriceViewModel GetMaxPrice(DateTime startDate, DateTime endDate) => 
            priceBusiness.GetMaxPrice(startDate, endDate);

        [HttpGet]
        public IActionResult Upload() => View(new PriceViewModel() { Response = string.Empty });

        [HttpPost]
        public async Task<IActionResult> Upload(PriceViewModel file)
        {
            var result = new PriceViewModel();

            using (var memoryStream = new MemoryStream())
            {
                await file.FormFile.CopyToAsync(memoryStream);

                if (memoryStream.Length > 0)
                {
                    // Instantiate LoadOptions specified by the LoadFormat
                    var loadOptions4 = new LoadOptions(LoadFormat.CSV);

                    var wb = new Workbook(memoryStream, loadOptions4);
                    
                    // Export all the data of the first worksheet to DataTable
                    DataTable dtPrices = wb.Worksheets[0].Cells.ExportDataTable(0, 0, wb.Worksheets[0].Cells.MaxDataRow + 1, wb.Worksheets[0].Cells.MaxDataColumn + 1, true);

                    List<PriceModel> prices = ConvertDataTable<PriceModel>(dtPrices);

                    string appName = Assembly.GetExecutingAssembly().GetName().Name;

                    WebAPILog webAPIResponse = priceBusiness.AddPrices(appName, prices);

                    if (webAPIResponse.IsSuccessful && Convert.ToInt32(webAPIResponse.CreatedID) >= 1)
                    {
                        result.Response = "Records inserted!";
                    }
                    else
                    {
                        result.Response = "Records not inserted! Please get in touch with your administrator with the following error:<br />";
                        result.Response += webAPIResponse.ReturnMessage;
                    }
                }
            }

            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        [NonAction]
        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        [NonAction]
        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName.Replace(" ", ""))
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}
