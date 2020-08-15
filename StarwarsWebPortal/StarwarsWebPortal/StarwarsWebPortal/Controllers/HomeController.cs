using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StarwarsWebPortal.Business;
using StarwarsWebPortal.Models;
using StarwarsWebPortal.ViewModels;

namespace StarwarsWebPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStarshipBusiness starshipBusiness;

        public HomeController(IStarshipBusiness starshipBusiness)
        {
            this.starshipBusiness = starshipBusiness;
        }

        public IActionResult Index() => View();

        [HttpPost]
        public WebAPIOutputModel<StarshipViewModel> GetStarships(int distance, int pageNumber) =>
            starshipBusiness.GetStarships(distance, pageNumber);

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
