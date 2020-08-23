using System;
using Geodesics.Api.Business;
using Geodesics.Api.Model;
using Geodesics.Api.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Geodesics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public class DistanceController : ControllerBase
    {
        // the commented out logger can be enabled to log custom messages
        // nlog logger is configured and used to capture exceptions only
        //private readonly ILogger<DistanceController> logger;
        private readonly IDistanceService distanceService;

        public DistanceController(
            //ILogger<DistanceController> logger,
            IDistanceService distanceService)
        {
            //this.logger = logger;
            this.distanceService = distanceService;
        }

        /// <summary>
        /// Retrieves the distance between the provided points.
        /// </summary>
        /// <response code="200">Distance successfully calculated.</response>
        /// <response code="400">Latitude or longitude of any of the given points is out of range.</response>
        /// <response code="500">Unexpected server exception.</response>
        [HttpGet]
        [Route("{distanceMethod}/{measureUnit}")]
        public ActionResult<DistanceResponse> Get(
            DistanceMethod distanceMethod,
            MeasureUnit measureUnit,
            [FromQuery] double point1Latitude,
            [FromQuery] double point1Longitude,
            [FromQuery] double point2Latitude,
            [FromQuery] double point2Longitude)
        {
            //logger.LogInformation("DistanceController.Get method called!!!");

            var point1 = new DistancePoint
            {
                Latitude = point1Latitude,
                Longitude = point1Longitude
            };

            var point2 = new DistancePoint
            {
                Latitude = point2Latitude,
                Longitude = point2Longitude
            };

            try
            {
                ValidatePoint(point1);
                ValidatePoint(point2);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }

            switch (distanceMethod)
            {
                case DistanceMethod.GeodesicCurve:
                    return new ActionResult<DistanceResponse>(
                        new DistanceResponse
                        {
                            Distance = distanceService.CalculateGeodesicCurve(
                                point1,
                                point2,
                                measureUnit)
                        });
                case DistanceMethod.Pythagoras:
                    return new ActionResult<DistanceResponse>(
                        new DistanceResponse
                        {
                            Distance = distanceService.CalculatePythagoras(
                                point1,
                                point2,
                                measureUnit)
                        });
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(distanceMethod), distanceMethod, null);
            }
        }

        private void ValidatePoint(DistancePoint point)
        {
            if (point.Latitude < -90 || point.Latitude > 90)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(point.Latitude),
                    "Must be in [-90, 90] interval");
            }
            
            if (point.Longitude < -180 || point.Longitude > 180)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(point.Longitude),
                    "Must be in [-180, 180] interval");
            }
        }
    }
}
