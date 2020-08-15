using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherWebPortal.Business;
using WeatherWebPortal.Models;

namespace WeatherWebPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWeatherBusiness weatherBusiness;

        public HomeController(IWeatherBusiness weatherBusiness)
        {
            this.weatherBusiness = weatherBusiness;
        }

        public IActionResult Index() => View();

        [HttpPost]
        public WebAPIOutputModel<WeatherModel> GetWeather(string WOEIDLocation) =>
            weatherBusiness.GetWeather(WOEIDLocation, DateTime.Today, 5);
            

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
