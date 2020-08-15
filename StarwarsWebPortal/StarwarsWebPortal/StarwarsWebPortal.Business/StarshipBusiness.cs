using StarwarsWebPortal.Data;
using StarwarsWebPortal.Models;
using StarwarsWebPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace StarwarsWebPortal.Business
{
    public interface IStarshipBusiness
    {
        WebAPIOutputModel<StarshipViewModel> GetStarships(int distance, int pageNumber);
    }

    public class StarshipBusiness : IStarshipBusiness
    {
        private IStarshipData starshipData { get; }

        public StarshipBusiness(IStarshipData starshipData) => this.starshipData = starshipData;

        public WebAPIOutputModel<StarshipViewModel> GetStarships(int distance, int pageNumber)
        {
            WebAPIOutputModel<StarshipViewModel> starshipContext = starshipData.GetStarships(pageNumber);

            string previousLink = starshipContext.DataList.FirstOrDefault().Previous;
            string nextLink = starshipContext.DataList.FirstOrDefault().Next;

            int previousPageNumber = Convert.ToInt32(GetQueryStringValue("page", previousLink));
            int nextPageNumber = Convert.ToInt32(GetQueryStringValue("page", nextLink));
            int currentPageNumber = GetCurrentPageNumber(previousPageNumber, nextPageNumber);

            starshipContext.DataList.FirstOrDefault().PreviousPageNumber = (!string.IsNullOrEmpty(previousLink)) ? previousPageNumber : 0;
            starshipContext.DataList.FirstOrDefault().CurrentPageNumber = currentPageNumber;
            starshipContext.DataList.FirstOrDefault().NextPageNumber = (!string.IsNullOrEmpty(nextLink)) ? nextPageNumber : 0;

            foreach (StarshipModel s in starshipContext.DataList[0].Starships)
            {
                s.RequiredResupplies = GetResuplyCount(
                    distance,
                    s.ConsumableRate,
                    s.ConsumableAmount,
                    s.Megalights);
            }

            return starshipContext;
        }

        private int GetCurrentPageNumber(int previousPageNumber, int nextPageNumber)
        {
            int result = 0;

            if (previousPageNumber != 0)
            {
                result = previousPageNumber + 1;
            }
            else
            {
                result = nextPageNumber - 1;
            }

            return result;
        }

        private string GetQueryStringValue(string queryStringName, string URL)
        {
            string result = "0";

            if (!string.IsNullOrEmpty(URL))
            {
                var uri = new Uri(URL);
                result = HttpUtility.ParseQueryString(uri.Query).Get(queryStringName);
            }

            return result;
        }

        private int GetResuplyCount(
            int distance,
            string consumableRate,
            int consumableAmount,
            string megalights)
        {
            int result = 0;
            const int hours = 24;
            const int daysInWeek = 7;
            const double weeksInMonth = 4.5;
            const int monthsInYear = 12;

            if (megalights != "unknown")
            {
                double mglt = Convert.ToInt32(megalights);

                switch (consumableRate)
                {
                    case "hour":
                        result = Convert.ToInt32(distance / (consumableAmount * mglt));
                        break;
                    case "day":
                        result = Convert.ToInt32(distance / (hours * consumableAmount * mglt));
                        break;
                    case "week":
                        result = Convert.ToInt32(distance / (hours * daysInWeek * consumableAmount * mglt));
                        break;
                    case "month":
                        result = Convert.ToInt32(distance / (hours * daysInWeek * weeksInMonth * consumableAmount * mglt));
                        break;
                    case "year":
                        result = Convert.ToInt32(distance / (hours * daysInWeek * weeksInMonth * monthsInYear * consumableAmount * mglt));
                        break;
                    default: // unknown
                        result = 0;
                        break;
                }
            }
            else
            {
                switch (consumableRate)
                {
                    case "hour":
                        result = Convert.ToInt32(distance / (consumableAmount));
                        break;
                    case "day":
                        result = Convert.ToInt32(distance / (hours * consumableAmount));
                        break;
                    case "week":
                        result = Convert.ToInt32(distance / (hours * daysInWeek * consumableAmount));
                        break;
                    case "month":
                        result = Convert.ToInt32(distance / (hours * daysInWeek * weeksInMonth * consumableAmount));
                        break;
                    case "year":
                        result = Convert.ToInt32(distance / (hours * daysInWeek * weeksInMonth * monthsInYear * consumableAmount));
                        break;
                    default: // unknown
                        result = 0;
                        break;
                }
            }

            return result;
        }
    }
}
