using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MarketWebAPI.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketWebAPI.Features.Price
{
    [ApiController]
    [Produces("application/json")]
    [Route("market/[controller]")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public class PriceController : Controller
    {
        private readonly IPriceService priceService;

        public PriceController(IPriceService priceService)
        {
            this.priceService = priceService;
        }

        /// <summary>
        /// Returns all prices.
        /// </summary>
        /// <returns>PriceOutput object</returns>
        [ProducesResponseType(typeof(IEnumerable<PriceOutput>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetPrices()
        {
            List<PriceOutput> prices = await priceService.GetPricesAsync();

            return Ok(prices);
        }

        /// <summary>
        /// Inserts all prices.
        /// </summary>
        /// <returns>InsertedID</returns>
        [ProducesResponseType(201)]
        [HttpPost]
        public async Task<IActionResult> InsertPrice([FromBody]Collection<PriceInput> body)
        {
            int insertedID = await priceService.InsertPricesAsync(
                Request.Headers.CreatedByHeader(),
                body);

            return Created(insertedID.ToString(), insertedID);
        }
    }
}